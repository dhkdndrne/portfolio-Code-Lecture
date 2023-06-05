using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GameClearPresenter : MonoBehaviour
{
	[BoxGroup("버튼")]
	[SerializeField] private Button nextBtn;

	[BoxGroup("버튼")]
	[SerializeField] private Button backBtn;

	[SerializeField] private GameObject panel;
	[SerializeField] private GameObject stageSelectPanel;

	private void Awake()
	{
		nextBtn.onClick.AddListener(() =>
		{
			GameManager.Instance.SetLevel(GameManager.Instance.GameModel.SelectLevel.Value + 1);
			GameManager.Instance.LoadMap().Forget();
			panel.SetActive(false);
		});

		backBtn.onClick.AddListener(() =>
		{
			stageSelectPanel.transform.parent.GetComponent<StageSelectPresenter>().Open();
			GameManager.Instance.GameModel.IsGameOver.Value = false;
			panel.SetActive(false);
		});

		GameManager.Instance.StageClearSubject.Subscribe(_ =>
		{
			if (GameManager.Instance.GameModel.maxStage == GameManager.Instance.GameModel.SelectLevel.Value)
			{
				GameManager.Instance.GameModel.maxStage++;
				
				#if UNITY_ANDROID
				DataManager.Instance.UpdateUserData("maxStage",GameManager.Instance.GameModel.maxStage);
				//GPGSBinder.Inst.SaveCloud(DataManager.Instance.STAGEKEY,GameManager.Instance.GameModel.maxLevel.ToString());
				#endif
			}
				
			panel.SetActive(true);

			if (GameManager.Instance.GameModel.SelectLevel.Value == DataManager.Instance.MapSaveDataDic.Count)
			{
				nextBtn.gameObject.SetActive(false);
			}
			else nextBtn.gameObject.SetActive(true);
			
		}).AddTo(gameObject);
	}
}