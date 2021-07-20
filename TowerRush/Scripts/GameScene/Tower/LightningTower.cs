using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTower : TowerBase
{
    [SerializeField]  float reduceEvadeFactor;
    [SerializeField] int maxOverlap;
    public float ReduceEvadeFactor { get { return reduceEvadeFactor; } }

    public override Debuff GetTowerDebuff()
    {
        return new LightningDebuff(targetUnit, DebuffDuration,reduceEvadeFactor, maxOverlap, Debuff.DebuffType.OVERLAP);
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
        bulletPrefab = Resources.Load<GameObject>("Tower/Bullet/LightningBullet");
        int world = DataController.CurrentStage / 10;
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tower/TowerImage/World" + (world + 1) + "/Lightning");
        gameObject.name = "LightningTower";

        SetTowerINfo("Lightning");
        reduceEvadeFactor = DBManager.Instance.towerDB.towerDB["Lightning"][level - 1].debuffRate;
        maxOverlap = DBManager.Instance.towerDB.towerDB["Lightning"][level - 1].maxOverlap;

        description = "빠르게 공격하며 히로디온의 회피율을 낮춘다.";
        debuffDescription = "회피율 감소";
        base.Init();
    }

    protected override void UpdateTarget()
    {
        base.UpdateTarget();
    }
}
