using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

[System.Serializable]
public struct Enemy
{
    public GameObject enemyPrefabs;
    public string name;
    public int id;
    public int hp;
    public float moveSpeed;
}
[System.Serializable]
public struct Round
{
    public Enemy[] enemiesBody;
    public int[] monsterId;
    public string level;
    public int roundEnemyCnt;
    public int rounds;

}
[System.Serializable]
public class TowerInfo_
{
    public int ID;
    public int TILENUMBER;
    public RareList RARELIST;

    public TowerInfo_(int _id, int _tileNumber, RareList _rareList)
    {
        ID = _id;
        TILENUMBER = _tileNumber;
        RARELIST = _rareList;
    }
}

public class GameDB : MonoBehaviour
{
    #region Singleton
    private static GameDB instance = null;
    public static GameDB Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (GameDB)FindObjectOfType(typeof(GameDB));

                if (instance == null)
                {
                    Debug.Log("매니저 클래스 오브젝트가 없다");
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion

    [HideInInspector] public Enemy[] enemyDB;
    [HideInInspector] public Round[] roundDB;
    [Header("라운드 정보")]
    [SerializeField] public Round[] rounds;

    //타워 종류
    [HideInInspector] public GameObject[] normalTower = new GameObject[5];
    [HideInInspector] public GameObject[] magicTower = new GameObject[5];
    [HideInInspector] public GameObject[] rareTower = new GameObject[5];
    [HideInInspector] public GameObject[] uniqueTower = new GameObject[5];
    [HideInInspector] public GameObject[] epicTower = new GameObject[5];

    //타워 25종류 30렙까지 공격력 배열
    [HideInInspector] public float[,] towerAttackCost = new float[25, 30];


    //타일위에 건설된 타워목록
    List<towerCtrl> normalTowerList = new List<towerCtrl>();
    List<towerCtrl> magicTowerList = new List<towerCtrl>();
    List<towerCtrl> rareTowerList = new List<towerCtrl>();
    List<towerCtrl> uniqueTowerList = new List<towerCtrl>();
    List<towerCtrl> epicTowerList = new List<towerCtrl>();
    public List<TowerInfo_> loadInfo = new List<TowerInfo_>();
    //타워 목록들 관리할 딕셔너리
    [SerializeField] public Dictionary<RareList, List<towerCtrl>> t_dictionary = new Dictionary<RareList, List<towerCtrl>>();

    //로드용 타일
    Tile[] tempTile;
    //이어하기인지
    bool isContinue;
    //난이도
    string level;
    int round;
    int gold;
    int hp;
    int killCount;
    bool isStartRound;
    //미션용 타워 번호들
    public int[] towerId_Count = new int[25];
    //타워레벨
    [HideInInspector] public int normalTowerLV = 1;
    [HideInInspector] public int magicTowerLV = 1;
    [HideInInspector] public int rareTowerLV = 1;
    [HideInInspector] public int uniqueTowerLV = 1;
    [HideInInspector] public int epicTowerLV = 1;
    //업그레이드 비용
     public float normalUpgradeCost;
     public float magicUpgradeCost;
     public float rareUpgradeCost;
     public float uniqueUpgradeCost;
     public float epicUpgradeCost;

    //퀘스트
    [HideInInspector] public bool isQuest1 = true;
    [HideInInspector] public bool isQuest2 = true;
    [HideInInspector] public bool isQuest3 = true;

    //퀘스트 타이머 분
    [HideInInspector] public int[] min = new int[3];
    //초
    [HideInInspector] public float[] tik = new float[3];
    [HideInInspector] public float questTimer1 = 150;
    [HideInInspector] public float questTimer2 = 300;
    [HideInInspector] public float questTimer3 = 360;

    //퀘스트 세이브 로드용
    [HideInInspector] public bool Qmon1Alive = false;
    [HideInInspector] public bool Qmon2Alive = false;
    [HideInInspector] public bool Qmon3Alive = false;
    //쿨타임 이미지용
    [HideInInspector] public float questCoolTime1 = 0f;
    [HideInInspector] public float questCoolTime2 = 0f;
    [HideInInspector] public float questCoolTime3 = 0f;
    //타워 프리팹 설정
    void InsertTowerPrefabs()
    {
        normalTower[0] = Resources.Load("Prefabs/Tower/Normal-01") as GameObject;
        normalTower[1] = Resources.Load("Prefabs/Tower/Normal-02") as GameObject;
        normalTower[2] = Resources.Load("Prefabs/Tower/Normal-03") as GameObject;
        normalTower[3] = Resources.Load("Prefabs/Tower/Normal-04") as GameObject;
        normalTower[4] = Resources.Load("Prefabs/Tower/Normal-05") as GameObject;

        magicTower[0] = Resources.Load("Prefabs/Tower/Magic-01") as GameObject;
        magicTower[1] = Resources.Load("Prefabs/Tower/Magic-02") as GameObject;
        magicTower[2] = Resources.Load("Prefabs/Tower/Magic-03") as GameObject;
        magicTower[3] = Resources.Load("Prefabs/Tower/Magic-04") as GameObject;
        magicTower[4] = Resources.Load("Prefabs/Tower/Magic-05") as GameObject;

        rareTower[0] = Resources.Load("Prefabs/Tower/Rare-01") as GameObject;
        rareTower[1] = Resources.Load("Prefabs/Tower/Rare-02") as GameObject;
        rareTower[2] = Resources.Load("Prefabs/Tower/Rare-03") as GameObject;
        rareTower[3] = Resources.Load("Prefabs/Tower/Rare-04") as GameObject;
        rareTower[4] = Resources.Load("Prefabs/Tower/Rare-05") as GameObject;

        uniqueTower[0] = Resources.Load("Prefabs/Tower/Unique-01") as GameObject;
        uniqueTower[1] = Resources.Load("Prefabs/Tower/Unique-02") as GameObject;
        uniqueTower[2] = Resources.Load("Prefabs/Tower/Unique-03") as GameObject;
        uniqueTower[3] = Resources.Load("Prefabs/Tower/Unique-04") as GameObject;
        uniqueTower[4] = Resources.Load("Prefabs/Tower/Unique-05") as GameObject;

        epicTower[0] = Resources.Load("Prefabs/Tower/Epic-01") as GameObject;
        epicTower[1] = Resources.Load("Prefabs/Tower/Epic-02") as GameObject;
        epicTower[2] = Resources.Load("Prefabs/Tower/Epic-03") as GameObject;
        epicTower[3] = Resources.Load("Prefabs/Tower/Epic-04") as GameObject;
        epicTower[4] = Resources.Load("Prefabs/Tower/Epic-05") as GameObject;
    }
    void Start()
    {
        InsertTowerPrefabs();
        List<Dictionary<string, object>> monData = CSVReader.Read("MonsterDB_");
        List<Dictionary<string, object>> roundData = CSVReader.Read("RoundDB");
        List<Dictionary<string, object>> levelDamageData = CSVReader.Read("LevelDamageDB");

        t_dictionary.Add(RareList.NORMAL, normalTowerList);
        t_dictionary.Add(RareList.MAGIC, magicTowerList);
        t_dictionary.Add(RareList.RARE, rareTowerList);
        t_dictionary.Add(RareList.UNIQUE, uniqueTowerList);
        t_dictionary.Add(RareList.EPIC, epicTowerList);

        //db크기는 몬스터db.csv의 크기만큼
        enemyDB = new Enemy[monData.Count];
        //모든난이도 라운드DB
        roundDB = new Round[roundData.Count];
        //선택한 난이도 라운드
        rounds = new Round[roundData.Count / 3];

        //몬스터 정보 db 받아옴
        for (int i = 0; i < monData.Count; i++)
        {
            enemyDB[i].name = (string)monData[i]["NAME"];
            enemyDB[i].id = (int)monData[i]["ID"];
            enemyDB[i].hp = (int)monData[i]["HP"];
            enemyDB[i].moveSpeed = System.Convert.ToSingle(monData[i]["SPEED"]);
        }

        for (int i = 0; i < roundData.Count; i++)
        {
            roundDB[i].level = (string)roundData[i]["LEVEL"];
            roundDB[i].rounds = (int)roundData[i]["ROUND"];
            roundDB[i].roundEnemyCnt = (int)roundData[i]["MONSTERCOUNT"];

            var variable = (roundData[i]["MONSTERNUM"]);
            string text = variable.ToString();
            //몬스터 번호 공백으로 분류
            String[] lines = text.Split(' ');
            //몬스터 id 공간 할당
            roundDB[i].monsterId = new int[roundDB[i].roundEnemyCnt];

            int cnt = 0;

            //몬스터 숫자만큼반복
            for (int j = 0; j < roundDB[i].roundEnemyCnt; j++)
            {
                //몬스터 여러종류일때
                if (lines.Length > 1 && lines.Length > j)
                {
                    if (lines[j] == "") continue;
                    roundDB[i].monsterId[j] = int.Parse(lines[j]);
                }
                //라운드 몬스터숫자보다 지정한 몬스터 숫자가 적으면 처음부터 다시채워줌
                else if (lines.Length > 1 && lines.Length <= j)
                {
                    roundDB[i].monsterId[j] = int.Parse(lines[cnt]);
                    cnt++;
                }
                //단일 몬스터
                else
                {
                    roundDB[i].monsterId[j] = int.Parse(lines[cnt]);
                }
            }

        }
        int ldx = 0;
        //타워종류
        for (int i = 0; i < 25; i++)
        {
            //종류별 레벨제한
            for (int j = 0; j < 30; j++)
            {
                towerAttackCost[i, j] = System.Convert.ToSingle(levelDamageData[ldx]["DAMAGE"]);
                ldx++;
            }
        }
        isContinue = false;
        //난이도
        level = null;
        round = 1;
        gold = 600;
    }

    //난이도 받아서 정보설정
    public void SelectDifficulty(string _level)
    {
        level = _level;
        //이지
        if (level == "EASY")
        {
            hp = 30;
        }
        //노말
        else if (level == "NORMAL")
        {
            hp = 20;
        }
        //하드
        else
        {
            hp = 10;
        }
        ReadData(level);
    }

    //csv파일읽기
    void ReadData(string _level)
    {
        int idx = 0;
        int count = 0;

        switch (_level)
        {
            case "EASY":
                idx = 0;
                break;
            case "NORMAL":
                idx = 40;
                break;
            case "HARD":
                idx = 80;
                break;
        }

        //라운드별 몬스터 정보 설정해준다
        for (int i = idx; i < (idx + rounds.Length); i++)
        {

            rounds[count].rounds = roundDB[i].rounds;
            //라운드 레벨
            rounds[count].level = level;
            //라운드에 나오는 몬스터 숫자
            rounds[count].roundEnemyCnt = roundDB[i].roundEnemyCnt;
            //몬스터 id배열 크기 설정
            rounds[count].monsterId = new int[rounds[count].roundEnemyCnt];
            //몬스터오브젝트 크기설정
            rounds[count].enemiesBody = new Enemy[rounds[count].monsterId.Length];

            //라운드에 나오는 몬스터 id 넣어줌
            for (int j = 0; j < rounds[count].monsterId.Length; j++)
            {
                rounds[count].monsterId[j] = roundDB[i].monsterId[j];

            }
            count++;
        }

    }

    //타워 데미지 업그레이드 
    public void UpgradeTowerDamage(int _num)
    {
        switch (_num)
        {
            case 0:
                for (int i = 0; i < normalTowerList.Count; i++)
                {
                    normalTowerList[i].Damage = towerAttackCost[normalTowerList[i].id - 1, normalTowerLV - 1];
                }
                break;
            case 1:
                for (int i = 0; i < magicTowerList.Count; i++)
                {
                    magicTowerList[i].Damage = towerAttackCost[magicTowerList[i].id - 1, magicTowerLV - 1];
                }
                break;
            case 2:
                for (int i = 0; i < rareTowerList.Count; i++)
                {
                    rareTowerList[i].Damage = towerAttackCost[rareTowerList[i].id - 1, rareTowerLV - 1];
                }
                break;
            case 3:
                for (int i = 0; i < uniqueTowerList.Count; i++)
                {
                    uniqueTowerList[i].Damage = towerAttackCost[uniqueTowerList[i].id - 1, uniqueTowerLV - 1];
                }
                break;
            case 4:
                for (int i = 0; i < epicTowerList.Count; i++)
                {
                    epicTowerList[i].Damage = towerAttackCost[epicTowerList[i].id - 1, epicTowerLV - 1];
                }
                break;

        }
    }

    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    }
    public int Round
    {
        get { return round; }
        set { round = value; }
    }
    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    public string Level
    {
        get { return level; }
        set { level = value; }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainScene" && isContinue)
        {
            tempTile = GameObject.Find("Map").GetComponent<Transform>().GetComponentsInChildren<Tile>();
            for (int i = 0; i < loadInfo.Count; i++)
            {
                LoadData(loadInfo[i].ID, loadInfo[i].TILENUMBER, loadInfo[i].RARELIST);
            }
            GameMgr gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();

            isContinue = false;
        }
    }

    //타일 로드용
    public void LoadData(int _id, int _tileID, RareList _rareList)
    {

        if (_rareList == RareList.NORMAL)
        {

            tempTile[_tileID].unit = Instantiate(normalTower[_id - 1], new Vector3(tempTile[_tileID].GetComponent<Transform>().position.x,
                tempTile[_tileID].GetComponent<Transform>().position.y + 1.2f,
                tempTile[_tileID].GetComponent<Transform>().position.z),
                Quaternion.identity, GameObject.Find("Player").GetComponent<Transform>().GetChild(0).gameObject.transform);

            tempTile[_tileID].unit.GetComponent<towerCtrl>().onTile = tempTile[_tileID];
            tempTile[_tileID].isCreated = true;
            tempTile[_tileID].unit.GetComponent<towerCtrl>().Damage = GameDB.Instance.towerAttackCost[tempTile[_tileID].unit.GetComponent<towerCtrl>().id - 1, normalTowerLV - 1];
            normalTowerList.Add(tempTile[_tileID].unit.GetComponent<towerCtrl>());

        }
        else if (_rareList == RareList.MAGIC)
        {
            tempTile[_tileID].unit = Instantiate(magicTower[_id - 6], new Vector3(tempTile[_tileID].GetComponent<Transform>().position.x,
         tempTile[_tileID].GetComponent<Transform>().position.y + 1.2f,
         tempTile[_tileID].GetComponent<Transform>().position.z),
         Quaternion.identity, GameObject.Find("Player").GetComponent<Transform>().GetChild(0).gameObject.transform);

            tempTile[_tileID].unit.GetComponent<towerCtrl>().onTile = tempTile[_tileID];
            tempTile[_tileID].isCreated = true;
            tempTile[_tileID].unit.GetComponent<towerCtrl>().Damage = GameDB.Instance.towerAttackCost[tempTile[_tileID].unit.GetComponent<towerCtrl>().id - 1, magicTowerLV - 1];
            magicTowerList.Add(tempTile[_tileID].unit.GetComponent<towerCtrl>());
        }
        else if (_rareList == RareList.RARE)
        {
            tempTile[_tileID].unit = Instantiate(rareTower[_id - 11], new Vector3(tempTile[_tileID].GetComponent<Transform>().position.x,
            tempTile[_tileID].GetComponent<Transform>().position.y + 1.2f,
            tempTile[_tileID].GetComponent<Transform>().position.z),
            Quaternion.identity, GameObject.Find("Player").GetComponent<Transform>().GetChild(0).gameObject.transform);

            tempTile[_tileID].unit.GetComponent<towerCtrl>().onTile = tempTile[_tileID];
            tempTile[_tileID].isCreated = true;
            tempTile[_tileID].unit.GetComponent<towerCtrl>().Damage = GameDB.Instance.towerAttackCost[tempTile[_tileID].unit.GetComponent<towerCtrl>().id - 1, rareTowerLV - 1];
            rareTowerList.Add(tempTile[_tileID].unit.GetComponent<towerCtrl>());
        }
        else if (_rareList == RareList.UNIQUE)
        {
            tempTile[_tileID].unit = Instantiate(uniqueTower[_id - 16], new Vector3(tempTile[_tileID].GetComponent<Transform>().position.x,
           tempTile[_tileID].GetComponent<Transform>().position.y + 1.2f,
           tempTile[_tileID].GetComponent<Transform>().position.z),
           Quaternion.identity, GameObject.Find("Player").GetComponent<Transform>().GetChild(0).gameObject.transform);

            tempTile[_tileID].unit.GetComponent<towerCtrl>().onTile = tempTile[_tileID];
            tempTile[_tileID].isCreated = true;
            tempTile[_tileID].unit.GetComponent<towerCtrl>().Damage = GameDB.Instance.towerAttackCost[tempTile[_tileID].unit.GetComponent<towerCtrl>().id - 1, uniqueTowerLV - 1];
            uniqueTowerList.Add(tempTile[_tileID].unit.GetComponent<towerCtrl>());
        }
        else
        {
            tempTile[_tileID].unit = Instantiate(epicTower[_id - 21], new Vector3(tempTile[_tileID].GetComponent<Transform>().position.x,
             tempTile[_tileID].GetComponent<Transform>().position.y + 1.2f,
             tempTile[_tileID].GetComponent<Transform>().position.z),
            Quaternion.identity, GameObject.Find("Player").GetComponent<Transform>().GetChild(0).gameObject.transform);

            tempTile[_tileID].unit.GetComponent<towerCtrl>().onTile = tempTile[_tileID];
            tempTile[_tileID].isCreated = true;
            tempTile[_tileID].unit.GetComponent<towerCtrl>().Damage = GameDB.Instance.towerAttackCost[tempTile[_tileID].unit.GetComponent<towerCtrl>().id - 1, epicTowerLV - 1];
            epicTowerList.Add(tempTile[_tileID].unit.GetComponent<towerCtrl>());
        }
    }

    //게임오버후 초기화
    public void ResetData()
    {
        normalTowerList.Clear();
        magicTowerList.Clear();
        rareTowerList.Clear();
        uniqueTowerList.Clear();
        epicTowerList.Clear();

        gold = 600;
        round = 1;
        Level = null;
        killCount = 0;
        isStartRound = true;
        isQuest1 = true;
        isQuest2 = true;
        isQuest3 = true;

        Qmon1Alive = false;
        Qmon2Alive = false;
        Qmon3Alive = false;

        questCoolTime1 = 1f;
        questCoolTime2 = 1f;
        questCoolTime3 = 1f;

        for (int i = 0; i < min.Length; i++)
        {
            min[i] = 0;
            tik[i] = 0;
        }

        normalTowerLV = 1;
        magicTowerLV = 1;
        rareTowerLV = 1;
        uniqueTowerLV = 1;
        epicTowerLV = 1;

        normalUpgradeCost = 10;
        magicUpgradeCost = 10;
        rareUpgradeCost = 10;
        uniqueUpgradeCost = 10;
        epicUpgradeCost = 10;

        for(int i =0;i< MissionMgr.instance.h_Quest.Length;i++)
        {
            MissionMgr.instance.h_Quest[i] = false;
        }
        for(int i =0;i< towerId_Count.Length;i++)
        {
            towerId_Count[i] = 0;
        }
    }
    public bool IsContinue
    {
        set { isContinue = value; }
        get { return isContinue; }
    }
    public int KillCount
    {
        get { return killCount; }
        set { killCount = value; }
    }

    public bool IsStartRound
    {
        get { return isStartRound; }
        set { isStartRound = value; }
    }
}
