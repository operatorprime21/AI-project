using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachineBase : MonoBehaviour
{
    public Movement moveScript;
    protected RuleManager manager;
    public List<FloorData> startPos = new List<FloorData>();

    public void StartingPosition()
    {
        manager = GameObject.Find("Game Manager").GetComponent<RuleManager>();
        int r = Random.Range(0, startPos.Count);
        moveScript.start = startPos[r];
        moveScript.nextPathNode = moveScript.start;
        moveScript.nextPathNode.listed = FloorData.looking.ignore;
        moveScript.walkable.Remove(moveScript.start);
    }

    public void ResetPath(bool interupted)
    {
        moveScript.pathWay = new List<FloorData>();

        moveScript.walkable.Add(moveScript.start);
        moveScript.start.listed = FloorData.looking.none;
        if(interupted)
        {
            moveScript.start = moveScript.nextTile;
        }
        else
        {
            moveScript.start = moveScript.curTile;
        }
        
        moveScript.nextPathNode = moveScript.start;
        moveScript.nextPathNode.listed = FloorData.looking.ignore;
        moveScript.walkable.Remove(moveScript.start);
        moveScript.curTile = moveScript.start;
    }



    public abstract void StepEvent();
    public bool ReachedEnd()
    {
        if (Vector3.Distance(transform.position, moveScript.end.pos.position) < 0.01f && moveScript.canMove == true)
        {
            moveScript.startTime = Time.time;
            moveScript.canMove = false;
            return true;
        }
        else
        {
            return false;
        }
    }
}
