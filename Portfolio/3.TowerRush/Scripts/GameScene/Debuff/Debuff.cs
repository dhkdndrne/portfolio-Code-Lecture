using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Debuff
{
    public enum DebuffType
    {
        NULL,
        OVERLAP,
        RESET
        
    }
    public DebuffType debuffType;
    protected UnitAbillity target_Unit;
    protected TowerBase target_Tower;
    
    //지속시간
    float elapsed;
    public float Elapsed
    {
        get { return elapsed; }
        set { elapsed = value; }
    }
    float duration;
    
    public int maxOverlap;      //최대 중첩수
    public int overlap;         //처음 중첩
    public bool applied;

    protected bool isInfinity;            //버프 종료x
    public Debuff(float _Duration,DebuffType _Type = DebuffType.NULL)
    {
        duration = _Duration;
        debuffType = _Type;
    }

    public virtual void Update()
    {
        if(!isInfinity)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= duration)
            {
                Remove();
            }
        }
    }

    public virtual void Remove()
    {
        if (target_Tower != null)
        {
            target_Tower.RemoveDebuff(this);
        }
        else if (target_Unit != null)
        {
            target_Unit.RemoveDebuff(this);
        }
    }
}

public class DebuffToTower : Debuff
{
    public DebuffToTower(float _Duration, TowerBase _Tower, DebuffType _Type = DebuffType.NULL) : base(_Duration, _Type)
    {
        target_Tower = _Tower;
    }
}
