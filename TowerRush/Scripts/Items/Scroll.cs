using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
public enum ScrollType
{
    NULL,
    HP,
    EVADE,
    DEFENSE,
    HEAL,
    SPEED,
    SHIELD,
    SPECIAL
}
public class Scroll : Item
{
    public ScrollType scrollType;
    public UnitSkill unitSkill;
    public Scroll(int _ID, string _Name, ItemType _Type, ItemRank _Rank, ScrollType _ScrollType, float _PickRate,string _Description) : base(_ID, _Name, _Type, _Rank, _PickRate, _Description)
    {
        scrollType = _ScrollType;
        image = Resources.Load<Sprite>("Scroll/" + _ID); //ex) 3002~3004
    }
    public Scroll(int _ID,ItemRank _Rank,ScrollType _ScrollType) 
    {
        id = _ID;
        itemRank = _Rank;
        scrollType = _ScrollType;
    }
    public void GetSkillData()
    {
        // 실행중인 어셈블리를 모두 탐색 하면서 그 안에 찾고자 하는 Type이 있는지 검사.
        Assembly assembly = Assembly.GetExecutingAssembly();
        string skill = "Skill" + id.ToString();
        System.Type t = assembly.GetType(skill);    //해당 스킬의 타입 찾기

        object obj = System.Activator.CreateInstance(t); //스킬의 인스턴스 만들기

        unitSkill = obj as UnitSkill;
        ////스킬의 정보 넣어줌
        unitSkill.Init(DBManager.Instance.itemDB.SkillInfoDB[id.ToString()][(int)itemRank-1]);
    }
   
}
