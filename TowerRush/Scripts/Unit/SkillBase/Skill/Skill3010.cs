public class Skill3010 : CountSkill
{
    public override void Active()
    {
        if (owner.isBind) return;
        if (isReady)
        {
            UnitAbillity unit = owner.transform.GetComponent<UnitAbillity>();
            //전체값 * 퍼센트 / 100
            float shield = unit.MaxHp * skillInfo.shieldFactor * 0.01f;
            unit.AddBuff(new ShieldUp((int)shield, skillInfo.duration, unit));
            ActiveEffect(owner.gameObject.transform, "ShieldEff", 2);
            isReady = false;
        }
    }
}
