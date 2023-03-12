using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Gold : MonoBehaviour
{

    SpriteRenderer sprite;
    public PlayerMovement playerMovementScript;
    public ItemPickup itemScript;
    public GameObject GoldLarge;

    float distance;


 

    Vector3 spawnLocation;

    Color32 baseColour = new Color32(255, 255, 255, 255);
    Color32 tintColour = new Color32(55, 179, 123, 255);

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        GoldLarge.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, playerMovementScript.rb.position);
       // Debug.Log(distance);


        if (distance < 0.5f)
        {
            sprite.color = tintColour;
            if (Input.GetKeyUp("e") && itemScript.hasItem == false)
            {
                GoldLarge.SetActive(true);
                Instantiate(GoldLarge,transform.position, Quaternion.identity);
                GoldLarge.SetActive(false);
            }
        }
        else
        {
            sprite.color = baseColour;
        }




    }
}
