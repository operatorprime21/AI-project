using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineChaser : StateMachineBase
{
    public enum State { patrol, chase }
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
        if (base.ReachedEnd() == true)
        {
            StartCoroutine(RestartNewPath());
        }

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
        base.ResetPath();
        yield return new WaitForSeconds(2f);

        if (state == State.patrol)
        {
            RandomPatrolEnd();
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
        switch (state)
        {
            case State.patrol:
                break;
            case State.chase:
                base.ResetPath();
                moveScript.end = target.curTile;
                Debug.Log(moveScript.end);
                moveScript.LookFrom();
                break;
        }
    }
}
