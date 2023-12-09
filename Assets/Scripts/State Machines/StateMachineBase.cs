using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachineBase : MonoBehaviour
{
    public Movement moveScript;
    public Movement opponent;

    public float sightRange;
    public LayerMask wallsMask;

    protected RuleManager manager;
    public List<FloorData> startPos = new List<FloorData>();

    public List<FloorData> opponentNoiseArea = new List<FloorData>();
    public int noiseRange;
    public void StartingPosition()
    {
        manager = GameObject.Find("Game Manager").GetComponent<RuleManager>();
        int r = Random.Range(0, startPos.Count);
        moveScript.start = startPos[r];
        moveScript.nextPathNode = moveScript.start;
        moveScript.walkable.Remove(moveScript.start);
    }

    public void ResetPath(bool interupted)
    {
        moveScript.pathWay = new List<FloorData>();

        moveScript.walkable.Add(moveScript.start);
        if(interupted)
        {
            moveScript.start = moveScript.nextTile;
        }
        else
        {
            moveScript.start = moveScript.curTile;
        }
        
        moveScript.nextPathNode = moveScript.start;
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
    public bool DetectTarget()
    {
        Vector3 dirToTarget = (opponent.transform.position - transform.position).normalized;
        float distToTarget = Vector3.Distance(transform.position, opponent.transform.position);
        if (distToTarget < sightRange)
        {
            if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, wallsMask))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else return false;
    }

    public List<FloorData> NoiseArea(int range)
    {
        List<FloorData> area = new List<FloorData>();
        area = moveScript.curTile.GetSurroundingFloor(range);
        return area;
    }
}
