using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool unlocked;
    public List<GameObject> keys = new List<GameObject>();
    public FloorData doorData;
    public FloorData step;
    public GameObject doorMesh;
    public GameObject lockR;
    public GameObject lockG;
    public GameObject lockB;

    private RuleManager manager;
    private void Start()
    {
        manager = GameObject.Find("Game Manager").GetComponent<RuleManager>();
    }
    public void UseKey(GameObject playerkey)
    {
        keys.Add(playerkey);

        if(playerkey == manager.keyR)
        {
            lockR.SetActive(false);
        }
        if (playerkey == manager.keyG)
        {
            lockG.SetActive(false);
        }
        if (playerkey == manager.keyB)
        {
            lockB.SetActive(false);
        }


        if (IsDoorUnlocked() == true)
        {
            doorData.Type = FloorData.type.walkable;
            doorMesh.SetActive(false);
        }
    }

    private bool IsDoorUnlocked()
    {
        if (keys.Count == 3)
        {
            return true;
        }
        else return false;
    }
}
