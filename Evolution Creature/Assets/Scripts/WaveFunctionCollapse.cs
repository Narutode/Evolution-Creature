using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
 * 6 Conditions : N, E, S, W, H, B
 *
 *  if(n) > n 
 * 
 */
public class WaveFunctionCollapse : MonoBehaviour
{
    private WFCTile[,,] grid;

    public GameObject cube;

    private int sizeX = 20, sizeY = 20, sizeZ = 20;
    //private int curX, curY;

    public List<RoadTile> availableTiles;
    private int minP = 12;
    private WFCTile curTile;
    //private List<WFCTile> tileToCheck;
    //private List<WFCTile> tileToRemove;
    //private List<WFCTile> tileToAdd;
    //                 0      1        2        3        4         5        6        7     
    private RoadTile empty, roadNS, roadEW, roadCross, roadUpN, roadUpS, roadUpE, roadUpW;
    public GameObject roadNSGO,roadEWGO,roadUpNGO,roadCrossGO,roadUpSGO,roadUpEGO,roadUpWGO; 
    
    // Start is called before the first frame update
    void Start()
    {
        availableTiles = new List<RoadTile>(); //add road tiles 

        empty = new RoadTile(0, null);
        roadNS = new RoadTile(1, roadNSGO);
        roadEW = new RoadTile(2, roadEWGO);
        roadCross = new RoadTile(3, roadCrossGO);
        roadUpN = new RoadTile(4, roadUpNGO);
        roadUpS = new RoadTile(5, roadUpSGO);
        roadUpE = new RoadTile(6, roadUpEGO);
        roadUpW = new RoadTile(7, roadUpWGO);
        
        availableTiles.Add(empty);
        availableTiles.Add(roadNS);
        availableTiles.Add(roadEW);
        availableTiles.Add(roadCross);
        availableTiles.Add(roadUpN);
        availableTiles.Add(roadUpE);
        availableTiles.Add(roadUpS);
        availableTiles.Add(roadUpW);

        fillConstraints();

        //tileToAdd = new List<WFCTile>();
        //tileToRemove = new List<WFCTile>();
        //tileToCheck = new List<WFCTile>();
        grid = new WFCTile[sizeX,sizeY,sizeZ];
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    grid[x, y, z] = new WFCTile(x, y, z);
                }
            }
        }

        curTile = grid[sizeX / 2, sizeY / 2, sizeZ / 2];
        curTile.tilePossible.Clear();
        curTile.cell = 2;
        //tileToCheck.Add(grid[sizeX/2,sizeY/2,sizeZ/2]);
    }

    // Update is called once per frame
    void Update()
    {
        if (curTile != null)
        {
            //foreach (var t in tileToCheck)
            //{
                //tileToRemove.Add(t);

                //WFCTile tile = t;

                if (curTile.cell == -1)
                {
                    if (curTile.tilePossible.Count == 0)
                    {
                        curTile.cell = 0;
                        curTile.tilePossible.Clear();
                    }
                    else
                    {
                        int r = Random.Range(0, curTile.tilePossible.Count);
                        curTile.cell = curTile.tilePossible[r];
                        curTile.tilePossible.Clear();
                    }
                    //Debug.Log(tile.cell);
                }// 

                if(curTile.cell != -1 && curTile.cell != 0)
                    Instantiate(availableTiles[curTile.cell].roadGO, new Vector3(curTile.posX, curTile.posY, curTile.posZ), availableTiles[curTile.cell].roadGO.transform.rotation);

                for (int a = -1; a < 2; a++)
                {
                    for (int b = -1; b < 2; b++)
                    {
                        for (int c = -1; c < 2; c++)
                        {
                            if ((a != 0 || b != 0 || c != 0) && curTile.posX + a >= 0 && curTile.posX + a < sizeX &&
                                curTile.posY + b >= 0 &&
                                curTile.posY + b < sizeY && curTile.posZ + c >= 0 && curTile.posZ + c < sizeZ)
                            {
                                var lookCell = grid[curTile.posX + a, curTile.posY + b, curTile.posZ + c];
                                lookCell.Update(availableTiles[curTile.cell].contraintes[a + 1, b + 1, c + 1]);
                            }
                        }
                    }
                }

                minP = 12;
                curTile = null;
                for (int x = 0; x < sizeX; x++)
                {
                    for (int y = 0; y < sizeY; y++)
                    {
                        for (int z = 0; z < sizeZ; z++)
                        {
                            if (grid[x, y, z].cell == -1 && grid[x, y, z].tilePossible.Count < minP)
                            {
                                curTile = grid[x, y, z];
                                minP = curTile.tilePossible.Count;
                            }
                        }
                    }
                }

                
                
                Debug.Log(minP);
        }

        //Debug.Log(tileToAdd.Count);
            /*
            foreach (var v2 in tileToRemove)
            {
                tileToCheck.Remove(v2);
            }
            tileToRemove.Clear();
        
            foreach (var v2 in tileToAdd)
            {
                tileToCheck.Add(v2);
            }
            tileToAdd.Clear();*/
    }
    //           0      1     2        3        4        5       6      7     
    //RoadTile empty,roadNS,roadEW,roadCross,roadUpN,roadUpE,roadUpS,roadUpW
    void fillConstraints()
    {
        int[,,] exGrid = {{
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0}},
            {
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0}},
            {
                {0, 0, 0, 0, 0},
                {0, 0, 0, 2, 5},
                {2, 2, 5, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0}},
            {
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0}}, 
            {
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0}},
        };

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                for (int z = 0; z < 5; z++)
                {
                    
                    RoadTile curRoadTile = availableTiles[exGrid[x, y, z]];
                    for (int a = -1; a < 2; a++)
                    {
                        for (int b = -1; b < 2; b++)
                        {
                            for (int c = -1; c < 2; c++)
                            {
                                if ((a != 0 || b != 0 || c != 0) && x + a >= 0 && x + a < 5 && y + b >= 0 &&
                                    y + b < 5 && z + c >= 0 && z + c < 5)
                                {
                                    int cons = exGrid[x + a, y + b, z + c];
                                    if(!curRoadTile.contraintes[a+1,b+1,c+1].Contains(cons))
                                        curRoadTile.contraintes[a+1,b+1,c+1].Add(cons);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
