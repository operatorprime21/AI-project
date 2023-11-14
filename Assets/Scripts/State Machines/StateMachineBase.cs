using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineBase : MonoBehaviour
{
    public Movement moveScript;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position == moveScript.end.pos.position && moveScript.canMove == true)
        {
            moveScript.startTime = Time.time;
            StartCoroutine(RestartNewPath());
            moveScript.canMove = false;
        }
    }
    public virtual IEnumerator RestartNewPath()
    {
        foreach (FloorData data in moveScript.pathWay)
        {
            data.parent = null;
        }
        moveScript.pathWay = new List<FloorData>();
        yield return new WaitForSeconds(1f);
        moveScript.walkable.Add(moveScript.start);
        moveScript.start.listed = FloorData.looking.none;
        moveScript.start = moveScript.end;

        moveScript.nextPathNode = moveScript.start;
        moveScript.nextPathNode.listed = FloorData.looking.ignore;
        moveScript.walkable.Remove(moveScript.start);
        moveScript.curTile = moveScript.start;
    }
}
