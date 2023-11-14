using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineLurker : StateMachineBase
{
    RuleManager manager;
    private bool doingSomething = false;
    public enum State { searching, hiding, running}
    public State state;
    void Start()
    {
        manager = GameObject.Find("Game Manager").GetComponent<RuleManager>();
        FindRandomChest();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.searching)
        {
            if (this.transform.position == moveScript.end.pos.position && doingSomething == false)
            {
                moveScript.canMove = false;
                StartCoroutine(DoThing());
                doingSomething = true;
            }
        }
    }

    void FindRandomChest()
    {
        int r = Random.Range(0, manager.chestPosition.Count - 1);
        FloorData chosenChest = manager.chestPosition[r];
        moveScript.end = chosenChest;
        moveScript.LookFrom();
    }

    IEnumerator DoThing()
    {
        yield return new WaitForSeconds(2f);
    }
}
