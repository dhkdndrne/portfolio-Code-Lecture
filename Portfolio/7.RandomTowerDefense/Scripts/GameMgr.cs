using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WaypointsFree;
using TMPro;
public class GameMgr : MonoBehaviour
{
    public GameObject[] enemyBody = new GameObject[43];

    GameObject enemyList;
    GameObject wayPoint;
    //라운드 시간
    public float time;
    //라운드마다 적 숫자
    int enemyCount;
    //퀘스트이미지 텍스트
    public Image[] skillFilter = new Image[3];
    public TextMeshProUGUI[] timerTxt = new TextMeshProUGUI[3];


    void Start()
    {
        FindQuestTimeTxt();
        SetEnemyPrefabs();
        time = 15;
        wayPoint = GameObject.Find("Map").GetComponent<Transform>().GetChild(77).GetComponent<Transform>().gameObject;
        enemyList = GetComponent<Transform>().GetChild(0).GetComponent<Transform>().gameObject;
        GameDB.Instance.IsStartRound = true;

        if (!GameDB.Instance.isQuest1)
        {
            StartCoroutine("Quest_1_CoolTime");
            timerTxt[0].enabled = true;

        }

        if (!GameDB.Instance.isQuest2)
        {
            StartCoroutine("Quest_2_CoolTime");
            timerTxt[1].enabled = true;

        }

        if (!GameDB.Instance.isQuest3)
        {
            StartCoroutine("Quest_3_CoolTime");
            timerTxt[2].enabled = true;

        }

        StartCoroutine("RoundStart");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            GameDB.Instance.Gold += 1000;
        }
    }

    public void FindQuestTimeTxt()
    {
        for (int i = 0; i < skillFilter.Length; i++)
        {
            //skillFilter[i] = GameObject.Find("Mission" + (i + 1).ToString()).GetComponent<Transform>().GetChild(1).GetComponent<Image>();
            //timerTxt[i] = GameObject.Find("Mission" + (i + 1).ToString()).GetComponent<Transform>().GetChild(2).GetComponent<TextMeshProUGUI>();
            timerTxt[i].enabled = false;
            skillFilter[i].fillAmount = 0;
        }
    }
    //적 프리펩 넣어주는 함수
    void SetEnemyPrefabs()
    {
        for (int i = 0; i < enemyBody.Length; i++)
        {
            string idx = "Enemy" + (i + 1).ToString();
            enemyBody[i] = Resources.Load("Prefabs/Enemy/" + idx) as GameObject;
        }
        enemyBody[40] = Resources.Load("Prefabs/Enemy/Mission1_Enemy") as GameObject;
        enemyBody[41] = Resources.Load("Prefabs/Enemy/Mission2_Enemy") as GameObject;
        enemyBody[42] = Resources.Load("Prefabs/Enemy/Mission3_Enemy") as GameObject;
    }

    //적생성 코루틴
    IEnumerator SpawnEnemy()
    {
        //몬스터 객체 iD구분용
        int[] idNum = new int[GameDB.Instance.rounds[GameDB.Instance.Round - 1].monsterId.Length];
        for (int i = 0; i < idNum.Length; i++)
        {
            idNum[i] = GameDB.Instance.rounds[GameDB.Instance.Round - 1].monsterId[i];
        }

        while (GameDB.Instance.rounds[GameDB.Instance.Round - 1].roundEnemyCnt != enemyCount)
        {
            yield return new WaitForSeconds(1.0f);
            CreateEnemy(GameDB.Instance.Round, enemyCount, idNum[enemyCount] - 1);
        }

        StopCoroutine("SpawnEnemy");
        enemyCount = 0;
        time = 15;

        StartCoroutine("FindEnemy");

        yield break;
    }
    //저장
    public void Save()
    {
        DataController.Instance.SaveGameData();
    }
    //메뉴로가기
    public void GotoTitleScene()
    {
        GameDB.Instance.ResetData();
        SceneManager.LoadScene("Title");
    }
    //적 생성
    void CreateEnemy(int _round, int _enemyCount, int _id)
    {
        GameDB.Instance.rounds[_round - 1].enemiesBody[_enemyCount].enemyPrefabs = Instantiate(enemyBody[_id], enemyList.transform);
        //wayPoint 설정
        GameDB.Instance.rounds[_round - 1].enemiesBody[_enemyCount].enemyPrefabs.GetComponent<WaypointsTraveler>().SetWaypointsGroup(wayPoint.GetComponent<WaypointsGroup>());
        //이동속도
        GameDB.Instance.rounds[_round - 1].enemiesBody[_enemyCount].enemyPrefabs.GetComponent<WaypointsTraveler>().MoveSpeed = GameDB.Instance.enemyDB[_id].moveSpeed;
        GameDB.Instance.rounds[_round - 1].enemiesBody[_enemyCount].moveSpeed = GameDB.Instance.enemyDB[_id].moveSpeed;
        //최대체력
        GameDB.Instance.rounds[_round - 1].enemiesBody[_enemyCount].enemyPrefabs.GetComponent<EnemyCtrl>().maxHp = GameDB.Instance.enemyDB[_id].hp;
        GameDB.Instance.rounds[_round - 1].enemiesBody[_enemyCount].hp = GameDB.Instance.enemyDB[_id].hp;
        //현재체력
        GameDB.Instance.rounds[_round - 1].enemiesBody[_enemyCount].enemyPrefabs.GetComponent<EnemyCtrl>().hp = GameDB.Instance.enemyDB[_id].hp;
        //아이디 
        GameDB.Instance.rounds[_round - 1].enemiesBody[_enemyCount].enemyPrefabs.GetComponent<EnemyCtrl>().id = GameDB.Instance.enemyDB[_id].id;
        GameDB.Instance.rounds[_round - 1].enemiesBody[_enemyCount].id = GameDB.Instance.enemyDB[_id].id;
        //이름  
        GameDB.Instance.rounds[_round - 1].enemiesBody[_enemyCount].name = GameDB.Instance.enemyDB[_id].name;
        GameDB.Instance.rounds[_round - 1].enemiesBody[_enemyCount].enemyPrefabs.GetComponent<EnemyCtrl>().name = GameDB.Instance.enemyDB[_id].name;
        //인스펙터에 보이는이름
        GameDB.Instance.rounds[_round - 1].enemiesBody[_enemyCount].enemyPrefabs.name = "적 " + _round + "-" + enemyCount;

        enemyCount++;

    }
    //퀘스트 적
    void CreateQuestEnemy(int _id)
    {
        GameObject questEnemy = Instantiate(enemyBody[_id], enemyList.transform);
        questEnemy.GetComponent<EnemyCtrl>().IsQuestEnemy = true;
        //wayPoint 설정
        questEnemy.GetComponent<WaypointsTraveler>().SetWaypointsGroup(wayPoint.GetComponent<WaypointsGroup>());
        questEnemy.GetComponent<WaypointsTraveler>().MoveSpeed = GameDB.Instance.enemyDB[_id].moveSpeed;
        //최대체력
        questEnemy.GetComponent<EnemyCtrl>().maxHp = GameDB.Instance.enemyDB[_id].hp;
        //현재체력
        questEnemy.GetComponent<EnemyCtrl>().hp = GameDB.Instance.enemyDB[_id].hp;
        //아이디 
        questEnemy.GetComponent<EnemyCtrl>().id = GameDB.Instance.enemyDB[_id].id;
        //이름       
        questEnemy.GetComponent<EnemyCtrl>().name = GameDB.Instance.enemyDB[_id].name;
        //인스펙터에 보이는이름
        questEnemy.name = questEnemy.GetComponent<EnemyCtrl>().name;
    }
    //라운드 카운트
    IEnumerator RoundStart()
    {
        while (time > 0 && GameDB.Instance.IsStartRound)
        {
            time -= (1 * Time.smoothDeltaTime);
            yield return null;
        }
        GameDB.Instance.IsStartRound = false;
        StartCoroutine("SpawnEnemy");
        yield break;
    }
    //살아있는적 있는지
    IEnumerator FindEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        while (enemies.Length > 0)
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            yield return new WaitForSeconds(1.0f);
        }

        if(GameDB.Instance.Round != 40)
        {
            GameDB.Instance.Round++;
            GameDB.Instance.IsStartRound = true;

            if (GameDB.Instance.Level == "EASY")
            {
                GameDB.Instance.Gold += 300;
            }
            else if (GameDB.Instance.Level == "NORMAL")
            {
                if (GameDB.Instance.Round <= 20)
                {
                    GameDB.Instance.Gold += 300;
                }
                else
                {
                    GameDB.Instance.Gold += 200;
                }
            }
            else
            {
                GameDB.Instance.Gold += 200;
            }
            StartCoroutine("RoundStart");
        }
        else
        {
            SceneManager.LoadScene("EndingScene");
        }
        yield break;
    }
    //퀘스트
    public void StartQuest_1()
    {
        if (GameDB.Instance.isQuest1)
        {
            GameDB.Instance.isQuest1 = false;

            timerTxt[0].enabled = true;
            CreateQuestEnemy(40);
            GameDB.Instance.Qmon1Alive = true;

            GameDB.Instance.tik[0] = 0;
            GameDB.Instance.tik[0] = GameDB.Instance.questTimer1;
            GameDB.Instance.questCoolTime1 = 1;

            while (GameDB.Instance.tik[0] >= 60)
            {
                GameDB.Instance.min[0]++;
                GameDB.Instance.tik[0] -= 60;
            }

            StartCoroutine("Quest_1_CoolTime");
        }
    }
    public void StartQuest_2()
    {
        if (GameDB.Instance.isQuest2)
        {
            GameDB.Instance.isQuest2 = false;

            timerTxt[1].enabled = true;
            CreateQuestEnemy(41);
            GameDB.Instance.Qmon2Alive = true;

            GameDB.Instance.tik[1] = 0f;
            GameDB.Instance.tik[1] = GameDB.Instance.questTimer2;
            GameDB.Instance.questCoolTime2 = 1;

            while (GameDB.Instance.tik[1] >= 60)
            {
                GameDB.Instance.min[1]++;
                GameDB.Instance.tik[1] -= 60;
            }

            StartCoroutine("Quest_2_CoolTime");
        }
    }
    public void StartQuest_3()
    {
        if (GameDB.Instance.isQuest3)
        {
            GameDB.Instance.isQuest3 = false;

            timerTxt[2].enabled = true;
            CreateQuestEnemy(42);
            GameDB.Instance.Qmon3Alive = true;

            GameDB.Instance.tik[2] = 0f;
            GameDB.Instance.tik[2] = GameDB.Instance.questTimer3;
            GameDB.Instance.questCoolTime3 = 1;

            while (GameDB.Instance.tik[2] >= 60)
            {
                GameDB.Instance.min[2]++;
                GameDB.Instance.tik[2] -= 60;
            }

            StartCoroutine("Quest_3_CoolTime");
        }
    }
    IEnumerator Quest_1_CoolTime()
    {

        skillFilter[0].fillAmount = GameDB.Instance.questCoolTime1;

        while (skillFilter[0].fillAmount > 0)
        {
            skillFilter[0].fillAmount -= 1 * Time.smoothDeltaTime / GameDB.Instance.questTimer1;
            if (GameDB.Instance.tik[0] > 0)
            {
                GameDB.Instance.tik[0] -= 1 * Time.smoothDeltaTime;
            }
            else
            {
                if (GameDB.Instance.min[0] > 0)
                {
                    GameDB.Instance.min[0]--;
                }

                GameDB.Instance.tik[0] = 60;
            }
            timerTxt[0].text = string.Format("{0:D2} : {1:D2}", GameDB.Instance.min[0], (int)GameDB.Instance.tik[0]);
            GameDB.Instance.questCoolTime1 = skillFilter[0].fillAmount;
            yield return null;
        }
        GameDB.Instance.isQuest1 = true;
        timerTxt[0].enabled = false;
        yield break;
    }
    IEnumerator Quest_2_CoolTime()
    {
        skillFilter[1].fillAmount = GameDB.Instance.questCoolTime2;

        while (skillFilter[1].fillAmount > 0)
        {
            skillFilter[1].fillAmount -= 1 * Time.smoothDeltaTime / GameDB.Instance.questTimer2;
            if (GameDB.Instance.tik[1] > 0)
            {
                GameDB.Instance.tik[1] -= 1 * Time.smoothDeltaTime;
            }
            else
            {
                if (GameDB.Instance.min[1] > 0)
                {
                    GameDB.Instance.min[1]--;
                }
                GameDB.Instance.tik[1] = 60;
            }
            timerTxt[1].text = string.Format("{0:D2} : {1:D2}", GameDB.Instance.min[1], (int)GameDB.Instance.tik[1]);
            GameDB.Instance.questCoolTime2 = skillFilter[1].fillAmount;
            yield return null;
        }
        GameDB.Instance.isQuest2 = true;
        timerTxt[1].enabled = false;
        yield break;
    }
    IEnumerator Quest_3_CoolTime()
    {
        skillFilter[2].fillAmount = GameDB.Instance.questCoolTime3;
        //1
        while (skillFilter[2].fillAmount > 0)
        {
            skillFilter[2].fillAmount -= 1 * Time.smoothDeltaTime / GameDB.Instance.questTimer3;
            if (GameDB.Instance.tik[2] > 0)
            {
                GameDB.Instance.tik[2] -= 1 * Time.smoothDeltaTime;
            }
            else
            {
                if (GameDB.Instance.min[2] > 0)
                {
                    GameDB.Instance.min[2]--;
                }
                GameDB.Instance.tik[2] = 60;
            }
            timerTxt[2].text = string.Format("{0:D2} : {1:D2}", GameDB.Instance.min[2], (int)GameDB.Instance.tik[2]);
            GameDB.Instance.questCoolTime3 = skillFilter[2].fillAmount;
            yield return null;
        }
        GameDB.Instance.isQuest3 = true;
        timerTxt[2].enabled = false;
        yield break;
    }

}
