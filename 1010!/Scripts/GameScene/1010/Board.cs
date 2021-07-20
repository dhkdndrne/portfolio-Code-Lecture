using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Board : SerializedMonoBehaviour
{
    [SerializeField] Color[] blockColors;

    int[,] board = new int[BoardSize, BoardSize];
    GameObject[] cells;

    const int BoardSize = 10;
    private void Start()
    {
        InitCell();
        InvokeRepeating("CheckEnd", 0,0.5f);
    }

    void InitCell()
    {
        cells = new GameObject[100];
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                GameObject obj = transform.GetChild(y * 10 + x).gameObject;
                obj.name = "Cell_" + y + "_" + x;
                cells[y * 10 + x] = obj;
            }
        }
    }

    GameObject GetCell(int x, int y)
    {
        return cells[y * 10 + x];
    }

    bool CheckRange(int x, int y)
    {
        if (x < 0 || y < 0 || x > BoardSize - 1 || y > BoardSize - 1) return false;
        return true;
    }

    public bool InputBlock(int colorIndex, Vector3 lastPos, Vector3[] blockShapePos, Transform transform)
    {
        for (int i = 0; i < blockShapePos.Length; i++)
        {
            Vector3 sum = blockShapePos[i] + lastPos;
            if (!CheckRange((int)sum.x, (int)sum.y)) return false;      //범위 벗어났을때
            else if (board[(int)sum.y, (int)sum.x] != 0) return false;  //이미 타일이 놓여져 있을때
        }

        for (int i = 0; i < blockShapePos.Length; i++)
        {
            Vector3 sum = blockShapePos[i] + lastPos;
            GameObject cell = GetCell((int)sum.x, (int)sum.y);

            board[(int)sum.y, (int)sum.x] = 1;
            cell.GetComponent<SpriteRenderer>().color = blockColors[colorIndex];
        }
        CheckMatch();
        return true;
    }

    bool CheckAvailable(Vector3[] Shape)
    {
        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                int count = 0;
                for (int k = 0; k < Shape.Length; k++)
                {
                    Vector3 shapePos = Shape[k] + new Vector3(y, x, 0);
                    if (!CheckRange((int)shapePos.x, (int)shapePos.y)) break;
                    if (board[(int)shapePos.y, (int)shapePos.x] != 0) break;
                    count++;
                }

                if (count == Shape.Length) return true;
            }
        }
        return false;
    }

    void CheckEnd()
    {
        Transform[] group = GameManager.Instance.PosGroup;
        bool isEnd = true;

        for (int i = 0; i < group.Length; i++)
        {
            if (group[i].childCount != 0)
            {
                if (CheckAvailable(group[i].GetComponentInChildren<Block>().BlockShape)) isEnd = false;
            }
        }

        if (isEnd)
        {
            GameManager.Instance.GameOver();
        }
    }

    void CheckMatch()
    {
        int lineCnt = 0;

        for (int y = 0; y < BoardSize; y++)
        {
            int rowCnt = 0;
            int colCnt = 0;
            for (int x = 0; x < BoardSize; x++)
            {
                if (board[y, x] != 0) rowCnt++;
                if (board[x, y] != 0) colCnt++;
            }

            if (colCnt == 10)
            {
                lineCnt++;
                for (int x = 0; x < BoardSize; x++)
                {
                    board[x, y] = -1;
                }
            }
            if (rowCnt == 10)
            {
                lineCnt++;
                for (int x = 0; x < BoardSize; x++)
                {
                    board[y, x] = -1;
                }
            }
        }
        StartCoroutine(BlockDestroyAnim());
        GameManager.Instance.GetScore(lineCnt * 10);
    }

    IEnumerator BlockDestroyAnim()
    {
        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                if (board[y, x] == -1)
                {
                    board[y, x] = 0;
                    GameObject obj = GetCell(x, y);
                    yield return obj.transform.DOScale(Vector3.zero, 0.06f).WaitForCompletion();

                    obj.GetComponent<SpriteRenderer>().color = blockColors[0];
                    obj.transform.localScale = new Vector3(0.9f, 0.9f, 1);
                }
            }
        }
    }
}
