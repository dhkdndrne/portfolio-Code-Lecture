using UnityEngine;
public class Skill3007 : UnitSkill
{   
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
                if (hit.transform.gameObject != owner.gameObject)
                {
                    if (hit.transform.gameObject.activeSelf == false) continue;
                    if (count < skillInfo.targetNum) count++;
                    else break;

                   
                    float duration = skillInfo.duration;
                    int healFactor = skillInfo.healFactor;

                    UnitAbillity unit = hit.transform.GetComponent<UnitAbillity>();
                    if (unit.isBind) continue;
                    unit.AddBuff(new HealTickBuff(1f, healFactor, duration, unit));
                    ActiveEffect(hit.transform, "HealEff", skillInfo.duration);
                }
            }
        }
    }
}
