using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineChaser : MonoBehaviour
{
    public enum State { patrol, chase }
    public State state;

    public Movement moveScript;
    public Movement target;
    public float detectionRange;
    public LayerMask wallsMask;
    void Start()
    {
        Spawn();
        RandomPatrolEnd();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position == moveScript.end.pos.position && moveScript.canMove == true)
        {
            moveScript.startTime = Time.time;
            StartCoroutine(RestartNewPath());
            moveScript.canMove = false;
        }

        if (DetectTarget() == true)
        {
            state = State.chase;
        }
        else
        {
            state = State.patrol;
        }
    }

    void Spawn()
    {
        //moveScript = this.gameObject.GetComponent<Movement>();
        int r = Random.Range(0, moveScript.walkable.Count - 1);
        moveScript.start = moveScript.walkable[r];
        moveScript.nextPathNode = moveScript.start;
        moveScript.nextPathNode.listed = FloorData.looking.ignore;
        moveScript.walkable.Remove(moveScript.start); 
    }

    public void RandomPatrolEnd()
    {
        int r = Random.Range(0, moveScript.walkable.Count - 1);
        moveScript.end = moveScript.walkable[r];
        moveScript.LookFrom();
    }

    public IEnumerator RestartNewPath()
    {
        foreach (FloorData data in moveScript.pathWay)
        {
            data.parent = null;
        }
        moveScript.pathWay = new List<FloorData>();
        yield return new WaitForSeconds(1f);
        moveScript.walkable.Add(moveScript.start);
        moveScript.start.listed = FloorData.looking.none;
        moveScript.start = moveScript.end;
        
        moveScript.nextPathNode = moveScript.start;
        moveScript.nextPathNode.listed = FloorData.looking.ignore;
        moveScript.walkable.Remove(moveScript.start);
        moveScript.curTile = moveScript.start;

        if (state == State.patrol)
        {
            RandomPatrolEnd();
        }

        if(state == State.chase)
        {
            moveScript.end = target.curTile;
            moveScript.LookFrom();
        }

        
    }

    private bool DetectTarget()
    {
        Vector3 dirToTarget = (target.transform.position- transform.position).normalized;
        float distToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (distToTarget < detectionRange)
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
}
