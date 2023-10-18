using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public FloorData start;
    public FloorData end;

    public GameObject floorObj;
    public List<GameObject> floorGrid = new List<GameObject>();
    void Start()
    {
        SpawnFloors();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnFloors()
    {
        for(int x = -5; x<6; x++)
        {
            for (int y = -5; y<6;y++)
            {
                GameObject floor = Instantiate(floorObj, new Vector3(x, 0, y), Quaternion.identity);
                floorGrid.Add(floor);
            }
        }
    }

}
