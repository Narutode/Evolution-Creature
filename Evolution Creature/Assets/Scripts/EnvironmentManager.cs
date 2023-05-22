using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    // Public variables
    public GameObject floorObject;
    //public int envDimension;
    public GameObject carnivorous, herbivorous, vegetables;
    public int carnNum, herbNum, vegNum;
    public GameObject[] carnTab, herbTab, vegTab;

    // Private variables
    
    


    // Start is called before the first frame update
    void Start()
    {
        floorObject = GameObject.Find("Floor");
        carnTab = new GameObject[carnNum];
        herbTab = new GameObject[herbNum];
        vegTab = new GameObject[vegNum];

        //floorObject.transform.localScale = new Vector3(envDimension, 0, envDimension);
        SpawnVegetables();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnVegetables()
    {
        float floorX = floorObject.transform.position.x;
        float floorY = floorObject.transform.position.y;
        float floorZ = floorObject.transform.position.z;

        float scaleX = floorObject.transform.lossyScale.x;
        float scaleY = floorObject.transform.lossyScale.y;
        float scaleZ = floorObject.transform.lossyScale.z;

        for (int i = 0; i < vegNum-1; i++)
        {
            vegTab[i] = Instantiate(vegetables, new Vector3(Random.Range(floorX - (scaleX *5), floorX+(scaleX *5)), 0.5f, Random.Range(floorZ-(scaleZ*5), floorZ+(scaleZ*5))), Quaternion.Euler(0,0,0));
                                                          
        }
    }
}
