using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

public static class SaveSystem
{
	private static string SavePath =>"Assets/Resources/Maps/";
	private static DirectoryInfo directoryInfo;

	public static int Num { get; private set; }

	public static void SetNum(int num)
	{
		Num = num;
	}

	#if UNITY_EDITOR
	
	public static void Save(MapSaveData saveData)
	{
		if (!Directory.Exists(SavePath))
		{
			Directory.CreateDirectory(SavePath);
		}

		if (directoryInfo == null)
		{
			directoryInfo = new DirectoryInfo(SavePath);
		}
		
		string saveJson = JsonUtility.ToJson(saveData);

		string num = Num < 10 ? "0" + Num : Num.ToString();
					
		string saveFilePath = SavePath+"save_" + num + ".json";
		File.WriteAllText(saveFilePath, saveJson);
		UtilClass.DebugLog("저장 성공");
		Debug.Log("Save Success: " + saveFilePath);
		AssetDatabase.Refresh();
	}
    #endif
	public static void Load()
	{
		var maps = Resources.LoadAll<TextAsset>("Maps");
		
		for (int i = 0; i < maps.Length; i++)
		{
			MapSaveData mapSaveData = JsonUtility.FromJson<MapSaveData>(maps[i].text);
			DataManager.Instance.MapSaveDataDic.Add(i+1,mapSaveData);
		}
	}
}


[System.Serializable]
public class MapSaveData
{
	public List<string> enemyIDList;
	public List<Vector2> enemyPosList = new();
	public List<Quaternion> enemyRotList = new();
	
	public List<Vector2> wallPosList = new();
	public List<Quaternion> wallRotList = new();
	public List<Vector3> wallScaleList = new();

	public List<MapDataSwitchWall> mapSwitchWallList = new();
	public Vector3 playerPos;
	public Quaternion playerRot;

	public List<ItemData> itemDataList = new();
}
[System.Serializable]
public class MapDataSwitchWall
{
	public bool isMove;
	public Vector2 v2Pos;
	public Vector2 v2Scale;

	public Vector2 v2WallMovePos;
	public Vector2 v2WallPos;
	public Vector2 v2WallScale;
	public Quaternion wallRot;
}
[System.Serializable]
public class ItemData
{
	public ItemType type;
	public int MaxCount;
	public float value;
	public Vector2 v2Pos;
}