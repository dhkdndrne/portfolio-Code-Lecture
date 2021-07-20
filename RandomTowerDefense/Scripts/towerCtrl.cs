using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    SINGLE,
    SPLASH,
    MULTI,
    MULTISPLASH
}
public enum RareList
{
    NORMAL,
    MAGIC,
    RARE,
    UNIQUE,
    EPIC
}

public class towerCtrl : MonoBehaviour
{
    //밟고있는타일
    public Tile onTile;
    //애니메이션
    public Animator anim;
    //타워 아이디
    public int id;
    //선택됐을때 파티클
    public GameObject[] particle = new GameObject[4];
    //공격이펙트
    [Header("딕셔너리 저장된 이펙트이름")]
    public string skillName = null;
    string enemyTag = "Enemy";
    //콜라이더
    public SphereCollider col;
    //타겟
    public Transform[] target = null;

    //공격타입
    public AttackType attackType;
    //레어도
    public RareList rareList;
    //이름
    public string towerName;
    //팔때 가격
    public int price;
    //공속
    public float attackSpeed;
    //데미지
    float damage;
    //공격범위
    public float attackRange;
    //공격 인원수
    public int attackCount;
    //이펙트시간
    public float effectTimer;
    //이펙트없는 타워 공격소리용
    new AudioCtrl audio;
    void Start()
    {           
        GameObject game = this.GetComponent<Transform>().Find("magic_ring_01").GetComponent<Transform>().gameObject;
        particle[0] = game.GetComponent<Transform>().GetChild(0).gameObject;
        particle[1] = game.GetComponent<Transform>().GetChild(1).gameObject;
        particle[2] = game.GetComponent<Transform>().GetChild(2).gameObject;
        particle[3] = game.GetComponent<Transform>().GetChild(3).gameObject;
        OnOffParticle(false);
        anim = GetComponent<Animator>();

        target = new Transform[attackCount];

        
        audio = GameObject.Find("SoundMgr").GetComponent<AudioCtrl>();
        
        if (attackType != AttackType.MULTI && attackType != AttackType.MULTISPLASH)
        {
            InvokeRepeating("UpdateTarget", 0f, 0.5f);
        }
        else
        {
            col = GetComponent<SphereCollider>();
            col.radius = attackRange;
            InvokeRepeating("CheckIsTarget", 0f, 0.5f);
        }

    }
    public void Hit()
    {
        if (skillName != "null")
        {
            switch (attackType)
            {

                case AttackType.MULTI:
                    for (int i = 0; i < target.Length; i++)
                    {
                        if (target[i])
                        {

                            GameObject eff = EffectPool.instance.GetQueue(skillName);
                            eff.transform.position = new Vector3(target[i].transform.position.x, target[i].transform.position.y + 1f, target[i].transform.position.z);
                            eff.GetComponent<AudioSource>().Play();
                            StartCoroutine(EffectPool.instance.DeleteEffect(skillName, effectTimer, eff));
                            target[i].GetComponent<EnemyCtrl>().GetDamage(damage);
                        }

                    }
                    break;

                case AttackType.SINGLE:

                    if (target[0] != null)
                    {
                        GameObject eff = EffectPool.instance.GetQueue(skillName);
                        eff.transform.position = new Vector3(target[0].transform.position.x, target[0].transform.position.y + 1f, target[0].transform.position.z);
                        eff.GetComponent<AudioSource>().Play();
                        StartCoroutine(EffectPool.instance.DeleteEffect(skillName, effectTimer, eff));
                        target[0].GetComponent<EnemyCtrl>().GetDamage(damage);
                    }
                    break;

                case AttackType.SPLASH:
                    if (target[0] != null)
                    {                        
                        GameObject splashEff = EffectPool.instance.GetQueue(skillName);
                        splashEff.transform.position = new Vector3(target[0].transform.position.x, target[0].transform.position.y + 1f, target[0].transform.position.z);
                        splashEff.GetComponent<Splash>().damage = damage;
                        splashEff.GetComponent<AudioSource>().Play();
                        StartCoroutine(EffectPool.instance.DeleteEffect(skillName, effectTimer, splashEff)); 
                    }
                    break;
                case AttackType.MULTISPLASH:
                    for (int i = 0; i < target.Length; i++)
                    {
                        if (target[i] != null)
                        {
                            //GameObject splashEff = Instantiate(attackEffect, new Vector3(target[i].transform.position.x, target[i].transform.position.y + 1f, target[i].transform.position.z), Quaternion.identity);
                            GameObject splashEff = EffectPool.instance.GetQueue(skillName);
                            splashEff.transform.position = new Vector3(target[i].transform.position.x, target[i].transform.position.y + 1f, target[i].transform.position.z);
                           splashEff.GetComponent<Splash>().damage = damage;
                            splashEff.GetComponent<AudioSource>().Play();
                            StartCoroutine(EffectPool.instance.DeleteEffect(skillName, effectTimer, splashEff));
                        }
                    }
                    break;
            }
        }
        //이펙트 없을때
        else
        {
            audio.PlayAudio(id);
            if (attackType != AttackType.MULTI)
            {
                if (target[0] != null)
                {
                    target[0].GetComponent<EnemyCtrl>().GetDamage(damage);
                }
            }
            else
            {
                for (int i = 0; i < target.Length; i++)
                {
                    if (target[i])
                    {
                        target[i].GetComponent<EnemyCtrl>().GetDamage(damage);
                    }
                }
            }

        }
    }
    public void Attack(bool _isAttack)
    {
        if (target[0])
        {
            Vector3 targetPos = new Vector3(target[0].position.x, transform.position.y, target[0].position.z);
            transform.LookAt(targetPos);
        }
        anim.SetBool("Attack", _isAttack);
        anim.SetFloat("AttackSpeed", attackSpeed);
    }
    public void Idle(bool _isIdle)
    {
        anim.SetBool("Idle", _isIdle);
    }
    //공격대상 설정
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject shortestTarget = null;

        float shortestDistance = Mathf.Infinity;

        if (enemies.Length > 0)
        {
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (shortestDistance > distance)
                {
                    shortestDistance = distance;
                    shortestTarget = enemy;
                }
            }

            if (shortestTarget != null && shortestDistance <= attackRange)
            {
                target[0] = shortestTarget.transform;
                //공격모션
                Idle(false);
                Attack(true);
            }
            else
            {
                target[0] = null;
                //공격모션
                Attack(false);
                Idle(true);
            }
        }
        else
        {
            target[0] = null;
            Attack(false);
            Idle(true);
            //아이들 모션
        }
    }

    //멀티타워
    private void OnTriggerEnter(Collider other)
    {
        if (attackType == AttackType.MULTI || attackType == AttackType.MULTISPLASH)
        {
            if (other.tag == "Enemy")
            {
                for (int i = 0; i < target.Length; i++)
                {
                    if (target[i] == null)
                    {
                        target[i] = other.transform;
                        break;
                    }

                }
                Attack(true);
            }

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (attackType == AttackType.MULTI || attackType == AttackType.MULTISPLASH)
        {
            if (other.tag == "Enemy")
            {
                Attack(true);
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (attackType == AttackType.MULTI || attackType == AttackType.MULTISPLASH)
        {
            if (other.tag == "Enemy")
            {
                for (int i = 0; i < target.Length; i++)
                {
                    if (target[i] == other.transform)
                    {
                        target[i] = null;
                        break;
                    }

                }
                
            }
        }
    }
    private void CheckIsTarget()
    {
        int cnt = 0;
        for (int i = 0; i < attackCount; i++)
        {
            if (target[i] == null) cnt++;
            if (cnt == attackCount)
                Attack(false);
                Idle(true);
         
        }
    }
    //파티클 효과 켜기, 끄기
    public void OnOffParticle(bool _onOff)
    {
        if (_onOff)
        {
            for (int i = 0; i < 4; i++)
            {
                particle[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                particle[i].SetActive(false);
            }
        }

    }

    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

}
