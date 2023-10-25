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
    public float fValue;

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
        if (listed == looking.close)
        {
            this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
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
}
