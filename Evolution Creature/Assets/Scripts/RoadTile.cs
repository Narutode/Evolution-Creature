using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTile
{
    private int tileId;
    public GameObject roadGO;
    //public List<int> contrainteN;
    //public List<int> contrainteE;
    //public List<int> contrainteS;
    //public List<int> contrainteW;
    public List<int>[,,] contraintes;

    public RoadTile(int id, GameObject prefab)
    {
        tileId = id;
        roadGO = prefab;
        contraintes = new List<int>[3,3,3];
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int z = 0; z < 3; z++)
                {
                    contraintes[x, y, z] = new List<int>();
                }
            }
        }
    }
}
