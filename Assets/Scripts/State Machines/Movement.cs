using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public FloorData curTile;
    public FloorData nextTile;

    public bool canMove = false;
    public float startTime = 0f;
    public float moveSpeed;

    public FloorData start;
    public FloorData end;
    public FloorData nextPathNode;

    public StateMachineBase stateMachine;

    public List<FloorData> openList = new List<FloorData>();
    public List<FloorData> closeList = new List<FloorData>();
    public List<FloorData> pathWay = new List<FloorData>();
    public List<FloorData> walkable = new List<FloorData>();

    public Color thisColor;
    public int id;
    // Start is called before the first frame update
    void Start()
    {
        startInit();
    }
    void startInit()
    {
        FloorManager manager = GameObject.Find("Grid Manager").GetComponent<FloorManager>();
        stateMachine = this.gameObject.GetComponent<StateMachineBase>();
        List<FloorData> grid = new List<FloorData>(manager.floorGrid);
        foreach (FloorData data in grid)
        {
            if (data.Type == FloorData.type.walkable)
            {
                walkable.Add(data);
            }
        }
    }
    void Update()
    {
        LerpMovement();
    }
    private void LerpMovement()
    {
        float movingTime = +(Time.time - startTime) * moveSpeed;
        if (canMove)
        {
            if (pathWay != null)
            {
                this.transform.position = Vector3.Lerp(curTile.pos.position, nextTile.pos.position, movingTime);
                if (Vector3.Distance(transform.position, nextTile.pos.position) <= 0.01f)
                {
                    startTime = Time.time;
                    ChangeTile();
                    stateMachine.StepEvent();
                }
            }
        }
    }

    public void ChangeTile()
    {
        if (pathWay.Count >= 2)
        {
            curTile = pathWay[pathWay.Count - 1];
            nextTile = pathWay[pathWay.Count - 2];
            pathWay.RemoveAt(pathWay.Count - 1);
        }
        else
        {
            curTile = nextTile;
        }
    }
    public void LookFrom()
    {
        bool looking = true;
        if (start != end)
        {
            foreach (FloorData floor in nextPathNode.GetSurroundingFloor(1))
            {
                if (!openList.Contains(floor) && !closeList.Contains(floor))
                {
                    floor.parent[id] = nextPathNode;
                    openList.Add(floor);
                }
                if (floor == end)
                {
                    if(!pathWay.Contains(floor))
                    {
                        pathWay.Add(floor);
                    }
                    looking = false;
                    floor.GetParent(this, thisColor);
                    curTile = pathWay[pathWay.Count - 1];
                    nextTile = pathWay[pathWay.Count - 2];
                    canMove = true;
                    EmptyData();
                    break;
                }
            }
        }
        else
        {
            looking = false;
            canMove = true;
            EmptyData();
        }

        if (looking)
        {
            GetNextFloor();
        }
    }
    public void GetNextFloor()
    {
        List<float> curF = new List<float>();
        foreach (FloorData data in openList)
        {
            data.f[id] = FValueCalc(data);
            if (data!=start)
            {
                curF.Add(data.f[id]);
            }
        }
        MinFValue(curF);
    }
    public float FValueCalc(FloorData floorFrom)
    {
        float gXFromParent = Mathf.Abs(floorFrom.parent[id].x - floorFrom.x);
        float gYFromParent = Mathf.Abs(floorFrom.parent[id].y - floorFrom.y);
        float gFromParent = Mathf.Sqrt(gXFromParent * gXFromParent + gYFromParent * gYFromParent);

        float hX = Mathf.Abs(end.x - floorFrom.x);
        float hY = Mathf.Abs(end.y - floorFrom.y);
        float hValue = Mathf.Sqrt(hX * hX + hY * hY);

        floorFrom.g[id] = gFromParent + floorFrom.parent[id].g[id];

        floorFrom.h[id] = hValue;
        return floorFrom.g[id] + floorFrom.h[id];
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
            if (data.f[id] == fToGet)
            {
                nextPathNode = data;
                closeList.Add(data);
                openList.Remove(data);
                break;
            }
        }
        LookFrom();
    }
    public void EmptyData()
    {
        foreach (FloorData data in openList)
        {
            data.ResetData(id);
        }
        openList = new List<FloorData>();
        foreach (FloorData data in closeList)
        {
            data.ResetData(id);
        }
        closeList = new List<FloorData>();
    }
}
