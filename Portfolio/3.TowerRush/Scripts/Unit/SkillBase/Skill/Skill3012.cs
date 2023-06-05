public class Skill3012 : CountSkill
{
    public override void Active()
    {
        if (owner.isBind) return;
        if (isReady)
        {
            UnitAbillity unit = owner.transform.GetComponent<UnitAbillity>();
            float speedFactor = skillInfo.speedFactor;
            float duration = skillInfo.duration;
            unit.AddBuff(new SpeedUp(speedFactor, 0, duration, unit));
            ActiveEffect(owner.gameObject.transform, "SpeedEff", skillInfo.duration);
            isReady = false;
        }
    }
}
