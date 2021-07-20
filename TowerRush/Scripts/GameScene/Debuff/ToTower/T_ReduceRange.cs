using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_ReduceRange : DebuffToTower
{
    float towerRadious;
    public T_ReduceRange(float _Radious,float _Duration, TowerBase _Tower, DebuffType _Type = DebuffType.NULL) : base(_Duration, _Tower, _Type)
    {
        towerRadious = target_Tower.range;
        target_Tower.range *= 1 - _Radious * 0.01f;
        target_Tower.ChangeTowerState(TowerState.BOOM);
    }

    public override void Remove()
    {
        base.Remove();
        target_Tower.range = towerRadious;
        target_Tower.ChangeTowerState(TowerState.BOOM,true);
    }
}
