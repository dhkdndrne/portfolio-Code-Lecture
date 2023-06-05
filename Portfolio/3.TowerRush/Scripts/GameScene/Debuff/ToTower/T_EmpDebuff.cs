using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_EmpDebuff : DebuffToTower
{
    public T_EmpDebuff(int _HpFactor,float _Duration, TowerBase _Tower, DebuffType _Type = DebuffType.NULL) : base(_Duration, _Tower, _Type)
    {
        target_Tower.EmpHpFactor = _HpFactor;
        target_Tower.ChangeTowerState(TowerState.EMP);
    }

    public override void Remove()
    {
        if (target_Tower != null)
        {
            base.Remove();
            target_Tower.ChangeTowerState(TowerState.EMP,true);
            target_Tower.EmpHpFactor = 0;           
        }
    }
}
