using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTickBuff : Buff
{
    float tickTime;
    int healFactor;
    float timeSinceTick;

    public HealTickBuff(float _TickTime, int _HealFactor, float _Duration, UnitAbillity _Target) : base(_Duration, _Target)
    {
        tickTime = _TickTime;
        healFactor = _HealFactor;
    }

    public override void Update()
    {
        if (target != null)
        {
            timeSinceTick += Time.deltaTime;

            if (timeSinceTick >= tickTime)
            {
                timeSinceTick = 0;

                int hp = Mathf.RoundToInt(target.MaxHp * healFactor * 0.01f);
                target.GetHp(hp);

                PoolingManager.Instance.damagePopUpManager.ShowDamagePopUp(target.transform, hp, PopUpType.HEAL);
            }
        }
        base.Update();
    }
}
