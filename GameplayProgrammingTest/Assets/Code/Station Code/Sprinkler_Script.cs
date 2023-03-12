using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkler_Script : MonoBehaviour
{
    SpriteRenderer sprite;

    public GameObject progressBar;
    public GameObject barBackground;
    public SpriteRenderer barBG;
    public SpriteRenderer progBar;


    public PlayerMovement playerMovementScript;
    public ItemPickup itemScript;
    public Gold_Script goldScript;
    float distance;

    public bool atCapacity = false;
    public bool inRange = false;
    public float sprinkNum = 0.0f;
    public float sprinkSpeed = 0.5f;
    public bool complete = false;

    Color32 baseColour = new Color32(255, 255, 255, 255);
    Color32 tintColour = new Color32(75, 199, 143, 255);

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        progressBar = this.transform.GetChild(0).gameObject;
        barBackground = this.transform.GetChild(1).gameObject;

        barBG = barBackground.GetComponent<SpriteRenderer>();
        progBar = progressBar.GetComponent<SpriteRenderer>();
        progBar.enabled = false;
        barBG.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, playerMovementScript.rb.position);

        if (distance < 0.5f)
        {
            sprite.color = tintColour;
            inRange = true;
        }
        else
        {
            sprite.color = baseColour;
            inRange = false;
        }

        progBar.enabled = false;
        barBG.enabled = false;
    }

    public void Sprinkle(Gold_Script goldObject)
    {
        if (goldObject.smeltNum >= 2.5f && goldObject.subStateName == "GoldBar")
        {
            progBar.enabled = true;
            barBG.enabled = true;

            float temp = 0f + (sprinkNum - 0f) * (1f - 0f) / (10f - 0f);
            progBar.transform.localScale = new Vector2(temp, 0.5f);


            if (Input.GetKeyDown("q"))
            {
                sprinkNum += 1;
            }
            else
            {
                sprinkNum -= sprinkSpeed * Time.deltaTime;
                if (sprinkNum < 0) { sprinkNum = 0; }
            }
            if (sprinkNum > 10)
            {
                complete = true;
                goldObject.barBG.enabled = false;
                goldObject.progBar.enabled = false;
                goldObject.currentSubState.transitions(goldObject, 1);
            }
        }
        else
        {
            sprinkNum = 0;
            progBar.enabled = false;
            barBG.enabled = false;
        }
    }


    public void Reset()
    {
        atCapacity = false;
        complete = false;
        sprinkNum = 0;
    }
}
