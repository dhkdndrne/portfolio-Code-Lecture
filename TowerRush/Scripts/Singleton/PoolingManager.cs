using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : SingleTon<PoolingManager>
{
    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

    public DeathEffectManager deathEffectManager;
    public DamagePopUpManager damagePopUpManager;
    public EffectPool effectManager;
}
