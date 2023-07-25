using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class Walker : MonoBehaviour
{

    public GameObject robot;
   


  
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        RobotController robotController = robot.GetComponent<RobotController>();

        
       
        for (int j = 0; j < robotController.joints.Length; j++)
        {
            
            float inputVal = Input.GetAxis(robotController.joints[j].inputAxis);
            
           

            if (Mathf.Abs(inputVal) > 0)
            {
                RotationDirection direction = GetRotationDirection(inputVal);
                robotController.RotateJoint(j, direction);
                return;
            }
        }

        robotController.StopAllJointRotations();







    }



    // HELPERS

    static RotationDirection GetRotationDirection(float inputVal)
    {
        if (inputVal > 0)
        {
            return RotationDirection.Positive;
        }
        else if (inputVal < 0)
        {
            return RotationDirection.Negative;
        }
        else
        {
            return RotationDirection.None;
        }
    }
}