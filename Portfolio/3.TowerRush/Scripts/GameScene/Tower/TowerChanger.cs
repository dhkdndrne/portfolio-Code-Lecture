using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//타워의 타입을 설정하면 자동으로 변환 해주는 스크립트
enum TowerType
{
    NORMAL,
    ICE,
    POISON,
    SNIPER,
    LIGHITNING,
    RANDOM
}
public class TowerChanger : MonoBehaviour
{

    [SerializeField] TowerType towerType;
    [SerializeField] int level;
    private void OnEnable()
    {
        TowerBase tower = null;
        switch (towerType)
        {
            case TowerType.NORMAL:
                tower = gameObject.AddComponent<NormalTower>();
                break;

            case TowerType.ICE:
                tower = gameObject.AddComponent<IceTower>();
                break;

            case TowerType.POISON:
                tower = gameObject.AddComponent<PoisonTower>();
                break;

            case TowerType.SNIPER:
                tower = gameObject.AddComponent<SniperTower>();
                break;

            case TowerType.LIGHITNING:
                tower = gameObject.AddComponent<LightningTower>();
                break;

            case TowerType.RANDOM:
                tower = gameObject.AddComponent<RandomTower>();
                break;
        }
        tower.level = level;
        tower.Init();
        gameObject.AddComponent<TowerStateIcon>();
        gameObject.AddComponent<BoxCollider2D>();
    }
}
