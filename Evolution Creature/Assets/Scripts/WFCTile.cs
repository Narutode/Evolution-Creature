using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/*
 *
 * 0 : Vide
 * 1,2 : Route N/S, E/W
 * 3 : route Croix
 * 4, 5, 6, 7 : Escalier Montent N, E, S, W
 * 8, 9, 10, 11 : Escalier descend N, E, S, W
 *
 *
 * 0 -> 0 _ 11
 * 1 -> 0, 1, 3, 4, 6, 8, 10
 * 2 -> 0, 2, 3, 5, 7, 9, 11
 * 3 -> 0 _ 11
 * 4 -> 
 * 
 */


public class WFCTile
{
    public List<int> tilePossible;
    public int cell = -1;
    public int posX, posY;

    public WFCTile(int x, int y)
    {
        tilePossible = new List<int>();
        for (int i = 0; i < 20; i++)
        {
            tilePossible.Add(i);
        }
        posX = x;
        posY = y;
    }

    public bool Update(WFCTile tile)
    {
        if (tile.cell == -1)
        { 
            List<int> cellToRemove = new List<int>();
            foreach (var p in tile.tilePossible)
            {
                if (Math.Abs(cell - p) > 1)
                {
                    cellToRemove.Add(p);
                }
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
