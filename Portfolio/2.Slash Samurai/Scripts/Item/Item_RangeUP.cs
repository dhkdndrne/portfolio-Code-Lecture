using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_RangeUP : ItemBase
{
	public override BuffBase Use()
	{
		gameObject.SetActive(false);
		return new Buff_RangeUp(UseCountBase,Value);
	}
}
