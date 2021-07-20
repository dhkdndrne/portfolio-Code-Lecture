using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Block : MonoBehaviour
{
    [SerializeField] Vector3[] blockShapePos;
    public Vector3[] BlockShape { get { return blockShapePos; } }
    [SerializeField] int colorIndex;
    Board board;
    Vector3 offset;
    Vector3 oldPos;

    void Start()
    {
        board = FindObjectOfType<Board>();
        Init();
    }

    void Init()
    {
        blockShapePos = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            blockShapePos[i] = transform.GetChild(i).localPosition;
        }
    }
    private void OnMouseDown()
    {
        offset = gameObject.transform.position - GetMouseWorldPosition();
        oldPos = transform.localPosition;
        transform.DOScale(1f, 0.15f);
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + offset + new Vector3(0, 1.5f, 0);
    }

    private void OnMouseUp()
    {
        Vector3 lastPos = transform.position;
        lastPos.x = Mathf.RoundToInt(lastPos.x);
        lastPos.y = Mathf.RoundToInt(lastPos.y);

        if (!board.InputBlock(colorIndex, lastPos, blockShapePos,transform))
        {
            transform.DOScale(0.7f, 0.2f);
            transform.DOLocalMove(oldPos, 0.2f);
        }
        else //성공했으면 오브젝트를 비활성화
        {
            GameManager.Instance.GetScore(gameObject.transform.childCount);
            PoolManager.Instance.Enequeue(gameObject);
            GameManager.Instance.CheckBlockCount();
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);

        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
