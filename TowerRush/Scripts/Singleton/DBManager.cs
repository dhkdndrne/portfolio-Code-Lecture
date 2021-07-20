using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBManager : SingleTon<DBManager>
{
    public WorldDataC worldDB;
    public ItemDataC itemDB;
    public SetItemDataC setItemDB;
    public TowerDataC towerDB;
    public ExpDataC expDB;
    private void Awake()
    {
        worldDB = new WorldDataC();
        worldDB.DownLoadInfo();

        itemDB = new ItemDataC();
        itemDB.DownLoadInfo();

        setItemDB = new SetItemDataC();
        setItemDB.DownLoadInfo();

        towerDB = new TowerDataC();
        towerDB.DownLoadInfo();

        expDB = new ExpDataC();
        expDB.DownLoadInfo();

        if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

}

public class ExpDataC
{
    public void DownLoadInfo()
    {
        List<Dictionary<string, object>> expData = CSVReader.Read("DBData/ExpDB");
        for (int i = 0; i < expData.Count; i++)
        {
            int[] temp = new int[2];
            temp[0] = (int)expData[i]["NeedExp"];
            temp[1] = (int)expData[i]["TotalExp"];

            ExpData.Add((int)expData[i]["Level"],temp);
        }
    }
    public Dictionary<int,int[]> ExpData = new Dictionary<int, int[]>();
}
