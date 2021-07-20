using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTower : TowerBase
{
    [SerializeField]
    float tickTime;
    public float TickTime
    {
        get
        {
            return tickTime;
        }
    }

    [SerializeField]
    float tickDamage;
    public float TickDamage
    {
        get { return tickDamage; }
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
        bulletPrefab = Resources.Load<GameObject>("Tower/Bullet/PoisionBullet");
        int world = DataController.CurrentStage / 10;
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tower/TowerImage/World" + (world + 1) + "/Poison");
        gameObject.name = "PoisonTower";

        SetTowerINfo("Poison");
        tickDamage = DBManager.Instance.towerDB.towerDB["Poison"][level - 1].debuffRate;
        tickTime = DBManager.Instance.towerDB.towerDB["Poison"][level - 1].tickTime;

        description = "5초간 방어력을 무시하는 도트 공격";
        debuffDescription = "5초간 초당 "+ tickDamage + " 데미지";
        base.Init();

    }

    protected override void UpdateTarget()
    {
        base.UpdateTarget();
    }
    public override Debuff GetTowerDebuff()
    {
        return new PoisonDebuff(TickDamage, TickTime, DebuffDuration, targetUnit,Debuff.DebuffType.RESET);
    }

}
