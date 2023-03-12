using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoldState
{
    public virtual void position(Gold_Script thisObject) { }
    public virtual void smeltProgression(Gold_Script thisObject) { }
    public virtual void transitions(Gold_Script thisObject) { }
}

///------------------------

public class FloorState : GoldState
{
    public override void position(Gold_Script thisObject)
    {
        thisObject.sprite.enabled = true;
        thisObject.sprite.sortingOrder = 3;

        if (thisObject.distance < thisObject.threshold && thisObject.itemScript.pickUpDelay == false && thisObject.itemScript.hasItem == false)
        {
            thisObject.transform.right = thisObject.playerMovementScript.rb.transform.position - thisObject.transform.position;
            thisObject.rb.velocity = thisObject.transform.right * thisObject.speed;
            thisObject.transform.right = Vector3.zero;
        }
        else
        { thisObject.rb.velocity = thisObject.transform.right * 0.0f; }

    }
    public override void smeltProgression(Gold_Script thisObject)
    {
        //ProgressBar Decay
        thisObject.smeltNum -= thisObject.floorDecaySpeed * Time.deltaTime;
        if (thisObject.smeltNum < 0) { thisObject.smeltNum = 0; }
        //ProgressBar Scale
        float temp = 0f + (thisObject.smeltNum - 0f) * (0.75f - 0f) / (10f - 0f);
        thisObject.progBar.transform.localScale = new Vector2(temp, 0.5f);
        //ProgressBar Colour
        temp = 0f + (thisObject.smeltNum - 0f) * (1f - 0f) / (10f - 0f);
        thisObject.progBar.color = new Color(1f - temp, temp, 0f);
        //ProgressBar Positioning
        float xOffset = 0.0f;
        float yOffset = 0.2f;
        thisObject.progBar.transform.localPosition = new Vector2(xOffset - 0.2f + 0.2f * temp, yOffset);
    }
    public override void transitions(Gold_Script thisObject)
    {
        //PickUp Object
        if (thisObject.distance < 0.15 && thisObject.itemScript.pickUpDelay == false && thisObject.itemScript.hasItem == false)
        {
            thisObject.animator.SetBool("Maximize", false);
            thisObject.itemScript.hasItem = true;
            thisObject.stateName = "HeldState";
            thisObject.currentState = new HeldState();
        }
    }
}

public class HeldState : GoldState
{
    public override void position(Gold_Script thisObject)
    {
        thisObject.sprite.enabled = true;
        thisObject.sprite.sortingOrder = 10;
        thisObject.transform.localScale = new Vector3(1, 1, 1);

        Vector3 goldPos = thisObject.playerMovementScript.rb.position;
        if (thisObject.playerMovementScript.direction)
        { goldPos.x += 0.35f; }
        else
        { goldPos.x -= 0.35f; }

        goldPos.y -= 0.1f;
        thisObject.transform.position = goldPos;
    }
    public override void smeltProgression(Gold_Script thisObject)
    {
        //ProgressBar Decay
        thisObject.smeltNum -= thisObject.heldDecaySpeed * Time.deltaTime;
        if (thisObject.smeltNum < 0) { thisObject.smeltNum = 0; }
        //ProgressBar Scale
        float temp = 0f + (thisObject.smeltNum - 0f) * (0.75f - 0f) / (10f - 0f);
        thisObject.progBar.transform.localScale = new Vector2(temp, 0.5f);
        //ProgressBar Colour
        temp = 0f + (thisObject.smeltNum - 0f) * (1f - 0f) / (10f - 0f);
        thisObject.progBar.color = new Color(1f - temp, temp, 0f);
        //ProgressBar Postioning
        float xOffset = 0.0f;
        float yOffset = 0.2f;
        thisObject.progBar.transform.localPosition = new Vector2(xOffset - 0.2f + 0.2f * temp, yOffset);
        thisObject.barBG.transform.localPosition = new Vector2(0, yOffset);
    }
    public override void transitions(Gold_Script thisObject)
    {
        //Drop Object
        if (Input.GetKeyDown("space") && thisObject.itemScript.hasItem)
        {
            thisObject.animator.SetBool("Maximize", true);
            thisObject.itemScript.hasItem = false;
            thisObject.itemScript.itemDelay();
            thisObject.stateName = "FloorState";
            thisObject.currentState = new FloorState();
        }

        //Bin Object
        if (thisObject.binScript.dispose)
        {
            thisObject.destroyed = true;
        }


        //Submit Object (Order)
        if (Input.GetKeyDown("e") && thisObject.orderScript.inRange)
        {
            thisObject.orderScript.submitOrder(thisObject);
        }

        //Smelt Object
        if (Input.GetKeyDown("e") && thisObject.furnaceScript.inRange)
        {
            if (thisObject.subStateName == "GoldOre" || thisObject.subStateName == "GoldBar")
            {
                thisObject.furnaceScript.addContents(thisObject);
            }
        }

        //Forge Object
        if (Input.GetKeyDown("e") && thisObject.anvilScript.inRange && thisObject.anvilScript.atCapacity == false && thisObject.subStateName == "GoldOre")
        {
            thisObject.itemScript.hasItem = false;
            thisObject.anvilScript.atCapacity = true;
            thisObject.stateName = "ForgeState";
            thisObject.currentState = new ForgeState();
        }

        //Brew Object
        if (Input.GetKeyDown("e") && thisObject.brewScript.inRange && thisObject.brewScript.atCapacity == false && thisObject.subStateName == "GoldOre" && thisObject.smeltNum >= 7.5f)
        {
            thisObject.itemScript.hasItem = false;
            thisObject.brewScript.atCapacity = true;
            thisObject.stateName = "BrewState";
            thisObject.currentState = new BrewState();
        }

        //Press Object
        if (Input.GetKeyDown("e") && thisObject.pressScript.inRange && thisObject.pressScript.atCapacity == false && thisObject.subStateName == "GoldBar")
        {
            thisObject.itemScript.hasItem = false;
            thisObject.pressScript.atCapacity = true;
            thisObject.stateName = "PressState";
            thisObject.currentState = new PressState();
        }

        //Sprinkle Object
        if (Input.GetKeyDown("e") && thisObject.sprinkScript.inRange && thisObject.sprinkScript.atCapacity == false && thisObject.subStateName == "GoldBar")
        {
            thisObject.itemScript.hasItem = false;
            thisObject.sprinkScript.atCapacity = true;
            thisObject.stateName = "SprinkleState";
            thisObject.currentState = new SprinkleState();
        }


    }
}

public class SmeltState : GoldState
{
    public override void position(Gold_Script thisObject)
    {
        thisObject.sprite.enabled = true;
        thisObject.sprite.sortingOrder = 10;
        thisObject.transform.localPosition = thisObject.furnaceScript.transform.position;
    }
    public override void smeltProgression(Gold_Script thisObject)
    {
        //ProgressBar Progression
        thisObject.smeltNum += thisObject.smeltSpeed * Time.deltaTime;
        if (thisObject.smeltNum > 10) { thisObject.smeltNum = 10; }
        //ProgressBar Scale
        float temp = 0f + (thisObject.smeltNum - 0f) * (0.75f - 0f) / (10f - 0f);
        thisObject.progBar.transform.localScale = new Vector2(temp, 0.5f);
        //ProgressBar Colour
        temp = 0f + (thisObject.smeltNum - 0f) * (1f - 0f) / (10f - 0f);
        thisObject.progBar.color = new Color(1f - temp, temp, 0f);

        //ProgressBar Positioning
        float xOffset = 0.5f;
        float yOffset = 0.0f;
        //Debug.Log(thisObject.smeltOffset);

        thisObject.transform.localPosition += new Vector3(-0.25f, thisObject.smeltOffset, 0);
        thisObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);

        thisObject.progBar.transform.localPosition = new Vector2(xOffset - 0.2f + 0.2f * temp, yOffset);
        thisObject.barBG.transform.localPosition = new Vector2(xOffset, yOffset);
    }
    public override void transitions(Gold_Script thisObject)
    {
    }

}

public class ForgeState : GoldState
{
    public override void position(Gold_Script thisObject)
    {
        thisObject.sprite.enabled = true;
        thisObject.sprite.sortingOrder = 3;

        Vector2 goldPos = thisObject.anvilScript.transform.position;
        goldPos.y += 0.45f;

        thisObject.transform.position = goldPos;
    }
    public override void smeltProgression(Gold_Script thisObject)
    {
        //ProgressBar Decay
        thisObject.smeltNum -= thisObject.heldDecaySpeed * Time.deltaTime;
        if (thisObject.smeltNum < 0) { thisObject.smeltNum = 0; }
        //ProgressBar Scale
        float temp = 0f + (thisObject.smeltNum - 0f) * (0.75f - 0f) / (10f - 0f);
        thisObject.progBar.transform.localScale = new Vector2(temp, 0.5f);
        //ProgressBar Colour
        temp = 0f + (thisObject.smeltNum - 0f) * (1f - 0f) / (10f - 0f);
        thisObject.progBar.color = new Color(1f - temp, temp, 0f);
        //ProgressBar Positioning
        float xOffset = 0.0f;
        float yOffset = 0.2f;
        thisObject.progBar.transform.localPosition = new Vector2(xOffset - 0.2f + 0.2f * temp, yOffset);

        /////////---------

        //Display Forge Threshold
        if (thisObject.subStateName == "GoldOre")
        {
            thisObject.progMark.enabled = true;
            thisObject.progMark.transform.localPosition = new Vector2(0f, 0.2f);
        }
        else
        { thisObject.progMark.enabled = false; }

    }
    public override void transitions(Gold_Script thisObject)
    {
        //PickUp Object
        if (Input.GetKeyDown("e") && thisObject.anvilScript.inRange && thisObject.itemScript.hasItem == false)
        {
            thisObject.progMark.enabled = false;
            thisObject.itemScript.hasItem = true;
            thisObject.anvilScript.Reset();
            thisObject.stateName = "HeldState";
            thisObject.currentState = new HeldState();
        }

        if (thisObject.anvilScript.complete)
        {
            thisObject.anvilScript.Reset();
            thisObject.stateName = "FloorState";
            thisObject.currentState = new FloorState();
        }

        //Trigger Forging
        if (thisObject.anvilScript.inRange)
        { thisObject.anvilScript.Forge(thisObject); }
    }
}

public class BrewState : GoldState
{
    public override void position(Gold_Script thisObject)
    {
        thisObject.barBG.enabled = false;
        thisObject.progBar.enabled = false;
        thisObject.sprite.enabled = false;
        thisObject.sprite.sortingOrder = 3;

        Vector2 goldPos = thisObject.brewScript.transform.position;
        goldPos.y -= 1f;

        thisObject.transform.position = goldPos;

    }
    public override void smeltProgression(Gold_Script thisObject)
    {
        //ProgressBar Decay
        thisObject.smeltNum -= thisObject.heldDecaySpeed * Time.deltaTime;
        if (thisObject.smeltNum < 0) { thisObject.smeltNum = 0; }
        //ProgressBar Scale
        float temp = 0f + (thisObject.smeltNum - 0f) * (0.75f - 0f) / (10f - 0f);
        thisObject.progBar.transform.localScale = new Vector2(temp, 0.5f);
        //ProgressBar Colour
        temp = 0f + (thisObject.smeltNum - 0f) * (1f - 0f) / (10f - 0f);
        thisObject.progBar.color = new Color(1f - temp, temp, 0f);
        //ProgressBar Positioning
        thisObject.progBar.transform.localPosition = new Vector2(-0.2f + 0.2f * temp, 0.2f);
    }
    public override void transitions(Gold_Script thisObject)
    {
        if (thisObject.brewScript.complete)
        {
            thisObject.brewScript.Reset();
            thisObject.stateName = "FloorState";
            thisObject.currentState = new FloorState();
        }

        //Trigger Brewing
        thisObject.brewScript.Brew(thisObject);
    }
}

public class PressState : GoldState
{
    public override void position(Gold_Script thisObject)
    {
        thisObject.sprite.enabled = false;
        thisObject.sprite.sortingOrder = 3;
        thisObject.transform.position = thisObject.pressScript.transform.position;
    }
    public override void smeltProgression(Gold_Script thisObject)
    {
        //ProgressBar Decay
        thisObject.smeltNum -= thisObject.heldDecaySpeed * Time.deltaTime;
        if (thisObject.smeltNum < 0) { thisObject.smeltNum = 0; }
        //ProgressBar Scale
        float temp = 0f + (thisObject.smeltNum - 0f) * (0.75f - 0f) / (10f - 0f);
        thisObject.progBar.transform.localScale = new Vector2(temp, 0.5f);
        //ProgressBar Colour
        temp = 0f + (thisObject.smeltNum - 0f) * (1f - 0f) / (10f - 0f);
        thisObject.progBar.color = new Color(1f - temp, temp, 0f);
        //ProgressBar Positioning
        float xOffset = 0.0f;
        float yOffset = 0.2f;
        thisObject.progBar.transform.localPosition = new Vector2(xOffset - 0.2f + 0.2f * temp, yOffset);

        /////////---------

        //Display Forge Threshold
        if (thisObject.subStateName == "GoldBar")
        {
            thisObject.progMark.enabled = true;
            thisObject.progMark.transform.localPosition = new Vector2(-0.1f, 0.2f);
        }
        else
        { thisObject.progMark.enabled = false; }

    }
    public override void transitions(Gold_Script thisObject)
    {
        //PickUp Object
        if (Input.GetKeyDown("e") && thisObject.pressScript.inRange && thisObject.itemScript.hasItem == false)
        {
            thisObject.itemScript.hasItem = true;
            thisObject.pressScript.Reset();
            thisObject.stateName = "HeldState";
            thisObject.currentState = new HeldState();
        }

        //Drop Object Once Complete
        if (thisObject.pressScript.complete)
        {
            thisObject.pressScript.Reset();
            thisObject.stateName = "FloorState";
            thisObject.currentState = new FloorState();
        }

        //Trigger Forging
        if (thisObject.pressScript.inRange)
        { thisObject.pressScript.Press(thisObject); }
    }
}

public class SprinkleState : GoldState
{
    public override void position(Gold_Script thisObject)
    {
        thisObject.sprite.enabled = false;
        thisObject.sprite.sortingOrder = 3;
        thisObject.transform.position = thisObject.sprinkScript.transform.position;
    }
    public override void smeltProgression(Gold_Script thisObject)
    {
        //ProgressBar Decay
        thisObject.smeltNum -= thisObject.heldDecaySpeed * Time.deltaTime;
        if (thisObject.smeltNum < 0) { thisObject.smeltNum = 0; }
        //ProgressBar Scale
        float temp = 0f + (thisObject.smeltNum - 0f) * (0.75f - 0f) / (10f - 0f);
        thisObject.progBar.transform.localScale = new Vector2(temp, 0.5f);
        //ProgressBar Colour
        temp = 0f + (thisObject.smeltNum - 0f) * (1f - 0f) / (10f - 0f);
        thisObject.progBar.color = new Color(1f - temp, temp, 0f);
        //ProgressBar Positioning
        float xOffset = 0.0f;
        float yOffset = 0.2f;
        thisObject.progBar.transform.localPosition = new Vector2(xOffset - 0.2f + 0.2f * temp, yOffset);

        /////////---------

        //Display Forge Threshold
        if (thisObject.subStateName == "GoldBar")
        {
            thisObject.progMark.enabled = true;
            thisObject.progMark.transform.localPosition = new Vector2(-0.1f, 0.2f);
        }
        else
        { thisObject.progMark.enabled = false; }

    }
    public override void transitions(Gold_Script thisObject)
    {
        //PickUp Object
        if (Input.GetKeyDown("e") && thisObject.sprinkScript.inRange && thisObject.itemScript.hasItem == false)
        {
            thisObject.itemScript.hasItem = true;
            thisObject.sprinkScript.Reset();
            thisObject.stateName = "HeldState";
            thisObject.currentState = new HeldState();
        }

        //Drop Object Once Complete
        if (thisObject.sprinkScript.complete)
        {
            thisObject.sprinkScript.Reset();
            thisObject.stateName = "FloorState";
            thisObject.currentState = new FloorState();
        }

        //Trigger Forging
        if (thisObject.sprinkScript.inRange)
        { thisObject.sprinkScript.Sprinkle(thisObject); }

    }
}


////////////////////////////////////////////////////////////////////////////////////////////////////


public class GoldSubState
{
    public virtual void transitions(Gold_Script thisObject, int switchNum) { }
}

///------------------------

public class GoldOre : GoldSubState
{

    public override void transitions(Gold_Script thisObject, int switchNum)
    {
        switch (switchNum)
        {
            case 0:
                thisObject.subStateName = "GoldBar";
                thisObject.animator.SetTrigger("Bar");
                thisObject.currentSubState = new GoldBar();
                break;
            case 1:
                thisObject.subStateName = "GoldDrink";
                thisObject.animator.SetTrigger("Drink");
                thisObject.currentSubState = new GoldDrink();
                break;
        }

    }
}

public class GoldBar : GoldSubState
{

    public override void transitions(Gold_Script thisObject, int switchNum)
    {
        switch (switchNum)
        {
            case 0:
                thisObject.subStateName = "GoldLeaf";
                thisObject.animator.SetTrigger("Leaf");
                thisObject.currentSubState = new GoldLeaf();
                break;
            case 1:
                thisObject.subStateName = "GoldSprinkles";
                thisObject.animator.SetTrigger("Sprinkles");
                thisObject.currentSubState = new GoldSprinkles();
                break;
        }
    }
}

public class GoldDrink : GoldSubState
{

    public override void transitions(Gold_Script thisObject, int switchNum)
    {
    }
}

public class GoldLeaf : GoldSubState
{

    public override void transitions(Gold_Script thisObject, int switchNum)
    {
    }
}

public class GoldSprinkles : GoldSubState
{

    public override void transitions(Gold_Script thisObject, int switchNum)
    {
    }
}


////////////////////////////////////////////////////////////////////////////////////////////////////


public class Gold_Script : MonoBehaviour
{
    //SCRIPT REFERENCES
    public ItemPickup itemScript;
    public PlayerMovement playerMovementScript;
    public Delete_Gold binScript;

    public Furnace_Script furnaceScript;
    public Anvil_Script anvilScript;
    public Brewery_Script brewScript;
    public Sprinkler_Script sprinkScript;
    public Press_Script pressScript;
    public Order_Script orderScript;
    public Round_Controller_Script roundScript;

    public GameObject progressBar;
    public GameObject barBackground;
    public GameObject progressMarker;

    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public Animator animator;

    public SpriteRenderer barBG;
    public SpriteRenderer progBar;
    public SpriteRenderer progMark;


    public float distance;
    public float speed = 5.0f;
    public float threshold = 2.0f;

    public float smeltNum = 0.0f;
    public float smeltMax = 10.0f;
    public float smeltSpeed = 0.7f;
    public float heldDecaySpeed = 0.4f;
    public float floorDecaySpeed = 0.6f;

    public bool destroyed = false;

    public GoldState currentState;
    public GoldSubState currentSubState;
    public string stateName = "FloorState";
    public string subStateName = "GoldOre";

    public float smeltOffset = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        currentState = new FloorState();
        currentSubState = new GoldOre();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        progressBar = this.transform.GetChild(0).gameObject;
        barBackground = this.transform.GetChild(1).gameObject;
        progressMarker = this.transform.GetChild(2).gameObject;

        barBG = barBackground.GetComponent<SpriteRenderer>();
        progBar = progressBar.GetComponent<SpriteRenderer>();
        progMark = progressMarker.GetComponent<SpriteRenderer>();

        //InvokeRepeating("Report", 0.0f, 3.0f);
        progMark.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, playerMovementScript.rb.position);
        currentState.position(this);
        currentState.transitions(this);
        currentState.smeltProgression(this);

        if (destroyed == true || roundScript.roundEnd == true)
        {
            itemScript.hasItem = false;
            Destroy(this.gameObject);
        }
    }


}
