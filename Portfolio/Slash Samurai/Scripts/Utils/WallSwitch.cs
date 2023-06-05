using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class WallSwitch : MonoBehaviour
{
	[field: SerializeField] public GameObject TargetWall{ get; private set; }
	[field: SerializeField] public bool IsMove { get; private set; }
	[field: SerializeField] public Vector2 V2MovePos { get; private set; }
	
	private Vector2 v2OriginPos;
	private bool isLoaded;
	private void Start()
	{
		v2OriginPos = TargetWall.transform.localPosition;
		GameManager.Instance.RefreshSubject.Subscribe(_ =>
		{
			if (isLoaded)
			{
				GetComponent<SpriteRenderer>().enabled = true;
				GetComponent<BoxCollider2D>().enabled = true;
			
				TargetWall.transform.DOKill();
				TargetWall.transform.localPosition = v2OriginPos;
				TargetWall.SetActive(true);
				gameObject.SetActive(true);
			}
		});

		GameManager.Instance.ReplaySubject.Subscribe(_ =>
		{
			if (isLoaded)
			{
				GetComponent<SpriteRenderer>().enabled = true;
				GetComponent<BoxCollider2D>().enabled = true;
			
				TargetWall.transform.DOKill();
				TargetWall.transform.localPosition = v2OriginPos;
				TargetWall.SetActive(true);
				gameObject.SetActive(true);
			}
	
		}).AddTo(gameObject);
	}

	public void Init(MapDataSwitchWall loadData)
	{
		v2OriginPos = loadData.v2WallPos;
		
		transform.position = loadData.v2Pos;
		transform.localScale = loadData.v2Scale;
		
		TargetWall.transform.localPosition = v2OriginPos;
		TargetWall.transform.localRotation = loadData.wallRot;
		TargetWall.transform.localScale = loadData.v2WallScale;
		
		IsMove = loadData.isMove;
		V2MovePos = loadData.v2WallMovePos;
		isLoaded = true;
	}
	
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag(Define.PLAYER_TAG))
		{
			if (IsMove)
			{
				GetComponent<SpriteRenderer>().enabled = false;
				GetComponent<BoxCollider2D>().enabled = false;
				
				GameManager.Instance.GameModel.isPlayingGimmick = true;
				TargetWall.transform.DOLocalMove(V2MovePos, 5).SetSpeedBased().OnComplete(() =>
				{
					GameManager.Instance.GameModel.isPlayingGimmick = false;
					
				});
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}
	
	public void Reset()
	{
		isLoaded = false;
	}
}