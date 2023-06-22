using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Walker : MonoBehaviour
{

    ArticulationBody leg1, leg2, leg3, leg4;


    private void Awake()
    {

        leg1 = GameObject.Find("Leg1.2").GetComponent<ArticulationBody>();
        leg2 = GameObject.Find("Leg2.2").GetComponent<ArticulationBody>();
        leg3 = GameObject.Find("Leg3.2").GetComponent<ArticulationBody>();
        leg4 = GameObject.Find("Leg4.2").GetComponent<ArticulationBody>();

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            leg1.AddTorque(Vector3.back * 80); 
            leg2.AddTorque(Vector3.forward * 80);
            leg3.AddTorque(Vector3.right * 80);
            leg4.AddTorque(Vector3.left * 80);


        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            leg1.anchorRotation = Quaternion.Inverse(leg1.anchorRotation);
            leg2.anchorRotation = Quaternion.Inverse(leg2.anchorRotation);
            leg3.anchorRotation = Quaternion.Inverse(leg3.anchorRotation);
            leg4.anchorRotation = Quaternion.Inverse(leg4.anchorRotation);




        }

    }
}