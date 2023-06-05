using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShakeSystem : MonoBehaviour
{
	[SerializeField] private float shakeTime;
	[SerializeField] private float shakeSpeed;
	[SerializeField] private float shakePower;
	private Vector3 v3OriginPos;
	private void Start()
	{
		v3OriginPos = transform.localPosition;
	}

	public async UniTask Shake()
	{
		float elapsedTime = 0;

		while (elapsedTime < shakeTime)
		{
			Vector3 v2RandomPos = v3OriginPos + Random.insideUnitSphere * shakePower;
			transform.localPosition = Vector3.Lerp(transform.localPosition, v2RandomPos, shakeSpeed * Time.deltaTime);

			await UniTask.Yield();

			elapsedTime += Time.deltaTime;
		}

		transform.localPosition = v3OriginPos;
	}
}