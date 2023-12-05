using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorData : MonoBehaviour
{
    public enum type { walkable, notWalkable};
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
        this.gameObject.name = this.transform.position.x.ToString() + ", " + this.transform.position.z.ToString();
        x = this.transform.position.x;
        y = this.transform.position.z;
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

    public void GetParent(Movement owner, Color color)
    {
        if(parent!=null)
        {
            //wsStopAllCoroutines();
            owner.pathWay.Add(parent);
            parent.GetParent(owner, color) ;
            Debug.DrawLine(pos.position, parent.pos.position, color, 2f);
            parent = null;
        }
    }

    //void OnDrawGizmosSelected()
    //{
    //    if (parent != null)
    //    {
    //        //Gizmos.color = Color.blue;
           
    //    }
    //}

    public void ResetData()
    {
        Type = type.walkable;
        listed = looking.none;
        g = 0;
        h = 0;
        f = 0;
        parent = null;
    }
}
