using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab; //타일 프리팹
    public Vector2 gridSize;
    public Node[,] grid;

    private void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[(int)gridSize.y, (int)gridSize.x];

        for (int y = 0; y < (int)gridSize.y; y++)
        {
            for (int x = 0; x < (int)gridSize.x; x++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector2(x, y), Quaternion.identity);
                tile.name = "[ " + y + " / " + x + " ]";
                
                grid[y, x] = new Node(x, y, false, tile.GetComponent<SpriteRenderer>());
            }
        }
    }

    //해당 좌표의 노드를 반환하는 함수
    public Node NodePoint(Transform tr)
    {
        int x = (int)tr.position.x;
        int y = (int)tr.position.y;

        return grid[y, x];
    }
}
