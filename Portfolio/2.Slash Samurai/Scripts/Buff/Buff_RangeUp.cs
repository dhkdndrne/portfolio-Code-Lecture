using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_RangeUp : BuffBase
{

	public Buff_RangeUp(int period, float value) : base(period, value)
	{
	}
	public override void ApplyBuff(Player player)
	{
		player.Model.atkRange += value;
	}
	public override void RemoveBuff(Player player)
	{
		player.Model.atkRange -= value;
	}
}
