using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ForceField", menuName = "Magic/ForceField")]
public class ForceField : PlayerMagic
{ 
    public override void ActiveMagic(Vector3 _Pos)
    {
        possessionCount--;
        GameObject effect = Instantiate(effectPrefab, new Vector3(_Pos.x,_Pos.y,0), Quaternion.identity);
        effect.transform.localScale = new Vector3(magicStat[level].radius / 4.17f, magicStat[level].radius / 4.17f, 1f);
        Destroy(effect, magicStat[level].duration);

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
                _coolTime = 20f;
                _radious = 5f;
                _duration = 3f;
                break;

            case 2:
                _coolTime = 19f;
                _radious = 5.5f;
                _duration = 3.2f;

                break;

            case 3:
                _coolTime = 19f;
                _radious = 6f;
                _duration = 3.4f;

                break;

            case 4:
                _coolTime = 19f;
                _radious = 6.5f;
                _duration = 3.6f;
                break;

            case 5:
                _coolTime = 18f;
                _radious = 7f;
                _duration = 3.8f;

                break;

            case 6:
                _coolTime = 17f;
                _radious = 7.5f;
                _duration = 4.0f;

                break;

            case 7:
                _coolTime = 17f;
                _radious = 8f;
                _duration = 4.2f;
                break;

            case 8:
               _coolTime = 16f;
               _radious = 8.5f;
               _duration = 4.5f;
                break;

            case 9:
                _coolTime = 16f;
                _radious = 9f;
                _duration = 4.7f;
                break;

            case 10:
                _coolTime = 15f;
                _radious = 10f;
                _duration = 5f;
                break;
        }

        MagicStat stat = new MagicStat(_coolTime, _radious, _duration, _statFactor);
        return stat;
    }
  
}

