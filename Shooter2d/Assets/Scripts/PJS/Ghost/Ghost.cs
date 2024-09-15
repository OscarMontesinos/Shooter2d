using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Ghost : PjBase
{
    public GameObject shadow;
    public GameObject currentShadow;
    public GameObject stunnParticle;
    public float habCD;
    [HideInInspector]
    public float habCurrentCD;
    public float habInvisSeconds;
    public float stunArea;  
    public float habStunTime;  
    public float habDmg;

    public override void Update()
    {
        base.Update();
        if (habCurrentCD > 0)
        {
            habCurrentCD -= Time.deltaTime;
        }

        if (ammoText != null)
        {
            if (habCurrentCD > 0)
            {
                habText.text = habCurrentCD.ToString("F1");
            }
            else
            {
                habText.text = "READY";
            }
        }
    }
    public override void Shoot(bool secondary)
    {
        if (!onAnim)
        {
            if (weapon == Weapon.principal)
            {
                if (!secondary && !recharging)
                {
                    StartCoroutine(NormalShoot());
                }
                else if (secondary)
                {
                    StartCoroutine(SecondaryShoot());
                }
            }
            else
            {
                if (!secondary)
                {
                    UseHability();
                }
                else
                {
                    EquipHability();
                }
            }
        }
    }

    public override IEnumerator NormalShoot()
    {
        if (shootCD <= 0)
        {
            if (currentAmmo > 0)
            {
                shootCD = shootRate;
                animator.Play("Idle");
                yield return null;
                animator.Play("Shoot");
                currentAmmo--;
                Projectile projectile = Instantiate(bullet, shootPoint.transform.position, shootPoint.transform.rotation).GetComponent<Projectile>();
                projectile.transform.localEulerAngles = new Vector3(0, 0, projectile.transform.localEulerAngles.z + Random.Range(-detour, detour));
                projectile.team = team;
                projectile.dmg = dmg;
            }
            else
            {
                Recharge();
            }
        }
        yield return base.NormalShoot();
    }

    public override IEnumerator SecondaryShoot()
    {
        if (currentShadow)
        {
            habCurrentCD = habCD;

            ShadowStunn();

            invisibleTime = 0.3f;
            transform.position = currentShadow.transform.position;
            Destroy(currentShadow);
        }
        yield return null;
    }

    void ShadowStunn()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, stunArea, GameManager.Instance.enemyLayer);
        PjBase enemy;
        foreach (Collider2D enemyColl in enemiesHit)
        {
            enemy = enemyColl.GetComponent<PjBase>();
            enemy.GetComponent<TakeDamage>().TakeDamage(this, habDmg);
            enemy.GetComponent<TakeDamage>().Stunn(habStunTime);
        }
        Instantiate(stunnParticle, transform.position, transform.rotation);
    }

    public override void EquipHability()
    {

        if (!onAnim)
        {
            if (weapon == Weapon.principal && !currentShadow && habCurrentCD<=0)
            {
                weapon = Weapon.secondary;
                onAnim = true;
                animator.Play("ShowHab");
            }
            else if (weapon != Weapon.principal)
            {
                weapon = Weapon.principal;
                onAnim = true;
                animator.Play("HideHab");
            }
        }
    }

    public override void AnimationCallHability()
    {
        invisibleTime = habInvisSeconds;
        currentShadow = Instantiate(shadow, transform.position, transform.rotation);
        base.AnimationCallHability();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, stunArea);
    }
}
