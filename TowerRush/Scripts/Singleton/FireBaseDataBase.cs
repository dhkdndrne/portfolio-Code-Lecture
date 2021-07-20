using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class FireBaseDataBase : SingleTon<FireBaseDataBase>
{

    public FirebaseAuth auth;
    public FirebaseUser user;
    public FirebaseApp firebaseApp;
    public DatabaseReference reference;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

    public void SetData()
    {
        string json = JsonUtility.ToJson(UserData.Instance);
        reference.Child("users").Child(user.UserId).SetRawJsonValueAsync(json);

    }
    //정보삭제
    public void OnClickRemove()
    {
        reference.Child("users").Child(user.UserId).RemoveValueAsync();
    }

    //타임 스탬프 저장
    public void SetTimeStamp()
    {
        //파이어 베이스에 현재 시간을 저장
        Dictionary<string, object> result = new Dictionary<string, object>();
        Dictionary<string, object> reward = new Dictionary<string, object>();
        result["lugaRewardTimeStamp"] = ServerValue.Timestamp;
        reward["rewardCount"] = UserData.Instance.userdata.rewardCount;

        FirebaseDatabase.DefaultInstance.RootReference.Child("users")
            .Child(user.UserId).Child("timeStamps").UpdateChildrenAsync(result);

        FirebaseDatabase.DefaultInstance.RootReference.Child("users")
    .Child(user.UserId).Child("userdata").UpdateChildrenAsync(reward);
    }
    //유저정보 업데이트
    public void UpdateUserData<T>(string _Key, T _Value)
    {
        if (!UserData.Instance.userdata.isFinishTutorial) return;

        Dictionary<string, object> save = new Dictionary<string, object>();
        save[_Key] = _Value;

        FirebaseDatabase.DefaultInstance.RootReference.Child("users")
            .Child(user.UserId).Child("userdata").UpdateChildrenAsync(save);
    }

    public void UpdateInvenData(int targetIndex)
    {
        if (!UserData.Instance.userdata.isFinishTutorial) return;

        Dictionary<string, object> save = new Dictionary<string, object>();
        List<Item> temp = LobbyManager.Instance.inventory.items[targetIndex];

        for (int i = 0; i < temp.Count; i++)
        {
            string id = temp[i].id.ToString();
            Item.ItemRank rank = temp[i].itemRank;
            Item.ItemType type = temp[i].itemType;

            UserData.Instance.inventoryItem.Add(new ItemInfo(id, rank, type));
        }

        save["inventoryItem"] = UserData.Instance.inventoryItem;
        FirebaseDatabase.DefaultInstance.RootReference.Child("users")
            .Child(user.UserId).Child("inventoryItem").UpdateChildrenAsync(save);

        UserData.Instance.inventoryItem.Clear();
    }
}
