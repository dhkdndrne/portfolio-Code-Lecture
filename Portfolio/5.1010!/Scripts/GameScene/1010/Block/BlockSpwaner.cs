using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BlockSpwaner
{
    public void SpawnBlocks(Transform[] Pos)
    {
        for (int i = 0; i < 3; i++)
        {
            string name = PoolManager.Instance.poolKeys[Random.Range(0, PoolManager.Instance.poolKeys.Length)];
            GameObject obj = PoolManager.Instance.Dequeue(name);

            obj.transform.position += new Vector3(obj.transform.position.x + 15, Pos[i].position.y);
            obj.transform.SetParent(Pos[i]);
            obj.transform.DOMove(Pos[i].position, 0.6f + (0.1f * i));
        }
    }
}
