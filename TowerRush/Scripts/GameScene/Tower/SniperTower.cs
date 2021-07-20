using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTower : TowerBase
{
    public override Debuff GetTowerDebuff()
    {
        return null;
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
        bulletPrefab = Resources.Load<GameObject>("Tower/Bullet/SniperBullet");
        int world = DataController.CurrentStage / 10;
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tower/TowerImage/World" + (world + 1) + "/Sniper");
        gameObject.name = "SniperTower";

        description = "높은 치명타 확률,넓은 범위 및 회피 무시효과로 공격";
        debuffDescription = "디버프 없음";

        SetTowerINfo("Sniper");
        base.Init();
        
    }

    protected override void UpdateTarget()
    {
        base.UpdateTarget();
    }
}
