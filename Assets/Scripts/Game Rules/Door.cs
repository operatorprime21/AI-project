using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Door : MonoBehaviour
{
    public bool unlocked;
    public List<GameObject> keys = new List<GameObject>();
    public FloorData doorData;
    public FloorData step;
    public bool unlocking = false;
    public GameObject doorMesh;
    public GameObject lockR;
    public GameObject lockG;
    public GameObject lockB;

    private RuleManager manager;
    private void Start()
    {
        manager = GameObject.Find("Game Manager").GetComponent<RuleManager>();
    }
    public void UseKey(List<GameObject> playerkey)
    {
        StartCoroutine(UseKeyTime(playerkey));
    }

    private IEnumerator UseKeyTime(List<GameObject> playerkey)
    {
        unlocking = true;
        for(int i = 0; i <= 2; i++)
        {
            keys.Add(playerkey[i]);
            yield return new WaitForSeconds(1f);
            DisableLockMesh(playerkey[i]);
        }
        yield return new WaitForSeconds(1f);
        if (IsDoorUnlocked() == true)
        {
            doorData.Type = FloorData.type.walkable;
            doorMesh.SetActive(false);
            SceneManager.LoadScene(2);
        }
        unlocking = false;
    }

    private void DisableLockMesh(GameObject playerkey)
    {
        if (playerkey == manager.keyR)
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
