using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTower : TowerBase
{
    int randomDamage;

    protected override void Fire()
    {
        damage = Random.Range(randomDamage / 2, randomDamage);
        criticalRate = Random.Range(0, 100f);
        fireDelay = Random.Range(0, 3f);
        base.Fire();
    }
    public override void Init()
    {
        bulletPrefab = Resources.Load<GameObject>("Tower/Bullet/Bullet");
        int world = DataController.CurrentStage / 10;
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tower/TowerImage/World" + (world + 1) + "/Random");
        gameObject.name = "RandomTower";

        SetTowerINfo("Random");
        description = "모든것이 랜덤인 타워";
        debuffDescription = "디버프 없음";

        randomDamage = damage;
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
