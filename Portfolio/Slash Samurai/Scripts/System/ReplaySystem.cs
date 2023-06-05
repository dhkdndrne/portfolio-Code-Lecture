using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Bam.Extensions;
using Bam.Singleton;
using DarkTonic.MasterAudio;
using UniRx;
public class ReplaySystem : MonoBehaviour
{
	public Queue<Vector2> queuePosRecord { get; private set; } = new();
	public Subject<Unit> ReplayDeadSubject { get; private set; } = new();
	private Player player;

	private void Start()
	{
		player = GameManager.Instance.player;
		GameManager.Instance.GameStartSubject.Subscribe(_ =>
		{
			queuePosRecord.Clear();
		}).AddTo(gameObject);
		
		GameManager.Instance.RefreshSubject.Where(_ => GameManager.Instance.GameModel.isRefresh).Subscribe(_ =>
		{
			queuePosRecord.Clear();
		}).AddTo(gameObject);
	}

	//리플레이 연출
	public async UniTaskVoid Replay()
	{
		GameManager.Instance.GameModel.isReplay = true;
		
		await UniTask.WaitUntil(() => !player.Model.isInAction);
		await UniTask.Delay(500);

		GameManager.Instance.ReplaySubject.OnNext(default);

		await UniTask.Delay(1000);

		var v2TargetPos = queuePosRecord.Dequeue();

		float speed = queuePosRecord.Count >= 6 ? 200 : queuePosRecord.Count > 12 ? 300 : 100;
		
		//벽력일섬
		while (true)
		{
			player.transform.position = Vector2.MoveTowards(player.transform.position, v2TargetPos, speed * Time.deltaTime);

			if (Vector2.SqrMagnitude((Vector2)player.transform.position - v2TargetPos) < Extensions.Pow(0.1f, 2))
			{
				player.transform.position = v2TargetPos;

				if (queuePosRecord.Count == 0)
					break;

				v2TargetPos = queuePosRecord.Dequeue();
				player.transform.rotation = Quaternion.FromToRotation(Vector3.up, (v2TargetPos - (Vector2)player.transform.position).normalized);
			}

			await UniTask.Yield(cancellationToken: this.GetCancellationTokenOnDestroy());
		}

		player.PlayerPresenter.ChangeAttackMotion().Forget();

		MasterAudio.PlaySoundAndForget("Putting in Sword");

		GameManager.Instance.CameraShakeSystem.Shake().Forget();
	
		ReplayDeadSubject.OnNext(default);

		await UniTask.Delay(1000);
		GameManager.Instance.StageClearSubject.OnNext(default);

		if (!DataManager.Instance.UserData.IsBuyAD)
		{
			ADManager.Instance.ShowFrontAd();
		}
		GameManager.Instance.GameModel.isReplay = false;
	}
}