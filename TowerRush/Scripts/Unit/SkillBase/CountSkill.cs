using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountSkill : UnitSkill
{
    int count;

    public override void Counting()
    {
        if (isReady && count < skillInfo.hitCount)
        {
            count++;            
            if (count.Equals(skillInfo.hitCount))
            {                
                count = 0;
                Active();               
            }
        }
    }

    public override void ResetSkill()
    {
        count = 0;
        base.ResetSkill();
    }
}
