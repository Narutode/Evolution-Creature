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
    private List<WFCTile> tileToCheck;
    private List<WFCTile> tileToRemove;
    private List<WFCTile> tileToAdd;
    //                 0      1        2        3        4         5        6        7     
    private RoadTile empty, roadNS, roadEW, roadCross, roadUpN, roadUpS, roadUpE, roadUpW;
    public GameObject roadNSGO,roadEWGO,roadUpNGO,roadCrossGO,roadUpSGO,roadUpEGO,roadUpWGO; 
    
    // Start is called before the first frame update
    void Start()
    {
        availableTiles = new List<RoadTile>(); //add road tiles 
        
        /*
        empty = new RoadTile(0, null, new[] {0, 2, 6, 7, 10, 11}, new[] {0, 1, 4, 5, 8, 9}, new[] {0, 2, 6, 7, 10, 11},
            new[] {0, 1, 4, 5, 8, 9});
        roadNS = new RoadTile(1, roadNSGO, new[] {1, 3, 4, 8}, new[] {0, 1, 4, 5, 8, 9}, new[] {1, 3, 5, 9},
            new[] {0, 1, 4, 5, 8, 9});
        roadEW = new RoadTile(2, roadEWGO, new[] {0, 6, 7, 10, 10}, new[] {2, 3, 6, 10}, new[] {0, 6, 7, 10},
            new[] {2, 3, 7, 11});
        roadCross = new RoadTile(3, roadCrossGO, new[] {1, 3, 4, 8}, new[] {2, 3, 6, 10}, new[] {1, 3, 5, 9},
            new[] {2, 3, 7, 11});
        roadUpN = new RoadTile(4, roadUpNGO, new[] {1, 3, 4}, new[] {0}, new[] {1, 3, 5},
            new[] {0});
        roadUpS = new RoadTile(5, roadUpSGO, new[] {1, 3, 8}, new[] {0}, new[] {1, 3, 9},
            new[] {0});
        roadUpE = new RoadTile(6, roadUpEGO, new[] {0}, new[] {2,3,6}, new[] {0},
            new[] {2,3,11});
        roadUpW = new RoadTile(7, roadUpWGO, new[] {0}, new[] {2,3,10}, new[] {0},
            new[] {2,3,7});
        roadDownN = new RoadTile(8, roadDownNGO, new[] {1, 3, 8}, new[] {0}, new[] {1, 3, 9},
            new[] {0});
        roadDownS = new RoadTile(9, roadDownSGO, new[] {1, 3, 4}, new[] {0}, new[] {1, 3, 5},
            new[] {0});
        roadDownE = new RoadTile(10, roadDownEGO, new[] {0}, new[] {2,3,10}, new[] {0},
            new[] {2,3,7});
        roadDownW = new RoadTile(11, roadDownpWGO, new[] {0}, new[] {2,3,6}, new[] {0},
            new[] {2,3,11});
*/
        
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

        tileToAdd = new List<WFCTile>();
        tileToRemove = new List<WFCTile>();
        tileToCheck = new List<WFCTile>();
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
        tileToCheck.Add(grid[sizeX/2,sizeY/2,sizeZ/2]);
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
                    //Debug.Log(tile.cell);
                    if(tile.cell != 0)
                        Instantiate(availableTiles[tile.cell].roadGO, new Vector3(tile.posX, tile.posY, tile.posZ), availableTiles[tile.cell].roadGO.transform.rotation);
                }

                for (int a = -1; a < 2; a++)
                {
                    for (int b = -1; b < 2; b++)
                    {
                        for (int c = -1; c < 2; c++)
                        {
                            if ((a != 0 || b != 0 || c != 0) && tile.posX + a >= 0 && tile.posX + a < sizeX && tile.posY + b >= 0 &&
                                tile.posY + b < sizeY && tile.posZ + c >= 0 && tile.posZ + c < sizeZ)
                            {
                                var lookCell = grid[tile.posX + a, tile.posY + b, tile.posZ + c];
                                if (tile.Update(lookCell,availableTiles[tile.cell].contraintes[a+1,b+1,c+1]))
                                {
                                    if(!tileToAdd.Contains(lookCell))
                                        tileToAdd.Add(lookCell);
                                }
                            }
                        }
                    }
                }
            }

            //Debug.Log(tileToAdd.Count);
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
    //           0      1     2        3        4        5       6      7     
    //RoadTile empty,roadNS,roadEW,roadCross,roadUpN,roadUpS,roadUpE,roadUpW
    void fillConstraints()
    {
        int[,,] exGrid = {{
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 1, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0}},
            {
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 1, 0, 1},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0}},
            {
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 1, 0, 1},
                {0, 2, 2, 2, 0},
                {0, 0, 0, 0, 0}},
            {
                {0, 0, 0, 0, 0},
                {2, 2, 2, 2, 2},
                {0, 0, 1, 0, 1},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0}},      
            {
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 2},
                {0, 0, 2, 6, 0},
                {2, 6, 0, 0, 0}},    
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
