using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;

public class PlayerInput : MonoBehaviour
{
	private PlayerModel playerModel;
	private Vector2 v2StartPos;
	private bool isMouseDown;
	private int pointerID;

	public Subject<bool> MouseDownStream { get; private set; } = new();
	public Subject<bool> MouseUpStream { get; private set; } = new();


	private void Start()
	{
		playerModel = GetComponent<Player>().Model;

		#if UNITY_EDITOR
		pointerID = -1;
		#elif UNITY_ANDROID
		pointerID = 0;
		#endif

		GameManager.Instance.RefreshSubject.Subscribe(_ => isMouseDown = false).AddTo(gameObject);
		
		var mouseClickDownStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0));
		var mouseClickUpStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonUp(0));

		mouseClickDownStream.Where(_ => !EventSystem.current.IsPointerOverGameObject(pointerID)
		                                && !playerModel.isInAction
		                                && GameManager.Instance.CanPlayable())
			.ThrottleFirst(TimeSpan.FromSeconds(.5f)).Subscribe(_ =>
			{
				v2StartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				playerModel.v2FirstTouch = v2StartPos;
				isMouseDown = true;
				MouseDownStream.OnNext(true);

			}).AddTo(gameObject);

		mouseClickUpStream.Where(_ => isMouseDown).Subscribe(_ =>
		{
			MouseDownStream.OnNext(false);
			Vector2 v2EndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (Vector2.Distance(v2StartPos, v2EndPos) < playerModel.minDragLength) return;

			playerModel.v2Dir = (v2EndPos - v2StartPos).normalized;
			isMouseDown = false;
			MouseUpStream.OnNext(default);

		}).AddTo(gameObject);

	}
}