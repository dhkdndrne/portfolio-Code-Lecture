using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Node
{
    public Node(int _x, int _y, Vector2 _Pos) { x = _x; y = _y; position = _Pos; }

    public bool isWall;
    public Node ParentNode;
    public Vector3 position;
    // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
    public int x, y, G, H;
    public int F { get { return G + H; } }
}
public class TileGrid : MonoBehaviour
{

    public static Node[,] NodeArray;
    Grid grid;
    Tilemap tileMap;
    int tileScale;

    public static int sizeX, sizeY;
    int minX, maxX, minY, maxY;


    //노드 확인용
    TextMesh[,] debugTextArray;
    void Awake()
    {
        grid = GetComponent<Grid>();
        tileMap = GameObject.Find("BG").GetComponent<Tilemap>();
        tileScale = (int)grid.transform.localScale.x;
        //tileMap.CompressBounds();


        //타일맵 최소 최대 xy 값
        minX = tileMap.cellBounds.min.x;
        maxX = tileMap.cellBounds.max.x;
        minY = tileMap.cellBounds.min.y;
        maxY = tileMap.cellBounds.max.y;

        sizeX = (maxX - minX) / tileScale;
        sizeY = (maxY - minY) / tileScale;

        NodeArray = new Node[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                NodeArray[i, j] = new Node(i, j, GetWorldPosition(i * tileScale, j * tileScale) + grid.cellSize * 4.5f);
                foreach (Collider2D col in Physics2D.OverlapCircleAll(NodeArray[i, j].position, 3f))
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall"))
                    {
                        NodeArray[i, j].isWall = true;
                    }
            }
        }
#if(UNITY_EDITOR)
        debugTextArray = new TextMesh[sizeX, sizeY];
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                debugTextArray[i, j] = CreateWorldText(i + "," + j, GameObject.Find("TextContainer").transform, GetWorldPosition(i * tileScale, j * tileScale) + grid.cellSize * 4.5f, 30, Color.black, TextAnchor.MiddleCenter);
                debugTextArray[i, j].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < sizeX * tileScale; i += tileScale)
        {
            for (int j = 0; j < sizeY * tileScale; j += tileScale)
            {
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + tileScale), Color.red, 999f);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + tileScale, j), Color.red, 999f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, sizeY * tileScale), GetWorldPosition(sizeX * tileScale, sizeY * tileScale), Color.red, 999f);
        Debug.DrawLine(GetWorldPosition(sizeX * tileScale, 0), GetWorldPosition(sizeX * tileScale, sizeY * tileScale), Color.red, 999f);
#endif
    }

    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    // Create Text in the World
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    debugTextArray[i, j].gameObject.SetActive(false);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    debugTextArray[i, j].gameObject.SetActive(true);
                }
            }
        }
    }

    Vector3 GetWorldPosition(int _X, int _Y)
    {
        return new Vector3(_X, _Y) * tileScale;
    }

    public Node TileClick(int _X, int _Y)
    {
        if (_X >= 0 && _Y >= 0 && _X < sizeX && _Y < sizeY)
        {
            return NodeArray[_X, _Y];
        }
        return null;
    }

    public Node TileClick(Vector3 _WorldPos)
    {
        int x, y;
        x = Mathf.FloorToInt(_WorldPos.x /Mathf.Pow(tileScale,2));
        y = Mathf.FloorToInt(_WorldPos.y / Mathf.Pow(tileScale, 2));

        return TileClick(x, y);
    }
}

