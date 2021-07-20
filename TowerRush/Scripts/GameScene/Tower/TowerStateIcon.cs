using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TowerStateIcon : MonoBehaviour
{
    TowerBase tower;
    Image[] towerStateImage = new Image[8];
    private void Start()
    {
        tower = gameObject.GetComponent<TowerBase>();
        tower.checkTowerStateCallBack += GetTowerState;

        towerStateImage = transform.GetComponentsInChildren<Image>();

        for (int i = 0; i < towerStateImage.Length; i++)
        {
            towerStateImage[i].gameObject.SetActive(false);
        }
    }
    void GetTowerState()
    {
        #region 나아~중에 바꿀수도
        //string temp = tower.towerState.ToString();
        //string[] st = temp.Split(',');

        //for(int i =0;i<st.Length;i++)
        //{
        //    TowerState state = (TowerState)Enum.Parse(typeof(TowerState), st[i]);
        //    SetIcon(state);
        //} 
        #endregion
        SetIcon(tower.towerState);

    }

    public void SetIcon(TowerState _State)
    {
        if (_State.HasFlag(TowerState.EMP)) { towerStateImage[0].gameObject.SetActive(true); } else { towerStateImage[0].gameObject.SetActive(false); }
        if (_State.HasFlag(TowerState.BREAKDOWN)) { towerStateImage[1].gameObject.SetActive(true); } else { towerStateImage[1].gameObject.SetActive(false); }
        if (_State.HasFlag(TowerState.THUNDERBOLT)) { towerStateImage[2].gameObject.SetActive(true); } else { towerStateImage[2].gameObject.SetActive(false); }
        if (_State.HasFlag(TowerState.FEATHER)) { towerStateImage[3].gameObject.SetActive(true); } else { towerStateImage[3].gameObject.SetActive(false); }
        if (_State.HasFlag(TowerState.SMOKE)) { towerStateImage[4].gameObject.SetActive(true); } else { towerStateImage[4].gameObject.SetActive(false); }
        if (_State.HasFlag(TowerState.ICEHORN)) { towerStateImage[5].gameObject.SetActive(true); } else { towerStateImage[5].gameObject.SetActive(false); }
        if (_State.HasFlag(TowerState.BOOM)) { towerStateImage[6].gameObject.SetActive(true); } else { towerStateImage[6].gameObject.SetActive(false); }
        if (_State.HasFlag(TowerState.CYCLONE)) { towerStateImage[7].gameObject.SetActive(true); } else { towerStateImage[7].gameObject.SetActive(false); }
    }
}
