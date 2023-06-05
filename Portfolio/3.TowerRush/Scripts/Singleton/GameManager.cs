using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingleTon<GameManager>
{
    
    InGameUI ingameUI;
    //--------------------------------------------
    [Header("Prefabs")]
    [SerializeField] GameObject slotPrefab;     // 유닛슬롯 프리팹
    [SerializeField] List<UnitSlot> unitSlot = new List<UnitSlot>();
    public List<UnitSlot> UnitSlots
    {
        get { return unitSlot; }
    }
    public GameObject unitPrefab;               // 유닛 프리팹

    public int stage;
    public int targetCount; // 목표 인원
    public int limitPop;    // 최대 인구수
    public int curPop;      // 현재 인구수

    float coolTime;  // 유닛 소환 전체 쿨타임
    float timeScale; //게임 속도조절 변수
    public float TimeScale{ get { return timeScale; } }

    public bool summonReady { get; private set; }

    public int min { get; private set; }
    public int sec { get; private set; }
    [SerializeField] FadeController fader;
    public FadeController Fader
    {
        get { return fader; }
    }
    [SerializeField] GameObject stageParent;
    public GameObject StageParent
    {
        get
        {
            return stageParent;
        }
    }


    System.Action fadeinAction;
    [Header("Option")]
    public GameOption gameOption;

    [Header("magicController")]
    public MagicController magicController;
    public TutorialManager tutorialManager;


    void Awake()
    {
        magicController = FindObjectOfType<MagicController>();
        stageParent.transform.GetChild(DataController.CurrentStage).gameObject.SetActive(true);      //플레이어가 선택한 스테이지의 맵을 활성화 시킨다.
        InitGameSetting();
        SetUnitSlotInfo();

        if (!UserData.Instance.userdata.isFinishTutorial) tutorialManager = FindObjectOfType<TutorialManager>();
       
    }
    public void FadeInToMainScene()
    {
        fader.FadeIn(0.5f, fadeinAction);
    }
    private void Start()
    {
        fadeinAction += ()=> LoadingControl.LoadScene("MainScene");

        fader.gameObject.SetActive(true);
        fader.FadeOut(0.5f);
       
        ingameUI = FindObjectOfType<InGameUI>();
        StartCoroutine(CountTimer());

        timeScale = 1;
        ingameUI.gameSpeedBtn.onClick.AddListener(ChangeGameSpeed);
        ingameUI.saveSoundCallback += gameOption.SaveOptions;
    }

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
    void InitGameSetting()
    {
        summonReady = true;
        coolTime = 0.5f;

        targetCount = DBManager.Instance.worldDB.DataBase[DataController.CurrentStage].clearCount;
        limitPop = DBManager.Instance.worldDB.DataBase[DataController.CurrentStage].limitPopulation;

        int timer = DBManager.Instance.worldDB.DataBase[DataController.CurrentStage].limitTime;
        min = timer / 60;
        sec = timer % 60;

    }

    //유닛 정보 슬롯에 넣어줌
    void SetUnitSlotInfo()
    {
        GameObject tempParent = GameObject.Find("Content");
        //유닛 슬롯 생성및 초기화
        for (int i = 0; i < DataController.UnitOrder.Count; i++)
        {
            GameObject objParent = Instantiate(slotPrefab, tempParent.transform);           
            UnitSlot slot = objParent.GetComponent<UnitSlot>();
            slot.Init(DataController.Factories[int.Parse(DataController.UnitOrder[i])],i+1);
            unitSlot.Add(slot);
        }       
    }

    IEnumerator CountTimer()
    {
        while (min > 0 || sec > 0)
        {
            if (sec > 0) sec--;
            else if (min > 0)
            {
                sec = 59;
                min--;
            }
            ingameUI.timerText.text = min + "분 " + sec + " 초";

            if (sec.Equals(0) && min.Equals(0))
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                //게임오버
                ingameUI.ShowResult(false);
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator UpdateCoolTime()
    {
        ingameUI.coolTimeImg.fillAmount = 1f;
        summonReady = false;

        while (ingameUI.coolTimeImg.fillAmount > 0)
        {
            ingameUI.coolTimeImg.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;
            if (ingameUI.coolTimeImg.fillAmount <= 0)
            {
                summonReady = true;
            }
            yield return null;
        }
    }

    public void EndStage(bool _Clear)
    {    
        //첫 클리어
        if(!DBManager.Instance.worldDB.DataBase[DataController.CurrentStage].isClear)
        {
            if(UserData.Instance.userdata.stage < DataController.maxStage) UserData.Instance.userdata.stage++;
            ingameUI.ShowResult(_Clear, true);          
        }
        else
        {
            ingameUI.ShowResult(_Clear);
        }
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        gameOption.SaveOptions();
    }

    void ChangeGameSpeed()
    {
        if (timeScale < 5f)
        {
            timeScale++;
        }
        else timeScale = 1f;

        
        ingameUI.gameSpeedText.text = "X" + timeScale;
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
