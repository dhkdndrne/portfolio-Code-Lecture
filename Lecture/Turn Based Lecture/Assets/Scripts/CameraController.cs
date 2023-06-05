using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
	private const float MIN_FOLLOW_Y_OFFSET = 2f;
	private const float MAX_FOLLOW_Y_OFFSET = 12f;

	private Vector3 targetFollowoffset;
	private CinemachineTransposer cinemachineTransposer;
	private void Start()
	{
		cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
		targetFollowoffset = cinemachineTransposer.m_FollowOffset;
	}
	private void Update()
	{
		HandleMovement();
		HandleRoation();
		HandleZoom();
	}

	private void HandleMovement()
	{
		Vector3 inputMoveDir = Vector3.zero;

		if (Input.GetKey(KeyCode.W))
		{
			inputMoveDir.z += 1f;
		}
		if (Input.GetKey(KeyCode.S))
		{
			inputMoveDir.z -= 1f;
		}
		if (Input.GetKey(KeyCode.A))
		{
			inputMoveDir.x -= 1f;
		}
		if (Input.GetKey(KeyCode.D))
		{
			inputMoveDir.x += 1f;
		}

		float moveSpeed = 10f;

		Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
		transform.position += moveVector * moveSpeed * Time.deltaTime;
	}

	private void HandleRoation()
	{
		Vector3 rotationVector = Vector3.zero;

		if (Input.GetKey(KeyCode.Q))
		{
			rotationVector.y += 1f;
		}

		if (Input.GetKey(KeyCode.E))
		{
			rotationVector.y -= 1f;
		}

		float rotationSpeed = 100f;
		transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
	}

	private void HandleZoom()
	{
		float zoomAmount = 1f;

		if (Input.mouseScrollDelta.y < 0)
		{
			targetFollowoffset.y -= zoomAmount;
		}
		if (Input.mouseScrollDelta.y > 0)
		{
			targetFollowoffset.y += zoomAmount;
		}

		targetFollowoffset.y = Mathf.Clamp(targetFollowoffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

		float zoomSpeed = 5f;
		cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset,targetFollowoffset,Time.deltaTime * zoomSpeed);
	}
}