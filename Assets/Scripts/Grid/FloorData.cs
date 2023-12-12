using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorData : MonoBehaviour
{
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

    public List<FloorData> GetSurroundingFloor(int range)
    {
        float newX = x;
        float newY = y;
        List<FloorData> nextFloors = new List<FloorData>();
        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                if (x != 0 || y != 0)
                {
                    float lookingX = newX + x;
                    float lookingY = newY + y;
                    GameObject floor = GameObject.Find(lookingX.ToString() + ", " + lookingY.ToString());                 
                    if (floor != null)
                    {
                        FloorData nextData = floor.GetComponent<FloorData>();
                        if (nextData.Type != FloorData.type.notWalkable)
                        {
                            nextFloors.Add(floor.GetComponent<FloorData>());
                        }
                    }
                }
            }
        }
        return nextFloors;
    }

    public void GetParent(Movement owner, Color color)
    {
        if(parent[owner.id]!=null && this!= owner.start)
        {
            if(!owner.pathWay.Contains(parent[owner.id]))
            {
                owner.pathWay.Add(parent[owner.id]);
                parent[owner.id].GetParent(owner, color);
                Debug.DrawLine(pos.position, parent[owner.id].pos.position, color, 2f);
                parent[owner.id] = null;
            }
        }
    }

    public void ResetData(int id)
    {
        Type = type.walkable;
        g[id] = 0f;
        h[id] = 0f;
        f[id] = 0f;
        parent[id] = null;
    }
}
