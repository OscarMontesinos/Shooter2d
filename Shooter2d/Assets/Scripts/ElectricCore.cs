using System.Collections;
using System.Collections.Generic;
using TMPro.SpriteAssetUtilities;
using UnityEngine;
using UnityEngine.UI;

public class ElectricCore : MonoBehaviour
{
    public GameObject aura;
    public Slider hackSlider;
    public bool isOn = true;
    public float timeToHack;
    float hackingTime;
    List<GameObject> hackingObjects = new List<GameObject>();

    public List<SpriteRenderer> energySprites = new List<SpriteRenderer>();
    public Color32 offColor;

    private void Awake()
    {
        hackSlider.maxValue = timeToHack;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("HackingTool"))
        {
            hackingObjects.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("HackingTool"))
        {
            hackingObjects.Remove(collision.gameObject);
        }
    }

    private void Update()
    {
        if (hackingObjects.Count <= 0)
        {
            if (hackingTime > 0)
            {
                hackingTime -= Time.deltaTime;
            }
        }
        else
        {
            hackingTime += Time.deltaTime * hackingObjects.Count;
            if(hackingTime >= timeToHack)
            {
                Hack();
            }
        }
        if (isOn)
        {
            hackSlider.value = hackingTime;
        }
        else
        {
            hackSlider.value = 0;
        }
    }

    void Hack()
    {
        isOn = false;

        foreach (SpriteRenderer sprite in energySprites)
        {
            sprite.color = offColor;
        }
        aura.SetActive(false);
    }
}
