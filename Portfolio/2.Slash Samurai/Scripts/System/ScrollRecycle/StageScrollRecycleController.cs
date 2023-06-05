using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScrollRecycleController : ScrollRecycleController<StageCellData>
{
	protected override void Start()
	{
		base.Start();
		tableData = new List<StageCellData>();
	}

	public void SetStageTableData(StageCellData stageData)
	{
		tableData.Add(stageData);
	}
	
	public void Init() =>InitializeTableView();
}