using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{   
    public enum ItemType
    {
        NULL,
        HEAD,
        CHARACTERISTIC,
        ARMOR,
        SHOE
    }
    public enum ItemRank
    {
        NULL,
        NORMAL,
        MAGIC,
        RARE,
        EPIC,
        LEGEND       
    }

    //아이템 공통 변수
    public Sprite image;
    public ItemType itemType;
    public ItemRank itemRank;
 
    public int id;
    public byte setID;
    public string itemName;
    public float pickRate;

    public string description;

    public float addOption;
    public float addOptionPer;

    public Item(int _ID, string _Name, ItemType _Type, ItemRank _Rank, float _PickRate, string _Description, byte _SetID = 0)
    {
        id = _ID;
        setID = _SetID;
        itemName = _Name;
        itemType = _Type;
        itemRank = _Rank;
        pickRate = _PickRate;
        description = _Description;
    }
    public Item() { }
    public int ItemTypeToInt()
    {
        int num = 0;

        switch (itemType)
        {
            case ItemType.HEAD:
                num = 0;
                break;
            case ItemType.CHARACTERISTIC:
                num = 1;
                break;
            case ItemType.ARMOR:
                num = 2;
                break;
            case ItemType.SHOE:
                num = 2;
                break;
        }

        return num;
    }
    public void Equip(int _Idx)
    {
        LobbyManager.Instance.factoryManager.EquipFactoryItem(this);
        RemoveFromInventory(_Idx);
    }
    void RemoveFromInventory(int _Idx)
    {
        LobbyManager.Instance.inventory.Remove(_Idx);
    }
}
