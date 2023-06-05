using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using DarkTonic.MasterAudio;
[Flags]
public enum TowerState
{
    EMP = 1 << 0,           //0000 0001         1
    BREAKDOWN = 1 << 1,     //0000 0010         2
    THUNDERBOLT = 1 << 2,   //0000 0100         4
    FEATHER = 1 << 3,       //0000 1000         8
    SMOKE = 1 << 4,         //0001 0000         16
    ICEHORN = 1 << 5,       //0010 0000         32
    BOOM = 1 << 6,          //0100 0000         64
    CYCLONE = 1 << 7        //1000 0000         128
}
public enum Element
{
    NORMAL,
    ICE,
    POISON,
    LIGHITNING,
}

public enum TargetOption
{
    NEAR,       // 제일 가까운적
    HIGH_HP,    // 체력이 많은적
    LOW_HP,     // 체력이 적은적
    RANDOM      // 랜덤
}
public abstract class TowerBase : MonoBehaviour
{
    public delegate void CheckTowerState();
    public event CheckTowerState checkTowerStateCallBack;

    public Transform target;
    public UnitAbillity targetUnit;

    public Transform firePos;
    public GameObject bulletPrefab = null;


    [Header("TowerStat")]
    public int level;
    public string towerName;
    public int damage;
    public float bulletSpeed;
    public float range;
    public float criticalRate;
    public float criticalDamageFactor;
    public float fireDelay;
    public bool inevitable;         //회피불가
    public string description;
    public string debuffDescription;
    public Element elementType = Element.NORMAL;
    public TargetOption targetOption;
    float fireCountDown;
    string enemyTag = "Unit";
    [SerializeField] float unitbindFactor;  // 유닛멈출 확률
    public float UnitBindFactor
    {
        get
        {
            return unitbindFactor;
        }
        set
        {
            unitbindFactor = value;
        }
    }
    List<GameObject> towerBullets = new List<GameObject>(); // 총알 오브젝트 풀
    int bulletIndex;

    // 유닛에게 적용되는 디버프 관련

    [SerializeField]
    float debuffDuration;
    public float DebuffDuration
    {
        get { return debuffDuration; }
        set { debuffDuration = value; }
    }

    [SerializeField]
    float debuffApplyRate; // 디버프 적용 확률
    public float DebuffApplyRate
    {
        get { return debuffApplyRate; }
    }


    //타워에게 적용되는 디버프관련
    //-------------------------------------------------

    public TowerState towerState { get; private set; }
    List<Debuff> debuffs = new List<Debuff>();
    List<Debuff> debuffstoRemove = new List<Debuff>();
    List<Debuff> newDebuffs = new List<Debuff>();


    int empHpFactor;
    public int EmpHpFactor { get { return empHpFactor; } set { empHpFactor = value; } }

    float randomFireFactor;
    public float RandomFireFactor { get { return randomFireFactor; } set { randomFireFactor = value; } }

    [SerializeField] float hitRate;
    public float HitRate { get { return hitRate; } set { hitRate = value; } }
    //------------------------------------------------------

    public void ChangeTowerState(TowerState _State, bool _IsDelete = false)
    {
        if (_IsDelete)
        {
            towerState &= ~_State;
        }
        else
        {
            towerState |= _State;
        }

        checkTowerStateCallBack.Invoke();
    }
    public void SetTowerINfo(string _Info)
    {
        towerName = DBManager.Instance.towerDB.towerDB[_Info][level - 1].towerName;
        damage = DBManager.Instance.towerDB.towerDB[_Info][level - 1].damage;
        fireDelay = DBManager.Instance.towerDB.towerDB[_Info][level - 1].fireDelay;
        range = DBManager.Instance.towerDB.towerDB[_Info][level - 1].range;
        criticalRate = DBManager.Instance.towerDB.towerDB[_Info][level - 1].criRate;
        criticalDamageFactor = DBManager.Instance.towerDB.towerDB[_Info][level - 1].criDamage;
        bulletSpeed = DBManager.Instance.towerDB.towerDB[_Info][level - 1].bulletSpeed;
        elementType = DBManager.Instance.towerDB.towerDB[_Info][level - 1].elementType;
        inevitable = DBManager.Instance.towerDB.towerDB[_Info][level - 1].inevitable;
        debuffApplyRate = DBManager.Instance.towerDB.towerDB[_Info][level - 1].debuffApplyRate;
        debuffDuration = DBManager.Instance.towerDB.towerDB[_Info][level - 1].debuffDuration;
        targetOption = DBManager.Instance.towerDB.towerDB[_Info][level - 1].targetOption;
    }
    public virtual void Init()
    {
        unitbindFactor = 5f;
        firePos = transform.GetChild(0);
        MasterAudio.MasterVolumeLevel = 0;//총알 프리팹 생성될때 사운드가 들리므로 잠깐 꺼주기 위해

        //총알 오브젝트 풀링
        for (int i = 0; i < 10; i++)
        {
            towerBullets.Add(Instantiate(bulletPrefab, firePos));
            towerBullets[i].GetComponent<Bullet>().Init(this);
            towerBullets[i].SetActive(false);
        }

        StartCoroutine(ApplyDebuff());
        StartCoroutine(DelayCount());
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }
    protected virtual void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        if (enemies.Length > 0)
        {
            float distanceToEnemy = 0;
            switch (targetOption)
            {
                case TargetOption.NEAR:
                    float shortestDistance = Mathf.Infinity;
                    GameObject nearestEnemy = null;
                    foreach (GameObject enemy in enemies)
                    {
                        distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                        if (distanceToEnemy < shortestDistance)
                        {
                            shortestDistance = distanceToEnemy;
                            nearestEnemy = enemy;
                        }
                    }
                    if (nearestEnemy != null && shortestDistance <= range)
                    {
                        target = nearestEnemy.transform;
                        targetUnit = target.GetComponent<UnitAbillity>();
                    }
                    else
                    {
                        target = null;
                    }
                    break;

                case TargetOption.HIGH_HP:
                    float high_hp = 0;
                    GameObject high_hpEnemy = null;
                    foreach (GameObject enemy in enemies)
                    {
                        distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                        float tempHp = enemy.GetComponent<UnitAbillity>().Hp;
                        if (high_hp < tempHp && distanceToEnemy <= range)
                        {
                            high_hp = tempHp;
                            high_hpEnemy = enemy;
                        }
                    }
                    if (high_hpEnemy != null)
                    {
                        target = high_hpEnemy.transform;
                        targetUnit = target.GetComponent<UnitAbillity>();
                    }
                    else
                    {
                        target = null;
                    }
                    break;

                case TargetOption.LOW_HP:
                    float low_hp = 0;
                    GameObject low_hpEnemy = null;
                    foreach (GameObject enemy in enemies)
                    {
                        distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                        float tempHp = enemy.GetComponent<UnitAbillity>().Hp;
                        if (low_hp > tempHp)
                        {
                            low_hp = tempHp;
                            low_hpEnemy = enemy;
                        }
                    }
                    if (low_hpEnemy != null && distanceToEnemy <= range)
                    {
                        target = low_hpEnemy.transform;
                        targetUnit = target.GetComponent<UnitAbillity>();
                    }
                    else
                    {
                        target = null;
                    }
                    break;

                case TargetOption.RANDOM:
                    int randomIdx = UnityEngine.Random.Range(0, enemies.Length);
                    target = enemies[randomIdx].transform;
                    targetUnit = target.GetComponent<UnitAbillity>();
                    distanceToEnemy = Vector3.Distance(transform.position, target.position);

                    if (distanceToEnemy >= range)
                    {
                        target = null;
                    }
                    break;
            }
        }
        else
        {
            target = null;
            foreach(var bullet in towerBullets)
            {
                bullet.SetActive(false);
            }
        }
    }

    IEnumerator DelayCount()
    {
        while (true)
        {
            if (target)
            {
                if (fireCountDown <= 0f)
                {
                    Fire();
                    fireCountDown = fireDelay;
                }

                fireCountDown -= Time.deltaTime;
            }
            yield return null;
        }
    }

    protected virtual void Fire()
    {
        if ((towerState & TowerState.THUNDERBOLT) != 0)
        {
            return;
        }
        else if ((towerState & TowerState.FEATHER) != 0)
        {
            float random = UnityEngine.Random.Range(0, 100f);
            if (randomFireFactor < random)
            {
                return;
            }
        }

        if (towerBullets[bulletIndex].activeSelf == false)
        {
            towerBullets[bulletIndex].GetComponent<Bullet>().Seek(target, damage,criticalRate,UnitBindFactor);       // 총알 타겟 설정
            towerBullets[bulletIndex].transform.position = firePos.position;      // 총알 위치타워 위치로 설정
            towerBullets[bulletIndex].SetActive(true);

            if (bulletIndex < towerBullets.Count - 1) bulletIndex++;
            else bulletIndex = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    //타워의 디버프 가져오는 함수
    public abstract Debuff GetTowerDebuff();

    /// /////////////////////////

    IEnumerator ApplyDebuff()
    {
        yield return new WaitUntil(() => newDebuffs.Count > 0);
        while (debuffs.Count > 0 || newDebuffs.Count > 0)
        {
            HandleDebuff();
            yield return null;
        }
        StartCoroutine(ApplyDebuff());
    }
    //디버프 효과 적용하는 함수

    void HandleDebuff()
    {
        if (newDebuffs.Count > 0)
        {
            debuffs.AddRange(newDebuffs);
            newDebuffs.Clear();
        }

        foreach (Debuff debuff in debuffstoRemove)
        {
            debuffs.Remove(debuff);
        }

        debuffstoRemove.Clear();

        //모든 디버프 효과 적용
        foreach (Debuff debuff in debuffs)
        {
            debuff.Update();
        }
    }
    //디버프 추가하는 함수
    public void AddDebuff(Debuff _Debuff)
    {
        // 다른버프이면 버프 추가
        if (!debuffs.Exists(x => x.GetType() == _Debuff.GetType()))
            newDebuffs.Add(_Debuff);
        else
        {
            //동일한 디버프가 입력되면 elapsed(디버프 지속시간) 초기화
            Debuff temp = debuffs.Find(x => x.GetType() == _Debuff.GetType());

            if (temp.debuffType.Equals(Debuff.DebuffType.OVERLAP))
            {
                if (temp.overlap < temp.maxOverlap - 1)
                {
                    temp.overlap++;
                    temp.applied = false;
                }
                temp.Elapsed = 0;
            }
            else if (temp.debuffType.Equals(Debuff.DebuffType.RESET))
            {
                temp.Elapsed = 0;
            }
        }
    }
    public void RemoveDebuff(Debuff _Debuff)
    {
        debuffstoRemove.Add(_Debuff);
    }

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
}
