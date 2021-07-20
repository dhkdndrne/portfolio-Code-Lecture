using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_DamageDown : DebuffToTower
{
    int towerDamage;
    public T_DamageDown(int _DamageFactor, float _Duration, TowerBase _Tower, DebuffType _Type = DebuffType.NULL) : base(_Duration, _Tower, _Type)
    {
        towerDamage = target_Tower.damage;
        target_Tower.ChangeTowerState(TowerState.CYCLONE);
        target_Tower.damage = (int)(target_Tower.damage * (1 - _DamageFactor * 0.01f));
    }

    public override void Remove()
    {
        base.Remove();
        target_Tower.ChangeTowerState(TowerState.CYCLONE,true);
        target_Tower.damage = towerDamage;
    }
}
