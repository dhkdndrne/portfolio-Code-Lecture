using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IPA : MonoBehaviour
{
    public void BuyDiamod(int _index)
    {
        switch(_index)
        {
            case 0:
                LobbyManager.Instance.lobbyUI.StartResourceEff(false,0,500);
                break;
            case 1:
                LobbyManager.Instance.lobbyUI.StartResourceEff(false, 0, 1200);
                break;
            case 2:
                LobbyManager.Instance.lobbyUI.StartResourceEff(false, 1, 2500);
                break;
            case 3:
                LobbyManager.Instance.lobbyUI.StartResourceEff(false, 2, 6500);
                break;
            case 4:
                LobbyManager.Instance.lobbyUI.StartResourceEff(false, 2, 14000);
                break;
        }
    }

    public void BuyRemoveAD()
    {
        UserData.Instance.userdata.isBuydeleteAD = true;
        FireBaseDataBase.Instance.UpdateUserData("isBuydeleteAD",true);
        gameObject.GetComponent<Button>().interactable = false;       
    }

    public void Fail()
    {
        
    }
}
