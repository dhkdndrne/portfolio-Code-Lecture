using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPController : MonoBehaviour
{
   private static GameObject iapButton => GameObject.FindWithTag("ADButton");

   public static void Init(bool isBuy) => iapButton.SetActive(!isBuy);

   public void Reward()
   {
      iapButton.SetActive(false);
      UserData userData = new UserData(DataManager.Instance.UserData.nickName,DataManager.Instance.UserData.Uid,DataManager.Instance.UserData.MaxStage ,true);
      DataManager.Instance.InitUserData(userData);
      DataManager.Instance.UpdateUserData("isBuyAD",true);
   }

   public void Failed()
   {
      Debug.Log("결제 실패");
   }
}
