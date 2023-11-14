using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineLurker : StateMachineBase
{
    RuleManager manager;
    private bool doingSomething = false;
    public Chest atChest;
    public List<GameObject> keys = new List<GameObject>();
    public List<string> objectives = new List<string>();
    public FloorData spawnPoint;
    public enum State { searching, hiding, running}
    public State state;
    void Start()
    {
        manager = GameObject.Find("Game Manager").GetComponent<RuleManager>();
        Spawn();
        FindRandomChest();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.searching)
        {
            if (this.transform.position == moveScript.end.pos.position && doingSomething == false)
            {
                moveScript.startTime = Time.time;
                doingSomething = true;
                moveScript.canMove = false;
                StartCoroutine(RestartNewPath());
                
            }
        }
    }
    void Spawn()
    {
        int r = Random.Range(0, moveScript.walkable.Count - 1);
        moveScript.start = moveScript.walkable[r];
        moveScript.nextPathNode = moveScript.start;
        moveScript.nextPathNode.listed = FloorData.looking.ignore;
        moveScript.walkable.Remove(moveScript.start);
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

    void DoThing()
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
        foreach (FloorData data in moveScript.pathWay)
        {
            data.parent = null;
        }
        moveScript.pathWay = new List<FloorData>();
        yield return new WaitForSeconds(3f);

        DoThing();
        moveScript.walkable.Add(moveScript.start);
        moveScript.start.listed = FloorData.looking.none;
        moveScript.start = moveScript.end;

        moveScript.nextPathNode = moveScript.start;
        moveScript.nextPathNode.listed = FloorData.looking.ignore;
        moveScript.walkable.Remove(moveScript.start);
        moveScript.curTile = moveScript.start;

        yield return new WaitForSeconds(0f);
        moveScript.canMove = true;
        doingSomething = false;

        if(objectives[0] == "find key")
        {
            FindRandomChest();
        }
        else if (objectives[0] == "unlock door")
        {
            moveScript.end = manager.GetDoorstep();
            moveScript.LookFrom();
        }
        if(moveScript.start = manager.GetDoorstep())
        {
            manager.door.GetComponent<Door>().UseKey(keys[0]);
        }

    }


}
