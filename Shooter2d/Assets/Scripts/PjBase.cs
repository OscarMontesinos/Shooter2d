using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PjBase : MonoBehaviour, TakeDamage
{
    PlayerController controller;
    Rigidbody2D rb;
    public GameObject pointer;
    public GameObject shootPoint;
    public GameObject habPoint;
    [HideInInspector]
    public Animator animator;

    public Weapon weapon;
    public GameObject bullet;
    public GameObject shootVFX;
    public float dmg;
    public float shootRate;
    public int shootBurst;
    public float burstDelay;
    [HideInInspector]
    public float shootCD;
    public float detour;
    public float ammo;

    [HideInInspector]
    public float currentAmmo;
    [HideInInspector]
    public bool recharging;
    [HideInInspector]
    public bool onAnim;

    public TextMeshProUGUI hpText;
    public Slider hpBar;
    public Slider stunBar;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI habText;

    public float stunTime;

    public int team;

    public bool dashing;
    public bool casting;
    public bool ignoreSoftCastDebuff;

    public float spd;
    public float mHp;
    [HideInInspector]
    public float hp;
    public float regenTime = 5;
    float timeOutOfCombat = 0;
    public float hpRegenPercentagePerSecond = 17.5f;

    public enum Weapon
    {
        principal, secondary, none
    }


    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        if (controller)
        {
            rb = controller.rb;
        }
        else
        {
            rb = GetComponent<Rigidbody2D>();
        }
        currentAmmo = ammo;
        hp = mHp;
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(stunTime> 0)
        {
            animator.Play("Idle");
            stunTime -= Time.deltaTime;
        }

        if (shootCD > 0)
        {
            shootCD -= Time.deltaTime;
        }
        if (habText != null)
        {
            ammoText.text = currentAmmo + " / " + ammo;

            hpText.text = hp.ToString("F0");
            hpBar.maxValue = mHp;
        }
        else
        {
            ammoText.text = currentAmmo.ToString();
        }
        hpBar.maxValue = mHp;
        hpBar.value = hp;

        if (stunBar != null)
        {
            if (stunBar.maxValue < stunTime)
            {
                stunBar.maxValue = stunTime;
            }
            stunBar.value = stunTime;
        }


        if (CompareTag("Player"))
        {
            if(timeOutOfCombat <= 0)
            {
                if(hp < mHp)
                {
                    hp += (mHp * (hpRegenPercentagePerSecond / 100)) * Time.deltaTime;
                }
            }
            else
            {
                timeOutOfCombat -= Time.deltaTime;
            }
        }
    }

    public virtual void Shoot(bool secondary)
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
                    EquipHability();
                }
            }
        }
    }

    public virtual IEnumerator NormalShoot()
    {
        yield return null;
    }
    public virtual IEnumerator SecondaryShoot()
    {
        yield return null;
    }
    public virtual void Recharge()
    {
        if (currentAmmo < ammo)
        {
            onAnim = false;
            weapon = Weapon.principal;
            animator.Play("Recharge");
            recharging = true;
        }
    }

    public virtual void AnimationCallRecharge()
    {
        currentAmmo = ammo;
        recharging = false;
    }
    public virtual void AnimationCallStopAnim()
    {
        onAnim = false;
        recharging = false;
    }
    public virtual void AnimationCallHability()
    {
        recharging = false;

    }

    public virtual void UseHability()
    {
        if (!onAnim)
        {
            onAnim = true;
            animator.Play("UseHab");
            weapon = Weapon.principal;
        }
    }
    public virtual void EquipHability()
    {
        if (!onAnim)
        {
            if (weapon == Weapon.principal)
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

    public void TakeDamage(PjBase user, float value)
    {
        hp -= value;
        if (hp <= 0)
        {
            Die(user);
        }


        if (CompareTag("Player"))
        {
            timeOutOfCombat = regenTime;
        }
    }

    public virtual void Stunn(float stunnTime)
    {
        stunTime = stunnTime;
        if(stunBar != null)
        {
            stunBar.maxValue = stunTime;
        }
    }

    public virtual void Die(PjBase killer)
    {
        Destroy(gameObject);
    }
}
