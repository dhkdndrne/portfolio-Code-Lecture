using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTower : TowerBase
{
    [SerializeField] int targetCount;
    public override Debuff GetTowerDebuff()
    {
        throw new System.NotImplementedException();
        //return new PoisonDebuff(TickDamage, TickTime, DebuffDuration, targetUnit, Debuff.DebuffType.RESET);
    }

    protected override void Fire()
    {
        base.Fire();
    }

    public override void Init()
    {
        SetTowerINfo("Poison");
        //tickDamage = DBManager.Instance.towerDB.towerDB["Poison"][level - 1].debuffRate;    
        base.Init();

    }

    protected override void UpdateTarget()
    {
        base.UpdateTarget();
    }
}
