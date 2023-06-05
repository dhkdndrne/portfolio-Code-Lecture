using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffBase
{
	public int period;
	public float value;

	public BuffBase(int period,float value)
	{
		this.value = value;
		this.period = period;
	}

	public abstract void ApplyBuff(Player player);
	public abstract void RemoveBuff(Player player);
}
