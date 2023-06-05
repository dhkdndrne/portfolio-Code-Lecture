using System;
using System.Collections;
using System.Collections.Generic;
using Bam.Singleton;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	[SerializeField] private TextMeshProUGUI stageText;
	[field: SerializeField] public Player player { get; private set; }

	public GameModel GameModel = new();

	#region System

	public MapLoadSystem MapLoadSystem { get; private set; }
	public CameraShakeSystem CameraShakeSystem { get; private set; }
	public ReplaySystem ReplaySystem { get; private set; }

	[SerializeField] private TipPresenter tipPresenter; 
    #endregion

	#region Subject

	public Subject<Unit> ReplaySubject { get; private set; } = new();    // 리플레이
	public Subject<Unit> RefreshSubject { get; private set; } = new();   // 맵 재설정
	public Subject<Unit> GameStartSubject { get; private set; } = new(); // 게임 시작할때 이벤트
	public Subject<Unit> StageClearSubject { get; private set; } = new();

    #endregion

	private void Start()
	{
		ReplaySystem = FindObjectOfType<ReplaySystem>();
		MapLoadSystem = FindObjectOfType<MapLoadSystem>();
		CameraShakeSystem = Camera.main.GetComponent<CameraShakeSystem>();

		GameModel.EnemyCnt.Subscribe(cnt =>
		{
			if (!GameModel.isRefresh && cnt == 0)
				ReplaySystem.Replay().Forget();
		});
		
		GameModel.SelectLevel.Subscribe(value =>
		{
			stageText.text = "Stage " + value;
			tipPresenter.PlayTip(value);
		}).AddTo(gameObject);
	}

	public async UniTaskVoid LoadMap()
	{
		GameModel.isRefresh = true;
		GameModel.EnemyCnt.Value = 0;
		await MapLoadSystem.LoadMapData();
		GameModel.isRefresh = false;
		GameModel.IsGameOver.Value = false;
	}

	// 다시하기 
	public async UniTaskVoid Reset()
	{
		// 플레이어 위치 재설정
		// 적 재설정
		GameModel.isRefresh = true;
		GameModel.IsGameOver.Value = false;
		GameModel.isPlayingGimmick = false;
		GameModel.EnemyCnt.Value = 0;

		RefreshSubject.OnNext(default);

		await UniTask.DelayFrame(1);
		GameModel.isRefresh = false;
	}

	public bool CanPlayable()
	{
		return !GameModel.isRefresh && !GameModel.IsGameOver.Value && !GameModel.isReplay && !GameModel.isPanelOpen && !GameModel.isPlayingGimmick;
	}

	public void SetLevel(int level = 1)
	{
		GameModel.SelectLevel.Value = level == GameModel.maxStage ? GameModel.maxStage : level;
	}
}