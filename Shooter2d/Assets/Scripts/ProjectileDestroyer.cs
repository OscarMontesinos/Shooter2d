using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDestroyer : MonoBehaviour
{
    GameObject objectToDestroy;
    // Start is called before the first frame update
    void Start()
    {
        objectToDestroy = transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("EndingWall") || collision.CompareTag("ElectronicWall"))
        {
            Destroy(objectToDestroy);
        }
    }
}
