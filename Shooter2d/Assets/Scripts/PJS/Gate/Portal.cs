using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [HideInInspector]
    public Gate user;
    public Portal linkedPortal;
    List<GameObject> objectMemory = new List<GameObject>();
    public float secondsToForgetObject;
    public LayerMask projectileLayer;
    public LayerMask playerLayer;

    private void Update()
    {
        if (user != null)
        {
            if (user.energy > 0)
            {
                user.energy -= (user.energyDrainedPerPortalOverTime / 2) * Time.deltaTime;
            }
            else if (user.energy < 0)
            {
                Die();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (linkedPortal != null && (collision.CompareTag("Player") || collision.CompareTag("Projectile") && collision.GetComponent<Projectile>().team == user.team) && !objectMemory.Contains(collision.gameObject))
        {
            Teleport(collision.gameObject);
        }
    }

    void Teleport(GameObject objectToTP)
    {
        linkedPortal.objectMemory.Add(objectToTP);
        linkedPortal.StartCoroutine(linkedPortal.ForgetObject(objectToTP));

        Vector3 dir = linkedPortal.transform.position - transform.position;
        dir = objectToTP.transform.position + dir;
        Vector3 dir2 = linkedPortal.transform.position - dir;
        dir = dir + (dir2* 2);
        objectToTP.transform.position = dir;
    }

    IEnumerator ForgetObject(GameObject objectToForget)
    {
        yield return new WaitForSeconds(secondsToForgetObject);
        objectMemory.Remove(objectToForget);
    }

    public void Die()
    {
        StartCoroutine(DieEnum());
    }
    IEnumerator DieEnum()
    {
        yield return null;
        user.portals.Remove(this);
        Destroy(gameObject);
    }
}
