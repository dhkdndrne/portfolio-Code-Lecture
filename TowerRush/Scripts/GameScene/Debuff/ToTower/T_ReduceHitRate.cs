using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_ReduceHitRate : DebuffToTower
{
    public T_ReduceHitRate(float _ReduceFactor,float _Duration, TowerBase _Tower, DebuffType _Type = DebuffType.NULL) : base(_Duration, _Tower, _Type)
    {
        target_Tower.HitRate = _ReduceFactor;
        target_Tower.ChangeTowerState(TowerState.SMOKE);
    }

    public override void Remove()
    {
        base.Remove();
        target_Tower.ChangeTowerState(TowerState.SMOKE,true);
    }
}
