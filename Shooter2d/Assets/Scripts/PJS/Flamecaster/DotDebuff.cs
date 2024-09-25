using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotDebuff : MonoBehaviour
{
    PjBase target;
    [HideInInspector]
    public PjBase user;
    public float duration;
    public float dmgPerSecond;
    float tickCounter = 0.1f;
    public void SetUp(PjBase target, PjBase user, float duration, float dmgPerSecond)
    {
        this.target = target;
        this.user = user;
        this.duration = duration;
        this.dmgPerSecond = dmgPerSecond;
    }

    private void Update()
    {
        if(duration > 0)
        {
            duration-=Time.deltaTime;
            tickCounter -= Time.deltaTime;
            if(tickCounter <= 0)
            {
                tickCounter = 0.1f;
                target.GetComponent<TakeDamage>().TakeDamage(user, dmgPerSecond*tickCounter);
            }
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(this);
    }
}
