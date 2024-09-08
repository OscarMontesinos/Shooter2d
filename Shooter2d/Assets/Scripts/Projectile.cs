using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [HideInInspector]
    public float dist = 0;
    [HideInInspector]
    public PjBase user;
    [HideInInspector]
    public float dmg;
    public float dmgMultiplierToBarriers = 1;
    [HideInInspector]
    public int team;
    public float speed;
    public float spdOverTime;
    public float range;
    [HideInInspector]
    public  Rigidbody2D _rigidbody;
    [HideInInspector]
    public Vector2 startPos;
    public bool collideWalls;
    public bool pierce;
    public GameObject particle;
    public bool withoutRange;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    public virtual void Update()
    {
        speed += spdOverTime * Time.deltaTime;
    }
    public virtual void FixedUpdate()
    {
        /*Vector2 dir = transform.up;
        _rigidbody.velocity = dir.normalized * speed;
        if (!withoutRange)
        {
            Vector2 dist = startPos - new Vector2(transform.position.x, transform.position.y);
            if (dist.magnitude > range)
            {
                Die();
            }
        }*/
        Vector2 dir = transform.up;
        _rigidbody.velocity = dir.normalized * speed;
        if (!withoutRange)
        {
            dist += speed * Time.deltaTime;
            if (dist > range)
            {
                Die();
            }
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (dmg > 0 && (collision.CompareTag("Unit") || collision.CompareTag("Player")) && collision.GetComponent<PjBase>().team != team)
        {
            collision.GetComponent<TakeDamage>().TakeDamage(user,dmg);
            if (!pierce)
            {
            Die();
            }
        }
        else if(collideWalls && (collision.CompareTag("Wall") || collision.CompareTag("EndingWall")))
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if(particle != null)
        {
            particle.transform.parent = null;
        }
        Destroy(gameObject);
    }
}
