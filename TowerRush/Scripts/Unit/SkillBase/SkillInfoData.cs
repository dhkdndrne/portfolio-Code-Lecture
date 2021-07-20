public struct SkillInfoData
{
    public ActiveType activeType;

    public int id;
    public float coolTime;
    public float duration;
    public int hpPer;
    public int targetNum;       //적용 인원 수
    public int shieldFactor;
    public int healFactor;
    public int hitCount;
    public float range;
    public float defenseFactor;
    public float evadeFactor;
    public float speedFactor;
    public bool invincible; //무적
    public bool revive;     //부활
    public bool isOneTime; // 한번만 발동
    public SkillInfoData(ActiveType _ActiveType, int _ID, float _CoolTime, float _Duration,
    int _HpPer, int _TargetNum, int _ShieldFactor, int _HealFactor,
    float _DefenseFactor, float _EvadeFactor, float _SpeedFactor, float _Range, bool _Invincible, bool _Revive,int _HitCount,bool _IsOneTime)
    {
        id = _ID;
        coolTime = _CoolTime;
        duration = _Duration;
        activeType = _ActiveType;

        hpPer = _HpPer;
        targetNum = _TargetNum;
        shieldFactor = _ShieldFactor;
        healFactor = _HealFactor;
        defenseFactor = _DefenseFactor;
        evadeFactor = _EvadeFactor;
        speedFactor = _SpeedFactor;
        range = _Range;
        invincible = _Invincible;
        revive = _Revive;
        isOneTime = _IsOneTime;

        hitCount = _HitCount;
    }

}
