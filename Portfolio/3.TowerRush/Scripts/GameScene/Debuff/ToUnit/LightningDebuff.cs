using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningDebuff : Debuff
{
    bool isSave;
    float redeuceFactor;
    public LightningDebuff(UnitAbillity _Target, float _Duration, float _ReduceFactor,int _MaxOverLap ,DebuffType _Type) : base(_Duration, _Type)
    {
        target_Unit= _Target;
        maxOverlap = _MaxOverLap;
        redeuceFactor = _ReduceFactor;
    }

    public override void Update()
    {
        if (target_Unit != null)
        {        
            if (!isSave)
            {
                target_Unit.MaxEvade = target_Unit.Evade;
                isSave = true;
            }
            if (!applied)
            {
                target_Unit.Evade -= redeuceFactor;
                applied = true;
            }
        }

        if (target_Unit.Evade <= 0) target_Unit.Evade = 0;
        base.Update();
    }

    public override void Remove()
    {
        if(target_Unit !=null)
        {
            target_Unit.Evade = target_Unit.MaxEvade;
            isSave = false;
            applied = false;
            overlap = 0;
            base.Remove();
        }       
    }
}
