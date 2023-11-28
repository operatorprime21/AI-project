using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public FloorData curTile;
    public FloorData nextTile;

    public bool canMove = false;
    private int revIndex = 0;
    public float startTime = 0f;
    public float moveSpeed;

    public FloorData start;
    public FloorData end;
    public FloorData nextPathNode;

    private StateMachineBase stateMachine;

    public List<FloorData> openList = new List<FloorData>();
    public List<FloorData> closeList = new List<FloorData>();
    public List<FloorData> pathWay = new List<FloorData>();
    public List<FloorData> walkable = new List<FloorData>();

    public bool oneByOne = false;
    // Start is called before the first frame update
    void Start()
    {
        startInit();
    }
    void startInit()
    {
        FloorManager manager = GameObject.Find("Manager").GetComponent<FloorManager>();
        stateMachine = this.gameObject.GetComponent<StateMachineBase>();
        foreach (FloorData data in manager.floorGrid)
        {
            if (data.Type == FloorData.type.walkable)
            {
                walkable.Add(data);
            }
        }
    }
    void Update()
    {
        float movingTime =+ (Time.time - startTime) * moveSpeed;
        if(canMove)
        {
            this.transform.position = Vector3.Lerp(curTile.pos.position, nextTile.pos.position, movingTime);
            if(Vector3.Distance(transform.position, nextTile.pos.position)<= 0.01f)
            {
                startTime = Time.time;
                stateMachine.StepEvent();
                ChangeTile();
            }
        }
    }
    public void ChangeTile()
    {
        revIndex++;
        if(pathWay.Count - revIndex == 0)
        {
            curTile = pathWay[1];
            nextTile = end;
            revIndex = 0;
        }
        else
        {
            curTile = pathWay[pathWay.Count - revIndex];
            nextTile = pathWay[pathWay.Count - revIndex - 1];
        }    
    }
    public void LookFrom()
    {
        bool looking = true;
        foreach (FloorData floor in nextPathNode.GetSurroundingFloor())
        {
            if (floor.listed == FloorData.looking.none)
            {
                floor.parent = nextPathNode;
                openList.Add(floor);
                floor.listed = FloorData.looking.open;
            }
            if (floor == end)
            {
                pathWay.Add(floor);
                looking = false;
                floor.GetParent(this);
                curTile = pathWay[pathWay.Count - 1];
                nextTile = pathWay[pathWay.Count - 2];
                canMove = true;
                EmptyData();
            }
        }
        if (looking)
        {
            GetNextFloor();
        }
    }
    public float FValueCalc(FloorData floorFrom)
    {
        float gXFromParent = Mathf.Abs(floorFrom.parent.x - floorFrom.x);
        float gYFromParent = Mathf.Abs(floorFrom.parent.y - floorFrom.y);
        float gFromParent = Mathf.Sqrt(gXFromParent * gXFromParent + gYFromParent * gYFromParent);

        float hX = Mathf.Abs(end.x - floorFrom.x);
        float hY = Mathf.Abs(end.y - floorFrom.y);
        float hValue = Mathf.Sqrt(hX * hX + hY * hY);

        floorFrom.g = gFromParent + floorFrom.parent.g;

        floorFrom.h = hValue;
        return floorFrom.g + floorFrom.h;
    }
    public void GetNextFloor()
    {
        List<float> curF = new List<float>();
        foreach (FloorData data in openList)
        {
            data.f = FValueCalc(data);
            if (data.listed != FloorData.looking.ignore)
            {
                curF.Add(data.f);
            }
        }
        MinFValue(curF);
    }
    private void MinFValue(List<float> curF)
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
        foreach (FloorData data in openList)
        {
            if (data.f == fToGet)
            {
                nextPathNode = data;
                data.listed = FloorData.looking.close;
                closeList.Add(data);
                openList.Remove(data);
                //if(oneByOne)
                //{
                //    end = data;
                //    pathWay.Add(data);
                //    data.GetParent(this);
                //    curTile = pathWay[pathWay.Count - 1];
                //    nextTile = pathWay[pathWay.Count - 2];
                //    canMove = true;
                //    EmptyData();
                //}
                break;
            }
        }
        //if (oneByOne == false)
        //{
            LookFrom();
        //}
        //else
        //{
        //    oneByOne = false;
        //}
        
    }
    public void EmptyData()
    {
        foreach (FloorData data in openList)
        {
            data.ResetData();
        }
        openList = new List<FloorData>();
        foreach (FloorData data in closeList)
        {
            data.ResetData();
        }
        closeList = new List<FloorData>();
    }

    //public void LookFrom(string next)
    //{
    //    oneByOne = true;
    //    foreach (FloorData floor in nextPathNode.GetSurroundingFloor())
    //    {
    //        if (floor.listed == FloorData.looking.none)
    //        {
    //            floor.parent = nextPathNode;
    //            openList.Add(floor);
    //            floor.listed = FloorData.looking.open;
    //        }
    //    }
    //    GetNextFloor();
    //}
}
