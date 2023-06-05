using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDebuff : Debuff
{
    float tickTime;
    float timeSinceTick;
    float tickDamage;

    public PoisonDebuff(float _TickDamage, float _TickTime, float _Duration, UnitAbillity _Target, DebuffType _Type) : base(_Duration, _Type)
    {
        target_Unit = _Target;
        tickDamage = _TickDamage;
        tickTime = _TickTime;
    }
    public override void Update()
    {
        if (target_Unit != null)
        {
            
            timeSinceTick += Time.deltaTime;
            if (timeSinceTick >= tickTime)
            {
                timeSinceTick = 0;
                target_Unit.Hit(tickDamage, Element.POISON,false,true);
            }            
        }
        base.Update();
    }
}
