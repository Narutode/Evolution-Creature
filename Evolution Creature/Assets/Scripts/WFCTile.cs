using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;





public class WFCTile
{
    public List<int> tilePossible;
    public int cell = -1;
    public int posX, posY, posZ;

    public WFCTile(int x, int y, int z)
    {
        tilePossible = new List<int>();
        for (int i = 0; i < 8; i++)
        {
            tilePossible.Add(i);
        }
        posX = x;
        posY = y;
        posZ = z;
    }

    public bool Update(WFCTile tile, List<int> cont)
    {
        if (tile.cell == -1)
        {
            List<int> cellToRemove = new List<int>();
            foreach (var p in tile.tilePossible)
            {
                if(!cont.Contains(p))
                    cellToRemove.Add(p);
            }
            foreach (var p in cellToRemove)
            {
                tile.tilePossible.Remove(p);
            }
            return true;
        }
        return false;
    }
}
