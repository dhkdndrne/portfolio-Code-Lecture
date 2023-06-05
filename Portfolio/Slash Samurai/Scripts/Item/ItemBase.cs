using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public enum ItemType
{
	RangeUp = 0,
}
public class ItemBase : MonoBehaviour
{
	[field: SerializeField] public ItemType Type { get; private set; }
	[field:SerializeField] public int UseCountBase { get; private set; } 
	[field: SerializeField] public float Value { get; private set; }
	
	private Vector2 v2OriginPos;
	private bool isLoaded;
	
	private void Start()
	{
		GameManager.Instance.RefreshSubject.Subscribe(_ =>
		{
			if (isLoaded)
			{
				gameObject.SetActive(true);
				transform.position = v2OriginPos;
			}
		}).AddTo(gameObject);
	}

	public void Reset()
	{
		isLoaded = false;
	}
	
	public void Init(ItemData itemData)
	{
		Type = itemData.type;
		UseCountBase = itemData.MaxCount;
		Value = itemData.value;
		v2OriginPos = itemData.v2Pos;
		transform.position = v2OriginPos;
		isLoaded = true;
	}
	
	public virtual BuffBase Use() { return null; }

}