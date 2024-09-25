using System.Collections;
using System.Collections.Generic;
using TMPro.SpriteAssetUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class FlameBullet : Projectile
{
    public float burnTime;
    public void SetUp(PjBase user, float dmg, float burnTime)
    {
        this.user = user;
        this.burnTime = burnTime;
        this.dmg = dmg;
        team = user.team;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Unit") || collision.CompareTag("Player")) && collision.GetComponent<PjBase>().team != team)
        {
            if(collision.GetComponent<DotDebuff>() && collision.GetComponent<DotDebuff>().user == user)
            {
                collision.GetComponent<DotDebuff>().SetUp(collision.GetComponent<PjBase>(), user, burnTime, dmg);
            }
            else
            {
                DotDebuff dot = collision.AddComponent<DotDebuff>();
                dot.SetUp(collision.GetComponent<PjBase>(), user, burnTime, dmg);
            }
            
            if (!pierce)
            {
                Die();
            }
        }
        else if (collideWalls && (collision.CompareTag("Wall") || collision.CompareTag("EndingWall")))
        {
            Die();
        }
    }

}
