using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectives : MonoBehaviour
{
    public FloorData floor;
    public GameObject item;
    public enum obj {chest, closet }
    public obj objec;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<StateMachineLurker>().objective = this;
        }
    }
}
