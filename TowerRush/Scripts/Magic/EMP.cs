using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EMP", menuName = "Magic/EMP")]
public class EMP : PlayerMagic
{
    public override void ActiveMagic(Vector3 _Pos)
    {       
        rayHits = Physics2D.CircleCastAll(_Pos, magicStat[level].radius, Vector2.up, 0, LayerMask.GetMask("Tower"));

        foreach (RaycastHit2D hit in rayHits)
        {
            TowerBase T = hit.transform.GetComponent<TowerBase>();
            T.AddDebuff(new T_EmpDebuff((int)magicStat[level].statFactor, magicStat[level].duration,T,Debuff.DebuffType.RESET));
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
                _duration = 3f;
                _statFactor = 3;
                break;

            case 2:
                _coolTime = 19f;
                _radious = 16f;
                _duration = 3.2f;
                _statFactor = 3;
                break;

            case 3:
                _coolTime = 19f;
                _radious = 16f;
                _duration = 3.4f;
                _statFactor = 3;
                break;

            case 4:
                _coolTime = 18f;
                _radious = 17f;
                _duration = 3.6f;
                _statFactor = 4;
                break;

            case 5:
                _coolTime = 18f;
                _radious = 17f;
                _duration = 3.8f;
                _statFactor = 4;
                break;

            case 6:
                _coolTime = 17f;
                _radious = 18f;
                _duration = 4f;
                _statFactor = 4;
                break;

            case 7:
                _coolTime = 17f;
                _radious = 18f;
                _duration = 4.2f;
                _statFactor = 5;
                break;

            case 8:
                _coolTime = 16f;
                _radious = 19f;
                _duration = 4.5f;
                _statFactor = 5;
                break;

            case 9:
                _coolTime = 16f;
                _radious = 19f;
                _duration = 4.7f;
                _statFactor = 5;
                break;

            case 10:
                _coolTime = 7f;
                _radious = 30f;
                _duration = 5f;
                _statFactor = 6;
                break;
        }

        MagicStat stat = new MagicStat(_coolTime, _radious, _duration, _statFactor);
        return stat;
    }
 
}
