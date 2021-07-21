using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSlotMover : MonoBehaviour
{
    [SerializeField] GameObject enterList;
    [SerializeField] GameObject armyList;

    private void Start()
    {
        armyList = transform.parent.gameObject;
        enterList = GameObject.Find("Entercontent").gameObject;
        transform.GetComponent<Button>().onClick.AddListener(SelectUnit);
    }

    void SelectUnit()
    {
        if (transform.parent.gameObject == armyList)
        {
            transform.SetParent(enterList.transform);
            LobbyManager.Instance.armyCount++;
        }
        else
        {
            transform.SetParent(armyList.transform);
            LobbyManager.Instance.armyCount--;
        }                    
    }

}
