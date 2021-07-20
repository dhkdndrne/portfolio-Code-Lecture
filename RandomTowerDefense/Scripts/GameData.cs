using System.Collections;
using System.Collections.Generic;
using System;
[Serializable]
public class TowerInfo
{
    public int id;
    public int tileNumber;
    public RareList rareList;

    public TowerInfo(int _id, int _tileNumber, RareList _rareList)
    {
        id = _id;
        tileNumber = _tileNumber;
        rareList = _rareList;
    }
}

[Serializable]
public class GameData
{
    //킬 카운트
    public int killCnt;
    //골드
    public int gold;
    //라운드
    public int round;
    //레벨
    public string level;
    //hp
    public int hp;
    //미션 타이머
    public int[] missionTimerMin = new int[3];
    public float[] missionTimerTik = new float[3];
    public bool[] isQuest = new bool[3];
    public float[] questCoolTime = new float[3];
    public int[] upgradeLevel = new int[5];
    public float[] upgradeCosts = new float[5];
    //라운드 정보
    public Round[] roundInfo;
    public List<TowerInfo> towerInfoList = new List<TowerInfo>();

    //히든미션정보
    public bool[] hiddenMission;
    //타워 개수
    public int[] towerCnt;
    //정보 저장
    public void SaveInfo()
    {
        roundInfo = GameDB.Instance.rounds;
        gold = GameDB.Instance.Gold;
        round = GameDB.Instance.Round;
        level = GameDB.Instance.Level;
        hp = GameDB.Instance.Hp;
        killCnt = GameDB.Instance.KillCount;

        for (int i = 0; i < 3; i++)
        {
            missionTimerMin[i] = GameDB.Instance.min[i];
            missionTimerTik[i] = GameDB.Instance.tik[i];
        }

        isQuest[0] = GameDB.Instance.isQuest1;
        isQuest[1] = GameDB.Instance.isQuest2;
        isQuest[2] = GameDB.Instance.isQuest3;

        questCoolTime[0] = GameDB.Instance.questCoolTime1;
        questCoolTime[1] = GameDB.Instance.questCoolTime2;
        questCoolTime[2] = GameDB.Instance.questCoolTime3;

        //살아있으면 퀘스트 안된걸로 해야됨
        if (GameDB.Instance.Qmon1Alive)
        {
            GameDB.Instance.Qmon1Alive = false;
            isQuest[0] = true;
            missionTimerMin[0] = 0;
            missionTimerTik[0] = 0;
            questCoolTime[0] = 1f;
        }
        if (GameDB.Instance.Qmon2Alive)
        {
            GameDB.Instance.Qmon2Alive = false;
            isQuest[1] = true;
            missionTimerMin[1] = 0;
            missionTimerTik[1] = 0;
            questCoolTime[1] = 1f;
        }
        if (GameDB.Instance.Qmon3Alive)
        {
            GameDB.Instance.Qmon3Alive = false;
            isQuest[2] = true;
            missionTimerMin[2] = 0;
            missionTimerTik[2] = 0;
            questCoolTime[2] = 1f;
        }


        upgradeLevel[0] = GameDB.Instance.normalTowerLV;
        upgradeLevel[1] = GameDB.Instance.magicTowerLV;
        upgradeLevel[2] = GameDB.Instance.rareTowerLV;
        upgradeLevel[3] = GameDB.Instance.uniqueTowerLV;
        upgradeLevel[4] = GameDB.Instance.epicTowerLV;

        upgradeCosts[0] = GameDB.Instance.normalUpgradeCost;
        upgradeCosts[1] = GameDB.Instance.magicUpgradeCost;
        upgradeCosts[2] = GameDB.Instance.rareUpgradeCost;
        upgradeCosts[3] = GameDB.Instance.uniqueUpgradeCost;
        upgradeCosts[4] = GameDB.Instance.epicUpgradeCost;

        hiddenMission = new bool[MissionMgr.instance.h_Quest.Length];
        
        for (int i = 0; i < hiddenMission.Length; i++)
        {
            hiddenMission[i] = MissionMgr.instance.h_Quest[i];
        }

        towerCnt = new int[GameDB.Instance.towerId_Count.Length];

        for(int i =0;i< towerCnt.Length;i++)
        {
            towerCnt[i] = GameDB.Instance.towerId_Count[i];
        }

    }

    public void SaveTileInfo()
    {
        towerInfoList.Clear();
        GameDB.Instance.loadInfo.Clear();

        if (GameDB.Instance.t_dictionary[RareList.NORMAL].Count > 0)
        {
            for (int i = 0; i < GameDB.Instance.t_dictionary[RareList.NORMAL].Count; i++)
            {
                towerInfoList.Add(new TowerInfo(GameDB.Instance.t_dictionary[RareList.NORMAL][i].id, GameDB.Instance.t_dictionary[RareList.NORMAL][i].onTile.ID,
                    GameDB.Instance.t_dictionary[RareList.NORMAL][i].rareList));
            }
        }
        if (GameDB.Instance.t_dictionary[RareList.MAGIC].Count > 0)
        {
            for (int i = 0; i < GameDB.Instance.t_dictionary[RareList.MAGIC].Count; i++)
            {
                towerInfoList.Add(new TowerInfo(GameDB.Instance.t_dictionary[RareList.MAGIC][i].id, GameDB.Instance.t_dictionary[RareList.MAGIC][i].onTile.ID,
                      GameDB.Instance.t_dictionary[RareList.MAGIC][i].rareList));
            }
        }
        if (GameDB.Instance.t_dictionary[RareList.RARE].Count > 0)
        {
            for (int i = 0; i < GameDB.Instance.t_dictionary[RareList.RARE].Count; i++)
            {
                towerInfoList.Add(new TowerInfo(GameDB.Instance.t_dictionary[RareList.RARE][i].id, GameDB.Instance.t_dictionary[RareList.RARE][i].onTile.ID
                    , GameDB.Instance.t_dictionary[RareList.RARE][i].rareList));
            }
        }
        if (GameDB.Instance.t_dictionary[RareList.UNIQUE].Count > 0)
        {
            for (int i = 0; i < GameDB.Instance.t_dictionary[RareList.UNIQUE].Count; i++)
            {
                towerInfoList.Add(new TowerInfo(GameDB.Instance.t_dictionary[RareList.UNIQUE][i].id, GameDB.Instance.t_dictionary[RareList.UNIQUE][i].onTile.ID
                    , GameDB.Instance.t_dictionary[RareList.UNIQUE][i].rareList));
            }
        }
        if (GameDB.Instance.t_dictionary[RareList.EPIC].Count > 0)
        {
            for (int i = 0; i < GameDB.Instance.t_dictionary[RareList.EPIC].Count; i++)
            {
                towerInfoList.Add(new TowerInfo(GameDB.Instance.t_dictionary[RareList.EPIC][i].id, GameDB.Instance.t_dictionary[RareList.EPIC][i].onTile.ID
                    , GameDB.Instance.t_dictionary[RareList.EPIC][i].rareList));
            }
        }

    }
    public void SetLoadDataInfo()
    {
        GameDB.Instance.rounds = roundInfo;
        GameDB.Instance.Gold = gold;
        GameDB.Instance.Round = round;
        GameDB.Instance.Level = level;
        GameDB.Instance.Hp = hp;

        for (int i = 0; i < 3; i++)
        {
            GameDB.Instance.min[i] = missionTimerMin[i];
            GameDB.Instance.tik[i] = missionTimerTik[i];
        }
        GameDB.Instance.isQuest1 = isQuest[0];
        GameDB.Instance.isQuest2 = isQuest[1];
        GameDB.Instance.isQuest3 = isQuest[2];

        GameDB.Instance.questCoolTime1 = questCoolTime[0];
        GameDB.Instance.questCoolTime2 = questCoolTime[1];
        GameDB.Instance.questCoolTime3 = questCoolTime[2];
        if (towerInfoList.Count > 0)
        {
            for (int i = 0; i < towerInfoList.Count; i++)
            {
                GameDB.Instance.loadInfo.Add(new TowerInfo_(towerInfoList[i].id, towerInfoList[i].tileNumber, towerInfoList[i].rareList));
            }

        }

        GameDB.Instance.normalTowerLV = upgradeLevel[0];
        GameDB.Instance.magicTowerLV = upgradeLevel[1];
        GameDB.Instance.rareTowerLV = upgradeLevel[2];
        GameDB.Instance.uniqueTowerLV = upgradeLevel[3];
        GameDB.Instance.epicTowerLV = upgradeLevel[4];

        GameDB.Instance.normalUpgradeCost = upgradeCosts[0];
        GameDB.Instance.magicUpgradeCost = upgradeCosts[1];
        GameDB.Instance.rareUpgradeCost = upgradeCosts[2];
        GameDB.Instance.uniqueUpgradeCost = upgradeCosts[3];
        GameDB.Instance.epicUpgradeCost = upgradeCosts[4];


        GameDB.Instance.IsContinue = true;
        MissionMgr.instance.IsContinue = true;

        for (int i = 0; i < hiddenMission.Length; i++)
        {
            MissionMgr.instance.h_Quest[i] = hiddenMission[i];
        }

        for (int i = 0; i < towerCnt.Length; i++)
        {
             GameDB.Instance.towerId_Count[i] =  towerCnt[i];
        }

    }
}
