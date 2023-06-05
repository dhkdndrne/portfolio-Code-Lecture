using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTower : TowerBase
{
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
        bulletPrefab = Resources.Load<GameObject>("Tower/Bullet/Bullet");
        int world = DataController.CurrentStage / 10;
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tower/TowerImage/World"+(world+1)+ "/Normal");
        gameObject.name = "NormalTower";

        SetTowerINfo("Normal");
        description = "일반적인 타워로 효과 없이 공격";
        debuffDescription = "디버프 없음";
        base.Init();
    }

    protected override void UpdateTarget()
    {
        base.UpdateTarget();
    }

    public override Debuff GetTowerDebuff()
    {
        return null;
    }
}
