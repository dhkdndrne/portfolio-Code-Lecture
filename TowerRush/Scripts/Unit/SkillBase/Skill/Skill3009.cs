public class Skill3009 : UnitSkill
{
    public override void Active()
    {
        if (owner.isBind) return;
        if (isReady)
        {
            UnitAbillity unit = owner.transform.GetComponent<UnitAbillity>();
            unit.AddBuff(new ShieldUp(skillInfo.shieldFactor,skillInfo.duration, unit));            
            ActiveEffect(owner.gameObject.transform, "ShieldEff", 2);
            isReady = false;
        }
    }
}
