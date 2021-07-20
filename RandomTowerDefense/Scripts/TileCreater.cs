using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCreater : MonoBehaviour
{

    public GameObject map;
    public GameObject wallPrefabs = null;

    [SerializeField] public GameObject[,] tilesObj = new GameObject[12, 12];
    [SerializeField] public Tile[] tiles;

    private void Awake()
    {
        int cnt = 0;
        int X = 0;
        int Y = 0;

        tiles = map.GetComponentsInChildren<Tile>();

        for (int num = 0; num < tiles.Length; num++)
        {
            if (X == 11)
            {
                Y++;
                X = 0;
            }
            tilesObj[X, Y] = tiles[cnt].GetComponent<Transform>().gameObject;
            tilesObj[X, Y].GetComponent<Tile>().ID = cnt;
            tilesObj[X, Y].name = cnt + " 번";
            cnt++;
        }


        for (int y = -1; y < 13; y++)
        {
            for (int x = -1; x < 13; x++)
            {
                if ((y == -1 || y == 12 || x == -1 || x == 12))
                {
                    if (!((y == 12) && (x == 0 || x == 4)))
                    {
                        Instantiate(wallPrefabs, new Vector3((map.transform.position.x) + (x * 2), (map.transform.position.y),
                       map.transform.position.z + (y * 2)), Quaternion.identity, map.GetComponent<Transform>().GetChild(75));
                    }

                }
            }

        }
    }
}

