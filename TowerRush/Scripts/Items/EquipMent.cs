using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EquipMent : Item
{
    //장비스텟
    public int hp;
    public float defense;
    public float evade;
    public float speed;
    public EquipMent(int _ID, string _Name, ItemType _Type, ItemRank _Rank, float _PickRate, string _Description, int _Hp, float _Defense, float _Evade, float _Speed, byte _SetID = 0) : base(_ID, _Name, _Type, _Rank, _PickRate, _Description, _SetID)
    {
        hp = _Hp;
        defense = _Defense;
        evade = _Evade;
        speed = _Speed;

        if (_ID / 1000 == 5) image = Resources.Load<Sprite>("Armor/" + _ID.ToString());
        else if (_ID / 1000 == 7) image = Resources.Load<Sprite>("Shoe/" + _ID.ToString());

    }
    public EquipMent() { }
}
