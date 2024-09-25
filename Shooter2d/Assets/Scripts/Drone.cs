using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http.Headers;
using UnityEngine;

public class Drone : MovingEnemy
{
    Vector2 startingDir;
    public float turnSpeed;
    public GameObject DroneToFollow;

    public override void Start()
    {
        base.Start();
        agent.SetDestination(transform.position);
        agent.speed = spd;
        agent.updateUpAxis = false;
        agent.updateRotation = false;

    }
    public override void IA()
    {
        bool isTargetsOnZero = false;


        if(targetsOnSight.Count== 0)
        {
            isTargetsOnZero = true;
        }
        DetectEnemies(false);
        if(isTargetsOnZero && targetsOnSight.Count > 0)
        {
            agent.SetDestination(transform.position);
        }


        if (stunTime <= 0)
        {
            if (targetsOnSight.Count > 0)
            {
                PjBase target = GetClosestTarget();

                if (agent.isOnNavMesh && agent.remainingDistance < 3)
                {
                    SetPositionCloseToTarget(target);
                }

                Vector2 dir = target.transform.position - transform.position;
                float rotateAmount = Vector3.Cross(dir.normalized, transform.up).z;
                transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + -rotateAmount * turnSpeed * Time.deltaTime);



                if (currentAmmo > 0 && !Physics2D.Raycast(transform.position, dir, dir.magnitude, GameManager.Instance.wallLayer))
                {
                    if (shootCD <= 0 && !recharging)
                    {
                        StartCoroutine(NormalShoot());
                    }
                    StartCoroutine(RestartIA());
                }
                else
                {
                    Recharge();
                    StartCoroutine(RestartIA());
                }
            }
            else if (agent.isOnNavMesh)
            {
                if(DroneToFollow != null)
                {
                    agent.SetDestination(DroneToFollow.transform.position);
                }
                else
                {
                    if(currentWaypoint != null)
                    {
                        Vector2 dist = currentWaypoint.transform.position - transform.position;
                        if (dist.magnitude < 5)
                        {
                           currentWaypoint = GetRandomWaypoint();
                        }
                    }
                    else
                    {
                        currentWaypoint = GetRandomWaypoint();
                    }
                    agent.SetDestination(currentWaypoint.transform.position);
                }
                Vector2 lookTowards = agent.destination - transform.position;
                transform.up = lookTowards.normalized;
                StartCoroutine(RestartIA());
            }
            else
            {
                StartCoroutine(RestartIA());
            }
        }
        else
        {
            StartCoroutine(RestartIA());
        }
    }


    public override IEnumerator NormalShoot()
    {
        shootCD = shootRate;
        currentAmmo--;
        animator.Play("Idle");
        yield return null;
        animator.Play("Shoot");
        Projectile projectile = Instantiate(bullet, shootPoint.transform.position, shootPoint.transform.rotation).GetComponent<Projectile>();
        projectile.transform.localEulerAngles = new Vector3(0, 0, projectile.transform.localEulerAngles.z + Random.Range(-detour, detour));
        projectile.team = team;
        projectile.dmg = dmg;
        yield return new WaitForSeconds(burstDelay);
    }


}
