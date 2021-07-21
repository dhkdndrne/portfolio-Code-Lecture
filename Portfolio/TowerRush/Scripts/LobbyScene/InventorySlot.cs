using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Item item { get; private set; }
    public ImageOutLine outline;

    public int yIndex;
    public bool isCombined;


    // 아이템 추가
    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.image;
        icon.enabled = true;
        if (!outline.IsInit)
        {
            outline.Init();
        }

        outline.SetOutLineColor(item);
    }
    public void SelectItem()
    {
        // 공장관리모드이고 아이템이 비어있지 않을때
        if (LobbyManager.Instance.inventory.IsFactoryManage && item != null)
        {
            LobbyManager.Instance.inventory.selectedSlot = this;
            LobbyManager.Instance.inventoryUI.equipButton.interactable = true;
        }

        LobbyManager.Instance.inventoryUI.SetItemInfo(this);
    }

    // 슬롯 초기화
    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }


}
