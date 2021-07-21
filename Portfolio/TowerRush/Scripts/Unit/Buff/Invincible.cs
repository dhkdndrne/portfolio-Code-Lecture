using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincible : Buff
{
    public Invincible(float _Duration, UnitAbillity _Target) : base(_Duration, _Target)
    {        
        target.Invincible = true;
    }

    public override void Remove()
    {
        if (target != null)
        {
            target.Invincible = false;
            base.Remove();
        }
    }
}
