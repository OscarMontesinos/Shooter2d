using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBullet : Projectile
{
    bool destroying;

    public GameObject portal;
    Portal firstPortal;

    List<GameObject> walls = new List<GameObject>();


    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            if (walls.Count == 0)
            {
                firstPortal = Instantiate(portal, transform.position, transform.rotation).GetComponent<Portal>();
                withoutRange = true;
            }
            walls.Add(collision.gameObject);
        }
        else if (collision.CompareTag("EndingWall") || collision.CompareTag("Air")|| collision.CompareTag("ElectronicWall"))
        {
            destroying = true;
            if(firstPortal != null)
            {
                Destroy(firstPortal.gameObject);
            }
            Die();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") && !destroying)
        {
            walls.Remove(collision.gameObject);
            if (walls.Count == 0)
            {
                Portal secondPortal = Instantiate(portal, transform.position, transform.rotation).GetComponent<Portal>();
                secondPortal.linkedPortal = firstPortal;
                firstPortal.linkedPortal = secondPortal;

                secondPortal.user = user.GetComponent<Gate>();
                firstPortal.user = user.GetComponent<Gate>();
                user.GetComponent<Gate>().portals.Add(firstPortal);
                user.GetComponent<Gate>().portals.Add(secondPortal);

                Die();
            }
        }
    }
}
