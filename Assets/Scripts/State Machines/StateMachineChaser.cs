using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineChaser : StateMachineBase
{
    public enum State { patrol, chase, ph }
    public State state;

    public Movement target;
    public float detectionRange;
    public LayerMask wallsMask;
    void Start()
    {
        base.StartingPosition();
        RandomPatrolEnd();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (DetectTarget() == true)
        {
            state = State.chase;
        }
        //else
        //{
        //    state = State.patrol;
        //}
    }

    public void RandomPatrolEnd()
    {
        int r = Random.Range(0, moveScript.walkable.Count - 1);
        moveScript.end = moveScript.walkable[r];
        moveScript.LookFrom();
    }

    public IEnumerator RestartNewPath()
    {
        base.ResetPath(false);
        yield return new WaitForSeconds(2f);

        switch (state)
        {
            case State.patrol:
                RandomPatrolEnd();
                break;
            case State.chase:
                
                break;
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

    public override void StepEvent()
    {
        if (base.ReachedEnd() == true)
        {
            StartCoroutine(RestartNewPath());
        }
        switch (state)
        {
            case State.patrol:
                
                break;
            case State.chase:
                moveScript.EmptyData();
                moveScript.end = target.curTile;
                base.ResetPath(true);
                moveScript.LookFrom();
                break;
        }
    }
}
