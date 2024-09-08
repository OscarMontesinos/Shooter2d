using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;

public class Enemy : PjBase
{
    public bool hackable;

    public GameObject UI;
    public List<PjBase> targetsOnSight;

    public GameObject particleOnDie;

    public float visionRange;
    public float visionAngle;

    public float startingAngle;

    public GameObject sting;

    public override void Awake()
    {
        base.Awake();
        startingAngle = transform.localEulerAngles.z;
    }
    public override void Start()
    {
        base.Start();
        IA();
    }
    public override void Update()
    {
        base.Update();
        UI.transform.localEulerAngles = -transform.localEulerAngles;

        if(team == 2 && sting == null)
        {
            team = 0;
        }
    }
    public virtual void IA()
    {

    }

    public virtual IEnumerator RestartIA()
    {
        yield return null;
        IA();
    }

    public void DetectEnemies(bool removeEnemies)
    {
        if (removeEnemies)
        {
            targetsOnSight.Clear();
        }

        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, visionRange, GameManager.Instance.playerLayer + GameManager.Instance.enemyLayer);
        PjBase enemy;
        foreach (Collider2D enemyColl in enemiesHit)
        {
            Transform target = enemyColl.transform;
            Vector2 dir = target.position - transform.position;

            if (Vector3.Angle(pointer.transform.up, dir.normalized) < visionAngle / 2 && !Physics2D.Raycast(transform.position, dir, dir.magnitude, GameManager.Instance.wallLayer))
            {
                enemy = enemyColl.GetComponent<PjBase>();
                if (enemy.team != team)
                {
                    targetsOnSight.Add(enemy);
                }
            }
        }
    }

    public PjBase GetClosestTarget()
    {
        PjBase closestTarget = null;
        Vector2 closestDir = Vector2.zero;
        foreach (PjBase target in targetsOnSight)
        {
            if(closestDir == Vector2.zero)
            {
                closestTarget = target;
                closestDir = target.transform.position - transform.position;
            }
            else
            {
                Vector2 actualDir = target.transform.position - transform.position;
                if(actualDir.magnitude < closestDir.magnitude)
                {
                    closestTarget = target;
                    closestDir = actualDir;
                }
            }
        }
        return closestTarget;
    }

   


    public override void Die(PjBase killer)
    {
        if(particleOnDie != null)
        {
            Instantiate(particleOnDie, transform.position, new Quaternion(0,0,0,0));
        }
        if(sting != null)
        {
            sting.transform.parent = null;
        }
        base.Die(killer);

    }

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Vector3 rightAngle = GameManager.DirectionFromAngle(transform.eulerAngles.z, -visionAngle / 2);
        Vector3 leftAngle = GameManager.DirectionFromAngle(transform.eulerAngles.z, visionAngle / 2);

        Gizmos.DrawLine(transform.position, transform.position + leftAngle * visionRange);
        Gizmos.DrawLine(transform.position, transform.position + rightAngle * visionRange);
    }
    public virtual void OnDrawGizmos()
    {
        transform.localEulerAngles = new Vector3(0, 0, startingAngle);
        UI.transform.localEulerAngles = -transform.localEulerAngles;
    }
}
