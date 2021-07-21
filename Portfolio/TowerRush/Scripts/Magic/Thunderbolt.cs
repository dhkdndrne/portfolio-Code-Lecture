using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunderbolt", menuName = "Magic/Thunderbolt")]
public class Thunderbolt : PlayerMagic
{
    public override void ActiveMagic(Vector3 _Pos)
    {      
        rayHits = Physics2D.CircleCastAll(_Pos,magicStat[level].radius, Vector2.up, 0, LayerMask.GetMask("Tower"));

        foreach (RaycastHit2D hit in rayHits)
        {
            TowerBase T = hit.transform.GetComponent<TowerBase>();
            T.AddDebuff(new StopTowerDebuff(magicStat[level].duration, T));
        }
        base.ActiveMagic(_Pos);
    }
    public override MagicStat SetLevelStat(int _Level)
    {
        float _coolTime = 0;
        float _radious = 0;
        float _duration = 0;
        float _statFactor = 0;

        switch (_Level)
        {
            case 1:
                _coolTime = 15f;
                _radious = 20f;
                _duration = 2f;
                break;

            case 2:
                _coolTime = 14f;
                _radious = 21f;
                _duration = 2.1f;
                break;

            case 3:
               _coolTime = 14f;
               _radious = 22f;
               _duration = 2.2f;
                break;

            case 4:
                _coolTime = 13f;
                _radious = 23f;
                _duration = 2.3f;
                break;

            case 5:
                _coolTime = 13f;
                _radious = 24f;
                _duration = 2.4f;
                break;

            case 6:
                _coolTime = 12f;
                _radious = 26f;
                _duration = 2.5f;
                break;

            case 7:
                _coolTime = 12f;
                _radious = 27f;
                _duration = 2.6f;
                break;

            case 8:
                _coolTime = 11f;
                _radious = 28f;
                _duration = 2.7f;
                break;

            case 9:
                _coolTime = 11f;
                _radious = 29f;
                _duration = 2.8f;
                break;

            case 10:
                _coolTime = 10f;
                _radious = 30f;
                _duration = 3f;
                break;
        }

        MagicStat stat = new MagicStat(_coolTime, _radious, _duration, _statFactor);
        return stat;
    }
 
}
public class StopTowerDebuff : DebuffToTower
{
    public StopTowerDebuff(float _Duration, TowerBase _Tower, DebuffType _Type = DebuffType.NULL) : base(_Duration, _Tower, _Type)
    {
        target_Tower.ChangeTowerState(TowerState.THUNDERBOLT);
    }

    public override void Remove()
    {
        base.Remove();
        target_Tower.ChangeTowerState(TowerState.THUNDERBOLT,true);
    }
}
