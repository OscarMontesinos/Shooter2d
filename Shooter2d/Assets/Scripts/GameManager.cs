using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    int charaterDisplayed;


    public LayerMask wallLayer;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public LayerMask playerWallLayer;

    public GameObject damageText;

    public GameObject FoV;

    public float ingameSpeed;

    public List<GameObject> waypoints;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Time.timeScale = ingameSpeed;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public static Vector2 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        Vector3 angle = new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        return new Vector2(-angle.x,angle.z);
    }
}
