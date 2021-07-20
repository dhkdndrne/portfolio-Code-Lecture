using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Transform target;
    public float speed;
    public int damage;
    public float criticalRate;
    float bindFactor; //속박확률
    TowerBase parent;
    bool isExplosion;
    bool inevitable;        //피할수 없는지

    Element elementType;

    [SerializeField] GameObject bulletParticle;
    [SerializeField] ParticleSystem explosionParticle;


    public delegate void BulletEffect();
    public BulletEffect bulletEffctCallback;

    public void Seek(Transform _Target,int _Damage,float _CriticalRate,float _BindFactor)
    {
        target = _Target;
        damage = _Damage;
        criticalRate = _CriticalRate;
        bindFactor = _BindFactor;
    }
    public void Init(TowerBase _Tower)
    {       
        speed = _Tower.bulletSpeed;        
        elementType = _Tower.elementType;
        inevitable = _Tower.inevitable;
        parent = _Tower;
        
    }
    private void Awake()
    {
        bulletParticle = transform.GetChild(0).gameObject;
        explosionParticle = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        if (!bulletParticle.activeSelf)
            bulletParticle.SetActive(true);

        explosionParticle.gameObject.SetActive(false);
        isExplosion = false;
    }

    private void FixedUpdate()
    {
        if (!isExplosion)
        {
            Vector3 dir = target.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }
    }
    void HitTarget()
    {
        DamageToTarget(target);      
        PlayerParitcleCor();
    }
    public void PlayerParitcleCor()
    {
        bulletParticle.SetActive(false);
        isExplosion = true;
        explosionParticle.gameObject.SetActive(true);
        StartCoroutine(PlayExplosionParticle());
    }

    IEnumerator PlayExplosionParticle()
    {
        yield return new WaitForSeconds(explosionParticle.main.duration);
        target = null;
        gameObject.SetActive(false);
    }
    void DamageToTarget(Transform _Unit)
    {
        UnitAbillity targetUnit = _Unit.GetComponent<UnitAbillity>();
        bool isMiss = false;
        if (targetUnit != null)
        {
            if((parent.towerState & TowerState.EMP) != 0)
            {
                int heal = targetUnit.MaxHp * parent.EmpHpFactor / 100;
                targetUnit.Hp += heal;
                PoolingManager.Instance.damagePopUpManager.ShowDamagePopUp(target.transform, heal, PopUpType.HEAL);
                return;
            }

            //유닛 속박
            if(!targetUnit.isBind && Random.Range(0, 100f) <= bindFactor && (parent.towerState & TowerState.BREAKDOWN) == 0)
            {
                targetUnit.AddDebuff(new BindDebuff(targetUnit));

                if (GameManager.Instance.tutorialManager != null && !GameManager.Instance.tutorialManager.check)
                {
                    GameManager.Instance.tutorialManager.check = true;
                }                
                   
            }

            //회피
            if (!inevitable)
            {
                //뭉게뭉게 스킬이 적중되지 않았으면 명중률은 100%
                if ((parent.towerState & TowerState.SMOKE) == 0) parent.HitRate = 100;

                //회피에 성공하거나 명중률로인한 회피 둘중 하나일때 들어옴
                if (Random.Range(0, 100f) <= targetUnit.Evade || 100 - parent.HitRate >= Random.Range(1, 100f))
                {
                    PoolingManager.Instance.damagePopUpManager.ShowDamagePopUp(target, "miss", PopUpType.MISS);
                    isMiss = true;
                }                        
            }

            if(!isMiss)
            {
                float finalDamage;
                bool isCritical = false;

                // 크리티컬 인지 확인
                if (Random.Range(0, 100f) <= criticalRate)
                {
                    finalDamage = damage * parent.criticalDamageFactor;
                    isCritical = true;
                }
                else finalDamage = damage;

                targetUnit.Hit(finalDamage, elementType, isCritical);
            }        
          
            //유닛의 디버프 무시 확률보다 크거나 기능고장 상태가 아니면 디버프 적용
            if (!targetUnit.IsDead &&Random.Range(0, 100) >= targetUnit.EgnorDebuffFactor && (parent.towerState & TowerState.BREAKDOWN) == 0)
            {
                ApplyDebuff(targetUnit,isMiss);
            }           
        }

        if (bulletEffctCallback != null)
            bulletEffctCallback.Invoke();
    }

    void ApplyDebuff(UnitAbillity _Target,bool _IsMiss)
    {
        if(_IsMiss && elementType.Equals(Element.LIGHITNING))
        {
            _Target.AddDebuff(parent.GetTowerDebuff());
            return;
        }

        if(parent.GetTowerDebuff() != null)
        _Target.AddDebuff(parent.GetTowerDebuff());
    }
}


