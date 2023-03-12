using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public bool hasItem = false;
    public bool pickUpDelay = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void itemDelay()
    {
        pickUpDelay = true;
        StartCoroutine(ExampleCoroutine());
        IEnumerator ExampleCoroutine()
        {
            yield return new WaitForSeconds(1);
            pickUpDelay = false;
           // Debug.Log("Time");
        }
    }
}
