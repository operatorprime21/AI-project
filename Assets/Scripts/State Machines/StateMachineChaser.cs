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
        SearchNoiseArea();
    }    
    // Update is called once per frame
    void Update()
    {
        if (base.DetectTarget() == true)
        {
            state = State.chasing;
        }
        else
        {
            if (state == State.chasing)
            {
                //moveScript.end = moveScript.nextTile;
                state = State.lostChase;
            }
        }
    }

    public void SearchNoiseArea()
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
                SearchNoiseArea();
                break;
            case State.lostChase:
                //moveScript.EmptyData();
                SearchNoiseArea();
                break;
        }
        StopAllCoroutines();
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
                moveScript.moveSpeed = 4f;
                break;
            case State.lostChase:
                moveScript.moveSpeed = 3.5f;
                break;
            case State.chasing:
                moveScript.moveSpeed = 6.1f;
                StopAllCoroutines();
                moveScript.EmptyData();
                moveScript.end = opponent.curTile;
                base.ResetPath(true);
                moveScript.LookFrom();
                break;
        }
        opponent.stateMachine.opponentNoiseArea = base.NoiseArea(noiseRange);
    }
}
