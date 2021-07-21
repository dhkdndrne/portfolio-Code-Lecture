using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitAbillity : MonoBehaviour
{

    public delegate void Del_Type<T>();

    public event Del_Type<UnitAbillity> Del_Die;
    public event Del_Type<UnitAbillity> Del_CoolTime;
    public event Del_Type<UnitAbillity> Del_HitCounting;
    public event Del_Type<UnitAbillity> Del_HpPer;

    Element debuffElement;
    private int maxHp;
    public int MaxHp { get { return maxHp; } set { maxHp = value; } }

    [SerializeField] private int hp;
    public int Hp { get { return hp; } set { hp = value; } }

    [SerializeField] private float defense;
    public float Defense { get { return defense; } set { defense = value; } }

    [SerializeField] private float maxDefense;
    public float MaxDefense { get { return maxDefense; } set { maxDefense = value; } }

    [SerializeField] private float evade;
    public float Evade { get { return evade; } set { evade = value; } }

    [SerializeField] private float maxEvade;
    public float MaxEvade { get { return maxEvade; } set { maxEvade = value; } }

    [SerializeField] private float staticSpeed;         //기본 이동속도(고정)
    public float StaticSpeed
    {
        get { return staticSpeed; }
    }
    [SerializeField] bool isdead;
    public bool IsDead
    {
        get { return isdead; }
    }

    [SerializeField] private float variableSpeed;       //이동속도 증감용 변수
    public float VariableSpeed { get { return variableSpeed; } set { variableSpeed = value; } }

    [SerializeField] private float maxSpeed;
    public float MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }
    //---------------------
    [SerializeField] private int shield;
    public int Shield { get { return shield; } set { shield = value; } }

    int maxShield;
    public int MaxShield { get { return maxShield; } set { maxShield = value; } }

    float egnoreDebuffFactor; // 디버프 무시 확률
    public float EgnorDebuffFactor { get { return egnoreDebuffFactor; } }

    [SerializeField] bool invincible;
    public bool Invincible { get { return invincible; } set { invincible = value; } }

    public bool isBind;

    public Scroll myScroll;
    public HpBar hpBar;

    List<Debuff> debuffs = new List<Debuff>();
    List<Debuff> debuffstoRemove = new List<Debuff>();
    List<Debuff> newDebuffs = new List<Debuff>();

    List<Buff> skillBuffs = new List<Buff>();
    List<Buff> skillBuffsToRemove = new List<Buff>();
    List<Buff> newSkillBuffs = new List<Buff>();

    [SerializeField] public Coroutine skillCoroutine;     //스킬 쿨타임

    void Start()
    {
        if (myScroll != null && myScroll.scrollType != ScrollType.NULL)
        {
            if (myScroll.unitSkill.skillInfo.activeType.Equals(ActiveType.COOLTIME)) { Del_CoolTime += myScroll.unitSkill.Active; }
            else if (myScroll.unitSkill.skillInfo.activeType.Equals(ActiveType.COUNTING)) { Del_HitCounting += myScroll.unitSkill.Counting; }
            else if (myScroll.unitSkill.skillInfo.activeType.Equals(ActiveType.DIE)) { Del_Die += myScroll.unitSkill.Active; }
            else if (myScroll.unitSkill.skillInfo.activeType.Equals(ActiveType.HP_PER)) { Del_HpPer += myScroll.unitSkill.Active; }
        }

        hpBar = GetComponent<HpBar>();
    }

    private void OnEnable()
    {
        StartCoroutine(FirstOutSkillDelay());
        StartCoroutine(ClearBuffOnBind());
        StartCoroutine("ApplyDebuff");
        StartCoroutine("ApplySkill");
    }
    private void OnDisable()
    {
        clearBuffList();
    }

    //처음 나오자마자 스킬 쓰지 않도록
    IEnumerator FirstOutSkillDelay()
    {
        yield return new WaitForSeconds(3f);
        CheckSkillCoolTime();
    }

    public void CheckSkillCoolTime()
    {
        if (myScroll != null && !isBind)
        {
            Del_CoolTime?.Invoke();
            skillCoroutine = StartCoroutine(myScroll.unitSkill.StartCoolTime());
        }
    }

    public void InitStat(Unit _Unit)
    {
        MaxHp = _Unit.unitStat.hp;
        hp = MaxHp;

        maxDefense = _Unit.unitStat.defense;
        defense = maxDefense;

        maxEvade = _Unit.unitStat.evade;
        evade = maxEvade;

        MaxSpeed = _Unit.unitStat.speed;
        staticSpeed = MaxSpeed;
        variableSpeed = staticSpeed;

        maxShield = _Unit.unitStat.shield;
        shield = maxShield;

        egnoreDebuffFactor = _Unit.unitStat.egnoreDebuffFactor;

        invincible = false; //무적 초기화
        isBind = false; //속박 초기화
        isdead = false; //죽음 초기화
    }

    //스킬의 인스턴스 만들어준다.
    public void InstanceSkillData(Unit _Unit)
    {
        if (_Unit.scrollItem != null)
        {
            Scroll sc = new Scroll(_Unit.scrollItem.id, _Unit.scrollItem.itemRank, _Unit.scrollItem.scrollType);
            myScroll = sc;
            //스킬 의 인스턴스 만들어준다.
            myScroll.GetSkillData();
            myScroll.unitSkill.owner = this;
        }
    }
    //데미지 계산 = (데미지 - 방어력)
    float CalculateDamage(float _Damage)
    {
        //방어력 = n% 딜 감소
        float result = _Damage * (1 - defense * 0.01f);

        if (result <= 0) return 1;
        return result;
    }

    public void Hit(float _Damage, Element _DmgSource = Element.NORMAL, bool _Critical = false, bool _DotDamage = false)
    {
        PopUpType popUptype;

        if (_DmgSource.Equals(Element.POISON)) popUptype = PopUpType.POISON;
        else popUptype = _Critical == true ? PopUpType.CRITICAL : PopUpType.DAMAGE;  //크리티컬 여부에따라 팝업 설정 다르게

        if (invincible)
        {
            PoolingManager.Instance.damagePopUpManager.ShowDamagePopUp(transform, "immortal", PopUpType.INVINCIBLE);
            return;
        }

        int damage = 0;
        //실드가 존재하면
        if (shield > 0)
        {
            PoolingManager.Instance.damagePopUpManager.ShowDamagePopUp(transform, (int)_Damage, PopUpType.SHIELD);
            // 실드를 깎는다 ( 100% 데미지)
            GetShield(-(int)_Damage);

            // 실드 < 데미지 경우 나머지 실드의 값을 데미지로 넣는다.
            if (shield < 0)
            {
                int lastDamage = Mathf.Abs(shield);
                shield = 0;
                MaxShield = 0;
                damage = (int)CalculateDamage(lastDamage);
                PoolingManager.Instance.damagePopUpManager.ShowDamagePopUp(transform, damage, popUptype);
                GetHp(-damage);
            }

        }
        else
        {
            damage = _DotDamage == true ? (int)_Damage : (int)CalculateDamage(_Damage);
            PoolingManager.Instance.damagePopUpManager.ShowDamagePopUp(transform, damage, popUptype);
            GetHp(-damage);
        }

        debuffElement = _DmgSource;

        if (_DotDamage != true && !isBind) //도트데미지가 아닐때 카운팅을 한다.
        {
            Del_HitCounting?.Invoke();
        }

        if (Del_HpPer != null && !myScroll.unitSkill.skillInfo.invincible)
        {
            if (hp < MaxHp * myScroll.unitSkill.skillInfo.hpPer * 0.01f)
            {
                Del_HpPer.Invoke();
            }
        }

        if (hp <= 0)
        {
            if (!isBind && Del_HpPer != null && myScroll.unitSkill.skillInfo.invincible && myScroll.unitSkill.isReady)
            {
                Del_HpPer.Invoke();
                hp = 1;
                return;
            }

            if (!isBind) Del_Die?.Invoke();
            PoolingManager.Instance.deathEffectManager.ActiveEffect(debuffElement.ToString(), transform);
            Die();
        }
    }
    public void Die()
    {
        if (!isdead)
        {
            GameManager.Instance.curPop = GameManager.Instance.curPop < 0 ? GameManager.Instance.curPop = 0 : --GameManager.Instance.curPop;
            if (myScroll != null)
            {
                if (skillCoroutine != null)
                {
                    StopCoroutine(skillCoroutine);
                    skillCoroutine = null;
                }

                myScroll.unitSkill.ResetSkill();
            }
            shield = 0;
            maxShield = 0;
            isdead = true;
            StartCoroutine(InitFromDead());
        }
    }
    //죽었을때 오브젝트 꺼주는 코루틴
    IEnumerator InitFromDead()
    {
        yield return new WaitUntil(() => isdead == true);
        clearBuffList();
        StopCoroutine("ApplyDebuff");
        StopCoroutine("ApplySkill");
        gameObject.SetActive(false);
    }
    void clearBuffList()
    {
        debuffs?.Clear();
        debuffstoRemove?.Clear();
        newDebuffs?.Clear();

        skillBuffs?.Clear();
        skillBuffsToRemove?.Clear();
        newSkillBuffs?.Clear();
    }

    public IEnumerator ClearBuffOnBind()
    {
        yield return new WaitUntil(() => isBind == true);
        foreach (var buff in skillBuffs)
        {
            RemoveBuff(buff);
        }
    }
    IEnumerator ApplySkill()
    {
        yield return new WaitUntil(() => newSkillBuffs.Count > 0);
        while (skillBuffs.Count > 0 || newSkillBuffs.Count > 0)
        {
            HandleSkillBuff();
            yield return null;
        }

        StartCoroutine(ApplySkill());
    }

    void HandleSkillBuff()
    {
        if (newSkillBuffs.Count > 0)
        {
            skillBuffs.AddRange(newSkillBuffs);
            newSkillBuffs.Clear();
        }

        foreach (Buff buff in skillBuffsToRemove)
        {
            skillBuffs.Remove(buff);
        }

        skillBuffsToRemove.Clear();

        //모든 버프 효과 적용
        foreach (Buff buff in skillBuffs)
        {
            buff.Update();
        }
    }

    public void AddBuff(Buff _Buff)
    {
        // 다른버프이면 버프 추가
        if (!skillBuffs.Exists(x => x.GetType() == _Buff.GetType()))
            newSkillBuffs.Add(_Buff);
    }
    public void RemoveBuff(Buff _Buff)
    {
        skillBuffsToRemove.Add(_Buff);
    }

    IEnumerator ApplyDebuff()
    {
        yield return new WaitUntil(() => newDebuffs.Count > 0);
        while (debuffs.Count > 0 || newDebuffs.Count > 0)
        {
            HandleDebuff();
            yield return null;
        }
        StartCoroutine(ApplyDebuff());
    }
    //디버프 효과 적용하는 함수

    void HandleDebuff()
    {
        if (newDebuffs.Count > 0)
        {
            debuffs.AddRange(newDebuffs);
            newDebuffs.Clear();
        }

        foreach (Debuff debuff in debuffstoRemove)
        {
            debuffs.Remove(debuff);
        }

        debuffstoRemove.Clear();

        //모든 디버프 효과 적용
        foreach (Debuff debuff in debuffs)
        {
            debuff.Update();
        }
    }
    //디버프 추가하는 함수
    public void AddDebuff(Debuff _Debuff)
    {
        // 다른 디버프이면 디버프 추가
        if (!debuffs.Exists(x => x.GetType() == _Debuff.GetType()))
            newDebuffs.Add(_Debuff);
        else
        {
            //동일한 디버프가 입력되면 elapsed(디버프 지속시간) 초기화
            Debuff temp = debuffs.Find(x => x.GetType() == _Debuff.GetType());

            if (temp.debuffType.Equals(Debuff.DebuffType.OVERLAP))
            {
                if (temp.overlap < temp.maxOverlap - 1)
                {
                    temp.overlap++;
                    temp.applied = false;
                }
                temp.Elapsed = 0;
            }
            else if (temp.debuffType.Equals(Debuff.DebuffType.RESET))
            {
                temp.Elapsed = 0;
            }
        }
    }
    public void RemoveDebuff(Debuff _Debuff)
    {
        debuffstoRemove.Add(_Debuff);
    }

    public void GetShield(int _Shield)
    {
        shield += _Shield;
        if (shield >= maxHp * 0.5f)
        {
            shield = (int)(maxHp * 0.5f);   //실드는 최대체력의 50%
            maxShield = shield;
        }
        if (shield >= maxShield) maxShield = shield;

        hpBar.ShowShieldValue(shield, MaxShield);
    }
    public void GetHp(int _Hp)
    {
        hp += _Hp;
        if (hp > maxHp) hp = maxHp;
        hpBar.ShowHpValue(hp, maxHp);
    }

}
