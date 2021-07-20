using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct MagicStat
{
    public float coolTime;
    public float radius;
    public float duration;
    public float statFactor;

    public MagicStat(float _CoolTime, float _Radius, float _Duration, float _StatFactor)
    {
        coolTime = _CoolTime;
        radius = _Radius;
        duration = _Duration;
        statFactor = _StatFactor;
    }
}

[System.Serializable]
public class PlayerMagic : ScriptableObject
{
    public Sprite icon;             //마법 아이콘
    public GameObject effectPrefab; //이팩트 프리펩
    public string magicName;         //이름
    public int level;           //마법레벨
    public int levelUp_Price;   //레벨업 가격
    public int perchasePrice;    //구매가격
    public int possessionCount; //소유 개수
    public bool isEquip;

    //스탯
    public Dictionary<int, MagicStat> magicStat = new Dictionary<int, MagicStat>();

    //public float coolTime;      //쿨타임
    //public float radious;       //범위
    //public float duration;      //지속시간
    //public float statFactor; //각각의 능력치

    public string description;  //설명
    protected RaycastHit2D[] rayHits;

    private void Awake()
    {
        SetMagicStatDic();
    }
    public virtual void ActiveMagic(Vector3 _Pos)
    {
        possessionCount--;
        foreach (var hits in rayHits)
        {
            GameObject effect = Instantiate(effectPrefab, hits.transform.position, Quaternion.identity);
            //Destroy(effect, duration/* effect.transform.GetChild(0).GetComponent<ParticleSystem>().main.duration + .5f*/);
            Destroy(effect, magicStat[level].duration); //duration/* effect.transform.GetChild(0).GetComponent<ParticleSystem>().main.duration + .5f*/);
        }

    }
    public virtual MagicStat SetLevelStat(int _Level) { return new MagicStat(); }
    public void SetMagicStatDic()
    {
        for (int i = 1; i <= 10; i++)
        {
            magicStat.Add(i, SetLevelStat(i));
        }
    }

}