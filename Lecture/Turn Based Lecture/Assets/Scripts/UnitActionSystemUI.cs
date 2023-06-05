using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
	[SerializeField] private Transform actionButtonPrefab;
	[SerializeField] private Transform actionButtonContainerTransform;

	private List<ActionButtonUI> actionButtonUIList;

	private void Awake()
	{
		actionButtonUIList = new List<ActionButtonUI>();
	}

	private void Start()
	{
		UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
		UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionhanged;
		CreateUnitActionButtons();
		UpdateSelectedVisual();
	}

	private void CreateUnitActionButtons()
	{
		foreach (Transform buttonTransform in actionButtonContainerTransform)
		{
			Destroy(buttonTransform.gameObject);
		}

		actionButtonUIList.Clear();

		var selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

		foreach (var baseAction in selectedUnit.GetBaseActionArray())
		{
			Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
			ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
			actionButtonUI.SetBaseAction(baseAction);
			
			actionButtonUIList.Add(actionButtonUI);
		}
	}

	private void UnitActionSystem_OnSelectedUnitChanged(object seneder, EventArgs e)
	{
		CreateUnitActionButtons();
		UpdateSelectedVisual();
	}
	private void UnitActionSystem_OnSelectedActionhanged(object seneder, EventArgs e)
	{
		UpdateSelectedVisual();
	}
	private void UpdateSelectedVisual()
	{
		foreach (var actionButtonUI in actionButtonUIList)
		{
			actionButtonUI.UpdateSelectedVisual();
		}
	}
}