using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDataC
{
    public Dictionary<string, Dictionary<string, object>> headDB = new Dictionary<string, Dictionary<string, object>>();
    public Dictionary<string, Dictionary<string, object>> scrollDB = new Dictionary<string, Dictionary<string, object>>();
    public Dictionary<string, Dictionary<string, object>> equipDB = new Dictionary<string, Dictionary<string, object>>();
    public Dictionary<string, List<SkillInfoData>> SkillInfoDB = new Dictionary<string, List<SkillInfoData>>(); //Key = item ID , List = rank에 따라 다른값들

    float[,] itemPickRate = { { 60, 40, 0 },
                              { 53, 36, 11 },
                              { 61, 38, 1 }};

    //뽑을 아이템 등급
    Item.ItemRank SetPickItemRank(int _BuyRank)
    {
        // 총 확률
        float pickRate = 0;
        // 뽑힐 확률값
        float weight = 0;
        // 아이템 등급 판단용
        int index = 0;

        for (int i = 0; i < 3; i++)
        {
            pickRate += itemPickRate[_BuyRank, i];
        }
        //총 확률
        weight = Random.Range(0, pickRate);    //Random.Range 정수 = 뒷값을 빼고 / 실수 = 뒷값을 포함
                  
        //등급 index 선택함
        for (int i = 0; i < 3; i++)
        {
            if (itemPickRate[_BuyRank, i] >= weight)
            {
                index = i;
                break;
            }

            weight -= itemPickRate[_BuyRank, i];
        }

        //아이템 등급리턴
        //일반뽑기
        if (_BuyRank.Equals(0))
        {
            if (index.Equals(0)) return Item.ItemRank.NORMAL;
            else return Item.ItemRank.MAGIC;
        }
        //중급뽑기
        else if (_BuyRank.Equals(1))
        {
            if (index.Equals(0)) return Item.ItemRank.MAGIC;
            else if (index.Equals(1)) return Item.ItemRank.RARE;
            else return Item.ItemRank.EPIC;
        }
        //그오오오급뽑기
        else
        {
            if (index.Equals(0)) return Item.ItemRank.RARE;
            else if (index.Equals(1)) return Item.ItemRank.EPIC;
            else return Item.ItemRank.LEGEND;
        }
    }

    public string ReturnRankToString<T>(T _Rank)
    {
        if (_Rank.Equals(Item.ItemRank.NORMAL) || _Rank.Equals((int)Item.ItemRank.NORMAL)) return "NORMAL";
        else if (_Rank.Equals(Item.ItemRank.MAGIC) || _Rank.Equals((int)Item.ItemRank.MAGIC)) return "MAGIC";
        else if (_Rank.Equals(Item.ItemRank.RARE) || _Rank.Equals((int)Item.ItemRank.RARE)) return "RARE";
        else if (_Rank.Equals(Item.ItemRank.EPIC) || _Rank.Equals((int)Item.ItemRank.EPIC)) return "EPIC";
        else return "LEGEND";
    }

    //총 확률 정하는 함수
    float SetTotalRate(int _ItemType, Item.ItemRank _PickedRank)
    {
        float totalPickRate = 0;
        string rank = ReturnRankToString(_PickedRank);

        switch (_ItemType)
        {
            case 0:
                foreach (var item in headDB[rank])
                {
                    totalPickRate += ((Head)item.Value).pickRate;
                }
                break;

            case 1:
                foreach (var item in scrollDB[rank])
                {
                    totalPickRate += ((Scroll)item.Value).pickRate;
                }
                break;

            case 2:
                foreach (var item in equipDB[rank])
                {
                    totalPickRate += ((EquipMent)item.Value).pickRate;
                }
                break;
        }

        return totalPickRate;
    }

    //랜덤 확률로 아이템 반환하는 함수
    public Item GetRandomItem(int _ItemType, int _BuyRank,bool _IsReward = false)
    {
        float weight = 0;
        float selectNum = 0;
        Item.ItemRank pickedRank = Item.ItemRank.NULL;

        if (!_IsReward) pickedRank = SetPickItemRank(_BuyRank);
        else pickedRank = (Item.ItemRank)_BuyRank;

        selectNum = Random.Range(0, SetTotalRate(_ItemType, pickedRank));

        string rank = ReturnRankToString(pickedRank);

        switch (_ItemType)
        {
            case 0:

                foreach (var item in headDB[rank])
                {
                    Head temp = (Head)item.Value;

                    weight += temp.pickRate;
                    if (selectNum <= weight)
                    {
                        return temp;
                    }
                }
                break;

            case 1:
                foreach (var item in scrollDB[rank])
                {
                    Scroll temp = (Scroll)item.Value;

                    weight += temp.pickRate;

                    if (selectNum <= weight)
                    {
                        //temp.GetSkillData();
                        return temp;
                    }
                }
                break;

            case 2:
                foreach (var item in equipDB[rank])
                {
                    EquipMent temp = (EquipMent)item.Value;

                    weight += temp.pickRate;
                    if (selectNum <= weight)
                    {
                        return temp;
                    }
                }
                break;
        }
        return null;
    }

    public void DownLoadInfo()
    {
        //히로디온
        List<Dictionary<string, object>> headData = CSVReader.Read("DBData/HeadData");

        Dictionary<string, object> head_normal = new Dictionary<string, object>();
        Dictionary<string, object> head_magic = new Dictionary<string, object>();
        Dictionary<string, object> head_rare = new Dictionary<string, object>();
        Dictionary<string, object> head_epic = new Dictionary<string, object>();
        Dictionary<string, object> head_legend = new Dictionary<string, object>();

        for (int i = 0; i < headData.Count; i++)
        {
            Head temp = new Head((int)headData[i]["ID"],
                (string)headData[i]["Name"],
                (Item.ItemType)headData[i]["Type"],
                (Item.ItemRank)headData[i]["Rank"],
                System.Convert.ToSingle(headData[i]["PickRate"]),
                (string)headData[i]["Description"],
                (int)headData[i]["AllStat"],
                (int)headData[i]["HpPer"],
                 System.Convert.ToSingle(headData[i]["DefensePer"]),
                  System.Convert.ToSingle(headData[i]["EvadePer"]),
                   System.Convert.ToSingle(headData[i]["SpeedPer"]),              
                System.Convert.ToByte(headData[i]["SetID"]));

            if (temp.itemRank.Equals(Item.ItemRank.NORMAL)) head_normal.Add(temp.id.ToString(), temp);
            else if (temp.itemRank.Equals(Item.ItemRank.MAGIC)) head_magic.Add(temp.id.ToString(), temp);
            else if (temp.itemRank.Equals(Item.ItemRank.RARE)) head_rare.Add(temp.id.ToString(), temp);
            else if (temp.itemRank.Equals(Item.ItemRank.EPIC)) head_epic.Add(temp.id.ToString(), temp);
            else head_legend.Add(temp.id.ToString(), temp);
        }

        headDB.Add("NORMAL", head_normal);
        headDB.Add("MAGIC", head_magic);
        headDB.Add("RARE", head_rare);
        headDB.Add("EPIC", head_epic);
        headDB.Add("LEGEND", head_legend);


        //유닛 스킬
        List<Dictionary<string, object>> skillData = CSVReader.Read("DBData/SkillData");
        for (int i = 0; i < skillData.Count;)
        {
            bool isFinish = false;
            int checkID = (int)skillData[i]["ID"];

            ActiveType checkType = (ActiveType)System.Enum.Parse(typeof(ActiveType), System.Convert.ToString(skillData[i]["Type"]));

            List<SkillInfoData> skillInfoDatas = new List<SkillInfoData>(); // ex) 3001번 normal~ legend 정보

            //레벨별 스킬 정보들 => 어짜피 스크롤에 들어가야한다;
            while (true)
            {
                if (checkID != (int)skillData[i]["ID"]) break;

                skillInfoDatas.Add(new SkillInfoData(checkType, checkID,
                    System.Convert.ToSingle(skillData[i]["CoolTime"]), System.Convert.ToSingle(skillData[i]["Duration"]),
                    (int)skillData[i]["HPper"], (int)skillData[i]["Targetnum"], (int)skillData[i]["Shield"], (int)skillData[i]["Heal"],
                    System.Convert.ToSingle(skillData[i]["Defense"]), System.Convert.ToSingle(skillData[i]["Evade"]), System.Convert.ToSingle(skillData[i]["Speed"]), System.Convert.ToSingle(skillData[i]["Range"])
                    , System.Convert.ToBoolean(skillData[i]["Invincible"]), System.Convert.ToBoolean(skillData[i]["Revive"]), (int)skillData[i]["Hitcount"], System.Convert.ToBoolean(skillData[i]["OneTime"])));

                if (i < skillData.Count - 1) i++;
                else { isFinish = true; break; }
            }
            SkillInfoDB.Add(checkID.ToString(), skillInfoDatas);
            if (isFinish) break;
        }

        //스크롤
        //--------------------------------------------------------------------------------------------
        List<Dictionary<string, object>> scrollData = CSVReader.Read("DBData/ScrollData");

        Dictionary<string, object> scroll_normal = new Dictionary<string, object>();
        Dictionary<string, object> scroll_magic = new Dictionary<string, object>();
        Dictionary<string, object> scroll_rare = new Dictionary<string, object>();
        Dictionary<string, object> scroll_epic = new Dictionary<string, object>();
        Dictionary<string, object> scroll_legend = new Dictionary<string, object>();

        for (int i = 0; i < scrollData.Count; i++)
        {
            Scroll temp = new Scroll((int)scrollData[i]["ID"],
                  (string)scrollData[i]["Name"],
                  (Item.ItemType)scrollData[i]["Type"],
                  (Item.ItemRank)scrollData[i]["Rank"],
                  (ScrollType)System.Enum.Parse(typeof(ScrollType), System.Convert.ToString(scrollData[i]["ScrollType"])),
                  System.Convert.ToSingle(scrollData[i]["PickRate"]),
                 (string)scrollData[i]["Description"]);

            if (temp.itemRank.Equals(Item.ItemRank.NORMAL)) scroll_normal.Add(temp.id.ToString(), temp);
            else if (temp.itemRank.Equals(Item.ItemRank.MAGIC)) scroll_magic.Add(temp.id.ToString(), temp);
            else if (temp.itemRank.Equals(Item.ItemRank.RARE)) scroll_rare.Add(temp.id.ToString(), temp);
            else if (temp.itemRank.Equals(Item.ItemRank.EPIC)) scroll_epic.Add(temp.id.ToString(), temp);
            else scroll_legend.Add(temp.id.ToString(), temp);
        }

        scrollDB.Add("NORMAL", scroll_normal);
        scrollDB.Add("MAGIC", scroll_magic);
        scrollDB.Add("RARE", scroll_rare);
        scrollDB.Add("EPIC", scroll_epic);
        scrollDB.Add("LEGEND", scroll_legend);

        //장비
        //--------------------------------------------------------------------------------------------
        List<Dictionary<string, object>> equipMentData = CSVReader.Read("DBData/EquipMentData");

        Dictionary<string, object> equip_normal = new Dictionary<string, object>();
        Dictionary<string, object> equip_magic = new Dictionary<string, object>();
        Dictionary<string, object> equip_rare = new Dictionary<string, object>();
        Dictionary<string, object> equip_epic = new Dictionary<string, object>();
        Dictionary<string, object> equip_legend = new Dictionary<string, object>();

        for (int i = 0; i < equipMentData.Count; i++)
        {
            EquipMent temp = new EquipMent((int)equipMentData[i]["ID"],
                 (string)equipMentData[i]["Name"],
                 (Item.ItemType)equipMentData[i]["Type"],
                 (Item.ItemRank)equipMentData[i]["Rank"],
                 System.Convert.ToSingle(equipMentData[i]["PickRate"]),
                 (string)equipMentData[i]["Description"],
                 (int)equipMentData[i]["Hp"],
                  System.Convert.ToSingle(equipMentData[i]["Defense"]),
                   System.Convert.ToSingle(equipMentData[i]["Evade"]),
                    System.Convert.ToSingle(equipMentData[i]["Speed"]),
                 System.Convert.ToByte(equipMentData[i]["SetID"]));

            if (temp.itemRank.Equals(Item.ItemRank.NORMAL)) equip_normal.Add(temp.id.ToString(), temp);
            else if (temp.itemRank.Equals(Item.ItemRank.MAGIC)) equip_magic.Add(temp.id.ToString(), temp);
            else if (temp.itemRank.Equals(Item.ItemRank.RARE)) equip_rare.Add(temp.id.ToString(), temp);
            else if (temp.itemRank.Equals(Item.ItemRank.EPIC)) equip_epic.Add(temp.id.ToString(), temp);
            else equip_legend.Add(temp.id.ToString(), temp);
        }

        equipDB.Add("NORMAL", equip_normal);
        equipDB.Add("MAGIC", equip_magic);
        equipDB.Add("RARE", equip_rare);
        equipDB.Add("EPIC", equip_epic);
        equipDB.Add("LEGEND", equip_legend);
    }
}

public class SetItemDataC
{

    public Dictionary<int, SetItem> setItems = new Dictionary<int, SetItem>();

    public void DownLoadInfo()
    {

        List<Dictionary<string, object>> SetData = CSVReader.Read("DBData/SetItemDB");

        for (int i = 0; i < SetData.Count; i++)
        {
            SetItem temp = new SetItem(
                       System.Convert.ToByte(SetData[i]["ID"]),
                         (int)SetData[i]["ShieldFactor"],
                         System.Convert.ToSingle(SetData[i]["SpeedFactor"]),
                         System.Convert.ToByte(SetData[i]["HpFactor"]),
                         System.Convert.ToSingle(SetData[i]["EvadeFactor"]),
                         System.Convert.ToSingle(SetData[i]["DefenseFactor"]),
                         System.Convert.ToSingle(SetData[i]["EgnorDebuffFactor"]));
            setItems.Add(temp.setID, temp);
        }
    }
}
public struct SetItem
{
    public byte setID;              // 세트 번호       
    public int shieldFactor;       // 실드
    public float speedFactor;        // 이속 증가
    public byte hpFactor;
    public float evadeFactor;
    public float defenseFactor;
    public float egnoreDebuffFactor; // 디버프 무시 확률

    public SetItem(byte _SetID, int _ShieldFactor, float _SpeedFactor, byte _HpFactor, float _EvadeFactor, float _DefenseFactor, float _EgnoreDebuff)
    {
        setID = _SetID;
        shieldFactor = _ShieldFactor;
        speedFactor = _SpeedFactor;
        hpFactor = _HpFactor;
        evadeFactor = _EvadeFactor;
        defenseFactor = _DefenseFactor;
        egnoreDebuffFactor = _EgnoreDebuff;
    }
}
