using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Bam.Extensions;
using DarkTonic.MasterAudio;
using UniRx;

public class Player : MonoBehaviour
{
	[field: SerializeField] public PlayerModel Model { get; private set; } = new();
	public PlayerPresenter PlayerPresenter { get; private set; }

	#region Field

	private Vector2 v2TargetPos;
	private CancellationTokenSource cts;
	private CircleCollider2D collider2D;

	private bool isAtkSuccess;
	private bool isMove;

	private List<BuffBase> buffList = new();
	private List<BuffBase> addBuffList = new();
	private List<BuffBase> buffRemoveList = new();

    #endregion

	private void Awake()
	{
		PlayerPresenter = GetComponent<PlayerPresenter>();
		collider2D = GetComponent<CircleCollider2D>();
	}

	private void Start()
	{
		PlayerInput playerInput = GetComponent<PlayerInput>();

		playerInput.MouseUpStream.Subscribe(_ =>
		{
			MoveAsync().Forget();
		}).AddTo(gameObject);

		GameManager.Instance.RefreshSubject.Subscribe(_ =>
		{
			var mapSaveData = DataManager.Instance.MapSaveDataDic[GameManager.Instance.GameModel.SelectLevel.Value];
			Init(mapSaveData.playerPos, mapSaveData.playerRot);
		}).AddTo(gameObject);
		
		GameManager.Instance.ReplaySubject.Subscribe(_ =>
		{
			var mapSaveData = DataManager.Instance.MapSaveDataDic[GameManager.Instance.GameModel.SelectLevel.Value];
			Init(mapSaveData.playerPos, mapSaveData.playerRot);
		}).AddTo(gameObject);
		
		GameManager.Instance.StageClearSubject.Subscribe(_ =>
		{
			foreach (var buff in buffList)
				buff.RemoveBuff(this);
			
			buffList.Clear();
			
		}).AddTo(gameObject);
		
		
		gameObject.SetActive(false);
	}

	public void Init(Vector3 v3StartPos, Quaternion rot)
	{
		cts = new CancellationTokenSource();
		
		foreach (var buff in buffList)
			buff.RemoveBuff(this);

		addBuffList.Clear();
		buffList.Clear();
		
		transform.SetPositionAndRotation(v3StartPos, rot);
		collider2D.enabled = false;
		Model.isInAction = false;
		isMove = false;
		
	}

	private async UniTaskVoid MoveAsync()
	{
		v2TargetPos = transform.position + Model.v2Dir * Model.atkRange;         // 목표 위치
		transform.rotation = Quaternion.FromToRotation(Vector3.up, Model.v2Dir); // 이동 방향으로 회전
		collider2D.enabled = true;

		Model.isInAction = true;
		isAtkSuccess = false;
		isMove = true;
		MasterAudio.PlaySoundAndForget("Cutting Sword");

		while (isMove)
		{
			transform.position =
				Vector2.MoveTowards(transform.position, v2TargetPos, Model.speed * Time.deltaTime);

			Vector2 v2ViewPortPos = Camera.main.WorldToViewportPoint(transform.position);

			if (!CheckOutOfCameraRange(v2ViewPortPos))
			{
				transform.position += -Model.v2Dir * (collider2D.radius * 2);
				break;
			}


			if (Vector2.SqrMagnitude(v2TargetPos - (Vector2)transform.position) < Extensions.Pow(0.1f, 2))
			{
				transform.position = v2TargetPos;
				break;
			}

			await UniTask.Yield(cancellationToken: cts.Token);
		}

		isMove = false;
		MasterAudio.PlaySoundAndForget("Putting in Sword");

		// 끝까지 이동 후 적을 쳤는데 게임오버 되는 경우가 있어서 10프레임만 대기
		await UniTask.DelayFrame(10);

		if (!isAtkSuccess)
		{
			GameOver();
			return;
		}

		UpdateBuff();

		GameManager.Instance.ReplaySystem.queuePosRecord.Enqueue(v2TargetPos);
		Model.isInAction = false;
	}

	private bool CheckOutOfCameraRange(Vector2 v2Pos)
	{
		bool check = true;

		if (v2Pos.x < 0)
		{
			v2Pos.x = 0;
			check = false;
		}
		if (v2Pos.x > 1)
		{
			v2Pos.x = 1;
			check = false;
		}
		if (v2Pos.y < 0)
		{
			v2Pos.y = 0;
			check = false;
		}
		if (v2Pos.y > 1)
		{
			v2Pos.y = 1;
			check = false;
		}

		return check;
	}

	private void OnTriggerEnter2D(Collider2D col)
	{

		if (col.CompareTag(Define.ENEMY_TAG))
		{
			PlayerPresenter.ChangeAttackMotion().Forget();
			col.GetComponent<Enemy>().Hit();
			GameManager.Instance.CameraShakeSystem.Shake().Forget();
			isAtkSuccess = true;
		}
		else if (col.CompareTag(Define.WALL_TAG) || col.CompareTag(Define.SWITCHWALL_TAG))
		{
			//벽에 부딪치면 이동 중지하고 플레이어 위치를 왔던 방향의 반대방향으로 콜라이더의 크기만큼 이동
			isMove = false;
			transform.position += -Model.v2Dir * (collider2D.radius * 2);
		}
		else if (col.CompareTag(Define.SWITCH_TAG))
		{
			isAtkSuccess = true;
		}
		else if (col.CompareTag(Define.ITEM_TAG))
		{
			AddBuff(col.GetComponent<ItemBase>().Use());
		}
	}

	private void AddBuff(BuffBase buff)
	{
		addBuffList.Add(buff);
	}
	
	/// <summary>
	/// 아이템 횟수 업데이트 및 횟수 소진 아이템 삭제, 아이템 추가
	/// </summary>
	private void UpdateBuff()
	{
		//버프 횟수 업데이트
		foreach (var buff in buffList)
		{
			buff.period--;

			//사용 다 끝났으면 삭제 리스트에 추가
			if (buff.period == 0)
				buffRemoveList.Add(buff);
		}

		//삭제리스트에 있는 버프 정리
		foreach (var removeBuff in buffRemoveList)
		{
			removeBuff.RemoveBuff(this);
			buffList.Remove(removeBuff);
		}

		//새로운 버프 추가
		foreach (var buff in addBuffList)
		{
			buff.ApplyBuff(this);
			buffList.Add(buff);
		}

		addBuffList.Clear();
		buffRemoveList.Clear();
	}

	private void GameOver()
	{
		cts?.Cancel();
		GameManager.Instance.GameModel.IsGameOver.Value = true;
	}
}