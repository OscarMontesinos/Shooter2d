using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSnapper : Projectile
{
    public bool snapsOnAir;
    public bool snapsOnDying;
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("EndingWall") || (collision.CompareTag("Air") && snapsOnAir) || (collision.CompareTag("ElectronicWall") && team != 0))
        {
            speed = 0;
            withoutRange = true;
        }
    }

    public override void Die()
    {
        if (!snapsOnDying)
        {
            base.Die();
        }
        else
        {
            speed = 0;
            withoutRange = true;
        }
    }
}
