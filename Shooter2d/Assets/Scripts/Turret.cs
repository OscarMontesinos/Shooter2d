using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Turret : Enemy
{
    Vector2 startingDir;
    public float turnSpeed;
    public float returningTurnSpeed;

    public override void Start()
    {
        base.Start();
        startingDir = transform.up;
    }
    public override void Update()
    {
        base.Update();
    }
    public override void IA()
    {
        DetectEnemies(true);
        if (stunTime <= 0)
        {
            if (targetsOnSight.Count > 0)
            {
                PjBase target = GetClosestTarget();
                Vector2 dir = target.transform.position - transform.position;
                float rotateAmount = Vector3.Cross(dir.normalized, transform.up).z;
                transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + -rotateAmount * turnSpeed * Time.deltaTime);
            }
            else
            {
                float rotateAmount = Vector3.Cross(startingDir.normalized, transform.up).z;
                transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + -rotateAmount * returningTurnSpeed * Time.deltaTime);
            }
            if (currentAmmo >= 3)
            {
                if (targetsOnSight.Count > 0)
                {
                    if (shootCD <= 0 && !recharging)
                    {
                        StartCoroutine(NormalShoot());
                    }
                    StartCoroutine(RestartIA());
                }
                else if (shootCD <= 0 && currentAmmo <= ammo * 0.4f)
                {
                    Recharge();
                    StartCoroutine(RestartIA());
                }
                else
                {
                    StartCoroutine(RestartIA());
                }
            }
            else
            {
                Recharge();
                StartCoroutine(RestartIA());
            }
        }
        else
        {
            StartCoroutine (RestartIA());
        }
    }

    public override IEnumerator NormalShoot()
    {
        shootCD = shootRate;
        int burst = shootBurst;
        while (burst > 0)
        {
            currentAmmo--;
            animator.Play("Idle");
            yield return null;
            animator.Play("Shoot");
            Projectile projectile = Instantiate(bullet, shootPoint.transform.position, shootPoint.transform.rotation).GetComponent<Projectile>();
            projectile.transform.localEulerAngles = new Vector3(0, 0, projectile.transform.localEulerAngles.z + Random.Range(-detour, detour));
            projectile.team = team;
            projectile.dmg = dmg;
            yield return new WaitForSeconds(burstDelay);
            burst--;
        }
    }
    
}
