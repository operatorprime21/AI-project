using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineChaser : StateMachineBase
{
    public enum State { patrol, chasing, lostChase }
    public State state;

    void Start()
    {
        StartCoroutine(StartRest());
    }

    IEnumerator StartRest()
    {
        yield return new WaitForSeconds(2f);
        base.StartingPosition();
        RandomPatrolEnd();
    }    
    // Update is called once per frame
    void Update()
    {
        if (base.DetectTarget() == true)
        {
            state = State.chasing;
            Debug.Log(base.DetectTarget());
        }
        else
        {
            if(state == State.chasing)
            {
                state = State.lostChase;
            }
        }    
    }

    public void RandomPatrolEnd()
    {
        int r = Random.Range(0, opponentNoiseArea.Count);
        moveScript.end = opponentNoiseArea[r];
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
            case State.lostChase:
                RandomPatrolEnd();
                break;
        }
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
            case State.chasing:
                moveScript.EmptyData();
                moveScript.end = opponent.curTile;
                base.ResetPath(true);
                moveScript.LookFrom();
                break;
        }
        opponent.stateMachine.opponentNoiseArea = base.NoiseArea(noiseRange);
    }
}
