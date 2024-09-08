using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject fog;
    public PlayerController playerController;
    public float speed;
    public float rSpeed;
    public float zPos;
    float camPos;
    public float camDistance;
    public float speedZoom;
    public float zoomOut;
    public float zoomIn;
    public Camera cam;
    bool rotateCamera;
    bool beginExpectate;
    // Start is called before the first frame update
    void Awake()
    {
        zoomIn = cam.orthographicSize;
    }

    private void Start()
    {
        zPos = cam.transform.position.z;
        if (playerController != null)
        {
            playerController.cam = transform.GetChild(0).GetComponent<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cam.orthographicSize = zoomOut;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            cam.orthographicSize = zoomIn;
        }

        if (playerController == null )
        {
            cam = FindObjectOfType<Camera>();
        }
        else
        {
            transform.position = new Vector3(playerController.transform.position.x, playerController.transform.position.y, playerController.transform.position.z);
        }

       
    }
}
