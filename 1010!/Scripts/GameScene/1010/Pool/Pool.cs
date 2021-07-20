using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    public GameObject prefab { get; private set; }
    public Transform root { get; set; }

    public Queue<GameObject> pool = new Queue<GameObject>();
    public void Init(GameObject prefab, int count)
    {
        this.prefab = prefab;

        root = new GameObject().transform;
        root.name = prefab.name+"_Root";

        for (int i = 0; i < count; i++)
        {
            Enqueue(Create());
        }
    }
    GameObject Create()
    {
        GameObject obj = Object.Instantiate(prefab);
        obj.name = prefab.name; // 뒤에 붙는 (Clone) 없앰. 원본 프리팹과 이름 같게.

        return obj;
    }

    public void Enqueue(GameObject obj)
    {
        if (obj == null) return;

        obj.transform.SetParent(root);
        obj.gameObject.SetActive(false);

        pool.Enqueue(obj);
    }

    public GameObject Dequeue()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return null;
    }
}
