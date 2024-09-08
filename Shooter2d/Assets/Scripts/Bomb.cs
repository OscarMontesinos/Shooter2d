using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class Bomb : Projectile
{
    public float area;
    public float stunTime;
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("EndingWall")|| collision.CompareTag("ElectronicWall"))
        {
            Vector2 normal = collision.ClosestPoint(transform.position) - new Vector2(transform.position.x, transform.position.y);
            Vector2 reflect = Vector2.Reflect(transform.up, normal.normalized);
            transform.up = reflect;
        }
        else if(collision.CompareTag("Player") && user.gameObject == collision.gameObject)
        {
            if(speed == 0)
            {
                user.GetComponent<Shocker>().TakeBomb();
                Destroy(gameObject);
            }
        }
    }

    public override void Die()
    {
        speed = 0;
    }

    public void Detonate()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, area, GameManager.Instance.enemyLayer);
        PjBase enemy;
        foreach (Collider2D enemyColl in enemiesHit)
        {
            enemy = enemyColl.GetComponent<PjBase>();
            enemy.GetComponent<TakeDamage>().TakeDamage(user,dmg);
            enemy.GetComponent<TakeDamage>().Stunn(stunTime);
        }
        Instantiate(particle, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, area);
    }
}
