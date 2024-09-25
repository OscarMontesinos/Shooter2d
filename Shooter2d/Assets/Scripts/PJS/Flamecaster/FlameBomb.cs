using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBomb : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    Flamecaster user;
    GameObject shape;
    float maxSpd;
    float minSpd;
    Vector2 targetPosition;
    public void SetUp(Flamecaster user, GameObject shape, float maxSpd,float minSpd, Vector2 targetPosition)
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        this.user = user;
        this.shape = shape;
        this.maxSpd = maxSpd;
        this.minSpd = minSpd;
        this.targetPosition = targetPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("EndingWall") || collision.CompareTag("ElectronicWall"))
        {
            Explode();
        }
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
        Vector2 dir = targetPosition - new Vector2 (transform.position.x, transform.position.y);
        _rigidbody.velocity = dir.normalized * Mathf.Lerp(minSpd, maxSpd, dir.magnitude / user.fireBombMaxRange);
        if(dir.magnitude < 0.5f)
        {
            Explode();
        }
           
    }

    void Explode()
    {
        FlamePuddle puddle = Instantiate(shape, transform.position, transform.rotation).GetComponent<FlamePuddle>();
        puddle.SetUp(user, user.fireBombDuration, user.fireBombDmgOverTime, user.fireBombSlow);
        Destroy(gameObject);
    }
}
