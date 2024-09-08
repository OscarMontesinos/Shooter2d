using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Barrier : MonoBehaviour
{
    public Slider hpSlider;
    public Slider secondaryHpSlider;
    public int team;
    public float mHp;
    float hp;
    public bool unbreakable;

    private void Awake()
    {
        hp = mHp;
        hpSlider.maxValue = mHp;
        if (secondaryHpSlider != null)
        {
            secondaryHpSlider.maxValue = mHp;
        }
    }
    public virtual void Update()
    {
        hpSlider.value = hp;
        if(secondaryHpSlider != null)
        {
            secondaryHpSlider.value = hp;
        }
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && collision.gameObject.GetComponent<Projectile>().team != team)
        {
            if (!unbreakable)
            {
                Projectile bullet = collision.gameObject.GetComponent<Projectile>();
                hp -= (bullet.dmg * bullet.dmgMultiplierToBarriers);
                if (hp <= 0)
                {
                    Die();
                }
            }
            Destroy(collision.gameObject);
        }

    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
