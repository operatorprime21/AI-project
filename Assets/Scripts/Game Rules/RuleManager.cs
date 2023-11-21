using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleManager : MonoBehaviour
{
    public StateMachineBase hunter;
    public StateMachineBase lurker;

    public GameObject keyR;
    public GameObject keyG;
    public GameObject keyB;

    public GameObject door;

    public List<GameObject> chests = new List<GameObject>();
    public List<FloorData> chestPosition = new List<FloorData>();
    void Start()
    {
        FindChests();
        foreach (GameObject chest in GameObject.FindGameObjectsWithTag("Chest"))
        {
            chestPosition.Add(chest.GetComponent<Chest>().floor);
        }
        RandomizeItem();
    }

    public List<GameObject> FindChests()
    {
        chests = new List<GameObject>();
        foreach (GameObject chest in GameObject.FindGameObjectsWithTag("Chest"))
        {
            chests.Add(chest);
        }
        return chests;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RandomizeItem()
    {
        for (int i = 0; i <= 2; i++)
        {
            int r = Random.Range(0, chests.Count);
            if(i == 0)
            {
                GameObject chest1 = chests[r];
                chest1.GetComponent<Chest>().item = keyR;
                chests.Remove(chest1);
            }
            if (i == 1)
            {
                GameObject chest2 = chests[r];
                chest2.GetComponent<Chest>().item = keyG;
                chests.Remove(chest2);
            }
            if (i == 2)
            {
                GameObject chest3 = chests[r];
                chest3.GetComponent<Chest>().item = keyB;
            }
        }
        chests = FindChests();
    }

    public FloorData GetDoorstep()
    {
        FloorData step = door.GetComponent<Door>().step;
        return step;
    }
}
