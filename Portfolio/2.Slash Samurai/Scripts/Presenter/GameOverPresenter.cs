using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GameOverPresenter : MonoBehaviour
{
	[BoxGroup("버튼")]
	[SerializeField] private Button restartBtn;
	
	[BoxGroup("버튼")]
	[SerializeField] private Button backBtn;

	[SerializeField] private GameObject panel;
	[SerializeField] private GameObject stageSelectPanel;
	
	private void Awake()
	{
		GameManager.Instance.GameModel.IsGameOver.Subscribe(isGameOver =>
		{
			panel.SetActive(isGameOver);
		}).AddTo(gameObject);
		
		restartBtn.onClick.AddListener(() =>
		{
			GameManager.Instance.Reset().Forget();
			
			// if(!DataManager.Instance.UserData.IsBuyAD)
			// 	ADManager.Instance.ShowFrontAd();
			
			panel.SetActive(false);
		});
		
		backBtn.onClick.AddListener(() =>
		{
			stageSelectPanel.SetActive(true);
			GameManager.Instance.GameModel.IsGameOver.Value = false;
			panel.SetActive(false);
		});
	}
}
