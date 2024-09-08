using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shocker : PjBase
{
    public GameObject bomb;
    public Bomb actualBomb;
    public float bombDmg;
    public float bombStunTime;
    public int maxBombs;
    [HideInInspector]
    public int bombs;
    public float bombCD;
    float actualBombCD;
    // Start is called before the first frame update

    public override void Awake()
    {
        base.Awake();
        actualBombCD = bombCD;
        bombs = maxBombs;
    }
    public override void Update()
    {
        base.Update();
        if (actualBombCD > 0 && bombs < maxBombs)
        {
            actualBombCD -= Time.deltaTime;
        }
        else
        {
            if (bombs < maxBombs)
            {
                bombs++;
                actualBombCD = bombCD;
            }
        }
        if (ammoText != null)
        {
            habText.text = actualBombCD.ToString("F1") + " - " + bombs + " / " + maxBombs;
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
        if (actualBomb != null)
        {
            actualBomb.Detonate();
        }
        return base.SecondaryShoot();
    }

    public override void EquipHability()
    {
        
        if (!onAnim)
        {
            if (weapon == Weapon.principal && actualBomb == null && bombs > 0)
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
            else if (actualBomb != null)
            {
                actualBomb.Detonate();
            }
        }
    }

    public override void UseHability()
    {
        if (!onAnim)
        {
            bombs--;
        }
        base.UseHability();
        
    }

    public override void AnimationCallHability()
    {
        base.AnimationCallHability();
        actualBomb = Instantiate(bomb, habPoint.transform.position, pointer.transform.rotation).GetComponent<Bomb>();
        actualBomb.dmg = bombDmg;
        actualBomb.user = this;
        actualBomb.stunTime = bombStunTime;
        actualBomb.team = team;
    }


    public void TakeBomb()
    {
        if (bombs < 3)
        {
            bombs++;
            if(bombs == 3)
            {
                actualBombCD = bombCD;
            }
        }
    }

}
