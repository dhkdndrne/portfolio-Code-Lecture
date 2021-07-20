using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActiveType 
{ 
    COOLTIME, 
    DIE, 
    COUNTING, 
    HP_PER 
}
public class UnitSkill
{
    public UnitAbillity owner;
    public SkillInfoData skillInfo;
    public bool isReady = true;
    public void Init(SkillInfoData _SkillData) //해당 스킬 정보들
    {
        skillInfo = _SkillData;
    }

    public IEnumerator StartCoolTime()
    {
        if (skillInfo.isOneTime) yield break;

        yield return new WaitForSeconds(skillInfo.duration);
        float time = skillInfo.coolTime;
        while (time > 0)
        {
            time -= 1 * Time.deltaTime;
            if(time % 1 ==0)Debug.Log(time);
            yield return null;
        }
        isReady = true;
        owner.CheckSkillCoolTime();
    }

    public void ActiveEffect(Transform _Transform,string _EffectName,float _Duration)
    {
        if(_Transform !=null)
        {
            PoolingManager.Instance.effectManager.ActiveEffect(_EffectName,_Transform, _Duration);                  
        }
    }

    public virtual void ResetSkill()
    {
        isReady = true;
    }
    public virtual void Active() {}
    public virtual void Counting() { }
}
