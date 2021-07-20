using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTower : TowerBase
{
    public override Debuff GetTowerDebuff()
    {
        throw new System.NotImplementedException();
    }

    //private void Start()
    //{
    //    Init();
    //}
    protected override void Fire()
    {
        base.Fire();
    }

    public override void Init()
    {
        SetTowerINfo("Teleport");
        base.Init();
    }
}
