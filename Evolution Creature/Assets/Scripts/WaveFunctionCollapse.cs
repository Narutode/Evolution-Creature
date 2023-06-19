using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveFunctionCollapse : MonoBehaviour
{
    private WFCTile[,,] grid;

    public GameObject cube;

    private int sizeX = 50, sizeY = 50;
    //private int curX, curY;

    private List<WFCTile> tileToCheck;
    private List<WFCTile> tileToRemove;
    private List<WFCTile> tileToAdd;


    // Start is called before the first frame update
    void Start()
    {
        tileToAdd = new List<WFCTile>();
        tileToRemove = new List<WFCTile>();
        tileToCheck = new List<WFCTile>();
        grid = new WFCTile[sizeX,sizeY];
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                grid[i, j] = new WFCTile(i,j);
            } 
        }
        tileToCheck.Add(grid[sizeX/2,sizeY/2]);
    }

    // Update is called once per frame
    void Update()
    {
        if (tileToCheck.Count > 0)
        {
            foreach (var t in tileToCheck)
            {
                tileToRemove.Add(t);

                WFCTile tile = t;

                if (tile.cell == -1)
                {
                    if (tile.tilePossible.Count == 0)
                    {
                        tile.cell = 0;
                        tile.tilePossible.Clear();
                    }
                    else if (tile.tilePossible.Count > 1)
                    {
                        int r = Random.Range(0, tile.tilePossible.Count);
                        tile.cell = tile.tilePossible[r];
                        tile.tilePossible.Clear();
                    }
                    else
                    {
                        tile.cell = tile.tilePossible.First();
                        tile.tilePossible.Clear();
                    }
                    Instantiate(cube, new Vector3(tile.posX, tile.cell, tile.posY), Quaternion.identity);
                }
                
                if (tile.posX + 1 < sizeX)
                {
                    if (tile.Update(grid[tile.posX + 1, tile.posY]))
                    {
                        if(!tileToAdd.Contains(grid[tile.posX + 1, tile.posY]))
                            tileToAdd.Add(grid[tile.posX + 1, tile.posY]);
                    }
                }

                if (tile.posX - 1 >= 0)
                {
                    if (tile.Update(grid[tile.posX - 1, tile.posY]))
                    {
                        if(!tileToAdd.Contains(grid[tile.posX - 1, tile.posY]))
                            tileToAdd.Add(grid[tile.posX - 1, tile.posY]);
                    }
                }

                if (tile.posY + 1 < sizeY)
                {
                    if (tile.Update(grid[tile.posX, tile.posY+1]))
                    {
                        if(!tileToAdd.Contains(grid[tile.posX, tile.posY + 1]))
                            tileToAdd.Add(grid[tile.posX, tile.posY+1]);
                    }
                }

                if (tile.posY - 1 >= 0)
                {
                    if (tile.Update(grid[tile.posX, tile.posY-1]))
                    {
                        if(!tileToAdd.Contains(grid[tile.posX, tile.posY - 1]))
                            tileToAdd.Add(grid[tile.posX, tile.posY-1]);
                    }
                }
            }

            Debug.Log(tileToAdd.Count);
            foreach (var v2 in tileToRemove)
            {
                tileToCheck.Remove(v2);
            }
            tileToRemove.Clear();
        
            foreach (var v2 in tileToAdd)
            {
                tileToCheck.Add(v2);
            }
            tileToAdd.Clear();
        }
    }
}
