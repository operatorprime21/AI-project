using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FloorManager : MonoBehaviour
{
    public FloorData start;
    public FloorData end;
    public FloorData nextPathNode; 

    public GameObject floorObj;
    public GameObject wallObj;

    public GameObject map;
    public List<FloorData> floorGrid = new List<FloorData>();

    public int xSize;
    public int ySize;

    //public int[,] grid;

    void Start()
    {
        //SpawnFloors();
        GetFloors();
    }
    

    public void SpawnFloors()
    {
        for (int x = 0; x < (xSize + 1); x++)
        {
            for (int y = 0; y < (ySize + 1); y++)
            {
                if (x == 0 || x == xSize || y == 0 || y == ySize)
                {
                    GameObject wall = Instantiate(wallObj, new Vector3(x, 0, y), Quaternion.identity);
                    floorGrid.Add(wall.GetComponent<FloorData>());
                }
                else
                {
                    GameObject floor = Instantiate(floorObj, new Vector3(x, 0, y), Quaternion.identity);
                    floorGrid.Add(floor.GetComponent<FloorData>());
                }
            }
        }
    }

    public void GetFloors()
    {
        foreach(GameObject grid in GameObject.FindGameObjectsWithTag("Grid"))
        {
            floorGrid.Add(grid.GetComponent<FloorData>());
        }
    }
}