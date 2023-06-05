using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageScrollCell : ScrollRecycleCell<StageCellData>
{
	[SerializeField] private Image image;
	[SerializeField] private Sprite[] sprites;
	[SerializeField] private TextMeshProUGUI tmpStage;

	private Button button;
	public override void UpdateContent(StageCellData itemData)
	{
		tmpStage.text = "STAGE " + itemData.stage;

		bool isLowLevel = itemData.stage > GameManager.Instance.GameModel.maxStage;
		image.sprite = isLowLevel ? sprites[0] : sprites[1];
		button.interactable = itemData.stage == 1 ? true : isLowLevel ? false : true;
	}

	private void Awake()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(SetStage);
	}
	private void SetStage()
	{
		GameManager.Instance.SetLevel(Index + 1);
		GameManager.Instance.GameStartSubject.OnNext(default);
	}
}

public class StageCellData
{
	public int stage;
}