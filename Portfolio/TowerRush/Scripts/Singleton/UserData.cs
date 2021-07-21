using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct ItemInfo
{
    public string id;
    public Item.ItemRank rank;
    public Item.ItemType type;

    public ItemInfo(string id, Item.ItemRank rank, Item.ItemType type)
    {
        this.id = id;
        this.rank = rank;
        this.type = type;

    }
}

[System.Serializable]
public struct MagicInfo
{
    public string name;
    public int level;
    public int possessionCount;
    public int levelUp_Price;
    public bool isEquip;

    public MagicInfo(string name,int level, int possession, int levelPrice,bool isEquip)
    {
        this.name = name;
        this.level = level;
        possessionCount = possession;
        levelUp_Price = levelPrice;
        this.isEquip = isEquip;
    }
}

[System.Serializable]
public struct FactoryInfo
{
    public ItemInfo head;
    public ItemInfo scroll;
    public ItemInfo armor;
    public ItemInfo shoe;
    public bool isPresent;
    public FactoryInfo(ItemInfo head, ItemInfo scroll, ItemInfo armor, ItemInfo shoe, bool isPresent)
    {
        this.head = head;
        this.scroll = scroll;
        this.armor = armor;
        this.shoe = shoe;
        this.isPresent = isPresent;
    }
}

[System.Serializable]
public struct TimeStamps
{
    public long goldRewardTimeStamp;
    public long lugaRewardTimeStamp;
    
    public TimeStamps(long goldTime,long lugaTime)
    {
        goldRewardTimeStamp = goldTime;
        lugaRewardTimeStamp = lugaTime;
        
    }
}
[System.Serializable]
public struct UserInfo
{
    public string uID;
    public int level;
    public int exp;
    public int gold;
    public int luga;
    public int stage;
    public int rewardCount;
    public bool isSaved;
    public bool isFinishTutorial;
    public bool isBuydeleteAD;
    public UserInfo(string id, int level, int exp, int gold, int luga, int stage, int rewardCount = 0, bool isSave = false, bool isFinishTutorial = false,bool isBuydeleteAD = false)
    {
        uID = id;
        this.level = level;
        this.exp = exp;
        this.gold = gold;
        this.luga = luga;
        this.stage = stage;
        this.rewardCount = rewardCount;
        isSaved = isSave;
        this.isFinishTutorial = isFinishTutorial;
        this.isBuydeleteAD = isBuydeleteAD;
    }
}
/// <summary>
/// // JSON 형태로 바꿀 때, 프로퍼티는 지원이 안됨. 프로퍼티로 X
/// </summary>
public class UserData : SingleTon<UserData>
{
    void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    public UserInfo userdata;
    public TimeStamps timeStamps;

    public List<MagicInfo> magicList = new List<MagicInfo>();
    public List<MagicInfo> equipedMagicList = new List<MagicInfo>();
    public List<ItemInfo> inventoryItem = new List<ItemInfo>();
    public List<FactoryInfo> factoryInfo = new List<FactoryInfo>();

    public void GetInventoryItem(List<List<Item>> _InventoryItem)
    {
        inventoryItem.Clear();
        // 등급과 아이템 id
        // rare   1001   head
        for (int i = 0; i < _InventoryItem.Count; i++)
        {
            for (int j = 0; j < _InventoryItem[i].Count; j++)
            {
                string id = _InventoryItem[i][j].id.ToString();
                Item.ItemRank rank = _InventoryItem[i][j].itemRank;
                Item.ItemType type = _InventoryItem[i][j].itemType;

                inventoryItem.Add(new ItemInfo(id, rank, type));
            }
        }
    }

    public void GetMagicInfo(List<PlayerMagic> _Magic)
    {
        magicList.Clear();
        foreach (var magic in _Magic)
        {
            magicList.Add(new MagicInfo(magic.magicName,magic.level, magic.possessionCount, magic.levelUp_Price,magic.isEquip));
        }
    }
    public void GetEquipedMagicInfo(List<PlayerMagic> _Magic)
    {
        equipedMagicList.Clear();
        foreach (var magic in _Magic)
        {
            equipedMagicList.Add(new MagicInfo(magic.magicName,magic.level, magic.possessionCount, magic.levelUp_Price, magic.isEquip));
        }
    }
    public void GetFactoryInfo(List<Factory> _Factory)
    {
        factoryInfo.Clear();
        foreach (var factory in _Factory)
        {
            ItemInfo head = factory.equipedItems[0] != null ? new ItemInfo(factory.equipedItems[0].id.ToString(), factory.equipedItems[0].itemRank, factory.equipedItems[0].itemType) : new ItemInfo(null, Item.ItemRank.NULL, Item.ItemType.NULL);
            ItemInfo scroll = factory.equipedItems[1] != null ? new ItemInfo(factory.equipedItems[1].id.ToString(), factory.equipedItems[1].itemRank, factory.equipedItems[1].itemType) : new ItemInfo(null, Item.ItemRank.NULL, Item.ItemType.NULL);
            ItemInfo armor = factory.equipedItems[2] != null ? new ItemInfo(factory.equipedItems[2].id.ToString(), factory.equipedItems[2].itemRank, factory.equipedItems[2].itemType) : new ItemInfo(null, Item.ItemRank.NULL, Item.ItemType.NULL);
            ItemInfo shoe = factory.equipedItems[3] != null ? new ItemInfo(factory.equipedItems[3].id.ToString(), factory.equipedItems[3].itemRank, factory.equipedItems[3].itemType) : new ItemInfo(null, Item.ItemRank.NULL, Item.ItemType.NULL);

            factoryInfo.Add(new FactoryInfo(head, scroll, armor, shoe, factory.IsRepresent));
        }
    }
}
