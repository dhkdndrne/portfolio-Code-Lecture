using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region 프리팹
    static PoolManager instance = new PoolManager();

    public static PoolManager Instance { get { return instance; } }
    #endregion
    [SerializeField] GameObject[] blockPrefabs;
    Dictionary<string, Pool> blockPool = new Dictionary<string, Pool>();
    Transform root;
    public string[] poolKeys { get; private set; }
  
    void Awake()
    {
        instance = this;
        Init();

        poolKeys = new string[blockPrefabs.Length];
        for (int i = 0; i < blockPrefabs.Length; i++)
        {
            CreatePool(blockPrefabs[i]);
            poolKeys[i] = blockPrefabs[i].name;
        }
    }
    void Init()
    {
        if (root == null)
        {
            root = new GameObject { name = "Pool_Root" }.transform;
            //Object.DontDestroyOnLoad(root);
        }
    }

    void CreatePool(GameObject prefab, int count =10)
    {
        Pool pool = new Pool();
        pool.Init(prefab, count);
        pool.root.SetParent(root);

        blockPool.Add(prefab.name, pool);
    }

    public void Enequeue(GameObject obj)
    {
        string name = obj.gameObject.name;

        if(blockPool.ContainsKey(obj.name) == false)
        {
            Destroy(obj);
            return;
        }

        blockPool[obj.name].Enqueue(obj);
    }

    public GameObject Dequeue(string key)
    {
        if (blockPool.ContainsKey(key) == false)
        {
            for (int i = 0; i < blockPrefabs.Length; i++)
            {
                if(blockPrefabs[i].name == key)
                {
                    CreatePool(blockPrefabs[i], 5);
                }
            }       
        }
        return blockPool[key].Dequeue();
    }
}
