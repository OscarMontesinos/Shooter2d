using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirStrike : MonoBehaviour
{
    public PjBase user;
    public bool active;
    public GameObject sprite;
    public float area;
    public float duration;
    public Vector2 direction;
    public float spd;
    public float dmgPerSecond;
    float tickCounter = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Activate(PjBase user, Vector2 direction, float duration, float spd, float dmgPerSecond)
    {
        this.user = user;
        active = true;
        this.direction = direction;
        this.spd = spd;
        this.duration = duration;
        this.dmgPerSecond = dmgPerSecond;
    }
    public void ChangeDirection(Vector2 direction)
    {
        this.direction = direction;
    }
    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            transform.Translate(direction * spd * Time.deltaTime);
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                Destroy(gameObject);
            }

            tickCounter -= Time.deltaTime;
            if (tickCounter <= 0)
            {
                Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, area, GameManager.Instance.enemyLayer);
                PjBase enemy;
                foreach (Collider2D enemyColl in enemiesHit)
                {
                    enemy = enemyColl.GetComponent<PjBase>();
                    enemy.GetComponent<TakeDamage>().TakeDamage(user, dmgPerSecond*0.25f);
                }
                tickCounter = 0.25f;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, area);
        sprite.transform.localScale = new Vector3(area*2,area*2,area*2);
    }
}
