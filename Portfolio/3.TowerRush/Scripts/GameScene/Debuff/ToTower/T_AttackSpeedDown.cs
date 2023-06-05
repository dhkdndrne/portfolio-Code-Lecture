using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_AttackSpeedDown : DebuffToTower
{
    float slowFactor;
    float maxSpeedFactor;
    bool isSave;

    public T_AttackSpeedDown(float _SlowFactor,float _Duration, TowerBase _Tower, DebuffType _Type = DebuffType.NULL) : base(_Duration, _Tower, _Type)
    {
        slowFactor = _SlowFactor;
        target_Tower.ChangeTowerState(TowerState.ICEHORN);
    }

    public override void Update()
    {
        if (target_Tower != null)
        {
            if (!isSave)
            {
                maxSpeedFactor = target_Tower.fireDelay;
                isSave = true;
            }
            if (!applied)
            {
                applied = true;
                target_Tower.fireDelay *= 1 + slowFactor * 0.01f;
            }
        }
        base.Update();
    }

    public override void Remove()
    {
        target_Tower.fireDelay = maxSpeedFactor;
        target_Tower.ChangeTowerState(TowerState.ICEHORN,true);
        isSave = false;
        applied = false;
        overlap = 0;
        base.Remove();
    }
}
