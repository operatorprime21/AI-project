using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineLurker : StateMachineBase
{
    
    private bool doingSomething = false;
    public Chest atChest;
    public List<GameObject> keys = new List<GameObject>();
    public List<string> objectives = new List<string>();
    public enum State { searching, hiding, running}
    public State state;
    void Start()
    {
        base.StartingPosition();
        
        FindRandomChest();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.searching)
        {
            if (base.ReachedEnd() == true && doingSomething == false)
            {
                if (this.transform.position == manager.GetDoorstep().pos.position && manager.door.GetComponent<Door>().unlocking == false)
                {
                    moveScript.enabled = false;
                    if(keys.Count == 0)
                    {
                        this.gameObject.SetActive(false);
                    }
                    else
                    {
                        manager.door.GetComponent<Door>().UseKey(keys[0]);
                        keys.RemoveAt(0);
                    }
                }
                else
                {
                    StartCoroutine(RestartNewPath());
                    doingSomething = true;
                }
            }
        }
    }
    
    void FindRandomChest()
    {
        if(manager.chestPosition.Count>0)
        {
            int r = Random.Range(0, manager.chestPosition.Count - 1);
            FloorData chosenChest = manager.chestPosition[r];
            moveScript.end = chosenChest;
            manager.chestPosition.Remove(chosenChest);

            moveScript.LookFrom();
        }
        
    }

    void SearchChest()
    {
        if(atChest.item != null)
        {
            keys.Add(atChest.item);
            atChest.item = null;
            objectives.Remove("find key");
        }
    }

    public IEnumerator RestartNewPath()
    {
        base.ResetPath();

        yield return new WaitForSeconds(3f);
        doingSomething = false;
        SearchChest();
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
                break;
            case State.hiding:
                break;
            case State.running:
                break;
        }
    }
}
