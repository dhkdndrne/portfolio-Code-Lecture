using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Bam.Extensions;
using DarkTonic.MasterAudio;
using Random = UnityEngine.Random;
public class Enemy : MonoBehaviour
{
	#region Inspector

	[field: SerializeField] public EnemyModelSO enemyModel;

	[SerializeField] private SpriteRenderer head;
	[SerializeField] private SpriteRenderer body;
	[SerializeField] private SpriteRenderer sunGlass;
	[SerializeField] private ParticleSystem hitFX;
	[SerializeField] private ParticleSystem FX;
    #endregion

	#region Field

	private int hp;
	private Animator animator;
	private BoxCollider2D collider2D;
	private CancellationTokenSource cts;
	private bool isFxActivated;
	
	#endregion

	#region AnimationHash

	private static readonly int Anim_HeadCut_Hash = Animator.StringToHash("Head_Cut");
	private static readonly int Anim_GlassHeadCut_Hash = Animator.StringToHash("Glass_Head_Cut");
	private static readonly int Anim_TakeOffSunGlass_Hash = Animator.StringToHash("GlassOff");
	private static readonly int Anim_CheckGlass_Hash = Animator.StringToHash("HasSunGlass");
	private static readonly int Anim_CheckRedGlass_Hash = Animator.StringToHash("IsRedSunGlass");
	private static readonly int Anim_Default_Hash = Animator.StringToHash("Default_NoSunGlass");

    #endregion


	private void Awake()
	{
		animator = GetComponent<Animator>();
		collider2D = GetComponent<BoxCollider2D>();
	}

	private void Start()
	{
		GameManager.Instance.RefreshSubject.Where(_ => enemyModel != null).Subscribe(_ =>
		{
			gameObject.SetActive(true);
			Init(enemyModel);
		});

		GameManager.Instance.ReplaySubject.Where(_ => enemyModel != null).Subscribe(_ =>
		{
			gameObject.SetActive(true);
			Init(enemyModel);
		});
		
		GameManager.Instance.ReplaySystem.ReplayDeadSubject.Where(_ => enemyModel != null).Subscribe(_ =>
		{
			animator.SetBool(Anim_CheckRedGlass_Hash,enemyModel.Hp == 3);
			animator.SetTrigger(enemyModel.Hp > 1 ? Anim_GlassHeadCut_Hash : Anim_HeadCut_Hash);

			if (isFxActivated)
				FX.Play();
			
			isFxActivated = false;
			Dead().Forget();

		}).AddTo(gameObject);
	}

	public void Init(EnemyModelSO enemyModelSo)
	{
		enemyModel = enemyModelSo;
		hp = enemyModel.Hp;
		
		animator.ResetTrigger(Anim_HeadCut_Hash);
		animator.ResetTrigger(Anim_TakeOffSunGlass_Hash);
		
		// UniTask 취소 토큰 갱신
		cts?.Cancel();
		cts = new();
		
		//오브젝트 초기화
		head.gameObject.SetActive(true);
		head.transform.position = Vector3.zero;
	
		body.gameObject.SetActive(true);

		sunGlass.gameObject.SetActive(hp > 1);
		sunGlass.gameObject.transform.position = Vector3.zero;

		collider2D.enabled = true;
		
		animator.SetBool(Anim_CheckGlass_Hash, hp > 1);
		animator.SetBool(Anim_CheckRedGlass_Hash,hp == 3);
		animator.Play(Anim_Default_Hash);

		GameManager.Instance.GameModel.EnemyCnt.Value++;
	}

	public void Hit()
	{
		hp--;
		hitFX.Play();

		float random = Random.Range(0, 1f);

		if (random <= 0.5f)
		{
			FX.Play();
			isFxActivated = true;
		}
		
		switch (hp)
		{
			case 2:
				sunGlass.color = Color.white;
				animator.SetBool(Anim_CheckRedGlass_Hash,false);
				break;

			case 1:
				animator.SetBool(Anim_CheckGlass_Hash, false);
				animator.SetTrigger(Anim_TakeOffSunGlass_Hash);
				break;

			case 0:
				GameManager.Instance.GameModel.EnemyCnt.Value--;
				animator.SetTrigger(Anim_HeadCut_Hash);
				Dead().Forget();
				break;
		}
	}

	private async UniTaskVoid Dead()
	{
		collider2D.enabled = false;
		MasterAudio.PlaySoundAndForget("Head Drop");
		await animator.WaitAnimationCompleteAsync(cts.Token);
		gameObject.SetActive(false);
	}
}