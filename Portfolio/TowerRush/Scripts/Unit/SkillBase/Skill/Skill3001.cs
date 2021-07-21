using UnityEngine;

public class Skill3001 : CountSkill
{
    //힘이 모자라도 나아가게 해준다. 10대 맞으면 0m범위 n명의 유닛이 초당 n의 체력회복   
    public override void Active()
    {
        if (owner.isBind) return;
        if (isReady)
        {
            int count = 0;
            isReady = false;
            RaycastHit2D[] rayHits = Physics2D.CircleCastAll(owner.transform.position, skillInfo.range, Vector2.up, 0, LayerMask.GetMask("Unit"));

            foreach (RaycastHit2D hit in rayHits)
            {
                if (hit.transform.gameObject.activeSelf == false) continue;

                if (count < skillInfo.targetNum) count++;
                else break;

                UnitAbillity unit = hit.transform.GetComponent<UnitAbillity>();
                if (unit.isBind) continue;

                int healFactor = skillInfo.healFactor;
                float duration = skillInfo.duration;

                unit.AddBuff(new HealTickBuff(1f, healFactor, duration, unit));
                ActiveEffect(hit.transform, "HealEff",skillInfo.duration);
            }
        }
    }
}
