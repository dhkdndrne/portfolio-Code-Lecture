using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Feather_Smoke", menuName = "Magic/Feather_Smoke")]
public class FeatherSmoke : PlayerMagic
{
    public override void ActiveMagic(Vector3 _Pos)
    {      
        rayHits = Physics2D.CircleCastAll(_Pos, magicStat[level].radius, Vector2.up, 0, LayerMask.GetMask("Tower"));

        foreach (RaycastHit2D hit in rayHits)
        {
            TowerBase T = hit.transform.GetComponent<TowerBase>();
            T.AddDebuff(new T_RandomAttackRate(magicStat[level].duration, magicStat[level].statFactor, T,Debuff.DebuffType.RESET));
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
                _radious = 15f;
                _duration = 2f;
                _statFactor = 30;
                break;

            case 2:
                _coolTime = 14f;
                _radious = 16f;
                _duration = 2.1f;
                _statFactor = 32;
                break;

            case 3:
                _coolTime = 14f;
                _radious = 16f;
                _duration = 2.2f;
                _statFactor = 34;
                break;

            case 4:
                _coolTime = 13f;
                _radious = 17f;
                _duration = 2.3f;
                _statFactor = 36;
                break;

            case 5:
                _coolTime = 13f;
                _radious = 17f;
                _duration = 2.4f;
                _statFactor = 38;
                break;

            case 6:
                _coolTime = 12f;
                _radious = 18f;
                _duration = 2.5f;
                _statFactor = 40;
                break;

            case 7:
                _coolTime = 12f;
                _radious = 18f;
                _duration = 2.6f;
                _statFactor = 42;
                break;

            case 8:
                _coolTime = 11f;
                _radious = 19f;
                _duration = 2.7f;
                _statFactor = 45;
                break;

            case 9:
                _coolTime = 11f;
                _radious = 19f;
                _duration = 3f;
                _statFactor = 47;
                break;

            case 10:
                _coolTime = 10f;
                _radious = 20f;
                _duration = 3.5f;
                _statFactor = 50;
                break;
        }

        MagicStat stat = new MagicStat(_coolTime, _radious, _duration, _statFactor);
        return stat;
    }
  
}
