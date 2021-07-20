using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlashBang", menuName = "Magic/FlashBang")]
public class FlashBang : PlayerMagic
{
    public override void ActiveMagic(Vector3 _Pos)
    {
        rayHits = Physics2D.CircleCastAll(_Pos, magicStat[level].radius, Vector2.up, 0, LayerMask.GetMask("Tower"));

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
                _radious = 15f;
                break;

            case 2:
                _radious = 16f;
                break;

            case 3:
               _radious = 16f;
                break;

            case 4:
                _radious = 17f;
                break;

            case 5:
                _radious = 17f;
                break;

            case 6:
                _radious = 18f;
                break;

            case 7:
                _radious = 18f;
                break;

            case 8:
                _radious = 19f;
                break;

            case 9:
                _radious = 19f;
                break;

            case 10:
                _radious = 20f;
                break;
        }

        MagicStat stat = new MagicStat(_coolTime, _radious, _duration, _statFactor);
        return stat;
    }
   
}
