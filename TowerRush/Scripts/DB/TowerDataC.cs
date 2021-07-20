using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public struct TowerData
{
    public string towerName;
    public int level;
    public int damage;
    public int maxOverlap;
    public int criDamage;

    public float fireDelay;
    public float range;
    public float criRate;
    public float bulletSpeed;
    public float debuffRate;
    public float debuffDuration;
    public float debuffApplyRate;
    public float tickTime;

    public bool inevitable;
    public Element elementType;
    public TargetOption targetOption;
    public TowerData(string _Name, int _Level, int _Damage,
        float _FireDelay, float _Range, float _CriRate, int _CriDamage, float _BulletSpeed,
        Element _Element, bool _Inevitable, float _DebuffRate,
        float _DebuffDuration, float _DebuffApplyRate, float _TickTime, int _MaxOverLap, TargetOption _TargetOption)
    {
        towerName = _Name;
        level = _Level;
        damage = _Damage;
        maxOverlap = _MaxOverLap;
        criDamage = _CriDamage;
        fireDelay = _FireDelay;
        range = _Range;
        criRate = _CriRate;
        bulletSpeed = _BulletSpeed;
        debuffRate = _DebuffRate;
        debuffDuration = _DebuffDuration;
        debuffApplyRate = _DebuffApplyRate;
        inevitable = _Inevitable;
        elementType = _Element;
        tickTime = _TickTime;
        targetOption = _TargetOption;
    }

}
public class TowerDataC
{
    public void DownLoadInfo()
    {

        List<Dictionary<string, object>> towerData = CSVReader.Read("DBData/TowerData");
        for (int i = 0; i < towerData.Count; i += 25)
        {
            List<TowerData> towerList = new List<TowerData>();         
            Element checkElementType = (Element)Enum.Parse(typeof(Element), Convert.ToString(towerData[i]["ElementType"]));
            TargetOption checkTargetOption = (TargetOption)Enum.Parse(typeof(TargetOption), Convert.ToString(towerData[i]["TargetType"]));
            string name = (string)towerData[i]["ID"];

            for (int j = i; j < i + 25; j++)
            {
                towerList.Add(new TowerData(
                    (string)towerData[i]["Name"],
                    (int)towerData[j]["Level"],
                    (int)towerData[j]["Damage"],
                    Convert.ToSingle(towerData[j]["FireDelay"]),
                    Convert.ToSingle(towerData[j]["Range"]),
                    Convert.ToSingle(towerData[j]["CriRate"]),
                    (int)(towerData[j]["CriDmg"]),
                    Convert.ToSingle(towerData[j]["BulletSpeed"]),
                    checkElementType,
                    Convert.ToBoolean(towerData[j]["Inevitable"]),
                    Convert.ToSingle(towerData[j]["DebuffRate"]),
                    Convert.ToSingle(towerData[j]["DebuffDuration"]),
                    Convert.ToSingle(towerData[j]["DebuffApplyRate"]),
                    Convert.ToSingle(towerData[j]["Ticktime"]),
                   (int)towerData[j]["MaxOverLap"],
                   checkTargetOption));
            }
            towerDB.Add(name, towerList);
        }
    }
    public Dictionary<string, List<TowerData>> towerDB = new Dictionary<string, List<TowerData>>();
}
