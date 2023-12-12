using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateMachineLurker : StateMachineBase
{
    public Objectives objective;
    public List<GameObject> keys = new List<GameObject>();
    public List<string> objectives = new List<string>();
    public int hp = 3;
    bool canDamage = true;
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
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Chaser" && canDamage == true)
        {
            hp--;
            canDamage = false;
            StartCoroutine(DamageCD());
        }
        if(hp<=0)
        {
            SceneManager.LoadScene(1);
        }
    }
    IEnumerator DamageCD()
    {
        yield return new WaitForSeconds(3f);
        canDamage = true;
    }
    private void StateSwitch()
    {
        switch (state)
        {
            case State.searching:
                if (base.HearTarget())
                {
                    state = State.hiding;
                    moveScript.canMove = true;
                }
                else
                {
                    state = State.searching;
                    moveScript.canMove = true;
                }
                break;
            case State.hiding:
                if (base.DetectTarget())
                {
                    state = State.running;
                    moveScript.canMove = true;
                    StopAllCoroutines();
                }
                else
                {
                    if (base.HearTarget())
                    {
                        state = State.hiding;
                    }
                    else state = State.searching;
                }
                break;

            case State.running:
                {
                    if (objective != null)
                    {
                        if (objective.GetComponent<Collider>().tag == "Closet")
                        {
                            state = State.inLocker;
                            StartCoroutine(HideTime());
                        }
                    }
                }
                break;

            case State.inLocker:
                if (base.DetectTarget() == false)
                {
                    if (base.HearTarget())
                    {
                        state = State.inLocker;
                    }
                    else state = State.hiding;
                }
                break;
        }
    }

    IEnumerator HideTime()
    {
        yield return new WaitForSeconds(1.5f);
        if (base.HearTarget())
        {
            state = State.inLocker;
            StartCoroutine(HideTime());
        }
        else
        {
            state = State.hiding;
            base.ResetPath(false);
            ObjectiveSearch();
            moveScript.LookFrom();
            moveScript.canMove = true; 

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
            moveScript.canMove = true;
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
    }

    public IEnumerator RestartNewPath()
    {
        base.ResetPath(false);

        yield return new WaitForSeconds(1.5f);

        switch (state)
        {
            case State.searching:
                ObjectiveSearch();
                break;
            case State.hiding:
                ObjectiveSearch();
                break;
            case State.running:
                break;
        }

    }

    private void ObjectiveSearch()
    {
        if (objectives[0] == "find key")
        {
            if (objective != null)
            {
                if(objective.GetComponent<Collider>().tag == "Chest")
                {
                    SearchChest();
                }
            }
            FindRandomChest();
        }
        if (objectives[0] == "unlock door")
        {
            if (this.transform.position == manager.GetDoorstep().pos.position && !manager.door.GetComponent<Door>().unlocking)
            {
                moveScript.enabled = false;
                manager.door.GetComponent<Door>().UseKey(keys);
                opponent.stateMachine.sightRange = 100f;
            }
            moveScript.end = manager.GetDoorstep();
            moveScript.LookFrom();
            moveScript.canMove = true;
        }
    }

    public override void StepEvent()
    {
        StateSwitch();
        if (base.ReachedEnd() == true)
        {
            StartCoroutine(RestartNewPath());
        }
        switch (state)
        {
            case State.searching:
                 noiseRange = 20;
                 moveScript.moveSpeed = 4f;
                 break;
            case State.hiding:
                 noiseRange = 20;
                 moveScript.moveSpeed = 2.5f;
                 break;
            case State.running:
                 StopAllCoroutines();
                 moveScript.EmptyData();
                 base.ResetPath(true);
                 FindCloset();
                 noiseRange = 10;
                 moveScript.moveSpeed = 6f;
                 break;
            case State.inLocker:
                noiseRange = 10;
                moveScript.moveSpeed = 2.5f;
                break;
        }
        opponent.stateMachine.opponentNoiseArea = base.HearTarget(noiseRange);
    }
}
