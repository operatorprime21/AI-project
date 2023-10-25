using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FloorManager : MonoBehaviour
{
    public FloorData start;
    public FloorData end;

    public GameObject floorObj;
    public List<GameObject> floorGrid = new List<GameObject>();

    public List<GameObject> openList = new List<GameObject>();
    public List<GameObject> pathWay = new List<GameObject>();

    void Start()
    {
        SpawnFloors();
        
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        GetStartEnd();
        LookFrom(start);
    }

    // Update is called once per frame
    void Update()
    {
        //if(restart)
        //{
        //    StartCoroutine(Scan());

        //}
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

    public void GetStartEnd()
    {
        foreach (GameObject floor in floorGrid)
        {
            if(floor.GetComponent<FloorData>().Type == FloorData.type.start)
            {
                start = floor.GetComponent<FloorData>();
                break;
            }
        }

        foreach (GameObject floor in floorGrid)
        {
            if (floor.GetComponent<FloorData>().Type == FloorData.type.end)
            {
                end = floor.GetComponent<FloorData>();
                break;
            }
        }
    }

    public void LookFrom(FloorData floorFrom)
    {
        float newX = floorFrom.x;
        float newY = floorFrom.y;
        int c = 0;
        for (int x = -1; x <= 1; x++)
        {
            for(int y = -1; y<=1; y++)
            {
                if (x != 0 || y != 0)
                {
                    float lookingX = newX + x;
                    float lookingY = newY + y;
                    GameObject floor = GameObject.Find(lookingX.ToString() + ", " + lookingY.ToString());
                    Debug.Log(floor);
                    if (floor != null)
                    {
                        FloorData nextData = floor.GetComponent<FloorData>();
                        if (nextData.x == newX + x && nextData.y == newY + y)
                        {
                            if (nextData.listed == FloorData.looking.none)
                            {
                                if (floor.GetComponent<FloorData>().Type == FloorData.type.walkable)
                                {
                                    openList.Add(floor);
                                    nextData.listed = FloorData.looking.open;
                                    c++;
                                }
                            }
                        }
                    }

                }
            }
        }
        if (c > 0)
        {
            floorFrom.listed = FloorData.looking.close;
            pathWay.Add(floorFrom.gameObject);
        }
        else
        {
            floorFrom.listed = FloorData.looking.ignore;
        }
        GetNextFloor();
    }

    public float FValueCalc(FloorData floorFrom)
    {
        float gValue = Mathf.Sqrt(floorFrom.x * floorFrom.x + start.x * start.x);
        float hValue = Mathf.Sqrt(floorFrom.x * floorFrom.x + end.x * end.x); ;
        return gValue + hValue;
    }

    public void GetNextFloor()
    {
        List<float> curF = new List<float>();
        foreach (GameObject openFloor in openList)
        {
            FloorData floorData = openFloor.GetComponent<FloorData>();
            FloorData data = floorData;
            data.fValue = FValueCalc(data);
            if (data.listed != FloorData.looking.ignore)
            {
                curF.Add(data.fValue);
            }
        }
        float fToGet = curF.Min();

        GetMinF(fToGet);
    }

    private void GetMinF(float fToGet)
    {
        foreach (GameObject openFloor in openList)
        {
            FloorData data = openFloor.GetComponent<FloorData>();
            if (data.fValue == fToGet)
            {
                LookFrom(data);
                break;
            }
        }
    }
}
