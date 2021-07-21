using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyOrganizer : MonoBehaviour
{
    public Transform blankSlot;
    [SerializeField] List<SlotArranger> slotArrangers = new List<SlotArranger>();
    [SerializeField] Button[] buttons = new Button[2];

    SlotArranger workingArranger;
    int originIndex;

    public static void SwapSlots(Transform _Sour, Transform _Dest)
    {
        Transform sourParent = _Sour.parent;
        Transform destParent = _Dest.parent;

        int sourIndex = _Sour.GetSiblingIndex();
        int destIndex = _Sour.GetSiblingIndex();

        _Sour.SetParent(destParent);
        _Sour.SetSiblingIndex(destIndex);

        _Dest.SetParent(sourParent);
        _Dest.SetSiblingIndex(sourIndex);
    }

    void SwapSlotsInHieracrchy(Transform _Sour, Transform _Dest)
    {
        SwapSlots(_Sour, _Dest);
        slotArrangers.ForEach(t => t.UpdateSlots());
    }

    //포지션이 RectTransform 안에 있는지 판단하는함수
    bool ContainPos(RectTransform _Rt, Vector2 _Pos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(_Rt, _Pos);
    }

    void BeginDrag(Transform _Slot)
    {
        workingArranger = slotArrangers.Find(t => ContainPos(t.transform as RectTransform, _Slot.position));
        originIndex = _Slot.GetSiblingIndex();
        SwapSlotsInHieracrchy(blankSlot, _Slot);
    }

    void Drag(Transform _Slot)
    {
        var whichArrangerUnit = slotArrangers.Find(t => ContainPos(t.transform as RectTransform, _Slot.position));


        if (whichArrangerUnit == null)
        {
            // 현재 transform이 blankslot의 부모가 아니라면 업데이트 해줘야함
            bool updateSlots = transform != blankSlot.parent;

            blankSlot.SetParent(transform);

            if (updateSlots)
            {
                slotArrangers.ForEach(t => t.UpdateSlots());
            }
        }
        else
        {
            bool insert = blankSlot.parent.Equals(transform);

            if (insert)
            {
                //선택 팩토리 수가 가지고있는 팩토리만큼
                if (whichArrangerUnit.isEnterList && whichArrangerUnit.slots.Count > LobbyManager.Instance.factoryManager.factoryCount) return;

                int index = whichArrangerUnit.GetIndexByPosition(_Slot);
                blankSlot.SetParent(whichArrangerUnit.transform);
                whichArrangerUnit.InsertSlot(blankSlot, index);

            }
            else
            {
                int blankSlotIndex = blankSlot.GetSiblingIndex();
                int targetIndex = whichArrangerUnit.GetIndexByPosition(_Slot, blankSlotIndex);
                //현재 빈슬롯의 인덱스와 있어야할 인덱스랑 다르면 바꿔준다
                if (blankSlotIndex != targetIndex)
                {
                    whichArrangerUnit.SwapSlot(blankSlotIndex, targetIndex);
                }
            }
        }
    }

    void EndDrag(Transform _Slot)
    {
        // 마우스를 땠을때 슬롯 밖에있으면
        if (blankSlot.parent.Equals(transform))
        {
            _Slot.SetParent(workingArranger.transform);
            workingArranger.InsertSlot(_Slot, originIndex);
            workingArranger = null;
            originIndex = -1;
        }
        else
        {
            SwapSlotsInHieracrchy(blankSlot, _Slot);
        }
        LobbyManager.Instance.armyCount = slotArrangers[1].transform.childCount;
    }


}
