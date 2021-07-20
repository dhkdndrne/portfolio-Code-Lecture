using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PathFinder : MonoBehaviour
{
    Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;
    public static List<Node> FinalNodeList;

    private void Start()
    {
        PathFinding();
    }
    public void PathFinding()
    {

        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = TileGrid.NodeArray[2, 30];
        TargetNode = TileGrid.NodeArray[26, 8];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();

        while (OpenList.Count > 0)
        {
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            // 마지막
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();
                return;
            }


            // ↗↖↙↘
            OpenListAdd(CurNode.x + 1, CurNode.y + 1);
            OpenListAdd(CurNode.x - 1, CurNode.y + 1);
            OpenListAdd(CurNode.x - 1, CurNode.y - 1);
            OpenListAdd(CurNode.x + 1, CurNode.y - 1);


            // ↑ → ↓ ←
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }
    }

    void OpenListAdd(int checkX, int checkY)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= 0 && checkX < TileGrid.sizeX && checkY >= 0 && checkY < TileGrid.sizeY && !TileGrid.NodeArray[checkX, checkY].isWall && !ClosedList.Contains(TileGrid.NodeArray[checkX, checkY]))
        {
            // 대각선 이동시, 벽 사이로 통과 안됨
            if (TileGrid.NodeArray[CurNode.x, checkY].isWall && TileGrid.NodeArray[checkX, CurNode.y].isWall) return;

            // 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
            if (TileGrid.NodeArray[CurNode.x, checkY].isWall || TileGrid.NodeArray[checkX, CurNode.y].isWall) return;

            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            Node NeighborNode = TileGrid.NodeArray[checkX, checkY];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);


            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }
    //void OnDrawGizmos()
    //{
    //    if (FinalNodeList.Count > 0)
    //    {
    //        for (int i = 0; i < FinalNodeList.Count - 1; i++)
    //        {
    //            Gizmos.DrawLine(FinalNodeList[i].position, FinalNodeList[i+1].position);
    //        }
    //    }
    //}
}
