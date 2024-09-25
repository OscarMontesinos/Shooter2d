using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamePuddle : MonoBehaviour
{
    Flamecaster user;
    float duration;
    float dmgOverTime;
    float slow;
    float tickCounter = 0.1f;

   public List<PjBase> affectedTargets = new List<PjBase>();

    private void Update()
    {
        if (duration > 0)
        {
            duration -= Time.deltaTime;
            if(tickCounter > 0)
            {
                tickCounter -= Time.deltaTime;
            }
            else if (affectedTargets.Count>0)
            {
                foreach (PjBase target in affectedTargets)
                {
                    if (target != null)
                    {
                        target.GetComponent<TakeDamage>().TakeDamage(user, dmgOverTime * 0.1f);
                    }
                }
                 tickCounter = 0.1f;
            }
        }
        else
        {
            Destroy(gameObject);
        }

    }
    public void SetUp(Flamecaster user, float duration, float dmgOverTime, float slow)
    {
        this.user = user;
        this.duration = duration;
        this.dmgOverTime = dmgOverTime;
        this.slow = slow;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Unit") || collision.CompareTag("Player"))
        {
            PjBase target = collision.GetComponent<PjBase>();
            affectedTargets.Add(target);
            target.spd -= slow;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Unit") || collision.CompareTag("Player"))
        {
            PjBase target = collision.GetComponent<PjBase>(); 
            if (target.hp > 0)
            {
                affectedTargets.Remove(target);
                target.spd += slow;
            }
        }
    }
}
