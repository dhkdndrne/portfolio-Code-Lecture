using UnityEngine;
public class Skill3006 : UnitSkill
{
    // 주위 아군의 이동속도를 일정시간 %증가후 서서히 감소 
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
                if (hit.transform.gameObject.activeSelf == false || owner.gameObject.Equals(hit.transform.gameObject)) continue;
                if (count < skillInfo.targetNum) count++;
                else break;             

                float speedFactor = skillInfo.speedFactor;
                float duration =skillInfo.duration;
                UnitAbillity unit = hit.transform.GetComponent<UnitAbillity>();
                if(unit.isBind) continue;

                unit.AddBuff(new SpeedUp(speedFactor, speedFactor * 0.5f,duration, unit));
                ActiveEffect(hit.transform, "SpeedEff", skillInfo.duration);
            }
        }
    }
}
