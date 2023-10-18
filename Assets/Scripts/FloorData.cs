using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorData : MonoBehaviour
{
    public enum type { walkable, notWalkable, start, end};
    public type Type;
    public float x;
    public float y;

    private void Start()
    {
        
    }

    private void Update()
    {
        switch (Type)
        {
            case type.start:
                this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                break;
            case type.end:
                this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                break;
            case type.walkable:
                this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                break;
            case type.notWalkable:
                this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                break;
        }
        
    }
}
