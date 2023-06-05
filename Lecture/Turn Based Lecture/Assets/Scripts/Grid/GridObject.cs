using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
	private GridPosition gridPosition;
	private GridSystem gridSystem;
	private List<Unit> unitList = new();

	public GridObject(GridSystem gridSystem, GridPosition gridPosition)
	{
		this.gridSystem = gridSystem;
		this.gridPosition = gridPosition;
	}

	public override string ToString()
	{
		string unitString = string.Empty;
		foreach (var unit in unitList)
		{
			unitString += unit + "\n";
		}
		return gridPosition.ToString() + "\n" + unitString;
	}

	public void AddUnit(Unit unit)
	{
		unitList.Add(unit);
	}

	public void RemoveUnit(Unit unit)
	{
		unitList.Remove(unit);
	}
	public List<Unit> GetUnitList()
	{
		return unitList;
	}

	public bool HasAnyUnit() => unitList.Count > 0;
}