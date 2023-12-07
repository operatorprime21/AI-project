using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorData : MonoBehaviour
{
    //Special 'start' ignore data to prevent parent looping
    public enum looking { none, ignore };
    public looking listed;

    //Fixed base data
    public enum type { walkable, notWalkable };
    public type Type;
    public float x;
    public float y;
    public Transform pos;

    //Individual A* data
    public float[] g;
    public float[] h;
    public float[] f;
    public FloorData[] parent;

    private void Start()
    {
        this.gameObject.name = this.transform.position.x.ToString() + ", " + this.transform.position.z.ToString();
        x = this.transform.position.x;
        y = this.transform.position.z;

        g = new float[] { 0, 0 };
        h = new float[] { 0, 0 };
        f = new float[] { 0, 0 };
        parent = new FloorData[] { null, null };
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
        if(parent[owner.id]!=null)
        {
            owner.pathWay.Add(parent[owner.id]);
            parent[owner.id].GetParent(owner, color);
            Debug.DrawLine(pos.position, parent[owner.id].pos.position, color, 2f);
            parent[owner.id] = null;
        }
    }

    public void ResetData(int id)
    {
        Type = type.walkable;
        listed = looking.none;
        g[id] = 0f;
        h[id] = 0f;
        f[id] = 0f;
        parent[id] = null;
    }
}
