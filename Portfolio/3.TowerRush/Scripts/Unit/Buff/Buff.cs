using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff
{

    protected UnitAbillity target;
    //지속시간
    float elapsed;
    float duration;
    bool isDead;
    public float Elapsed
    {
        get { return elapsed; }
        set { elapsed = value; }
    }

    public Buff(float _Duration,UnitAbillity _Target)
    {
        duration = _Duration;
        target = _Target;
    }
    public virtual void Update()
    {
        if (target.transform.gameObject.activeSelf == false) isDead = true;
        elapsed += Time.deltaTime;

        if (elapsed >= duration + 1 || isDead)
        {
            Remove();
        }
    }

    public virtual void Remove()
    {
        if (target != null)
        {
            target.RemoveBuff(this);
            isDead = false;
        }
    }
}
