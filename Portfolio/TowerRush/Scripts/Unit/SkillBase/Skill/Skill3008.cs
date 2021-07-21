public class Skill3008 : UnitSkill
{
    public override void Active()
    {
        if (owner.isBind) return;
        if (isReady)
        {
            owner.transform.GetComponent<UnitAbillity>().Defense += skillInfo.defenseFactor;
            ActiveEffect(owner.gameObject.transform, "DefenseEff", 2);
            isReady = false;
        }      
    }
}
