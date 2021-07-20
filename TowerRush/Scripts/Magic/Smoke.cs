using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Smoke", menuName = "Magic/Smoke")]
public class Smoke : PlayerMagic
{
    public override void ActiveMagic(Vector3 _Pos)
    {      
        rayHits = Physics2D.CircleCastAll(_Pos,magicStat[level].radius, Vector2.up, 0, LayerMask.GetMask("Tower"));

        foreach (RaycastHit2D hit in rayHits)
        {
            TowerBase T = hit.transform.GetComponent<TowerBase>();
            T.AddDebuff(new T_ReduceHitRate(magicStat[level].statFactor, magicStat[level].duration,T));
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
                _coolTime = 10f;
                _radious = 15f;
                _duration = 3f;
                _statFactor = 30f;
                break;

            case 2:
                _coolTime = 10f;
                _radious = 16f;
                _duration = 3.2f;
                _statFactor = 32f;
                break;

            case 3:
                _coolTime = 10f;
                _radious = 16f;
                _duration = 3.5f;
                _statFactor = 35f;
                break;

            case 4:
                _coolTime = 9f;
                _radious = 17f;
                _duration = 3.8f;
                _statFactor = 38f;
                break;

            case 5:
                _coolTime = 9f;
                _radious = 17f;
                _duration = 4.1f;
                _statFactor = 41f;
                break;

            case 6:
                _coolTime = 9f;
                _radious = 18f;
                _duration = 4.4f;
                _statFactor = 44f;
                break;

            case 7:
                _coolTime = 8f;
                _radious = 18f;
                _duration = 4.8f;
                _statFactor = 48f;
                break;

            case 8:
                _coolTime = 8f;
                _radious = 19f;
                _duration = 5.1f;
                _statFactor = 51f;
                break;

            case 9:
                _coolTime = 8f;
                _radious = 19f;
                _duration = 5.6f;
                _statFactor = 56f;
                break;

            case 10:
                _coolTime = 7f;
                _radious = 20f;
                _duration = 6f;
                _statFactor = 60f;
                break;
        }

        MagicStat stat = new MagicStat(_coolTime, _radious, _duration, _statFactor);
        return stat;
    }

}

