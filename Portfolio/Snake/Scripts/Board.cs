using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
public enum CellType : int
{
    EMPTY = 0,
    WALL = 1,
    SNAKE = 4,
    APPLE = 5
}


public class Board : MonoBehaviour
{
    [SerializeField] GameObject cellPrefab;
    [SerializeField] Color[] cellColor;     //0 = empty, 1 = wall , 2 = tile_1 , 3 = tile_2, 4 = snake , 5 = apple

    [SerializeReference] public int[,] grid = new int[SIZE, SIZE];
    public int[,] Grid { get { return grid; } }
    const int SIZE = 18;
    public int maxSize { get { return SIZE; } }

    GameObject[,] gridUnit = new GameObject[(SIZE - 2), (SIZE - 2)];   //벽 제외 보드유닛
    Vector3 Apple;
    int cnt = 0;
    public void ResetBoard()
    {
        for (int y = 1; y < SIZE - 1; y++)
        {
            for (int x = 1; x < SIZE - 1; x++)
            {
                gridUnit[y - 1, x - 1].GetComponent<SpriteRenderer>().color = cellColor[0];
                grid[y, x] = 0;
            }
        }
        Apple = Vector3.zero;
    }

    public void CreateMap()
    {
        for (int y = 0; y < SIZE; y++)
        {
            for (int x = 0; x < SIZE; x++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3(x, y, 10), Quaternion.identity);
                cell.transform.SetParent(transform);

                //테두리
                if (y == 0 || x == 0 || y == SIZE - 1 || x == SIZE - 1)
                {
                    cell.GetComponent<SpriteRenderer>().color = cellColor[1];
                    grid[y, x] = (int)CellType.WALL;
                }
                else if ((x + y) % 2 == 0)
                {
                    cell.GetComponent<SpriteRenderer>().color = cellColor[2];
                }
                else
                {
                    cell.GetComponent<SpriteRenderer>().color = cellColor[3];
                }
            }
        }

        GameObject parent = GameObject.Find("CellBoard");
        //실제 쓰이는 보드
        for (int y = 1; y < SIZE - 1; y++)
        {
            for (int x = 1; x < SIZE - 1; x++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3(x, y, 10), Quaternion.identity);
                cell.name = y + " / " + x;
                cell.transform.SetParent(parent.transform);
                cell.GetComponent<SpriteRenderer>().color = cellColor[0];
                gridUnit[y - 1, x - 1] = cell;
            }
        }
    }

    public GameObject GetCellObj(int x, int y)
    {
        return gridUnit[y - 1, x - 1];
    }

    public void ChangeGridValue(int x, int y, CellType type)
    {
        if (x <= 0 || x > SIZE - 2 || y <= 0 || y > SIZE - 2) return;

        grid[y, x] = (int)type;

        ChangeGridColor(x, y, (int)type);
    }

    void ChangeGridColor(int x, int y, int num)
    {
        GameObject obj = GetCellObj(x, y);
        obj.GetComponent<SpriteRenderer>().color = cellColor[num];
    }

    public bool CreateApple()
    {
        int x = Random.Range(1, SIZE - 1);
        int y = Random.Range(1, SIZE - 1);

        if (grid[y, x] != (int)CellType.WALL && grid[y, x] != (int)CellType.SNAKE)
        {
            ChangeGridValue(x, y, CellType.APPLE);
            Apple = new Vector3(x, y);
            return true;
        }
        return false;
    }

    public void CreateWall()
    {
        int x = Random.Range(1, SIZE - 1);
        int y = Random.Range(1, SIZE - 1);

        while (grid[y, x] == (int)CellType.APPLE && grid[y, x] == (int)CellType.SNAKE)
        {
            x = Random.Range(1, SIZE - 1);
            y = Random.Range(1, SIZE - 1);
        }
        ChangeGridValue(x, y, CellType.WALL);
    }
    public bool CheckEatApple(int x, int y)
    {
        if (x == Apple.x && y == Apple.y) return true;
        return false;
    }
}
