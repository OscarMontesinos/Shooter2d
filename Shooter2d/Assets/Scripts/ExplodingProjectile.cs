using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingProjectile : Projectile
{
    public float area;
    public GameObject explodingParticle;
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (dmg > 0 && (collision.CompareTag("Unit") || collision.CompareTag("Player")) && collision.GetComponent<PjBase>().team != team)
        {
            if (!pierce)
            {
                Explode();
            }
        }
        else if (collideWalls && (collision.CompareTag("Wall") || collision.CompareTag("EndingWall")))
        {
            Explode();
        }
    }
    public void Explode()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, area, GameManager.Instance.enemyLayer);
        PjBase enemy;
        foreach (Collider2D enemyColl in enemiesHit)
        {
            enemy = enemyColl.GetComponent<PjBase>();
            if (enemy.GetComponent<DotDebuff>() && enemy.GetComponent<DotDebuff>().user == user)
            {
                enemy.GetComponent<DotDebuff>().SetUp(enemy.GetComponent<PjBase>(), user, user.GetComponent<Flamecaster>().dotDuration, user.GetComponent<Flamecaster>().missileDmgOverTime);
            }
            else
            {
                DotDebuff dot = enemy.gameObject.AddComponent<DotDebuff>();
                dot.SetUp(enemy, user, user.GetComponent<Flamecaster>().dotDuration, user.GetComponent<Flamecaster>().missileDmgOverTime);
            }

            enemy.GetComponent<TakeDamage>().TakeDamage(user, dmg);
        }
        Instantiate(explodingParticle, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, area);
    }
}
