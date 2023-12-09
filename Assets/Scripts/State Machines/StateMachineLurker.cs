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
            if (base.ReachedEnd() == true)
            {
                StartCoroutine(RestartNewPath());
            }
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

        if (objectives[0] == "find key")
        {
            FindRandomChest();
        }
        else if (objectives[0] == "unlock door")
        {
            moveScript.end = manager.GetDoorstep();
            moveScript.LookFrom();
        }

    }

    public override void StepEvent()
    {
        switch (state)
        {
            case State.searching:
                {
                    noiseRange = 5;
                    if (objectives[0] == "find key")
                    {
                        if (atChest != null)
                        {
                            SearchChest();
                        }
                    }
                    if (objectives[0] == "unlock door")
                    {
                        if(this.transform.position == manager.GetDoorstep().pos.position && !manager.door.GetComponent<Door>().unlocking)
                        {
                            moveScript.enabled = false;
                            manager.door.GetComponent<Door>().UseKey(keys);
                        }
                    }
                }
                break;
            case State.hiding:
                break;
            case State.running:
                break;
        }
        opponent.stateMachine.opponentNoiseArea = base.NoiseArea(noiseRange);
    }
}
