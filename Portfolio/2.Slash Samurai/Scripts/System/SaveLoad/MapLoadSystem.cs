using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MapLoadSystem : MonoBehaviour
{
	private bool isMapLoadComplete;
	private bool isEnemyLoadComplete;
	public async UniTask LoadMapData()
	{
		SaveSystem.SetNum(GameManager.Instance.GameModel.SelectLevel.Value);
		MapSaveData mapSaveData = DataManager.Instance.MapSaveDataDic[GameManager.Instance.GameModel.SelectLevel.Value];

		ObjectPoolManager.Instance.DespawnAll();

		LoadMap(mapSaveData);
		LoadEnemy(mapSaveData);
		LoadSwitchWall(mapSaveData);
		LoadItem(mapSaveData);
		
		GameManager.Instance.player.gameObject.SetActive(true);
		GameManager.Instance.player.Init(mapSaveData.playerPos, mapSaveData.playerRot);

		await UniTask.WaitUntil(() => isMapLoadComplete && isEnemyLoadComplete);
	}

	private void LoadMap(MapSaveData loadData)
	{
		for (int i = 0; i < loadData.wallPosList.Count; i++)
		{
			var obj = ObjectPoolManager.Instance.Spawn(Define.WALL_TAG);
			Transform objTransform = obj.transform;

			objTransform.SetPositionAndRotation(loadData.wallPosList[i], loadData.wallRotList.Count == 0 ? Quaternion.identity : loadData.wallRotList[i]);
			objTransform.localScale = loadData.wallScaleList[i];
		}

		isMapLoadComplete = true;
	}

	private void LoadEnemy(MapSaveData loadData)
	{
		for (int i = 0; i < loadData.enemyPosList.Count; i++)
		{
			var obj = ObjectPoolManager.Instance.Spawn(Define.ENEMY_TAG);
			obj.transform.SetPositionAndRotation(loadData.enemyPosList[i], loadData.enemyRotList.Count == 0 ? Quaternion.identity : loadData.enemyRotList[i]);

			var enemy = obj.GetComponent<Enemy>();
			enemy.Init(DataManager.Instance.EnemyModelDic[loadData.enemyIDList[i]]);
		}

		isEnemyLoadComplete = true;
	}

	private void LoadSwitchWall(MapSaveData loadData)
	{
		foreach (var switchWall in loadData.mapSwitchWallList)
		{
			var obj = ObjectPoolManager.Instance.Spawn(Define.SWITCH_TAG);
			obj.GetComponent<WallSwitch>().Init(switchWall);
		}
	}

	private void LoadItem(MapSaveData loadData)
	{
		foreach (var item in loadData.itemDataList)
		{
			var obj = ObjectPoolManager.Instance.Spawn(item.type + "_Item");
			obj.GetComponent<ItemBase>().Init(item);
		}
	}
}