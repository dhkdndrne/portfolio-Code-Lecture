#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using UniRx;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine.Rendering;

public class SaveLoadSystem : MonoBehaviour
{
	[SaveLoad]
	public SaveLoadAttribute attribute;
}

[System.Serializable]
public class SaveLoadAttribute : Attribute
{
	public int num;
}

public class SaveLoadAttributeDrawer : OdinAttributeDrawer<SaveLoadAttribute>
{
	private InspectorProperty num;
	
	protected override void Initialize()
	{
		num = this.Property.Children["num"];
	}

	protected override void DrawPropertyLayout(GUIContent label)
	{
		int NumValue = SirenixEditorFields.IntField("번호", (int)num.ValueEntry.WeakSmartValue);
		num.ValueEntry.WeakSmartValue = NumValue;
		
		Rect rect = EditorGUILayout.GetControlRect(true, 100);
		rect = EditorGUI.PrefixLabel(rect, label);
		
		if (GUI.Button(rect.SplitGrid(300, 20, 0), "Save"))
		{
			var allEnemy = GameObject.FindGameObjectsWithTag(Define.ENEMY_TAG).Where(obj => obj.activeSelf);
			var allWalls = GameObject.FindGameObjectsWithTag(Define.WALL_TAG).Where(obj => obj.activeSelf);
			var player = GameObject.FindGameObjectWithTag(Define.PLAYER_TAG);
			var switchWalls = GameObject.FindGameObjectsWithTag(Define.SWITCH_TAG).Where(obj => obj.activeSelf);
			var items = GameObject.FindGameObjectsWithTag(Define.ITEM_TAG).Where(obj => obj.activeSelf);

			List<MapDataSwitchWall> switchWallList = new();
			foreach (var switchWall in switchWalls)
			{
				var wall = switchWall.GetComponent<WallSwitch>();
				switchWallList.Add(new MapDataSwitchWall()
				{
					isMove = wall.IsMove,
					v2WallPos = wall.TargetWall.transform.localPosition,
					wallRot = wall.TargetWall.transform.localRotation,
					v2WallScale = wall.TargetWall.transform.localScale,
					v2WallMovePos = wall.V2MovePos,
					v2Scale = wall.transform.localScale,
					v2Pos = wall.transform.position,
				});
			}

			List<ItemData> itemDataList = new();
			foreach (var item in items)
			{
				var itemBase = item.GetComponent<ItemBase>();
				itemDataList.Add(new ItemData()
				{
					value = itemBase.Value,
					v2Pos = itemBase.transform.position,
					MaxCount = itemBase.UseCountBase
				});
			}

			MapSaveData saveData = new MapSaveData()
			{
				enemyPosList = allEnemy.Select(obj => (Vector2)obj.transform.position).ToList(),
				enemyRotList = allEnemy.Select(obj => obj.transform.localRotation).ToList(),
				enemyIDList = allEnemy.Select(obj => obj.GetComponent<Enemy>().enemyModel.ID).ToList(),
				wallPosList = allWalls.Select(obj => (Vector2)obj.transform.position).ToList(),
				wallRotList = allWalls.Select(obj => obj.transform.localRotation).ToList(),
				wallScaleList = allWalls.Select(obj => obj.transform.localScale).ToList(),
				playerPos = player.transform.position,
				playerRot = player.transform.localRotation,
				mapSwitchWallList = switchWallList,
				itemDataList = itemDataList
			};

			SaveSystem.SetNum(NumValue);
			SaveSystem.Save(saveData);
		}
	}
}
#endif