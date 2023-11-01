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
    public float f;
    public float g;
    public float h;
    public FloorData parent;

    private void Start()
    {
        StartCoroutine(Scan());
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
        yield return new WaitForSeconds(1f);
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
            case type.walkable:
                this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                break;
            case type.notWalkable:
                this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                break;
        }
    }

    public List<GameObject> GetSurroundingFloor()
    {
        float newX = x;
        float newY = y;
        int c = 0;
        List<GameObject> nextFloors = new List<GameObject>();
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
                                nextFloors.Add(floor);
                                c++;
                            }
                        }
                    }
                }
            }
        }

        return nextFloors;
    }
}
