using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindDebuff : Debuff
{
    public BindDebuff(UnitAbillity _Target, float _Duration = 0, DebuffType _Type = DebuffType.NULL) : base(_Duration, _Type)
    {
        target_Unit = _Target;
        isInfinity = true;
        target_Unit.isBind = true;
        target_Unit.VariableSpeed = 0;
    }

    public override void Update()
    {
        if (target_Unit != null)
        {
            
            if (!target_Unit.isBind) Remove();
        }
        base.Update();
    }

    public override void Remove()
    {
        target_Unit.VariableSpeed = target_Unit.StaticSpeed;
        applied = false;
        overlap = 0;
        base.Remove();
    }
}
