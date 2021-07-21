using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Factory
{
    public int factoryId;
    public Item[] equipedItems;
    public Unit unit;

    bool isRepresent;
    public bool IsRepresent { get { return isRepresent; } set { isRepresent = value; } }
    public int setID { get; private set; }
    public float coolTime { get; private set; }

    public bool CheckExistItem(int _Index)
    {
        if (equipedItems[_Index] != null) return true;
        return false;
    }

    public void EquipItem(Item _Item)
    {
        int slotIndex = (int)_Item.itemType - 1;
        UnEquipItem(slotIndex);
        equipedItems[slotIndex] = _Item;

        LobbyManager.Instance.lobbyUI.SetAnimItemImage(slotIndex, _Item, CheckExistItem(slotIndex));   // 팩토리 애니메이션 이미지
        LobbyManager.Instance.lobbyUI.UpdateFactoryItemInfo(factoryId);                                // 팩토리 아이템 이미지 갱신      
        ChangeUI(_Item, slotIndex, true);                                                   // 팩토리선택창 장착이미지
            
        bool check = true;
        for (int i = 0; i < 4; i++)
        {
            if (i.Equals(1)) continue;
            if (CheckExistItem(i)) continue;

            check = false;
        }

        ChangeRepreSentImg();
        unit.InputStatInfo();                                        //유닛 정보 갱신
        LobbyManager.Instance.lobbyUI.UpdateFactoryUIInfo(factoryId);          // 팩토리 유닛 스텟 갱신

        //아이템이 모두 장착되어있으면 세트인지 확인
        if (check)
        {
            byte ID = equipedItems[0].setID;
            if (!equipedItems[2].setID.Equals(ID)) return;
            if (!equipedItems[3].setID.Equals(ID)) return;

            setID = ID;
        }
        SetCoolTime(_Item,false);
    }

    public void SetCoolTime(Item _Item,bool _IsUnEquip)
    {
        if(_IsUnEquip) coolTime -= (int)_Item.itemRank;
        else coolTime += (int)_Item.itemRank;
    }
    public void ChangeRepreSentImg()
    {
        if (isRepresent)
        {
            LobbyManager.Instance.lobbyUI.representCharImg.sprite = equipedItems[0]?.image ?? LobbyManager.Instance.lobbyUI.defaultHeadImage;
            LobbyManager.Instance.lobbyUI.representHead.sprite = equipedItems[0]?.image ?? LobbyManager.Instance.lobbyUI.defaultHeadImage;
            LobbyManager.Instance.lobbyUI.representBody.sprite = equipedItems[2]?.image ?? LobbyManager.Instance.lobbyUI.defaultBodyImage;

            string temp = "";
            if (equipedItems[3] != null)
            {
                temp = equipedItems[3].id.ToString();
            }
            Sprite leg = null;
            if (temp != "") leg = Resources.Load<Sprite>("InGameShoe/" + temp);
            else leg = Resources.Load<Sprite>("InGameShoe/Basic");

            LobbyManager.Instance.lobbyUI.representLeft.sprite = leg;
            LobbyManager.Instance.lobbyUI.representRight.sprite = leg;
        }
    }
    //공장 대표캐릭터 보여주는 이미지 수정
    public void ChangeUI(Item _Item, int _ItemType, bool flag)
    {
        if (flag)
        {
            if (_ItemType.Equals(0)) LobbyManager.Instance.lobbyUI.f_HeadImages[factoryId].sprite = _Item.image;
            else if (_ItemType.Equals(2)) LobbyManager.Instance.lobbyUI.f_BodyImages[factoryId].sprite = _Item.image;
            else if (_ItemType.Equals(3)) LobbyManager.Instance.lobbyUI.f_ShoeImages[factoryId].sprite = _Item.image;
        }
        else
        {
            if (_ItemType.Equals(0)) LobbyManager.Instance.lobbyUI.f_HeadImages[factoryId].sprite = LobbyManager.Instance.lobbyUI.defaultHeadImage;
            else if (_ItemType.Equals(2)) LobbyManager.Instance.lobbyUI.f_BodyImages[factoryId].sprite = LobbyManager.Instance.lobbyUI.defaultBodyImage;
            else if (_ItemType.Equals(3)) LobbyManager.Instance.lobbyUI.f_ShoeImages[factoryId].sprite = LobbyManager.Instance.lobbyUI.defaultShoeImage;
        }
    }
    //장비 해제 하는 함수
    public void UnEquipItem(int _SlotIndex)
    {
        setID = 0;
        Item oldItem = null;

        //아이템이 장착되어있으면
        if (equipedItems[_SlotIndex] != null)
        {
            //이전 장비를 인벤토리에 추가한다.
            oldItem = equipedItems[_SlotIndex];
            LobbyManager.Instance.inventory.AddItem(oldItem);
            equipedItems[_SlotIndex] = null;
            unit.InputStatInfo();
            SetCoolTime(oldItem, true);
        }

        LobbyManager.Instance.lobbyUI.UpdateFactoryItemInfo(factoryId);
        ChangeRepreSentImg();
        // 공장 애니메이션 이미지 바꿔줌
        LobbyManager.Instance.lobbyUI.SetAnimItemImage(_SlotIndex, null, CheckExistItem(_SlotIndex));
        ChangeUI(null, _SlotIndex, false);
        LobbyManager.Instance.lobbyUI.UpdateFactoryUIInfo(factoryId);          // 팩토리 유닛 스텟 갱신
    }
    public void UnEquipAllItem()
    {
        for (int i = 0; i < equipedItems.Length; i++)
        {
            UnEquipItem(i);
        }
    }
}
