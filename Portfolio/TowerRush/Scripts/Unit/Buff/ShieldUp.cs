using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUp : Buff
{
    int shieldFactor;
    public ShieldUp(int _ShieldFactor,float _Duration, UnitAbillity _Target) : base(_Duration, _Target)
    {
        shieldFactor = _ShieldFactor;
        target.GetShield(shieldFactor);
    }

    //public override void Remove()
    //{
    //    if (target != null)
    //    {
    //        target.Shield -= shieldFactor;
    //        if (target.Shield <= 0) target.Shield = 0;
    //        base.Remove();
    //    }
    //}
}
