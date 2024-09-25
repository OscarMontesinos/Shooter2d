using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Flamecaster : PjBase
{
    public float dmgOverTime;
    public float missileDmgOverTime;
    public float dotDuration;
    public GameObject flameMissile;
    public float fuel;
    float currentFuel;
    public float fuelPerBomb;
    public float fuelPerSecond;
    public GameObject flameBomb;
    public GameObject shapeCircle;
    public GameObject shapeBoxWide;
    public GameObject shapeBoxLarge;
    public GameObject circleParticle;
    public GameObject boxParticle;
    public float fireBombMaxRange;
    public float fireBombMaxSpd;
    public float fireBombMinSpd;
    public float fireBombDuration;
    public float fireBombDmgOverTime;
    public float fireBombSlow;
    FireShape habShape;
    enum FireShape
    {
        circle, boxWide, boxLarge
    }
    public override void Awake()
    {
        base.Awake();
        currentFuel = fuel;
    }
    public override void Update()
    {
        base.Update();
        if (ammoText != null)
        {
            habText.text = currentFuel.ToString("F0") + " / " + fuel;
        }
        if (currentFuel < fuel)
        {
            if (weapon != Weapon.secondary)
            {
                currentFuel += fuelPerSecond * Time.deltaTime;
            }
            else
            {
                currentFuel += fuelPerSecond * 0.5f * Time.deltaTime;
            }
        }
    }
    void ChangeHabShape()
    {
        switch (habShape)
        {
            case FireShape.circle:
                habShape = FireShape.boxWide; 
                break;
            case FireShape.boxWide:
                habShape = FireShape.boxLarge; 
                break;
            case FireShape.boxLarge:
                habShape = FireShape.circle; 
                break;
        }

        if (habShape == FireShape.circle)
        {
            circleParticle.SetActive(true);
            boxParticle.SetActive(false);
        }
        else
        {
            circleParticle.SetActive(false);
            boxParticle.SetActive(true);
            if (habShape == FireShape.boxLarge)
            {
                boxParticle.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                boxParticle.transform.localEulerAngles = new Vector3(0, 0, 90);
            }
        }
    }
    public override void Shoot(bool secondary)
    {
        if (!onAnim && !recharging)
        {
            if (weapon == Weapon.principal && fuel > fuelPerSecond)
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
                    ChangeHabShape();
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
                FlameBullet projectile = Instantiate(bullet, shootPoint.transform.position, shootPoint.transform.rotation).GetComponent<FlameBullet>();
                projectile.transform.localEulerAngles = new Vector3(0, 0, projectile.transform.localEulerAngles.z + Random.Range(-detour, detour));
                projectile.SetUp(this, dmgOverTime, dotDuration);
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
        if (shootCD <= 0)
        {
            if (currentAmmo >= ammo / 12)
            {
                shootCD = burstDelay * 8;
                yield return null;
                animator.Play("Shoot");
                currentAmmo -= ammo / 6;
                Projectile projectile = Instantiate(flameMissile, shootPoint.transform.position, pointer.transform.rotation).GetComponent<Projectile>();
                projectile.user = this;
                projectile.team = team;
                projectile.dmg = dmg;

                yield return new WaitForSeconds(burstDelay);

                if (currentAmmo >= ammo / 12)
                {
                currentAmmo -= ammo / 6;
                    Projectile projectile2 = Instantiate(flameMissile, shootPoint.transform.position, pointer.transform.rotation).GetComponent<Projectile>();
                    projectile2.team = team;
                    projectile2.user = this;
                    projectile2.dmg = dmg;
                    projectile2.transform.localEulerAngles = new Vector3(0, 0, projectile2.transform.localEulerAngles.z + -detour * 1.5f);
                }

                yield return new WaitForSeconds(burstDelay);

                if (currentAmmo >= ammo / 12)
                {
                currentAmmo -= ammo / 6;
                    Projectile projectile3 = Instantiate(flameMissile, shootPoint.transform.position, pointer.transform.rotation).GetComponent<Projectile>();
                    projectile3.team = team;
                    projectile3.user = this;
                    projectile3.dmg = dmg;
                    projectile3.transform.localEulerAngles = new Vector3(0, 0, projectile3.transform.localEulerAngles.z + detour*1.5f);
                }
                if (currentAmmo <= 0)
                {
                    currentAmmo = 0;
                    Recharge();
                }
            }
            else
            {
                Recharge();
            }
        }
    }

    public override void EquipHability()
    {

        if (!onAnim)
        {
            
            if (weapon == Weapon.principal && currentFuel >= fuelPerBomb)
            {
                habShape = FireShape.boxLarge; 
            ChangeHabShape();
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
        base.AnimationCallHability();

        currentFuel -= fuelPerBomb;

        FlameBomb bomb = Instantiate(flameBomb, habPoint.transform.position, pointer.transform.rotation).GetComponent<FlameBomb>();
        GameObject shape = null;

        switch (habShape)
        {
            case FireShape.circle:
                shape = shapeCircle;
                break;
            case FireShape.boxWide:
                shape = shapeBoxWide;
                break;
            case FireShape.boxLarge:
                shape = shapeBoxLarge;
                break;
        }

        Vector3 destination = UtilsClass.GetMouseWorldPosition();
        Vector3 dist = destination - transform.position;
        if (dist.magnitude > fireBombMaxRange)
        {
            destination = transform.position + (dist.normalized * fireBombMaxRange);
        }

        bomb.SetUp(this, shape, fireBombMaxSpd,fireBombMinSpd, destination);
    }
}
