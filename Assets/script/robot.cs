using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class robot : MonoBehaviour
{
    public float GravityMultipler;
    float AddGravity =0.0f;
 
    float horizontalSpeed = 0.3f;
    float verticalSpeed = 12.0f;
    float v = 0.0f;
    float h = 0.0f;
    public GameObject backLeftWheel;
    public GameObject backRightWheel;
    public GameObject frontLeftWheel;
    public GameObject frontRightWheel;
    
    void Update()
    {
        if(transform.position.y > 0)
        {
            AddGravity += GravityMultipler;

        }
        else
            AddGravity =   0.0f;

         v = horizontalSpeed * Input.GetAxis("Vertical");
        h = verticalSpeed * Input.GetAxis("Horizontal");
      
            transform.Translate(0, -AddGravity, v*2);
    
            transform.Rotate(0, h, 0);
        backLeftWheel.transform.Rotate(v*90,0, 0);
        backRightWheel.transform.Rotate(v*90, 0, 0);
        frontLeftWheel.transform.Rotate(v*90, 0,0);
        frontRightWheel.transform.Rotate(v*90, 0,0);
        if (Input.GetKeyDown("space")&& transform.position.y<=0.01)
        {
            transform.Translate(Vector3.up *55* Time.deltaTime);
        }
        }
    private void OnTriggerEnter(Collider other)
    {

        transform.Translate(0, 0, -v);
       
   
    }

}
