using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public ItemPickup script;

    public float moveSpeed = 4f;

    public Rigidbody2D rb;

  
    public Animator animator;

    Vector2 playerMovement;
    // Update is called once per frame

    public bool direction = true;
    bool vertical;

   // bool triggerCheck = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    Vector2 playerPosition;

    void Update()
    {

        playerMovement.x = Input.GetAxisRaw("Horizontal");
        playerMovement.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Horizontal", playerMovement.x);


        if(playerMovement.x > 0)
        {direction = true;}
        if (playerMovement.x < 0)
        {direction = false;}
        if (playerMovement.x > -0.5 && playerMovement.x < 0.5 && playerMovement.sqrMagnitude > 0.01)
        {vertical = true;}
        else
        {vertical = false;}
        

        /*
        if (triggerCheck != script.hasItem)
        {
            if (triggerCheck == false)
            {animator.SetTrigger("SetGold");}
            else
            {animator.SetTrigger("SetDefault");}
            
            triggerCheck = script.hasItem;
        }
        */

        animator.SetFloat("Speed", playerMovement.sqrMagnitude);
        animator.SetBool("Direction", direction);
        animator.SetBool("Vertical", vertical);
 
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + playerMovement * moveSpeed * Time.fixedDeltaTime);
    }
}
