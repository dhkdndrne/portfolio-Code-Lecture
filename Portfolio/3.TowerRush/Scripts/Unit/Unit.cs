
using UnityEngine;

public struct Stats
{
    public int hp;
    public float defense;
    public float evade;
    public float speed;

    public int shield;        // 실드
    public float egnoreDebuffFactor;  // 디버프 무시 확률
    public Stats(int _Hp, float _Defense, float _Evade, float _Speed, int _Shield = 0, float _EgnoreDebuff = 0)
    {
        hp = _Hp;
        defense = _Defense;
        evade = _Evade;
        speed = _Speed;
        shield = _Shield;
        egnoreDebuffFactor = _EgnoreDebuff;
    }
}

public class Unit
{
    public void InputStatInfo()
    {
        headItem = parent.equipedItems[0] as Head;
        scrollItem = parent.equipedItems[1] as Scroll;
        armorItem = parent.equipedItems[2] as EquipMent;
        shoeItem = parent.equipedItems[3] as EquipMent;

        int hp = 100;       //기본체력
        float defense = 1;  //기본방어력
        float speed = 5;    //기본 속도
        float tempSpeed = 0;    //추가 속도
        float evade = 1;    //기본 회피

        // 세트 효과 스텟
        int shield = 0;
        float egnoreDebuffFactor = 0;
        float speedFactor = 0;
        int hpFactor = 0;
        float evadeFactor = 0;
        float defenseFactor = 0;


        // 세트 ID가 0이 아니고 딕셔너리에 있으면 참
        if (parent.setID != 0 && DBManager.Instance.setItemDB.setItems.ContainsKey(parent.setID))
        {
            SetItem itemSet = DBManager.Instance.setItemDB.setItems[parent.setID];

            shield = itemSet.shieldFactor;
            egnoreDebuffFactor = itemSet.egnoreDebuffFactor;
            speedFactor = itemSet.speedFactor;
            hpFactor = itemSet.hpFactor;
            evadeFactor = itemSet.evadeFactor;
            defenseFactor = itemSet.defenseFactor;
        }

        if (armorItem != null) 
        { 
            hp += armorItem.hp; 
            defense += armorItem.defense;
            evade += armorItem.evade;
            tempSpeed += armorItem.speed;
        }

        if (shoeItem != null) {
            hp += shoeItem.hp;
            defense += shoeItem.defense;
            evade += shoeItem.evade;
            tempSpeed += shoeItem.speed;
        }

        if (headItem != null)
        {
            hp += headItem.hp;
            defense += headItem.defense;
            evade += headItem.evade;
            tempSpeed += headItem.speed;

            hp = PercentInt(hp, headItem.allStats);
            defense = PercentFloat(defense, headItem.allStats);
            evade = PercentFloat(evade, headItem.allStats);
            speed = PercentFloat(speed, tempSpeed + headItem.allStats);
        }

        unitStat = new Stats(hp, defense, evade, speed, shield, egnoreDebuffFactor);
    }

    public Factory parent;

    float PercentFloat(float _Stat, float _PerCent)
    {
        return Mathf.Round(_Stat * (1 + _PerCent * 0.01f));
    }

    int PercentInt(float _Stat, float _PerCent)
    {
        return Mathf.RoundToInt(_Stat * (1 + _PerCent * 0.01f));
    }

    public Head headItem { get; private set; }
    public EquipMent shoeItem { get; private set; }
    public EquipMent armorItem { get; private set; }
    public Scroll scrollItem { get; private set; }
    public Stats unitStat { get; private set; }

}
