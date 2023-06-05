using System.Collections.Generic;
using Bam.Singleton;
using Firebase.Database;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class DataManager : Singleton<DataManager>
{
	[SerializeField] private List<EnemyModelSO> enemymodelSoList = new();

	public Dictionary<string, EnemyModelSO> EnemyModelDic { get; private set; } = new();
	public Dictionary<int, MapSaveData> MapSaveDataDic { get; private set; } = new();

	public UserData UserData { get; private set; }
	private Dictionary<string, object> saveDic = new Dictionary<string, object>();

	protected override void Awake()
	{
		base.Awake();

		foreach (var So in enemymodelSoList)
			EnemyModelDic.Add(So.ID, So);

		SaveSystem.Load();
	}

	public void InitUserData(UserData userData)
	{
		Debug.Log($"유저 데이터 초기화 완료{userData}");
		UserData = userData;
	}
	public void WriteData()
	{
		string jsonData = JsonUtility.ToJson(UserData);
		FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(UserData.Uid).SetRawJsonValueAsync(jsonData);
	}
	public void UpdateUserData<T>(string key, T value)
	{
		saveDic.Clear();
		saveDic[key] = value;

		FirebaseDatabase.DefaultInstance.RootReference.Child("users")
			.Child(UserData.Uid).UpdateChildrenAsync(saveDic);
	}
}

[System.Serializable]
public class UserData
{
	public string nickName;
	public string uid;
	public bool isBuyAD;
	public int maxStage = 1;

	public string NickName => nickName;
	public string Uid => uid;
	public bool IsBuyAD => isBuyAD;
	public int MaxStage => maxStage;

	public UserData(string nickName, string uid, int maxStage, bool buyAD)
	{
		this.nickName = nickName;
		this.uid = uid;
		isBuyAD = buyAD;
		this.maxStage = maxStage;
	}
}


[System.Serializable]
public class GameModel
{
	public bool isRefresh = true;
	public bool isReplay;
	public bool isPanelOpen;
	public bool isPlayingGimmick;

	public int maxStage;
	public ReactiveProperty<int> SelectLevel { get; private set; } = new(1);
	public ReactiveProperty<int> EnemyCnt = new();
	public ReactiveProperty<bool> IsGameOver { get; private set; } = new();

	public ReactiveProperty<int> Timer { get; private set; } = new();

}