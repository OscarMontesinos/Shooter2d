using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{   
    [HideInInspector]
    public Vector2 inputMov;
    public Camera cam;
    public LayerMask wallLayer;
    float speedDecrease = 5;
    [HideInInspector]
    public Rigidbody2D rb;
    public PjBase character;
    bool lockPointer;
    public GameObject targetBoss;
    float maxViewportDistance = 16;

    public void LockPointer(bool value)
    {
        lockPointer = value;
    }

    void Awake()
    {
        rb = character.GetComponent<Rigidbody2D>();
        cam = FindObjectOfType<Camera>();
        cam.transform.parent.GetComponent<CameraController>().playerController = this;
        //character.hpBar.gameObject.SetActive(false);
        //character.stunnBar.gameObject.SetActive(false);
    }

    void Start()
    {
        Instantiate(GameManager.Instance.FoV, transform).GetComponent<FieldOfView>().team = character.team;

        StartCoroutine(PostStart());    
    }
    
    IEnumerator PostStart()
    {
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if(cam != Camera.main)
        {
            cam = Camera.main;
        }

        if (character != null)
        {
            transform.position = character.transform.position;

            HandleHabilities();

            if (character.stunTime <= 0)
            {
                HandlePointer();

                HandleMovement();
            }

            HandleCamera();
        }



        //if (unit.pointer != null) { unit.pointer.transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - cam.transform.position.z)); }




        /*foreach (Unit unit_ in unit.manager.units)
        {
            if (unit_ != null)
            {
                if (unit_.team != unit.team)
                {
                    var dir = unit_.transform.position - transform.position;
                    if (!Physics2D.Raycast(transform.position, dir, dir.magnitude, wallLayer))
                    {
                        unit_.oculto = false;
                    }
                    else
                    {
                        unit_.oculto = true;
                    }
                }
            }
        }*/


        /*if (unit.aim)
        {
            transform.GetChild(0).up = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - cam.transform.position.z)) - transform.position;
        }
        transform.GetChild(0).eulerAngles = new Vector3(0, 0, transform.GetChild(0).eulerAngles.z);
        unit.pointer.transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - cam.transform.position.z));*/
    }


    public virtual void FixedUpdate()
    {
        if (character != null)
        {
            if (!character.dashing)
            {
                if (character.stunTime <= 0)
                {
                    if (!character.casting || character.ignoreSoftCastDebuff)
                    {
                        rb.velocity = (transform.right * inputMov.x + transform.up * inputMov.y) * character.spd;
                    }
                    else
                    {
                        rb.velocity = (transform.right * inputMov.x + transform.up * inputMov.y) * (character.spd / 1.5f);
                    }
                }
                else
                {
                    rb.velocity = Vector3.zero;
                }
            }
        }
    }

    void HandlePointer()
    {
        if (!lockPointer && Time.timeScale != 0)
        {
            Vector2 dir = UtilsClass.GetMouseWorldPosition() - character.pointer.transform.position;
            character.pointer.transform.up = dir;
        }
    }
    void HandleCamera()
    {
        /* cam.transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);
         if (targetBoss == null)
         {
             Camera.main.orthographicSize = maxViewportDistance;
         }
         else
         {
             Vector3 dist = targetBoss.transform.position - character.transform.position;
             dist = character.transform.position + (dist * 0.5f);
             cam.transform.position = new Vector3(dist.x, dist.y , cam.transform.position.z);

             dist = targetBoss.transform.position - character.transform.position;
             if (dist.magnitude > maxViewportDistance)
             {
                 Camera.main.orthographicSize = dist.magnitude;
             }
         }*/
    }

    void HandleHabilities()
    {
        if (Input.GetMouseButton(0))
        {
            character.Shoot(false);
        }
        if (Input.GetMouseButtonDown(1))
        {
            character.Shoot(true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            character.EquipHability();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            character.Recharge();
        }
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            inputMov.x = Input.GetAxisRaw("Horizontal");
        }
        else if (inputMov.x != 0)
        {
            if (inputMov.x <= 0.2f && inputMov.x >= -0.2f /*|| unit.casting*/)
            {
                inputMov.x = 0;
            }
            if (inputMov.x != 0 && inputMov.x > 0)
            {
                inputMov.x -= speedDecrease * Time.deltaTime;
            }
            if (inputMov.x != 0 && inputMov.x < 0)
            {
                inputMov.x += speedDecrease * Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            inputMov.y = Input.GetAxisRaw("Vertical");
        }
        else if (inputMov.y != 0)
        {
            if (inputMov.y <= 0.2f && inputMov.y >= -0.2f /*|| unit.casting*/)
            {
                inputMov.y = 0;
            }
            if (inputMov.y != 0 && inputMov.y > 0)
            {
                inputMov.y -= speedDecrease * Time.deltaTime;
            }
            if (inputMov.y != 0 && inputMov.y < 0)
            {
                inputMov.y += speedDecrease * Time.deltaTime;
            }
        }

    }
}
