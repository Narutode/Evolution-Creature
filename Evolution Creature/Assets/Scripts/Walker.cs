using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Walker : MonoBehaviour
{
    
    CharacterJoint[] joints;
    Rigidbody[] rigidbodies;

    private void Awake()
    {
        
        joints = gameObject.GetComponents<CharacterJoint>();
        rigidbodies = GameObject.Find("Legs").GetComponentsInChildren<Rigidbody>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach(Rigidbody r in rigidbodies)
            {
                switch (r.gameObject.tag)
                {
                    case "Part1":
                        r.AddForce(Vector3.up * 100);
                        break;
                    case "Part2":
                        r.AddForce(Vector3.forward * 100);
                        break;
                    case "Part3":
                        r.AddForce(Vector3.back * 100);
                        break;
                }
                ;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            foreach (Rigidbody r in rigidbodies)
            {
                switch (r.gameObject.tag)
                {
                    case "Part1":
                        r.AddForce(Vector3.down * 100);
                        break;
                    case "Part2":
                        r.AddForce(Vector3.back * 100);
                        break;
                    case "Part3":
                        r.AddForce(Vector3.forward * 100);
                        break;
                }
                ;
            }
        }
    }
}
