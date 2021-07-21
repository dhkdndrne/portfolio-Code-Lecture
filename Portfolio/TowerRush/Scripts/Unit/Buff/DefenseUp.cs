using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseUp : Buff
{
    float defenseFactor;
    public DefenseUp(float _DefenseFactor,float _Duration, UnitAbillity _Target) : base(_Duration, _Target)
    {
        defenseFactor = _DefenseFactor;
        target.MaxDefense = target.Defense;
        target.Defense += defenseFactor;
    }

    public override void Remove()
    {
        if (target != null)
        {
            target.Defense = target.MaxDefense;
            base.Remove();
        }
    }
}
