using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_RandomAttackRate : DebuffToTower
{
    public T_RandomAttackRate(float _Duration,float _RandomFactor ,TowerBase _Tower, DebuffType _Type = DebuffType.NULL) : base(_Duration, _Tower, _Type)
    {
        target_Tower.RandomFireFactor = _RandomFactor;
        target_Tower.ChangeTowerState(TowerState.FEATHER);
    }

    public override void Remove()
    {
        base.Remove();
        target_Tower.ChangeTowerState(TowerState.FEATHER,true);
    }
}
