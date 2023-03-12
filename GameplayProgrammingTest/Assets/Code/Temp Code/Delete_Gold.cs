using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete_Gold : MonoBehaviour
{
    SpriteRenderer sprite;

    public PlayerMovement playerMovementScript;
    public bool dispose = false;
    float distance;
   // public bool dispose = false;
    Color32 baseColour = new Color32(255, 255, 255, 255);
    Color32 tintColour = new Color32(55, 179, 123, 255);

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, playerMovementScript.rb.position);
        //Debug.Log(distance);
        dispose = false;

        if (distance < 0.5f)
        {
            sprite.color = tintColour;
            if (Input.GetKeyUp("e")){
                dispose = true;
            }

        }
        else
        {
            sprite.color = baseColour;
        }
    }

}
