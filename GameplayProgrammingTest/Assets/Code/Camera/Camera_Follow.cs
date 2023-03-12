using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    Vector3 camera_position;
    public PlayerMovement playerMovementScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // camera_position.x = playerMovementScript.rb.position.x;
        camera_position = playerMovementScript.rb.position;
        camera_position.z = -0.3f;
        transform.position = camera_position;
    }
}
