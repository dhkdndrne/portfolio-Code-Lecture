using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{

	private float totalSpinAmount;

	private void Update()
	{

		if (!isActive)
			return;

		float spinAddAmount = 360f * Time.deltaTime;
		transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
		totalSpinAmount += spinAddAmount;

		if (totalSpinAmount >= 360f)
		{
			isActive = false;
			onActionCompete();
		}
	}
	public override void TakeAction(GridPosition gridPosition,Action onSpinCompete)
	{
		this.onActionCompete = onSpinCompete;
		isActive = true;
		totalSpinAmount = 0;
	}
	public override List<GridPosition> GetValidActionGridPositionList()
	{
		List<GridPosition> validGridPositionList = new List<GridPosition>();
		GridPosition unitGridPosition = unit.GetGridPosition();

		return new List<GridPosition>
		{
			unitGridPosition
		};
	}
	public override string GetActionName() => "Spin";

}