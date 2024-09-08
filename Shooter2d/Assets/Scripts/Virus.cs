using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Virus : PjBase
{
    bool isAiming = false;
    public GameObject aimingBullet;

    public GameObject stingBullet;
    public GameObject actualStingBullet;
    public int maxStings;
    int stings;

    public float aimingShootRate;
    public float aimingDmg;
    public override void Awake()
    {
        base.Awake();
        stings = maxStings;
    }
    public override void Update()
    {
        base.Update();

        if (ammoText != null)
        {
            habText.text = stings.ToString() + " / " + maxStings;
        }
    }

    public override void Shoot(bool secondary)
    {
        if (!onAnim && !recharging)
        {
            if (weapon == Weapon.principal)
            {
                if (!secondary)
                {
                    StartCoroutine(NormalShoot());
                }
                else if (secondary )
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
                if (currentAmmo >= ammo * 0.25f && isAiming)
                {
                    shootCD = aimingShootRate;
                    animator.Play("Idle");
                    yield return null;
                    animator.Play("AimShoot");
                    currentAmmo -= ammo*0.25f;
                    Projectile projectile = Instantiate(aimingBullet, shootPoint.transform.position, shootPoint.transform.rotation).GetComponent<Projectile>();
                    projectile.team = team;
                    projectile.dmg = aimingDmg;
                }
                else if (!isAiming)
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
            else
            {
                Recharge();
            }
        }
        yield return base.NormalShoot();
    }


    public override void Recharge()
    {
        if (!recharging)
        {
            if (currentAmmo < ammo && !onAnim)
            {
                onAnim = false;
                weapon = Weapon.principal;
                recharging = true;
                if (isAiming)
                {
                    animator.Play("AimRecharge");
                    spd *= 2;
                }
                else
                {
                    animator.Play("Recharge");
                }
            }
        }
    }
    public override IEnumerator SecondaryShoot()
    {
        isAiming = !isAiming;
        if (isAiming)
        {
            onAnim = true;
            animator.Play("Aim");
            spd *= 0.5f;
        }
        else
        {
            onAnim = true;
            animator.Play("StopAim");
            spd *= 2;
        }
        yield return null;
    }

    public override void EquipHability()
    {
        if (!onAnim && !isAiming)
        {
            if (weapon == Weapon.principal && stings > 0)
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

    public override void UseHability()
    {
        base.UseHability();
        stings--;
        ProjectileSnapper projectile = Instantiate(stingBullet, shootPoint.transform.position, shootPoint.transform.rotation).GetComponent<ProjectileSnapper>();
        projectile.team = team;
        projectile.user = this;
        actualStingBullet = projectile.gameObject;
    }


    public void HackEnemy(Enemy enemy)
    {
        enemy.team = 2;
        enemy.sting = actualStingBullet;
    }
    public override void AnimationCallRecharge()
    {
        base.AnimationCallRecharge();
        if (isAiming)
        {
            spd *= 0.5f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == actualStingBullet)
        {
            Destroy(actualStingBullet);
            stings++;
        }
    }
}
