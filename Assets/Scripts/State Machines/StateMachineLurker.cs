using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineLurker : StateMachineBase
{
    public Objectives objective;
    public List<GameObject> keys = new List<GameObject>();
    public List<string> objectives = new List<string>();
    public enum State { searching, hiding, running, inLocker }
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
            if (opponent.stateMachine.opponentNoiseArea.Contains(moveScript.curTile))
            {
                state = State.hiding;
            }
        }

        if(base.DetectTarget()==true && state == State.hiding)
        {
            state = State.running;
        }
        
        if (base.DetectTarget() == false && state == State.inLocker)
        {
            if (opponent.stateMachine.opponentNoiseArea.Contains(moveScript.curTile))
            {
                state = State.hiding;
            }
        }

        //if(state == State.hiding)
        //{

        //}
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

    void FindCloset()
    {
        List<float> dists = new List<float>();
        foreach(FloorData closet in manager.closetPosition)
        {
            //float d = Vector3.Distance(this.transform.position, closet.pos.transform.position);
            float hX = Mathf.Abs(closet.x - moveScript.curTile.x);
            float hY = Mathf.Abs(closet.y - moveScript.curTile.y);
            float hValue = Mathf.Sqrt(hX * hX + hY * hY);
            closet.h[moveScript.id] = hValue;
            dists.Add(hValue);
        }
        for (int i = 0; i <= dists.Count; i++)
        {
            if (dists.Count > 1)
            {
                if (dists[i] < dists[i + 1])
                {
                    dists.RemoveAt(i + 1);
                    i--; 
                }
                else
                {
                    dists.RemoveAt(i);
                    i--;
                }
            }
        }
        foreach(FloorData closet in manager.closetPosition)
        {
            if(dists[0] == closet.h[moveScript.id])
            {
                moveScript.end = closet;
                break;
            }
        }
        moveScript.LookFrom();
    }

    void SearchChest()
    {
        if (objective.item != null)
        {
            keys.Add(objective.item);
            objective.item = null;
            objectives.Remove("find key");
        }
        objective = null;
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
                    if (objective != null)
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
                 noiseRange = 10;
                 moveScript.moveSpeed = 4f;
                 break;
            case State.hiding:
                 base.ResetPath(true);
                 StopAllCoroutines();
                 moveScript.EmptyData();
                 noiseRange = 20;
                 moveScript.moveSpeed = 2.5f;
                 break;
            case State.running:
                 base.ResetPath(true);
                 StopAllCoroutines();
                 moveScript.EmptyData();
                 noiseRange = 2;
                 moveScript.moveSpeed = 6f;
                 break;
        }
        opponent.stateMachine.opponentNoiseArea = base.NoiseArea(noiseRange);
    }
}
