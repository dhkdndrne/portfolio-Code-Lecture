using UnityEngine;

public class Skill3002 : UnitSkill
{
    //이 유닛이 죽으면 주변 n명의 유닛들의 방어력이 n초간 n 상승한다.
    public override void Active()
    {
        if (owner.isBind) return;
        if (isReady)
        {
            int count = 0;
            RaycastHit2D[] rayHits = Physics2D.CircleCastAll(owner.transform.position, skillInfo.range, Vector2.up, 0, LayerMask.GetMask("Unit"));

            foreach (RaycastHit2D hit in rayHits)
            {
                if (hit.transform.gameObject.activeSelf == false || hit.transform.gameObject.Equals(owner.gameObject)) continue;
                if (count < skillInfo.targetNum) count++;
                else break;

                UnitAbillity unit = hit.transform.GetComponent<UnitAbillity>();
                if (unit.isBind) continue;

                float defense = skillInfo.defenseFactor;
                float duration =skillInfo.duration;

                unit.AddBuff(new DefenseUp(defense, duration, unit));
                ActiveEffect(hit.transform, "DefenseEff", skillInfo.duration);

            }
        }
    }
}
