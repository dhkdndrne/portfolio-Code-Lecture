using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    List<Node> openList = new List<Node>();
    List<Node> closeList = new List<Node>();
    List<Node> finalNodeList = new List<Node>();

    Node currNode;
    TIleSetting tileset;
    Grid grid;


    // 수평 수직은 10 대각선은 14의 비용

    private void Start()
    {
        tileset = FindObjectOfType<TIleSetting>();
        grid = FindObjectOfType<Grid>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {            
            StartCoroutine(FindPath());
        }
    }
    void AddCloseList(Node node)
    {
        closeList.Add(node);
        if (!node.Equals(tileset.startNode) && !node.Equals(tileset.endNode))
            node.nodeObj.color = Color.red;
    }
    void AddOpenList(Node node)
    {
        openList.Add(node);
        if (!node.Equals(tileset.startNode) && !node.Equals(tileset.endNode))
            node.nodeObj.color = Color.green;
    }

    IEnumerator FindPath()
    {
        openList.Add(tileset.startNode);

        while (openList.Count > 0)
        {
            currNode = openList[0];

            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].F <= currNode.F && openList[i].H < currNode.H)
                {
                    currNode = openList[i];
                }
            }

            openList.Remove(currNode);
            AddCloseList(currNode);
            
            if (currNode.Equals(tileset.endNode))
            {
                Node temp = tileset.endNode;
                while (temp != tileset.startNode)
                {
                    finalNodeList.Add(temp);
                    if (!temp.Equals(tileset.startNode) && !temp.Equals(tileset.endNode))
                        temp.nodeObj.color = Color.cyan;
                    yield return new WaitForEndOfFrame();
                    temp = temp.parent;
                }
                openList.Clear();
                closeList.Clear();
                finalNodeList.Add(tileset.startNode);
                break;
            }

            OpenListAdd(currNode.X, currNode.Y + 1);            //↑
            OpenListAdd(currNode.X + 1, currNode.Y + 1);        //↗
            OpenListAdd(currNode.X + 1, currNode.Y);            //→
            OpenListAdd(currNode.X + 1, currNode.Y - 1);        //↘
            OpenListAdd(currNode.X, currNode.Y - 1);            //↓
            OpenListAdd(currNode.X - 1, currNode.Y - 1);        //↙
            OpenListAdd(currNode.X - 1, currNode.Y);            //←
            OpenListAdd(currNode.X - 1, currNode.Y + 1);        //↖

            yield return new WaitForEndOfFrame();
        }
    }

    void OpenListAdd(int X, int Y)
    {
        if (X > grid.gridSize.x - 1) return;
        if (X < 0) return;
        if (Y > grid.gridSize.y - 1) return;
        if (Y < 0) return;

        // 대각선 허용시, 벽 사이로 통과 안됨
        if (grid.grid[currNode.Y, X].wall && grid.grid[Y, currNode.X].wall) return;

        // 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
        //if (grid.grid[currNode.Y,X].wall || grid.grid[Y, currNode.X].wall) return;

        //벽이 아니고 closeList에 없을때
        if (!grid.grid[Y, X].wall && !closeList.Contains(grid.grid[Y, X]))
        {
            //이웃노드 체크 
            Node neighbor = grid.grid[Y, X];

            //직선은 10 대각선은 14의 길이
            int cost = currNode.G + (currNode.X - X == 0 || currNode.Y - Y == 0 ? 10 : 14);

            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
            if (cost < neighbor.G || !openList.Contains(neighbor))
            {
                neighbor.G = cost;
                neighbor.H = (Mathf.Abs(tileset.endNode.X - neighbor.X) + Mathf.Abs(tileset.endNode.Y - neighbor.Y)) * 10;
                neighbor.parent = currNode;

                // openList.Add(neighbor);
                AddOpenList(neighbor);
            }
        }
    }

}