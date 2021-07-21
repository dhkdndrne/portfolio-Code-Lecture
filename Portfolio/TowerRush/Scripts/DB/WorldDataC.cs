using System;
using System.Collections.Generic;
public class WorldDataC
{
    public void DownLoadInfo()
    {
        List<Dictionary<string, object>> WorldData = CSVReader.Read("DBData/WorldData");
        for (int i = 0; i < WorldData.Count; i++)
        {
            DataBase.Add(new World((int)WorldData[i]["Id"],
                (int)WorldData[i]["World"],
                (string)WorldData[i]["StageName"],
                (int)WorldData[i]["NeedStageId"],
                 (int)WorldData[i]["LimitTime"],
                  (int)WorldData[i]["LimitPopulation"],
                   (int)WorldData[i]["ClearCount"],
                    (int)WorldData[i]["FirstClearGold"],
                     (int)WorldData[i]["FisrtClearLUGA"],
                     (int)WorldData[i]["FirstClearExp"],
                     (int)WorldData[i]["ClearMinGold"],
                     (int)WorldData[i]["ClearMaxGold"],
                     (int)WorldData[i]["ClearMinLuga"],
                     (int)WorldData[i]["ClearMaxLuga"],
                       (int)WorldData[i]["ClearExp"],
                      Convert.ToBoolean(WorldData[i]["CheckClear"])));
        }
    }

    public List<World> DataBase = new List<World>();
    public void SetClearWorldInfo(int _MaxStage)
    {
        for (int i = 0; i < _MaxStage; i++)
        {
            DataBase[i].isClear = true;
        }
    }
}

public class World
{
    public int world { get; private set; }
    public int id { get; private set; }
    public string stageName { get; private set; }
    public int needStageId { get; private set; }
    public int limitTime { get; private set; }
    public int limitPopulation { get; private set; }
    public int clearCount { get; private set; }
    public int firstClear_Gold { get; private set; }
    public int firstClear_Luga { get; private set; }
    public int firstClear_Exp { get; private set; }

    public int clear_MinLuga { get; private set; }
    public int clear_MaxLuga { get; private set; }

    public int clear_MinGold { get; private set; }
    public int clear_MaxGold { get; private set; }

    public int clear_Exp { get; private set; }
    public bool isClear;

    public World(int _ID, int _World, string _StageName, int _NeedStageID, int _LimitTime,
        int _LimitPopulation, int _ClearCount, int _FirstClearGold, int _FirstClearLuga, int _FirstClearExp, int _ClearMinGold,int _ClearMaxGold,int _ClearMinLuga,int _ClearMaxLuga, int _ClearExp, bool _IsClear)
    {
        id = _ID;
        world = _World;
        stageName = _StageName;
        needStageId = _NeedStageID;
        limitTime = _LimitTime;
        limitPopulation = _LimitPopulation;
        clearCount = _ClearCount;
        firstClear_Gold = _FirstClearGold;
        firstClear_Luga = _FirstClearLuga;
        firstClear_Exp = _FirstClearExp;

        clear_MaxGold = _ClearMaxGold;
        clear_MinGold = _ClearMinGold;

        clear_MaxLuga= _ClearMaxLuga;
        clear_MinLuga = _ClearMinLuga;

        clear_Exp = _ClearExp;
        isClear = _IsClear;
    }
}
