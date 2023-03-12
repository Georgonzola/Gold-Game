using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RoundState
{
    public virtual void running(Round_Controller_Script thisObject) { }
    public virtual void transition(Round_Controller_Script thisObject) { }
    public virtual void setDisplay(Round_Controller_Script thisObject) { }
}

public class StartState : RoundState
{
    public override void running(Round_Controller_Script thisObject)
    {

    }
    public override void transition(Round_Controller_Script thisObject)
    {
    
    }
    public override void setDisplay(Round_Controller_Script thisObject)
    {
    
    }
}

public class ActiveRoundState : RoundState
{
    public override void running(Round_Controller_Script thisObject)
    {
        thisObject.roundTime += Time.deltaTime;
        //Debug.Log(thisObject.roundTime);
    }
    public override void transition(Round_Controller_Script thisObject)
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            thisObject.currentState = new PauseState();
            thisObject.currentState.setDisplay(thisObject);
        }

        if (thisObject.roundTime >= thisObject.roundLength)
        {
            Time.timeScale = 0f;
            thisObject.currentState = new RoundEndMenuState();
            thisObject.currentState.setDisplay(thisObject);
            thisObject.roundReset();
        }
    }
    public override void setDisplay(Round_Controller_Script thisObject)
    {
       thisObject.pauseCanvas.SetActive(false);
       thisObject.roundMenuCanvas.SetActive(false);
       thisObject.upgradeCanvas.SetActive(false);

       thisObject.pauseButton.SetActive(true);
       thisObject.orderCanvas.SetActive(true);
    }
}

public class PauseState : RoundState
{
    public override void running(Round_Controller_Script thisObject)
    {
    }
    public override void transition(Round_Controller_Script thisObject)
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            thisObject.currentState = new ActiveRoundState();
            thisObject.currentState.setDisplay(thisObject);
        }

    }
    public override void setDisplay(Round_Controller_Script thisObject)
    {
        thisObject.roundMenuCanvas.SetActive(false);
        thisObject.upgradeCanvas.SetActive(false);
        thisObject.orderCanvas.SetActive(false);

        thisObject.pauseCanvas.SetActive(true);
        thisObject.pauseButton.SetActive(true);
    }
}

public class RoundEndMenuState : RoundState
{
    public override void running(Round_Controller_Script thisObject)
    {

    }
    public override void transition(Round_Controller_Script thisObject)
    {

    }
    public override void setDisplay(Round_Controller_Script thisObject)
    {
        thisObject.upgradeCanvas.SetActive(false);
        thisObject.orderCanvas.SetActive(false);
        thisObject.pauseCanvas.SetActive(false);
        thisObject.pauseButton.SetActive(false);

        thisObject.roundMenuCanvas.SetActive(true);
    }
}

public class UpgradeMenuState : RoundState
{
    public override void running(Round_Controller_Script thisObject)
    {

    }
    public override void transition(Round_Controller_Script thisObject)
    {

    }
    public override void setDisplay(Round_Controller_Script thisObject)
    {
        thisObject.pauseButton.SetActive(false);
        thisObject.orderCanvas.SetActive(false);
        thisObject.pauseCanvas.SetActive(false);
        thisObject.roundMenuCanvas.SetActive(false);

        thisObject.upgradeCanvas.SetActive(true);


    }
}



public class Round_Controller_Script : MonoBehaviour
{
    public RoundState currentState;

    public GameObject pauseButton;

    public GameObject pauseCanvas;
    public GameObject roundMenuCanvas;
    public GameObject upgradeCanvas;

    public GameObject orderCanvas;

    public float roundLength = 60f;
    public float roundTime = 0f;

    public GameObject continueButton;
    public GameObject upgradesMenuButton;
    public GameObject upgradesBackButton;

    public Order_Script orderScript;
    public Round_Menu_Script roundMenuScript;
    public Upgrade_Menu_Script upgradeScript;
    public Furnace_Script smeltScript;

    public Anvil_Script forgeScript;
    public Press_Script pressScript;
    public Sprinkler_Script sprinkScript;
    public Brewery_Script brewScript;

    public bool roundEnd = false;


    public float totalMoney = 0;
    public float orderValue = 20;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        currentState = new ActiveRoundState();

        Button pauseButtonComp = pauseButton.GetComponent<Button>();
        pauseButtonComp.onClick.AddListener(TogglePauseMenu);

        Button continueButtonComp = continueButton.GetComponent<Button>();
        continueButtonComp.onClick.AddListener(StartNewRound);

        Button upgradesButtonComp = upgradesMenuButton.GetComponent<Button>();
        upgradesButtonComp.onClick.AddListener(goToUpgrades);

        Button backButtonComp = upgradesBackButton.GetComponent<Button>();
        backButtonComp.onClick.AddListener(goToMenu);


        currentState.setDisplay(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.running(this);
        currentState.transition(this);
    }

    void TogglePauseMenu()
    {
        if (currentState is PauseState)
        {
            Time.timeScale = 1.0f;
            currentState = new ActiveRoundState();
            currentState.setDisplay(this);
        }
        else
        {
            Time.timeScale = 0f;
            currentState = new PauseState();
            currentState.setDisplay(this);
        }
    }

    void StartNewRound()
    {
        if (currentState is RoundEndMenuState)
        {
            Time.timeScale = 1f;
            roundTime = 0f;
            currentState = new ActiveRoundState();
            currentState.setDisplay(this);
            roundEnd = false;
        }
    }

    void goToUpgrades()
    {
        if (currentState is RoundEndMenuState)
        {
            currentState = new UpgradeMenuState();
            currentState.setDisplay(this);
        }


    }

    void goToMenu()
    {
        if (currentState is UpgradeMenuState)
        {
            currentState = new RoundEndMenuState();
            currentState.setDisplay(this);

        }
    }

    public void roundReset()
    {
        calculateMoney();
        roundMenuScript.setTextValues();

        orderScript.Reset();
        brewScript.Reset();
        smeltScript.Reset();
        sprinkScript.Reset();
        pressScript.Reset();
        forgeScript.Reset();

        Debug.Log("roundReset");
        upgradeCanvas.SetActive(true);
        upgradeScript.setValues();
        upgradeCanvas.SetActive(false);

        //workaround for deleting gold spawns
        roundEnd = true;
    }

    public void calculateMoney()
    {
        float roundTotal;

        roundTotal = (orderScript.completedOrders - (orderScript.failedOrders * 0.5f)) * orderValue;

        Debug.Log(roundTotal);

        totalMoney += roundTotal;

        roundMenuScript.roundMoney = roundTotal;
    }
}
