using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
	[SerializeField] private Animator unitAnimator;
	[SerializeField] private int maxModeDistance = 4;

	private Vector3 targetPosition;

	private readonly int isWalkingHash = Animator.StringToHash("IsWalking");
	protected override void Awake()
	{
		base.Awake();
		targetPosition = transform.position;
	}
	private void Update()
	{
		if (!isActive)
			return;
		
		Vector3 moveDirection = (targetPosition - transform.position).normalized;

		float stoppingDistance = .1f;
		if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
		{
			float moveSpeed = 4f;
			transform.position += moveDirection * Time.deltaTime * moveSpeed;

			unitAnimator.SetBool(isWalkingHash, true);
		}
		else
		{
			unitAnimator.SetBool(isWalkingHash, false);
			isActive = false;
			onActionCompete();
		}

		float rotateSpeed = 10f;
		transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
	}

	public override string GetActionName() => "Move";

	public override void TakeAction(GridPosition gridPosition,Action onActionCompete)
	{
		this.onActionCompete = onActionCompete;
		this.targetPosition = LevelGrid.Instance.GetWorldGridPosition(gridPosition);
		isActive = true;
	}
	
	public override List<GridPosition> GetValidActionGridPositionList()
	{
		List<GridPosition> validGridPositionList = new List<GridPosition>();
		GridPosition unitGridPosition = unit.GetGridPosition();

		for (int x = -maxModeDistance; x <= maxModeDistance; x++)
		{
			for (int z = -maxModeDistance; z <= maxModeDistance; z++)
			{
				var offsetGridPosition = new GridPosition(x, z);
				var testGridPosition = unitGridPosition + offsetGridPosition;

				if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
				{
					continue;
				}

				//유닛이 이미 타일에 위치했을때
				if (unitGridPosition == testGridPosition)
				{
					continue;
				}

				//타일에 다른 유닛이 존재하는지 체크
				if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
				{
					continue;
				}

				validGridPositionList.Add(testGridPosition);
			}
		}

		return validGridPositionList;
	}
}