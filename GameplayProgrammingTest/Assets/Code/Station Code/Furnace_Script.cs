using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace_Script : MonoBehaviour
{
    SpriteRenderer sprite;
    public PlayerMovement playerMovementScript;
    public ItemPickup itemScript;
    public Gold_Script goldScript;


    float distance;
    public bool atCapacity = false;
    public bool inRange = false;

    public int selectedElement = 0;
    public int maxCapacity = 3;

    Color32 baseColour = new Color32(255, 255, 255, 255);
    Color32 tintColour = new Color32(75, 199, 143, 255);

    public float vertTransform = 0.2f;

    public List<Gold_Script> furnaceContents = new List<Gold_Script>();

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, playerMovementScript.rb.position);

        displayContents();

        if (distance < 0.5f)
        {
            sprite.color = tintColour;
            inRange = true;
            selectContents(itemScript);
        }
        else
        {
            sprite.color = baseColour;
            inRange = false;
            selectedElement = furnaceContents.Count - 1;
        }



    }


    public void displayContents()
    {
        for(int i = 0; i < furnaceContents.Count; i++)
        {
            if (furnaceContents[i] != null)
            {
                furnaceContents[i].smeltOffset = 0.2f + (vertTransform * i);
                furnaceContents[i].barBG.color = baseColour;
                furnaceContents[i].sprite.color = baseColour;
            }
        }
    }

    public void selectContents(ItemPickup itemScript)
    {
       // Debug.Log(selectedElement);
        if (itemScript.hasItem == false&& furnaceContents.Count > 0) {


            if (Input.GetKeyDown("q")){
                if (selectedElement > 0){
                    selectedElement--;
                }else{
                 selectedElement = (furnaceContents.Count - 1);}
            }

            furnaceContents[selectedElement].barBG.color = new Color32(225, 25, 120, 255);
            furnaceContents[selectedElement].sprite.color = new Color32(225, 25, 120, 255);

            if (Input.GetKeyDown("f")){
                itemScript.hasItem = true;
                deleteContents(selectedElement);
            }  
     
        }
    }

    public void addContents(Gold_Script goldObject)
    {
        if (furnaceContents.Count < maxCapacity)
        {
            //Debug.Log(furnaceContents.Count);
            goldObject.itemScript.hasItem = false;
            goldObject.currentState = new SmeltState();
            goldObject.stateName = "SmeltState";
            furnaceContents.Add(goldObject);
            selectedElement = furnaceContents.Count - 1;
        }

    }

    public void deleteContents(int position)
    {
        atCapacity = false;

        //Debug.Log("deleted ");
        //Debug.Log(position);
        furnaceContents[position].barBG.color = baseColour;
        furnaceContents[position].sprite.color = baseColour;
        furnaceContents[position].currentState = new HeldState();
        furnaceContents[position].stateName = "HeldState";

        furnaceContents.RemoveAt(position);
    }


    public void Reset()
    {
        for(int i = 0; i < furnaceContents.Count; i++)
        {
            furnaceContents[i].destroyed = true;
        }
        furnaceContents.Clear();
    }
    /////////////////////////////RESEARCH UNITY LISTS
    ///////https://learn.unity.com/tutorial/lists-and-dictionaries#63561975edbc2a0cf1ad33b2
}

//furnaceContents.RemoveAt(3); Removes at position
//furnaceContents.count; number of elements
//furnaceContents[3]; gets object at that position
