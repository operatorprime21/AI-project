using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDefinitions : MonoBehaviour
{
    public int startX;
    public int startY;

    public int lengthX;
    public int lengthY;

    public int[,] wallData;
    public List<int[,]> walls = new List<int[,]>();
    //List<int[,]> walls = new List<int[,]>();

    private void Start()
    {
        wallData = new int[lengthX, lengthY];
        
        for(int x = 0; x<= lengthX; x++)
        {
            for (int y = 0; y <= lengthY; y++)
            {
                //walls.Add(wallData[x, y]);
                //walls.Add(newWall);
            }
        }
       
    }
}
