using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : TowerBase
{
    [SerializeField] float slowRate;
    [SerializeField] int maxOverlap;
    public override Debuff GetTowerDebuff()
    {
        return new SlowDebuff(maxOverlap, slowRate,DebuffDuration,targetUnit,Debuff.DebuffType.OVERLAP);
    }

    //private void OnEnable()
    //{
    //    Init();
    //}

    protected override void Fire()
    {
        base.Fire();
    }

    public override void Init()   
    {
        bulletPrefab = Resources.Load<GameObject>("Tower/Bullet/IceBullet");
        int world = DataController.CurrentStage / 10;
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tower/TowerImage/World" + (world + 1) + "/Ice");
        gameObject.name = "IceTower";

        SetTowerINfo("Ice");
        slowRate = DBManager.Instance.towerDB.towerDB["Ice"][level - 1].debuffRate;
        maxOverlap = DBManager.Instance.towerDB.towerDB["Ice"][level - 1].maxOverlap;
        description = "최대 5중첩까지 유닛의 속도 감소 공격";
        debuffDescription = "중첩당 이동속도 20% 감소";

        base.Init();
    }
   
    protected override void UpdateTarget()
    {
        base.UpdateTarget();
    }
}
