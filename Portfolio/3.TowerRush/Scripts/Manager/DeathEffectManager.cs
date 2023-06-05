using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeathEffectManager : MonoBehaviour
{
    [SerializeField] GameObject normalDeath;
    [SerializeField] GameObject frostDeath;
    [SerializeField] GameObject poisonDeath;
    [SerializeField] GameObject lightningDeath;
    [SerializeField] GameObject sniperDeath;

    public Queue<GameObject> normalDeathPool = new Queue<GameObject>();
    public Queue<GameObject> frostDeathPool = new Queue<GameObject>();
    public Queue<GameObject> poisonDeathPool = new Queue<GameObject>();
    public Queue<GameObject> lightingDeathPool = new Queue<GameObject>();
    public Queue<GameObject> sniperDeathPool = new Queue<GameObject>();

    public Dictionary<string, Queue<GameObject>> deathPoolDic = new Dictionary<string, Queue<GameObject>>();
    private void Start()
    {
        normalDeath.SetActive(false);
        frostDeath.SetActive(false);
        poisonDeath.SetActive(false);
        lightningDeath.SetActive(false);
        sniperDeath.SetActive(false);

        GameObject effect = null;

        for (int i = 0; i < 20; i++)
        {
            effect = Instantiate(normalDeath, Vector3.zero, Quaternion.identity, transform);
            effect.transform.SetParent(gameObject.transform.GetChild(0));
            normalDeathPool.Enqueue(effect);

            effect = Instantiate(frostDeath, Vector3.zero, Quaternion.identity, transform);
            effect.transform.SetParent(gameObject.transform.GetChild(1));
            frostDeathPool.Enqueue(effect);

            effect = Instantiate(poisonDeath, Vector3.zero, Quaternion.identity, transform);
            effect.transform.SetParent(gameObject.transform.GetChild(2));
            poisonDeathPool.Enqueue(effect);

            effect = Instantiate(lightningDeath, Vector3.zero, Quaternion.identity, transform);
            effect.transform.SetParent(gameObject.transform.GetChild(3));
            lightingDeathPool.Enqueue(effect);

            effect = Instantiate(sniperDeath, Vector3.zero, Quaternion.identity, transform);
            effect.transform.SetParent(gameObject.transform.GetChild(4));
            sniperDeathPool.Enqueue(effect);
        }

        deathPoolDic.Add("NORMAL", normalDeathPool);
        deathPoolDic.Add("ICE", frostDeathPool);
        deathPoolDic.Add("POISON", poisonDeathPool);
        deathPoolDic.Add("LIGHITNING", lightingDeathPool);
        deathPoolDic.Add("SNIPER", sniperDeathPool);

    }

    //이펙트 활성화
    public void ActiveEffect(string _Key, Transform _Transform)
    {
        GameObject obj = GetQueue(_Key);

        obj.transform.position = _Transform.position;

        float timer = obj.GetComponent<ParticleSystem>().main.duration;
        StartCoroutine(DeleteEffect(_Key, timer, obj));
    }

    //이펙트 추가생성
    void CreatePooledObject(string _Key)
    {
        GameObject effect = null;
        switch (_Key)
        {
            case "NORMAL":
                effect = Instantiate(normalDeath, Vector3.zero, Quaternion.identity, transform);
                effect.transform.SetParent(gameObject.transform.GetChild(0));
                effect.SetActive(false);
                normalDeathPool.Enqueue(effect);
                break;

            case "ICE":
                effect = Instantiate(frostDeath, Vector3.zero, Quaternion.identity, transform);
                effect.transform.SetParent(gameObject.transform.GetChild(1));
                effect.SetActive(false);
                frostDeathPool.Enqueue(effect);
                break;

            case "POISON":
                effect = Instantiate(poisonDeath, Vector3.zero, Quaternion.identity, transform);
                effect.transform.SetParent(gameObject.transform.GetChild(2));
                effect.SetActive(false);
                poisonDeathPool.Enqueue(effect);
                break;

            case "LIGHITNING":
                effect = Instantiate(lightningDeath, Vector3.zero, Quaternion.identity, transform);
                effect.transform.SetParent(gameObject.transform.GetChild(3));
                effect.SetActive(false);
                lightingDeathPool.Enqueue(effect);
                break;

            case "SNIPER":
                effect = Instantiate(sniperDeath, Vector3.zero, Quaternion.identity, transform);
                effect.transform.SetParent(gameObject.transform.GetChild(4));
                effect.SetActive(false);
                sniperDeathPool.Enqueue(effect);
                break;                                                                        
        }
    }
    //이펙트의 지속시간뒤에 이펙트를 끈다.
    public IEnumerator DeleteEffect(string _Key, float _EffectTimer, GameObject _Effect)
    {
        yield return new WaitForSeconds(_EffectTimer);
        InsertQueue(_Key, _Effect);
    }

    //큐에 이펙트를 넣는다
    public void InsertQueue(string key, GameObject _obj)
    {
        deathPoolDic[key].Enqueue(_obj);
        _obj.SetActive(false);
    }
    //큐에서 이펙트를 꺼냄
    GameObject GetQueue(string key)
    {
        if (deathPoolDic[key].Count <= 0)
        {
            CreatePooledObject(key);
        }

        GameObject t_object = deathPoolDic[key].Dequeue();
        t_object.SetActive(true);
        return t_object;
    }

}
