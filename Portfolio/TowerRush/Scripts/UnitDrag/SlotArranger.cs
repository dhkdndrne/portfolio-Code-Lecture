using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotArranger : MonoBehaviour
{
    public List<Transform> slots = new List<Transform>();
    public bool isEnterList;
    void Start()
    {
        UpdateSlots();
    }

    public void UpdateSlots()
    {
        for(int i = 0;i<transform.childCount;i++)
        {
            if(i.Equals(slots.Count))
                slots.Add(null);

            var slot = transform.GetChild(i);

            if (slot != slots[i])
                slots[i] = slot;
        }

        slots.RemoveRange(transform.childCount, slots.Count - transform.childCount);
    }
    //슬롯 위치를 변경하는 함수
    public void SwapSlot(int _Index1,int _Index2)
    {
        ArmyOrganizer.SwapSlots(slots[_Index1], slots[_Index2]);
        UpdateSlots();
    }
    public void InsertSlot(Transform _Slot,int _Index)
    {
        slots.Add(_Slot);
        _Slot.SetSiblingIndex(_Index);
        UpdateSlots();
    }
    //위치에 따라 인덱스를 반환하는 함수
    public int GetIndexByPosition(Transform _Slot, int skipIndex = -1)
    {
        int result = 0;
        for (int i = 0; i<slots.Count;i++)
        {
            //현재 슬롯의 x 슬롯의 i 의 x보다 작으면 나간다
            if (_Slot.position.x < slots[i].position.x)
                break;
            else if (skipIndex != i)
                result++;
        }

        return result;
    }
}
