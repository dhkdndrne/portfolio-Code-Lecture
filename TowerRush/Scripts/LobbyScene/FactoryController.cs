using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryController : MonoBehaviour
{
    public List<Factory> factories = new List<Factory>();
    public Factory representFactory;

    [SerializeField] GameObject factoryPrefab;
    //현재 팩토리 개수
    public int factoryCount;

    //최대 팩토리 개수
    public int factoryMaxCount;

    //팩토리 가격
    public int factoryPrice;

    //현재 팩토리 인덱스
    public int currFactoryIndex;

    public void Init()
    {
        factoryCount = UserData.Instance.userdata.isSaved ? UserData.Instance.factoryInfo.Count : 4;
        for (int i = 0; i < factoryCount; i++)
        {
            Instantiate(factoryPrefab, LobbyManager.Instance.lobbyUI.factoryImageParents.transform);
            LobbyManager.Instance.lobbyUI.SetFactoryUI(i);
        }

        if (UserData.Instance.userdata.isSaved)
        {
            LoadFactoryInfo();
        }
        else
        {
            for (int i = 0; i < factoryCount; i++)
            {
                InitFactory(i);
            }
            factories[0].IsRepresent = true;
            SetRepresntFactory(0);
        }

        for (int i = 0; i < factoryCount; i++)
        {
            SetFactoryItemUnit(i);
        }
    }

    void LoadFactoryInfo()
    {
        for (int i = 0; i < factoryCount; i++)
        {
            InitFactory(i);
            FactoryInfo temp = UserData.Instance.factoryInfo[i];          

            if (temp.head.type != Item.ItemType.NULL) factories[i].EquipItem((Item)DBManager.Instance.itemDB.headDB[temp.head.rank.ToString()][temp.head.id.ToString()]);
            if (temp.scroll.type != Item.ItemType.NULL)
            {
                Scroll tempScroll = (Scroll)DBManager.Instance.itemDB.scrollDB[temp.scroll.rank.ToString()][temp.scroll.id.ToString()];
                //tempScroll.GetSkillData();
                factories[i].EquipItem(tempScroll);
            }               
                            
            if (temp.armor.type != Item.ItemType.NULL) factories[i].EquipItem((Item)DBManager.Instance.itemDB.equipDB[temp.armor.rank.ToString()][temp.armor.id.ToString()]);
            if (temp.shoe.type != Item.ItemType.NULL) factories[i].EquipItem((Item)DBManager.Instance.itemDB.equipDB[temp.shoe.rank.ToString()][temp.shoe.id.ToString()]);

            if (temp.isPresent) SetRepresntFactory(i);
        }

    }

    //대표 팩토리 설정
    public void SetRepresntFactory(int _Num)
    {
        foreach (Factory factory in factories)
        {
            if (factory.IsRepresent)
            {
                factory.IsRepresent = false;
                break;
            }
        }
        factories[_Num].IsRepresent = true;
        factories[_Num].ChangeRepreSentImg();
    }

    //팩토리의 유닛 및 아이템 설정
    void SetFactoryItemUnit(int _Idx)
    {
        for (int j = 0; j < factories[_Idx].equipedItems.Length; j++)
        {
            if (factories[_Idx].equipedItems[j] != null)
            {
                int index = (int)factories[_Idx].equipedItems[j].itemType - 1;
                factories[_Idx].ChangeUI(factories[_Idx].equipedItems[j], index, true);
            }
            else
            {
                if (j != 1)
                    factories[_Idx].ChangeUI(null, j, false);
            }
        }
        LobbyManager.Instance.lobbyUI.UpdateFactoryUIInfo(_Idx);

    }
    //팩토리 인스턴스 생성
    void InitFactory(int _Idx)
    {
        factories.Add(new Factory());
        factories[_Idx].equipedItems = new Item[4];
        factories[_Idx].factoryId = _Idx;

        factories[_Idx].unit = new Unit();
        factories[_Idx].unit.parent = factories[_Idx];
        factories[_Idx].unit.InputStatInfo();
    }

    //팩토리 구매
    public void AddFactory()
    {
        if (LobbyManager.Instance.Gold >= factoryPrice && factoryCount < factoryMaxCount)
        {
            Instantiate(factoryPrefab, LobbyManager.Instance.lobbyUI.factoryImageParents.transform);
            LobbyManager.Instance.Gold -= factoryPrice;

            factoryCount++;
            factoryPrice *= 2;
            InitFactory(factoryCount - 1);
            LobbyManager.Instance.lobbyUI.SetFactoryUI(factoryCount - 1);
            SetFactoryItemUnit(factoryCount - 1);
            LobbyManager.Instance.lobbyUI.UpdateFactoryUIInfo(factoryCount - 1);
            LobbyManager.Instance.lobbyUI.open_F_PanelBtn.onClick?.Invoke();
        }
    }

    public void FactorySetting(int _Num)
    {
        currFactoryIndex = _Num;
        for (int i = 0; i < 4; i++)
            LobbyManager.Instance.lobbyUI.SetAnimItemImage(i, factories[_Num].equipedItems[i], factories[_Num].CheckExistItem(i));
    }
    //팩토리 아이템 장착 해제하는 함수
    public void UnEquipFactoryItem(int _ItemType)
    {
        int tempIndex = _ItemType.Equals(3) ? 2 : _ItemType;

        //아이템이 장착되어있지 않으면 리턴
        if (factories[currFactoryIndex].equipedItems[tempIndex] == null) return;

        //인벤토리가 꽉 차있지 않으면 장착 해제
        if (LobbyManager.Instance.inventory.CheckIsFull(tempIndex, 1))
            factories[currFactoryIndex].UnEquipItem(_ItemType);
    }


    //팩토리 아이템 장착 함수
    public void EquipFactoryItem(Item _Item)
    {
        factories[currFactoryIndex].EquipItem(_Item);
    }

}

