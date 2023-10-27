using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FloorManager : MonoBehaviour
{
    public FloorData start;
    public FloorData end;
    public FloorData nextPathNode;

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
        nextPathNode = start;
        yield return new WaitForSeconds(.1f);
        StartCoroutine(LookFrom());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnFloors()
    {
        for(int x = -10; x<11; x++)
        {
            for (int y = -10; y<11;y++)
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

    IEnumerator LookFrom()
    {
        bool looking = true;
        yield return new WaitForSeconds(0.01f);
        foreach(GameObject floors in nextPathNode.GetSurroundingFloor())
        {
            if(floors.GetComponent<FloorData>().listed == FloorData.looking.none)
            {
                openList.Add(floors);
                floors.GetComponent<FloorData>().listed = FloorData.looking.open;
            }
            if (floors.GetComponent<FloorData>().Type == FloorData.type.end)
            {
                looking = false;
                Debug.Log("REACHED!");
            }

        }
        if(looking)
        {
            GetNextFloor();
        }
    }

    public float FValueCalc(FloorData floorFrom)
    {
        float gX = Mathf.Abs(start.x - floorFrom.x);
        float gY = Mathf.Abs(start.y - floorFrom.y);
        float hX = Mathf.Abs(end.x - floorFrom.x);
        float hY = Mathf.Abs(end.y - floorFrom.y);
        float gValue = Mathf.Sqrt(gX * gX + gY * gY);
        float hValue = Mathf.Sqrt(hX * hX + hY * hY); 
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
        GetMinFValue(curF);
    }

    private void GetMinFValue(List<float> curF)
    {
        for (int i = 0; i <= curF.Count; i++)
        {
            if (curF.Count > 1)
            {
                if (curF[i] < curF[i + 1])
                {
                    curF.RemoveAt(i + 1);
                    i--;
                }
                else
                {
                    curF.RemoveAt(i);
                    i--;
                }
            }
        } 
        GetMinF(curF[0]);
    }

    private void GetMinF(float fToGet)
    {
        foreach (GameObject openFloor in openList)
        {
            FloorData data = openFloor.GetComponent<FloorData>();
            if (data.fValue == fToGet)
            {
                nextPathNode = data;
                data.listed = FloorData.looking.close;
                openList.Remove(data.gameObject);
                break;
            }  
        }
        StartCoroutine(LookFrom());
    }
}
