using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitDrag : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    RectTransform rectTransform;
    Transform root;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        root = GameObject.Find("ArmyPanel").transform;

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        root.BroadcastMessage("BeginDrag", transform, SendMessageOptions.DontRequireReceiver);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
        root.BroadcastMessage("Drag", transform, SendMessageOptions.DontRequireReceiver);
    }


    public void OnEndDrag(PointerEventData eventData)      
    {
        root.BroadcastMessage("EndDrag", transform, SendMessageOptions.DontRequireReceiver);
    }

}
