using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusSting : ProjectileSnapper
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Unit") && collision.GetComponent<Enemy>().hackable)
        {
            user.GetComponent<Virus>().HackEnemy(collision.GetComponent<Enemy>());
            transform.parent = collision.transform;
            speed = 0;
            withoutRange = true;
        }
    }

}
