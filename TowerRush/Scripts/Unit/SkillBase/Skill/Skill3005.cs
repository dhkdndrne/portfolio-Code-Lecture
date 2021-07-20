public class Skill3005 : CountSkill
{
    //10대를 맞으면 n쉴드를 획득 한다. 
    public override void Active()
    {
        if (owner.isBind) return;
        if (isReady)
        {
            isReady = false;
            int shield = skillInfo.shieldFactor;
            owner.GetShield(shield);
            ActiveEffect(owner.transform, "ShieldEff", 2);
        }
    }
}
