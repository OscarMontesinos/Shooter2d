using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : PjBase
{
    public GameObject airStrike;
    public AirStrike currentAirStrike;
    float airStrikeDelay;
    public float airStrikeCD;
    float actualAirStrikeCD;
    public float airStrikeDuration;
    public float airStrikeSpd;
    public float airStrikeDmgPerSecond;
    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        base.Update();
        if (actualAirStrikeCD > 0)
        {
            actualAirStrikeCD -= Time.deltaTime;
        }
        if (airStrikeDelay > 0)
        {
            airStrikeDelay -= Time.deltaTime;
        }

        if (ammoText != null)
        {
            if (actualAirStrikeCD > 0)
            {
                habText.text = actualAirStrikeCD.ToString("F1");
            }
            else
            {
                habText.text = "READY";
            }
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
                else
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
                    StartCoroutine(SecondaryShoot());
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
            if (weapon == Weapon.principal && actualAirStrikeCD <= 0)
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
        if (currentAirStrike == null)
        {
           currentAirStrike = Instantiate(airStrike, UtilsClass.GetMouseWorldPosition(), new Quaternion(0, 0, 0, 0)).GetComponent<AirStrike>();
            airStrikeDelay = 0.25f;
        }
        else if (!currentAirStrike.active && airStrikeDelay <= 0)
        {
            Vector2 dir = UtilsClass.GetMouseWorldPosition() - currentAirStrike.transform.position;
            currentAirStrike.Activate(this, dir.normalized,airStrikeDuration,airStrikeSpd,airStrikeDmgPerSecond);
            airStrikeDelay = 0.25f;

            onAnim = true;
            animator.Play("UseHab");
            weapon = Weapon.principal;

            actualAirStrikeCD = airStrikeCD;
        }

    }

    public override IEnumerator SecondaryShoot()
    {
        if (currentAirStrike != null && !currentAirStrike.active)
        {
            Destroy(currentAirStrike.gameObject);
           
        }
        else if (weapon == Weapon.secondary)
        {
            EquipHability();
        }

        return base.SecondaryShoot();
    }
}
