using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Head : Item
{

    public int allStats;
    public int hp;
    public float defense;
    public float evade;
    public float speed;
    public Head(int _ID, string _Name, ItemType _Type, ItemRank _Rank, float _PickRate, string _Description, int _AllStat, int _HpPer, float _DefensePer, float _EvadePer, float _SpeedPer, byte _SetID = 0) : base(_ID, _Name, _Type, _Rank, _PickRate,  _Description, _SetID)
    {
        allStats = _AllStat;
        hp = _HpPer;
        defense = _DefensePer;
        evade = _EvadePer;
        speed = _SpeedPer;
        image = Resources.Load<Sprite>("Head/" + _ID.ToString());
    }
    public Head() { }
}
