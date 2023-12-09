using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineLurker : StateMachineBase
{
    public Chest atChest;
    public List<GameObject> keys = new List<GameObject>();
    public List<string> objectives = new List<string>();
    public enum State { searching, hiding, running }
    public State state;
    void Start()
    {
        base.StartingPosition();

        FindRandomChest();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.searching)
        {
            
        }

        if(base.DetectTarget()==true)
        {
            state = State.running;
        }
        else if(state == State.running)
        {
            if(opponent.stateMachine.opponentNoiseArea.Contains(moveScript.curTile))
            {
                state = State.hiding;
            }
        }

        if(state == State.hiding)
        {

        }
    }

    void FindRandomChest()
    {
        if (manager.chestPosition.Count > 0)
        {
            int r = Random.Range(0, manager.chestPosition.Count);
            FloorData chosenChest = manager.chestPosition[r];
            moveScript.end = chosenChest;
            manager.chestPosition.Remove(chosenChest);

            moveScript.LookFrom();
        }

    }

    void SearchChest()
    {
        if (atChest.item != null)
        {
            keys.Add(atChest.item);
            atChest.item = null;
            objectives.Remove("find key");
        }
        atChest = null;
        //atChest.gameObject.SetActive(false);
    }

    public IEnumerator RestartNewPath()
    {
        base.ResetPath(false);

        yield return new WaitForSeconds(3f);

        switch (state)
        {
            case State.searching:
                if (objectives[0] == "find key")
                {
                    if (atChest != null)
                    {
                        SearchChest();
                    }
                    FindRandomChest();
                }
                else if (objectives[0] == "unlock door")
                {
                    moveScript.end = manager.GetDoorstep();
                    moveScript.LookFrom();
                    if (this.transform.position == manager.GetDoorstep().pos.position && !manager.door.GetComponent<Door>().unlocking)
                    {
                        moveScript.enabled = false;
                        manager.door.GetComponent<Door>().UseKey(keys);
                    }
                }
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
            case State.searching:
                {
                    noiseRange = 10;
                    moveScript.moveSpeed = 4f;
                }
                break;
            case State.hiding:
                {
                    noiseRange = 20;
                    moveScript.moveSpeed = 2.5f;
                }
                break;
            case State.running:
                {
                    noiseRange = 2;
                    moveScript.moveSpeed = 6f;
                }
                break;
        }
        opponent.stateMachine.opponentNoiseArea = base.NoiseArea(noiseRange);
    }
}
