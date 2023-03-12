using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrade_Menu_Script : MonoBehaviour
{
    //Script references
    public Gold_Script goldScript;
    public Furnace_Script furnaceScript;
    public Anvil_Script anvilScript;
    public Sprinkler_Script sprinkScript;
    public Press_Script pressScript;
    public Brewery_Script brewScript;
    public PlayerMovement playerMovementScript;
    public Round_Controller_Script roundScript;

    public GameObject MoneyNum;

    public List<GameObject> upgradeOptions = new List<GameObject>();
    public ArrayList scriptUpgradeReferences = new ArrayList();

    public GameObject UpgradeSections;

    public int[] upgradeMaxes = new int[] { 4, 3, 4, 4, 4, 10, 4, 4, 10 };
    public int[] currentUpgrades = new int[9];
    public float[] upgradePrices = new float[9];
    public float[] upgradeBases;





    public Sprite UpgradeImg;
    public Sprite PoorImg;
    public Sprite MaxImg;

// Start is called before the first frame update
public float notchIndent;
    void Start()
    {
        Debug.Log("StartUpgrade");

        UpgradeImg = Resources.Load<Sprite>("UpgradeMenu/UpgradeButton");
        PoorImg = Resources.Load<Sprite>("UpgradeMenu/PoorButton");
        MaxImg = Resources.Load<Sprite>("UpgradeMenu/MaximumButton");

        for (int i = 0; i < UpgradeSections.transform.childCount; i++)
        {
            int temp = i;
            upgradeOptions.Add(UpgradeSections.transform.GetChild(i).gameObject);
            upgradeOptions[i].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => buttonTest(temp));
            upgradeOptions[i].transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
        }
 
        for (int i = 0; i < currentUpgrades.Length; i++)
        {
            currentUpgrades[i] = 0;
            upgradePrices[i] = 10;
        }
       // setValues();
        notchIndent = 16.85f;
        
        upgradeBases = new float[] {goldScript.smeltSpeed, furnaceScript.maxCapacity, anvilScript.forgeSpeed, sprinkScript.sprinkSpeed, pressScript.pressSpeed,
        brewScript.brewSpeed, playerMovementScript.moveSpeed, goldScript.heldDecaySpeed, roundScript.orderValue};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setValues()
    {
        //Set total money text



        MoneyNum = this.transform.GetChild(3).gameObject;
        string tempTotalMoney;
        tempTotalMoney = "$" + roundScript.totalMoney.ToString();
        MoneyNum.GetComponent<TextMeshProUGUI>().text = tempTotalMoney;


        string tempIndividualMoney;
        GameObject UpgradeMoneyNum;

        for (int i = 0; i < upgradeOptions.Count; i++)
        {
            //SetPrices
            upgradePrices[i] = 10 * (currentUpgrades[i] + 1);

            //Set Upgrade Price Text
            UpgradeMoneyNum = upgradeOptions[i].transform.GetChild(1).gameObject;
            tempIndividualMoney = "$" + upgradePrices[i].ToString();
            UpgradeMoneyNum.GetComponent<TextMeshProUGUI>().text = tempIndividualMoney;

            //setImages
            if (currentUpgrades[i] == upgradeMaxes[i])
            {
                upgradeOptions[i].transform.GetChild(0).GetComponent<Image>().sprite = MaxImg;
                //hide price text once maxed out
                upgradeOptions[i].transform.GetChild(1).gameObject.SetActive(false);

            }
            else if (upgradePrices[i] > roundScript.totalMoney)
            {
                upgradeOptions[i].transform.GetChild(0).GetComponent<Image>().sprite = PoorImg;
            }
            else
            {
                upgradeOptions[i].transform.GetChild(0).GetComponent<Image>().sprite = UpgradeImg;
            }
        }
    }

    void buttonTest(int type)
    {

        if (currentUpgrades[type] < upgradeMaxes[type] && upgradePrices[type] <= roundScript.totalMoney)
        {
            currentUpgrades[type]++;
            createNotch(upgradeOptions[type], currentUpgrades[type]);
            roundScript.totalMoney -= upgradePrices[type];
            setValues();

            float temp;
            switch (type)
            {

                case 0:
                    temp = 1 + (currentUpgrades[type] * 0.1f);
                    goldScript.smeltSpeed = upgradeBases[type] * temp;
                    break;
                case 1:
                    furnaceScript.maxCapacity = Mathf.FloorToInt(upgradeBases[type]) + Mathf.FloorToInt(currentUpgrades[type]);
                    break;
                case 2:
                    temp = 1 + (currentUpgrades[type] * 0.2f);
                    anvilScript.forgeSpeed = upgradeBases[type] * temp;
                    break;
                case 3:
                    temp = 1 + (currentUpgrades[type] * 0.2f);
                    sprinkScript.sprinkSpeed = upgradeBases[type] * temp;
                    break;
                case 4:
                    temp = 1 + (currentUpgrades[type] * 0.2f);
                    pressScript.pressSpeed = upgradeBases[type] * temp;
                    break;
                case 5:
                    temp = 1 + (currentUpgrades[type] * 0.1f);
                    brewScript.brewSpeed = upgradeBases[type] * temp;
                    break;
                case 6:
                    temp = 1 + (currentUpgrades[type] * 0.05f);
                    playerMovementScript.moveSpeed = upgradeBases[type] * temp;
                    break;
                case 7:
                    temp = 1 - (currentUpgrades[type] * 0.05f);
                    goldScript.heldDecaySpeed = upgradeBases[type] * temp;
                    break;
                case 8:
                    temp = 1 + (currentUpgrades[type] * 0.1f);
                    roundScript.orderValue = upgradeBases[type] * temp;
                    break;
            }

        }
    }

    void createNotch(GameObject upgradeSection, int upgradeNum)
    {
        GameObject notch = upgradeSection.transform.GetChild(2).gameObject;

        if (upgradeNum <= 1)
        {
            notch.GetComponent<UnityEngine.UI.Image>().enabled = true;

        }else if(upgradeNum <= 6)
        {
            Vector2 position = notch.GetComponent<RectTransform>().anchoredPosition + new Vector2 ((upgradeNum - 1) * notchIndent, 0);
            GameObject NotchClone = Instantiate(notch, transform.position, Quaternion.identity);

            NotchClone.transform.SetParent(upgradeSection.transform);
            NotchClone.GetComponent<RectTransform>().anchoredPosition = position;
            NotchClone.GetComponent<RectTransform>().localScale = new Vector2(0.1f, 0.1f);
        }
        else
        {
            Vector2 position = notch.GetComponent<RectTransform>().anchoredPosition + new Vector2((upgradeNum - 7) * notchIndent, -30.5f);
            GameObject NotchClone = Instantiate(notch, transform.position, Quaternion.identity);

            NotchClone.transform.SetParent(upgradeSection.transform);
            NotchClone.GetComponent<RectTransform>().anchoredPosition = position;
            NotchClone.GetComponent<RectTransform>().localScale = new Vector2(0.1f, 0.1f);
        }

    }
}
