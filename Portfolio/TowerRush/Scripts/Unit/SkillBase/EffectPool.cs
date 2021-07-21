using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    [SerializeField] GameObject healPrefab;
    [SerializeField] GameObject evadePrefab;
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] GameObject healthPrefab;
    [SerializeField] GameObject debuffPrefab;
    [SerializeField] GameObject defensePrefab;
    [SerializeField] GameObject speedPrefab;
    [SerializeField] GameObject immortalPrefab;

    Queue<GameObject> healEffect = new Queue<GameObject>();
    Queue<GameObject> evadeEffect = new Queue<GameObject>();
    Queue<GameObject> shieldEffect = new Queue<GameObject>();
    Queue<GameObject> debuffeffect = new Queue<GameObject>();
    Queue<GameObject> defenseEffect = new Queue<GameObject>();
    Queue<GameObject> healthEffect = new Queue<GameObject>();
    Queue<GameObject> speedEffect = new Queue<GameObject>();
    Queue<GameObject> immortalEffect = new Queue<GameObject>();

    //GameObject healParent;  //이펙트오브젝트 실제로 담는 공간(부모 체크용)
    //GameObject healParent;  //이펙트오브젝트 실제로 담는 공간(부모 체크용)
    //GameObject healParent;  //이펙트오브젝트 실제로 담는 공간(부모 체크용)
    //GameObject healParent;  //이펙트오브젝트 실제로 담는 공간(부모 체크용)
    //GameObject healParent;  //이펙트오브젝트 실제로 담는 공간(부모 체크용)
    //GameObject healParent;  //이펙트오브젝트 실제로 담는 공간(부모 체크용)
    //GameObject healParent;  //이펙트오브젝트 실제로 담는 공간(부모 체크용)

    public Dictionary<string, Queue<GameObject>> effectPoolDic = new Dictionary<string, Queue<GameObject>>();

    void Start()
    {
        healPrefab.SetActive(false);
        evadePrefab.SetActive(false);
        shieldPrefab.SetActive(false);
        healthPrefab.SetActive(false);
        debuffPrefab.SetActive(false);
        defensePrefab.SetActive(false);
        speedPrefab.SetActive(false);
        immortalPrefab.SetActive(false);

        for (int i = 0; i < 30; i++)
        {
            GameObject healObj = Instantiate(healPrefab);
            healObj.transform.SetParent(transform.GetChild(0).transform);

            GameObject evadeObj = Instantiate(evadePrefab);
            evadeObj.transform.SetParent(transform.GetChild(1).transform);

            GameObject shieldObj = Instantiate(shieldPrefab);
            shieldObj.transform.SetParent(transform.GetChild(2).transform);

            GameObject healthObj = Instantiate(healthPrefab);
            healthObj.transform.SetParent(transform.GetChild(3).transform);

            GameObject debuffObj = Instantiate(debuffPrefab);
            debuffObj.transform.SetParent(transform.GetChild(4).transform);

            GameObject defenseObj = Instantiate(defensePrefab);
            defenseObj.transform.SetParent(transform.GetChild(5).transform);

            GameObject speedObj = Instantiate(speedPrefab);
            speedObj.transform.SetParent(transform.GetChild(6).transform);

            GameObject immortalObj = Instantiate(immortalPrefab);
            immortalObj.transform.SetParent(transform.GetChild(7).transform);

            healEffect.Enqueue(healObj);
            evadeEffect.Enqueue(evadeObj);
            shieldEffect.Enqueue(shieldObj);
            healthEffect.Enqueue(healthObj);
            debuffeffect.Enqueue(debuffObj);
            defenseEffect.Enqueue(defenseObj);
            speedEffect.Enqueue(speedObj);
            immortalEffect.Enqueue(immortalObj);
        }
        effectPoolDic.Add("HealEff", healEffect);
        effectPoolDic.Add("EvadeEff", evadeEffect);
        effectPoolDic.Add("ShieldEff", shieldEffect);
        effectPoolDic.Add("HealthEff", healthEffect);
        effectPoolDic.Add("DebuffEff", debuffeffect);
        effectPoolDic.Add("DefenseEff", defenseEffect);
        effectPoolDic.Add("SpeedEff", speedEffect);
        effectPoolDic.Add("ImmortalEff", immortalEffect);
    }
    //이펙트 활성화
    public void ActiveEffect(string _Key, Transform _Transform,float _Duration)
    {
        GameObject obj = GetEffect(_Key);
        obj.transform.SetParent(_Transform);
        obj.transform.position = new Vector3(_Transform.position.x, _Transform.position.y - 1.6f, _Transform.position.z);
        StartCoroutine(DeleteEffect(_Key, _Duration, obj));
    }

    //이펙트 추가생성
    void CreatePooledObject(string _Key)
    {
        GameObject effect = null;
        switch (_Key)
        {
            case "HealEff":
                effect = Instantiate(healPrefab, Vector3.zero, Quaternion.identity, transform);
                effect.transform.SetParent(gameObject.transform.GetChild(0));
                effect.SetActive(false);
                healEffect.Enqueue(effect);
                break;

            case "EvadeEff":
                effect = Instantiate(evadePrefab, Vector3.zero, Quaternion.identity, transform);
                effect.transform.SetParent(gameObject.transform.GetChild(1));
                effect.SetActive(false);
                evadeEffect.Enqueue(effect);
                break;

            case "ShieldEff":
                effect = Instantiate(shieldPrefab, Vector3.zero, Quaternion.identity, transform);
                effect.transform.SetParent(gameObject.transform.GetChild(2));
                effect.SetActive(false);
                shieldEffect.Enqueue(effect);
                break;

            case "HealthEff":
                effect = Instantiate(healthPrefab, Vector3.zero, Quaternion.identity, transform);
                effect.transform.SetParent(gameObject.transform.GetChild(3));
                effect.SetActive(false);
                healthEffect.Enqueue(effect);
                break;

            case "DebuffEff":
                effect = Instantiate(debuffPrefab, Vector3.zero, Quaternion.identity, transform);
                effect.transform.SetParent(gameObject.transform.GetChild(4));
                effect.SetActive(false);
                debuffeffect.Enqueue(effect);
                break;


            case "DefenseEff":
                effect = Instantiate(defensePrefab, Vector3.zero, Quaternion.identity, transform);
                effect.transform.SetParent(gameObject.transform.GetChild(5));
                effect.SetActive(false);
                defenseEffect.Enqueue(effect);
                break;


            case "SpeedEff":
                effect = Instantiate(speedPrefab, Vector3.zero, Quaternion.identity, transform);
                effect.transform.SetParent(gameObject.transform.GetChild(6));
                effect.SetActive(false);
                speedEffect.Enqueue(effect);
                break;

            case "ImmortalEff":
                effect = Instantiate(immortalPrefab, Vector3.zero, Quaternion.identity, transform);
                effect.transform.SetParent(gameObject.transform.GetChild(7));
                effect.SetActive(false);
                immortalEffect.Enqueue(effect);
                break;
        }
    }
    //이펙트의 지속시간뒤에 이펙트를 끈다.
    public IEnumerator DeleteEffect(string _Key, float _EffectTimer, GameObject _Effect)
    {
        yield return new WaitForSeconds(_EffectTimer);
        InsertEffect(_Key, _Effect);
    }

    //큐에 이펙트를 넣는다
    public void InsertEffect(string key, GameObject _obj)
    {
        effectPoolDic[key].Enqueue(_obj);
        switch (key)
        {
            case "HealEff":
                _obj.transform.SetParent(gameObject.transform.GetChild(0));
                break;

            case "EvadeEff":
                _obj.transform.SetParent(gameObject.transform.GetChild(1));
                break;

            case "ShieldEff":
                _obj.transform.SetParent(gameObject.transform.GetChild(2));
                break;

            case "HealthEff":
                _obj.transform.SetParent(gameObject.transform.GetChild(3));
                break;

            case "DebuffEff":
                _obj.transform.SetParent(gameObject.transform.GetChild(4));
                break;

            case "DefenseEff":
                _obj.transform.SetParent(gameObject.transform.GetChild(5));
                break;

            case "SpeedEff":
                _obj.transform.SetParent(gameObject.transform.GetChild(6));
                break;
        }
        _obj.SetActive(false);
    }
    //큐에서 이펙트를 꺼냄
    GameObject GetEffect(string key)
    {
        if (effectPoolDic[key].Count <= 0)
        {
            CreatePooledObject(key);
        }

        GameObject t_object = effectPoolDic[key].Dequeue();
        t_object.SetActive(true);
        return t_object;
    }

}
