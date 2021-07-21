using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageOutLine : MonoBehaviour
{
    Image img;
    Material mat;
    bool isInit;

    public bool IsInit { get { return isInit; }private set { } }
    public void Init()
    {
        img = GetComponent<Image>();       
        isInit = true;
    }
    public void SetOutLineColor(Item _Item)
    {
        mat = LobbyManager.Instance.lobbyUI.itemRankMat[(int)_Item.itemRank-1];
        img.material = mat;
    }
}
