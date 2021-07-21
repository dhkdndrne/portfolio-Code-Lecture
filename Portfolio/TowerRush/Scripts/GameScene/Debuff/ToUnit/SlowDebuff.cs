using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDebuff : Debuff
{
    float slowingFactor;
    
    public SlowDebuff(int _MaxOverLap,float _SlowingFactor, float _Duration, UnitAbillity _Target, DebuffType _Type) : base(_Duration,_Type)
    {
        target_Unit = _Target;
        slowingFactor = _SlowingFactor;
        maxOverlap = _MaxOverLap;
    }
    public override void Update()
    {
        if (target_Unit != null)
        {        
            if (!applied)
            {
                applied = true;
                if(target_Unit.VariableSpeed > 0)
                {
                    target_Unit.VariableSpeed -= (target_Unit.VariableSpeed * slowingFactor) * 0.01f;
                }
                    
            }
        }
        base.Update();
    }

    public override void Remove()
    {   
        if(!target_Unit.isBind) target_Unit.VariableSpeed = target_Unit.StaticSpeed;
        applied = false;
        overlap = 0;
        base.Remove();
    }
}
