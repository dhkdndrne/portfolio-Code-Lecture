using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
	public static LevelGrid Instance { get; private set; }

	[SerializeField] private Transform gridDebugObjectPrefab;
	private GridSystem gridSystem;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;

		gridSystem = new GridSystem(10, 10, 2);
		gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
	}

	public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
	{
		gridSystem.GetGridObject(gridPosition).AddUnit(unit);
	}

	public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition).GetUnitList();

	public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
	{
		gridSystem.GetGridObject(gridPosition).RemoveUnit(unit);
	}

	public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
	{
		RemoveUnitAtGridPosition(fromGridPosition, unit);
		AddUnitAtGridPosition(toGridPosition, unit);
	}

	public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
	public Vector3 GetWorldGridPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);
	
	public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

	public int GetWidth() => gridSystem.GetWidth();
	public int GetHeight() => gridSystem.GetHeight();
	public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
	{
		GridObject gridObject = gridSystem.GetGridObject(gridPosition);
		return gridObject.HasAnyUnit();
	}
}