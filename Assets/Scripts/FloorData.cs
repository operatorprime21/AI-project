using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorData : MonoBehaviour
{
    public enum type { walkable, notWalkable, start, end};
    public type Type;
    public enum looking { none, open, close, ignore };
    public looking listed;

    public float x;
    public float y;
    public float g;
    public float h;
    public float f;

    public Transform pos;
    public FloorData parent;

    private void Start()
    {
        //StartCoroutine(Scan());
        StartCoroutine(InitColor());
        this.gameObject.name = this.transform.position.x.ToString() + ", " + this.transform.position.z.ToString();
        x = this.transform.position.x;
        y = this.transform.position.z;
    }

    private void Update()
    {

    }

    IEnumerator Scan()
    {
        yield return new WaitForSeconds(.2f);
        if (listed == looking.open)
        {
            this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        }
        if (listed == looking.close)
        {
            this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
        }
        StartCoroutine(Scan());
    }

    IEnumerator InitColor()
    {
        yield return new WaitForSeconds(0.5f);
        switch (Type)
        {
            case type.start:
                this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                break;
            case type.end:
                this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                break;
        }
    }

    public List<FloorData> GetSurroundingFloor()
    {
        float newX = x;
        float newY = y;
        int c = 0;
        List<FloorData> nextFloors = new List<FloorData>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 || y != 0)
                {
                    float lookingX = newX + x;
                    float lookingY = newY + y;
                    GameObject floor = GameObject.Find(lookingX.ToString() + ", " + lookingY.ToString());                 
                    if (floor != null)
                    {
                        //Debug.Log(floor);
                        FloorData nextData = floor.GetComponent<FloorData>();
                        if (nextData.listed == FloorData.looking.none)
                        {
                            if (nextData.Type != FloorData.type.notWalkable)
                            {
                                nextFloors.Add(floor.GetComponent<FloorData>());
                                c++;
                            }
                        }
                    }
                }
            }
        }

        return nextFloors;
    }

    public void GetParent()
    {
        if(parent!=null)
        {
            StopAllCoroutines();
            Movement owner = GameObject.Find("AI Hunter").GetComponent<Movement>();
            owner.pathWay.Add(parent);
            parent.GetParent();
            this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
        
    }

    public void ResetData()
    {
        Type = type.walkable;
        listed = looking.none;
        parent = null;
    }
}
