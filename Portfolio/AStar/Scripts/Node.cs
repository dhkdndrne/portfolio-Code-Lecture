using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public SpriteRenderer nodeObj;
   
    public int X;
    public int Y;
    public bool wall;
    public bool start;
    public bool end;

    public int F { get { return H + G; } }
    public int G;  // 이동했던 거리
    public int H;  //( | 가로 | + | 세로 | 장애물 무시한) 목표까지의 거리 

    public Node parent;

    //시작지점
    public bool ChangeStart
    {
        set
        {
            start = value;
            if (value)
            {             
                nodeObj.color = Color.yellow;
                wall = false;
            }
            else
            {
                nodeObj.color = Color.white;
            }
        }
    }
    //끝지점
    public bool ChangeEnd
    {
        set
        {
            end = value;
            if (value)
            {
               
                nodeObj.color = Color.blue;
                wall = false;
            }
            else nodeObj.color = Color.white;
        }
    }
    //벽
    public bool ChangeWall
    {
        set
        {
            Color color = value ? Color.black : Color.white;
            wall = value;
            nodeObj.color = color;
        }
    }

    public Node(int x, int y, bool wall, SpriteRenderer obj)
    {
        X = x;
        Y = y;
        this.wall = wall;
        nodeObj = obj;
    }
}
