using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIleSetting : MonoBehaviour
{
    Vector2 mousePosition;
    Grid grid;
    public Node startNode { get; private set; }
    public Node endNode { get; private set; }
    private void Start()
    {
        grid = FindObjectOfType<Grid>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Node node = ClickNode();
            if (node != null) StartCoroutine(ChangeWall(node));
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Node node = ClickNode();

            if (node != null && endNode != node)
            {
                if (startNode == null)
                {
                    node.ChangeStart = true;
                    startNode = node;

                }
                else if (startNode != node)
                {
                    startNode.ChangeStart = false;
                    startNode = node;
                    startNode.ChangeStart = true;

                }
            }
        }   //시작
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Node node = ClickNode();

            if (node != null && startNode != node)
            {
                if (endNode == null)
                {
                    node.ChangeEnd = true;
                    endNode = node;

                }
                else if (endNode != node)
                {
                    endNode.ChangeEnd  = false;
                    endNode = node;
                    endNode.ChangeEnd = true;

                }
            }
        }   //끝
    }
    IEnumerator ChangeWall(Node node)
    {
        //처음 선택한 노드의 walkable 반대값을 가짐
        bool wall = !node.wall;

        while (Input.GetMouseButton(0))//마우스 버튼을 누르고 있는 동안
        {
            node = ClickNode();
            if (node != null && !node.start && !node.end) node.ChangeWall = wall;
            yield return null;
        }
    }
    Node ClickNode()
    {

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Ray2D ray = new Ray2D(mousePosition, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction.normalized, Mathf.Infinity);

        if (hit.collider != null)
        {
            return grid.NodePoint(hit.collider.gameObject.transform);
        }

        return null;
    }
}
