using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Round_Menu_Script : MonoBehaviour
{
    public GameObject OrdersCompleted;
    public GameObject OrdersFailed;
    public GameObject MoneyMade;


    public Order_Script orderScript;
    public Upgrade_Menu_Script upgradeScript;

    public float roundMoney = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTextValues()
    {

        OrdersCompleted = this.transform.GetChild(4).gameObject;
        OrdersFailed = this.transform.GetChild(5).gameObject;
        MoneyMade = this.transform.GetChild(6).gameObject;

        OrdersCompleted.GetComponent<TextMeshProUGUI>().text = orderScript.completedOrders.ToString();
        OrdersFailed.GetComponent<TextMeshProUGUI>().text = orderScript.failedOrders.ToString();

        string temp;
        temp = "$" + roundMoney.ToString();
        MoneyMade.GetComponent<TextMeshProUGUI>().text = temp;
    }

   
}
