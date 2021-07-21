public class Skill3013 : UnitSkill
{
    public override void Active()
    {
        if (owner.isBind) return;
        if (isReady)
        {
            UnitAbillity unit = owner.transform.GetComponent<UnitAbillity>();
            unit.AddBuff(new Invincible(skillInfo.duration, unit));
            ActiveEffect(owner.gameObject.transform, "ImmortalEff", skillInfo.duration); //바꿩됨
            isReady = false;
        }
    }
}
