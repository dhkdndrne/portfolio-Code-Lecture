public class Skill3003 : UnitSkill
{
    //자신의 체력을 초당 n회복
    public override void Active()
    {
        if (owner.isBind) return;
        if (isReady)
        {
            int healFactor = skillInfo.healFactor;
            float duration = skillInfo.duration;
            isReady = false;
            owner.AddBuff(new HealTickBuff(1f, healFactor, duration, owner));
            ActiveEffect(owner.transform, "HealEff", skillInfo.duration);
        }
    }
}
