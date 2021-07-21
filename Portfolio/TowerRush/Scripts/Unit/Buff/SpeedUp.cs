using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : Buff
{
    float reduceFactor;     // 감소 퍼센트
    float speedDuration;    // 이동속도 유지 시간

    bool isMax;
    float timeSinceTick;

    public SpeedUp(float _SpeedFactor, float _ReduceFactor, float _Duration, UnitAbillity _Target) : base(_Duration, _Target)
    {
        reduceFactor = _ReduceFactor;
        speedDuration = _Duration;
        target.VariableSpeed += target.StaticSpeed * (1 + _SpeedFactor * 0.01f);
    }

    public override void Update()
    {
        if (target != null && !isMax && reduceFactor > 0)
        {
            timeSinceTick += Time.deltaTime;
            if (timeSinceTick >= speedDuration)
            {
                target.VariableSpeed = target.VariableSpeed * (1 - reduceFactor * 0.01f);
                isMax = true;
            }
        }
        base.Update();
    }
    public override void Remove()
    {
        if (target != null)
        {
            target.VariableSpeed = target.StaticSpeed;
            base.Remove();
        }
    }
}
