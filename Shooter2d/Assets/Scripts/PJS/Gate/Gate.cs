using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gate : PjBase
{
    public GameObject portalBullet;
    public float maxEnergy;
    public float energyRecoverOverTime;
    public float energyDrainedPerPortal;
    public float energyDrainedPerPortalOverTime;
    [HideInInspector]
    public float energy;
    [HideInInspector]
    public List<Portal> portals = new List<Portal>();

    public override void Awake()
    {
        base.Awake();
        energy = maxEnergy;
    }

    public override void Update()
    {
        base.Update();
       
        if (ammoText != null)
        {
            habText.text = energy.ToString("F0") + " / " + maxEnergy;
        }
        if (portals.Count == 0 && energy < maxEnergy)
        {
            if (weapon != Weapon.secondary)
            {
                energy += energyRecoverOverTime * Time.deltaTime;
            }
            else
            {
                energy += energyRecoverOverTime * 0.5f * Time.deltaTime;
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
        if (shootCD <= 0 )
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

    public override void EquipHability()
    {
        if (!onAnim)
        {
            if (weapon == Weapon.principal && energy > energyDrainedPerPortal)
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

    public override IEnumerator SecondaryShoot()
    {
        if (portals.Count > 0)
        {
            foreach (Portal portal in portals)
            {
                portal.Die();
            }
        }

        portals.Clear();
        return base.SecondaryShoot();
    }

    public override void AnimationCallHability()
    {
        base.AnimationCallHability();

        PortalBullet projectile = Instantiate(portalBullet, shootPoint.transform.position, shootPoint.transform.rotation).GetComponent<PortalBullet>();
        projectile.user = this;
        projectile.team = team;

        energy -= energyDrainedPerPortal;
    }
}
