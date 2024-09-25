using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusSting : ProjectileSnapper
{
    bool snapped;
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (!snapped && collision.CompareTag("Unit") && collision.GetComponent<Enemy>().hackable)
        {
            snapped = true;
            Vector2 dir = collision.transform.position - transform.position;
            transform.up = dir;

            user.GetComponent<Virus>().HackEnemy(collision.GetComponent<Enemy>());
            transform.parent = collision.transform;
            speed = 0;
            withoutRange = true;
        }
    }

}
