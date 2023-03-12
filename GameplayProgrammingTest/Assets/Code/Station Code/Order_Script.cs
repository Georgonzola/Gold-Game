using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodOrder
{
    public float timer;
    public string type;

    public FoodOrder(float maxTime, int type, Order_Script thisObject)
    {
        this.timer = maxTime;
        this.type = thisObject.orderTypes[type];
    }

    public void SetTime(float time)
    {
        timer -= time;
        //Debug.Log(timer);
        if(timer <= 0)
        {
            timer = 0;
        }
    }
}


public class Order_Script : MonoBehaviour
{
    SpriteRenderer sprite;


    public float maxTime = 45f;

    public Gold_Script goldScript;
    public PlayerMovement playerMovementScript;

    float distance;
    public bool inRange = false;
    public int maxOrders = 3;
    public int lastOrder;



    public int completedOrders = 0;
    public int failedOrders = 0;
    public float roundMoney = 0;

    public float orderValue = 20;

    public string[] orderTypes = new string[] { "GoldDrink", "GoldLeaf", "GoldSprinkles" };

    //public int points = 0;

    public Sprite LeafOrder;
    public Sprite DrinkOrder;
    public Sprite SprinkOrder;

    public float orderStart = 75f;
    public float orderIndent = 135f;

    public bool jump;

    Color32 baseColour = new Color32(255, 255, 255, 255);
    Color32 tintColour = new Color32(75, 199, 143, 255);

    public List <GameObject> UIOrders = new List<GameObject>();
    public List <GameObject> TimerComponents = new List<GameObject>();


    public List<FoodOrder> orderList = new List<FoodOrder>();

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        InvokeRepeating("createOrder", 5f, 5f);

        UIOrders.Add(GameObject.Find("Order1"));
        UIOrders.Add(GameObject.Find("Order2"));
        UIOrders.Add(GameObject.Find("Order3"));

        LeafOrder = Resources.Load<Sprite>("Orders/LeafOrder");
        DrinkOrder = Resources.Load<Sprite>("Orders/WineOrder");
        SprinkOrder = Resources.Load<Sprite>("Orders/SprinklesOrder");

        for(int i = 0; i < UIOrders.Count; i++)
        {
            //Background
            TimerComponents.Add(UIOrders[i].transform.GetChild(0).gameObject);
            //Bar
            TimerComponents.Add(UIOrders[i].transform.GetChild(1).gameObject);
        }
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

        if (orderList.Count > 0)
        {
        }


        displayOrders();
        timeOrders();

    }

    public void createOrder()
    {
        if (orderList.Count < maxOrders)
        {
            int orderNum;

            do
            {
                orderNum = (Random.Range(0, 3));

            } while (orderNum == lastOrder);

            Debug.Log("NewOrder");
            Debug.Log(orderNum);
            lastOrder = orderNum;

            orderList.Add(new FoodOrder(maxTime, orderNum, this));
            setImages();
        }
    }

    public void submitOrder(Gold_Script goldObject)
    {
        bool triggerOnce = false;
        Debug.Log("****Submit");

        for (int i = 0; i < orderList.Count; i++)
        {
            if(goldObject.subStateName == orderList[i].type && triggerOnce == false){
                completedOrders++;
                Debug.Log(orderList[i].type);
                orderList.RemoveAt(i);
                goldObject.destroyed = true;
                triggerOnce = true;
                setImages();
            }
        }
    }

    public void printData()
    {

        Debug.Log("----------------------------------");
        for (int i = 0; i < orderList.Count; i++)
        {
           Debug.Log(orderList[i].type);
        }
        //Debug.Log(points);
    }


    public void displayOrders()
    {
        //initially hide Orders
        for (int i = 0; i < UIOrders.Count; i++)
        {
            UIOrders[i].GetComponent<Image>().enabled = false;
        }

        //Initially hide timers
        for (int i = 0; i < TimerComponents.Count; i++)
        {
            TimerComponents[i].GetComponent<Image>().enabled = false;
        }

        for (int i = 0; i < orderList.Count; i++)
        {
            float temp;
            //Show number of active orders and translate card position
            UIOrders[i].GetComponent<Image>().enabled = true;
            UIOrders[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(-orderStart - (orderIndent*(orderList.Count-1-i)), 60, 0);

            //enables visibility of timer components
            TimerComponents[i * 2].GetComponent<Image>().enabled = true;
            TimerComponents[i * 2 + 1].GetComponent<Image>().enabled = true;

            //set scale of timer components
            temp = 0 + (orderList[i].timer - 0) * (0.585f - 0) / (maxTime - 0);
            TimerComponents[i * 2 + 1].GetComponent<RectTransform>().localScale = new Vector3(temp, 0.04f, 0);



            //set colour of timer components
            temp = 0 + (orderList[i].timer - 0) * (1f - 0) / (maxTime - 0);
            TimerComponents[i * 2 + 1].GetComponent<Image>().color = new Color(1f - temp, temp, 0f);

            //set position of timer components
            float xOffset = 11f;
            float yOffset = -23f;
            TimerComponents[i * 2 + 1].GetComponent<RectTransform>().anchoredPosition = new Vector2(xOffset - 30f + 30f * temp, yOffset);


        }


    }

    public void timeOrders()
    {
        for (int i = 0; i < orderList.Count; i++)
        {
            //decreases timer
            orderList[i].SetTime(Time.deltaTime);

            if(orderList[i].timer <= 0)
            {
                failedOrders++;
                Debug.Log(orderList[i].type);
                Debug.Log("TimeOut");
                orderList.RemoveAt(i);
                setImages();
            }

        }

    }
    public void setImages()
    {
        for (int i = 0; i < orderList.Count; i++)
        {
            switch (orderList[i].type)
            {
                //set image for orders
                case "GoldDrink":
                    UIOrders[i].GetComponent<Image>().sprite = DrinkOrder;
                    break;

                case "GoldLeaf":
                    UIOrders[i].GetComponent<Image>().sprite = LeafOrder;
                    break;

                case "GoldSprinkles":
                    UIOrders[i].GetComponent<Image>().sprite = SprinkOrder;
                    break;
            }
        }
    }

    public void Reset()
    {
        orderList.Clear();
        completedOrders = 0;
        failedOrders = 0;
    }
}
