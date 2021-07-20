using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    UnitAbillity unitAbillity;

    [SerializeField] Image hp;
    public Image HPImage
    {
        get
        {
            return hp;
        }
    }
    [SerializeField] Image shield;

    public Image ShieldImage
    {
        get
        {
            return shield;
        }
    }


    private void Awake()
    {
        unitAbillity = GetComponent<UnitAbillity>();

    }

    private void OnEnable()
    {
        ShowShieldValue(unitAbillity.Shield, unitAbillity.MaxShield);
        ShowHpValue(unitAbillity.Hp, unitAbillity.MaxHp);
    }

    public void ShowHpValue(int _Hp, int _MaxHp)
    {
        float value = (_Hp / (float)_MaxHp);
        hp.fillAmount = value;
    }

    public void ShowShieldValue(int _Shield, int _MaxShield)
    {
        if (_Shield <= 0)
        {
            shield.fillAmount = 0;
            return;
        }

        float value = (_Shield / (float)_MaxShield);
        shield.fillAmount = value;
    }
}
