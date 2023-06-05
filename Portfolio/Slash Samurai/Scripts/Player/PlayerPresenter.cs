using System;
using System.Collections;
using System.Collections.Generic;
using Bam.Extensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PlayerPresenter : MonoBehaviour
{
	#region Insepector

	[SerializeField] private SpriteRenderer playerSpr;
	[SerializeField] private GameObject objRange;
	[SerializeField] private GameObject objArrow;
	[SerializeField] private Sprite[] arrPlayerSprite;

    #endregion

	private float arrowRotationZ;
	private PlayerModel playerModel;

	private readonly float CIRCLE_RATIO_VALUE = 2.23f;
	//private readonly float ARROW_RATIO_VALUE = 8.1f;
	private readonly float ARROW_RATIO_VALUE =4.9f;
	
	private void Start()
	{
		playerModel = GetComponent<Player>().Model;
		var playerInput = GetComponent<PlayerInput>();

		// 마우스 클릭 뗄때까지 범위, 화살표 오브젝트 켜주기
		playerInput.MouseDownStream.Subscribe(isActivate =>
		{
			objRange.SetActive(isActivate);
			objArrow.SetActive(isActivate);

			if (isActivate)
			{
				objRange.transform.DOScale(playerModel.atkRange * CIRCLE_RATIO_VALUE, 0.1f).SetEase(Ease.Linear);
				
				objArrow.transform.DOScaleX(1f, 0.1f).SetEase(Ease.Linear);
				objArrow.transform.DOScaleY(playerModel.atkRange / ARROW_RATIO_VALUE, 0.1f).SetEase(Ease.Linear);
			}
			else
			{
				objRange.transform.localScale = Vector3.zero;
				objArrow.transform.localScale = Vector3.zero;
			}
		});

		objArrow.UpdateAsObservable().Where(_ => objArrow.activeSelf).Subscribe(_ =>
		{
			var mViewportPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			var mPos = new Vector2(mViewportPos.x - playerModel.v2FirstTouch.x, mViewportPos.y - playerModel.v2FirstTouch.y);

			ClockwisePolarCoord mousePC = ClockwisePolarCoord.FromVector2(mPos);
			arrowRotationZ = -mousePC.Angle;

			objArrow.transform.eulerAngles = Vector3.forward * arrowRotationZ;
		}).AddTo(gameObject);
	}

	public async UniTaskVoid ChangeAttackMotion()
	{
		playerSpr.sprite = arrPlayerSprite[1];

		await UniTask.Delay(500);

		playerSpr.sprite = arrPlayerSprite[0];
	}

}