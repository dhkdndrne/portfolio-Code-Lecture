using UnityEngine;


public class Skill3004 : UnitSkill
{
    //주변 n의 유닛이 n 쉴드를 형성한다.
    public override void Active()
    {
        if (owner.isBind) return;
        if (isReady)
        {
            int count = 0;
            RaycastHit2D[] rayHits = Physics2D.CircleCastAll(owner.transform.position, skillInfo.range, Vector2.up, 0, LayerMask.GetMask("Unit"));
            isReady = false;
            foreach (RaycastHit2D hit in rayHits)
            {
                //자신 제외 + 죽어있는 유닛이면 패스
                if (hit.transform.gameObject.activeSelf == false || hit.transform.gameObject.Equals(owner.gameObject)) continue;

                if (count < skillInfo.targetNum) count++;
                else break;

                UnitAbillity unit = hit.transform.GetComponent<UnitAbillity>();
                if (unit.isBind) continue;

                int shield = skillInfo.shieldFactor;
                unit.GetShield(shield);
                ActiveEffect(hit.transform, "ShieldEff", 2);
            }
        }
    }
}
