using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public class Inventory : MonoBehaviour
{
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public List<List<Item>> items = new List<List<Item>>();

    public InventorySlot selectedSlot;

    //아이템 조합에 선택됏는지
    [SerializeField] bool isCombined;
    public bool IsCombined { get { return isCombined; } set { isCombined = value; } }

    //인벤토리 최대 공간
    public int space;
    bool isFactoryManage;
    public bool IsFactoryManage { get { return isFactoryManage; } set { isFactoryManage = value; } }

    public int currIndex;
    public void Init()
    {
        selectedSlot = null;
        for (int i = 0; i < 3; i++)
        {
            items.Add(new List<Item>());
        }

        //저장된 정보가 있으면 로드
        if (UserData.Instance.userdata.isSaved)
        {
            LoadItems();
        }

        if (DataController.getItem != null)
        {
            AddItem(DataController.getItem);
            DataController.getItem = null;
        }
    }
    void LoadItems()
    {
        for (int i = 0; i < UserData.Instance.inventoryItem.Count; i++)
        {
            switch (UserData.Instance.inventoryItem[i].type)
            {
                case Item.ItemType.HEAD:
                    AddItem((Item)DBManager.Instance.itemDB.headDB[UserData.Instance.inventoryItem[i].rank.ToString()][UserData.Instance.inventoryItem[i].id.ToString()]);
                    break;

                case Item.ItemType.CHARACTERISTIC:
                    Scroll temp = (Scroll)DBManager.Instance.itemDB.scrollDB[UserData.Instance.inventoryItem[i].rank.ToString()][UserData.Instance.inventoryItem[i].id.ToString()];
                    // temp.GetSkillData();
                    AddItem(temp);
                    break;

                default:
                    AddItem((Item)DBManager.Instance.itemDB.equipDB[UserData.Instance.inventoryItem[i].rank.ToString()][UserData.Instance.inventoryItem[i].id.ToString()]);
                    break;

            }
        }

    }
    public void SellItem()
    {
        if (selectedSlot != null && selectedSlot.item != null)
        {
            switch (selectedSlot.item.itemRank)
            {
                case Item.ItemRank.NORMAL:
                    LobbyManager.Instance.Gold += 500;
                    break;

                case Item.ItemRank.MAGIC:
                    LobbyManager.Instance.Gold += 1000;
                    break;

                case Item.ItemRank.RARE:
                    LobbyManager.Instance.Gold += 3000;
                    break;

                case Item.ItemRank.EPIC:
                    LobbyManager.Instance.Gold += 5000;
                    break;

                case Item.ItemRank.LEGEND:
                    LobbyManager.Instance.Gold += 10000;
                    break;
            }
            int targetIndex = 0;

            if (selectedSlot.item.itemType == Item.ItemType.SHOE || selectedSlot.item.itemType == Item.ItemType.ARMOR) targetIndex = 2;
            else targetIndex = ((int)selectedSlot.item.itemType) - 1;

            for (int j = 0; j < items[targetIndex].Count; j++)
            {   //인벤토리에서 슬롯의 아이템과 같은 아이템을 삭제한다.
                if (selectedSlot.item.Equals(items[targetIndex][j]))
                {
                    Remove(j);
                    break;
                }
            }
            selectedSlot = null;
            LobbyManager.Instance.inventoryUI.sellButton.interactable = false;
            LobbyManager.Instance.inventoryUI.SetItemInfo(null);
            StartCoroutine(LobbyManager.Instance.lobbyUI.PanelAnim(LobbyManager.Instance.inventoryUI.sell_PopUpPanel, true));
        }
    }

    //아이템 추가
    public void AddItem(Item _Item)
    {
        int temp = _Item.ItemTypeToInt();
        if (_Item.itemType != Item.ItemType.NULL)
        {
            if (items[temp].Count >= space)
            {
                Debug.Log("인벤토리가 가득 찼습니다.");
                return;
            }

            items[temp].Add(_Item);
            onItemChangedCallback?.Invoke();
        }
    }

    //인벤토리 정렬
    public void SortInventory()
    {
        items[currIndex].Sort(Compare);

        //선택한 아이템정보 공백으로 바꿔줌
        LobbyManager.Instance.inventoryUI.SetItemInfo(null);
        onItemChangedCallback?.Invoke();
    }

    //아이템 제거
    public void Remove(int _Idx)
    {
        items[currIndex].RemoveAt(_Idx);
        onItemChangedCallback?.Invoke();
    }

    //인벤토리가 가득찼는지 체크하는 함수
    public bool CheckIsFull(int _Index, int _Num)
    {
        if (items[_Index].Count + _Num <= space) return true;
        else
        {
            LobbyManager.Instance.lobbyUI.ShowAlarm("인벤토리가 가득 찼습니다.");
        }
        return false;
    }

    int Compare(Item _A, Item _B)
    {
        // 아이템 랭크 비교
        int result = _B.itemRank.CompareTo(_A.itemRank);

        // 아이템 랭크가 같으면 아이디 비교값을 리턴
        if (result.Equals(0))
            return _A.id.CompareTo(_B.id);
        else
            return result;
    }

    //아이템 장착
    public void EquipItem()
    {
        if (selectedSlot != null && selectedSlot.item != null)
        {
            selectedSlot.item.Equip(selectedSlot.yIndex);
            LobbyManager.Instance.inventoryUI.SetItemInfo(null);
            LobbyManager.Instance.lobbyUI.ShowAlarm("장착되었습니다.");

            if (!UserData.Instance.userdata.isFinishTutorial)
            {
                LobbyManager.Instance.tutorialManager.check = true;
            }
        }
    }
}