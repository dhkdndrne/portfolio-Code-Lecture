using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    //메인씬
    [SerializeField] GameObject tutorialParent; // 튜토리얼 UI의 부모
    [SerializeField] GameObject clickPrefabs;    // 클릭 애니메이션 프리팹
    [SerializeField] GameObject longClickPrefabs;

    [SerializeField] Sprite[] tutorialSprites;  // 이미지에 들어갈 스프라이트들
    [SerializeField] Image tutorialImg;         // 튜토리얼 보여주는 이미지
    [SerializeField] Text tutorialText;         // 튜토리얼 텍스트
    [SerializeField] string[] narrations;

    [SerializeField] Button[] firstTutoBtn;     // 튜토리얼 버튼 묶음 1;
    [SerializeField] Button[] firstTutoBtn_2;     // 튜토리얼 버튼 묶음 1;

    [SerializeField] WorldSelectManager worldSelectManager;
    int textIdx;
    [SerializeField] int spriteIdx;

    public bool check; //클릭체크용 

    //게임씬
    [SerializeField] Image g_tutorialImg;
    private void Awake()
    {
        if (!UserData.Instance.userdata.isFinishTutorial)
        {
            var obj = FindObjectsOfType<TutorialManager>();
            if (obj.Length == 1)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

#if UNITY_ANDROID
        if (!UserData.Instance.userdata.isFinishTutorial)
        {
            Init();
            StartCoroutine(ShowTutorial_1());
            StartCoroutine(ShowTutorial_2());
            StartCoroutine(ShowTutorial_3());
        }
#endif
    }
    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
    void RemoveButtonFadeImage(int idx)
    {
        firstTutoBtn[idx].interactable = true;
        firstTutoBtn[idx].transform.GetChild(2).gameObject.SetActive(false);
    }
    void Init()
    {
        //튜토리얼까지 버튼 비활성화
        for (int i = 0; i < firstTutoBtn.Length; i++)
        {
            firstTutoBtn[i].interactable = false;
            firstTutoBtn[i].transform.GetChild(2).gameObject.SetActive(true);
        }
        for (int i = 0; i < firstTutoBtn_2.Length; i++)
        {
            firstTutoBtn_2[i].interactable = false;
        }
    }

    //튜토리얼 1(메인화면)
    IEnumerator ShowTutorial_1()
    {
        tutorialParent.SetActive(true);
        yield return new WaitForSeconds(3f);
        TweenParams tParms = new TweenParams().SetEase(Ease.Linear);
        yield return tutorialImg.transform.DOScale(new Vector3(1, 1, 1), 0.2f).SetAs(tParms).WaitForCompletion();

        // 환영인사 
        yield return tutorialText.DOText(narrations[textIdx++], 1).WaitForCompletion();
        yield return new WaitForSeconds(0.2f);
        yield return tutorialText.DOText(narrations[textIdx++], 1).WaitForCompletion();
        yield return new WaitForSeconds(1f);
        tutorialText.gameObject.SetActive(false);

        //게임씬으로 넘어가게
        tutorialImg.sprite = tutorialSprites[spriteIdx++];
        yield return new WaitForSeconds(2f);

        //검정 배경끄기
        tutorialParent.transform.GetChild(0).gameObject.SetActive(false);
        tutorialParent.SetActive(false);
    }

    //튜토리얼 2(월드화면)
    IEnumerator ShowTutorial_2()
    {
        //시작 버튼 누를때 까지 대기
        yield return new WaitUntil(() => LobbyManager.Instance.lobbyUI.worldPanel.activeSelf.Equals(true));

        GameObject clickObj = Instantiate(clickPrefabs);
        Transform tr = GameObject.Find("Stage 1").transform;
        clickObj.transform.localPosition = new Vector3(tr.position.x, tr.position.y - 0.1f, 0);

        // 스테이지 클릭했을때 입장버튼 위치
        yield return new WaitUntil(() => worldSelectManager.WorldInfoPanel.activeSelf.Equals(true));
        yield return clickObj.transform.DOMove(new Vector2(worldSelectManager.gameStartButton.transform.position.x, worldSelectManager.gameStartButton.transform.position.y - 0.1f), .7f);

        // 공장 목록 위치
        yield return new WaitUntil(() => worldSelectManager.IsOpen);
        tr = worldSelectManager.UnitList.transform.GetChild(0).transform;
        yield return clickObj.transform.DOMove(new Vector2(tr.position.x + 0.1f, tr.position.y - 0.05f), .8f).WaitForCompletion();

        tutorialParent.SetActive(true);
        tutorialImg.transform.localScale = new Vector2(.5f, .5f);   //크기 조절
        tutorialImg.transform.position = new Vector2(tr.position.x + 0.3f, tr.position.y - 0.3f);   //이미지 위치변경
        tutorialImg.sprite = tutorialSprites[spriteIdx++];  //다음 이미지 보여주기

        //유닛 선택했는지
        yield return new WaitUntil(() => worldSelectManager.EnterList.transform.childCount > 0);
        yield return clickObj.transform.DOMove(new Vector2(worldSelectManager.gameStartButton.transform.position.x, worldSelectManager.gameStartButton.transform.position.y - 0.1f), .8f);

        tutorialParent.SetActive(false);
        yield return new WaitForSeconds(1f);
        tutorialParent.SetActive(true);
        tutorialImg.transform.position = new Vector2(clickObj.transform.position.x + 0.1f, clickObj.transform.position.y - 0.2f);   //이미지 위치변경
        tutorialImg.sprite = tutorialSprites[spriteIdx++];  //다음 이미지 보여주기
    }
    //튜토리얼 3(게임 화면)
    IEnumerator ShowTutorial_3()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "GameScene");

        Button towerInfoBtn = GameObject.Find("TowerInfoBtn").GetComponent<Button>();
        towerInfoBtn.interactable = false;

        int index = 0;  //인덱스
        bool isReady = true;   //넘어가도되는지 체크용
        GameObject tutorialParent = GameObject.Find("tutorial");
        tutorialParent.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
        {
            if (!isReady)
            {
                isReady = true;
                index++;
            }
        });

        g_tutorialImg = tutorialParent.transform.GetChild(1).GetComponent<Image>();
        g_tutorialImg.gameObject.SetActive(false);

        TowerBase[] towers = GameObject.Find("Stage_1").transform.GetChild(5).GetComponentsInChildren<TowerBase>();

        foreach (var tower in towers)
        {
            tower.UnitBindFactor = 0;
        }

        yield return new WaitUntil(() => GameManager.Instance.Fader.finish);
        Transform stageText = GameObject.Find("StageText").transform.GetChild(0);
        isReady = false;

        //목표인구 소개
        spriteIdx = 4;
        g_tutorialImg.gameObject.SetActive(true);
        tutorialParent.transform.GetChild(0).gameObject.SetActive(true);
        g_tutorialImg.transform.position = new Vector3(stageText.transform.GetChild(0).position.x, stageText.transform.GetChild(0).position.y - 16, 0);

        //시간 소개
        yield return new WaitUntil(() => index.Equals(1));
        isReady = false;
        g_tutorialImg.sprite = tutorialSprites[spriteIdx++];
        g_tutorialImg.transform.position = new Vector3(stageText.transform.GetChild(2).position.x, stageText.transform.GetChild(2).position.y - 16, 0);


        //인구 소개
        yield return new WaitUntil(() => index.Equals(2));
        isReady = false;
        g_tutorialImg.sprite = tutorialSprites[spriteIdx++];
        g_tutorialImg.transform.position = new Vector3(stageText.transform.GetChild(3).position.x - 45, stageText.transform.GetChild(3).position.y - 16, 0);


        //배속 소개
        yield return new WaitUntil(() => index.Equals(3));
        isReady = false;
        Transform speedBtn = GameObject.Find("SpeedButton").transform;

        g_tutorialImg.sprite = tutorialSprites[spriteIdx++];
        g_tutorialImg.transform.position = new Vector3(speedBtn.transform.position.x + 1, speedBtn.transform.position.y - 16, 0);

        yield return new WaitUntil(() => index.Equals(4));
        isReady = false;

        Color color = tutorialParent.transform.GetChild(0).GetComponent<Image>().color;
        color.a = 0f;

        tutorialParent.transform.GetChild(0).GetComponent<Image>().color = color;       //버튼투명화(검정색패널)
        tutorialParent.transform.GetChild(0).gameObject.SetActive(false);   //버튼 비활성화

        GameObject slotParent = GameObject.Find("CharacterList");
        List<Button> unitslotBtn = new List<Button>();                 //유닛슬롯 버튼들 담아둘 리스트

        //유닛 슬롯 수 만큼
        for (int i = 0; i < slotParent.transform.GetChild(0).childCount; i++)
        {
            unitslotBtn.Add(slotParent.transform.GetChild(0).GetChild(i).GetComponent<Button>());
            unitslotBtn[i].interactable = false;
            unitslotBtn[i].transform.GetChild(3).gameObject.SetActive(true);   //fade 이미지 켜줌
        }

        GameObject clickObj = Instantiate(clickPrefabs);    //클릭 오브젝트 만들어주기
        clickObj.transform.localScale = new Vector3(5, 5, 5);   //클릭 오브젝트 크기 변경

        Transform towerTr = GameObject.Find("LightningTower").transform;    //번개타워 위치
        clickObj.SetActive(false);

        g_tutorialImg.sprite = tutorialSprites[spriteIdx++];    //타워클릭 이미지
        g_tutorialImg.rectTransform.pivot = new Vector2(1f, 0.5f);
        g_tutorialImg.transform.position = new Vector3(towerTr.position.x - 2, towerTr.position.y - 16, 0);

        GameObject longClickObj = Instantiate(longClickPrefabs);    //긴 클릭 오브젝트 만들어주기
        longClickObj.transform.localScale = new Vector3(5, 5, 5);   //클릭 오브젝트 크기 변경
        longClickObj.transform.position = new Vector3(towerTr.position.x + 1, towerTr.position.y - 6f, 0);

        //타워클릭할때까지 대기
        yield return new WaitUntil(() => check == true);
        check = false;
        longClickObj.SetActive(false);

        g_tutorialImg.sprite = tutorialSprites[spriteIdx++];    //타워클릭 이미지
        g_tutorialImg.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        g_tutorialImg.transform.localPosition = new Vector3(76, -57, 0);

        clickObj.SetActive(true);
        clickObj.transform.position = new Vector3(81.6f, 274.1f, 0);
        //타워 정보 버튼 누르기 까지의 과정
        towerInfoBtn.interactable = true;

        //타워 정보보기 종료 할떄까지대기
        yield return new WaitUntil(() => check == true);
        //타워정보 버튼 껏다 켜기..
        check = false;

        g_tutorialImg.sprite = tutorialSprites[spriteIdx++];
        g_tutorialImg.transform.localPosition = new Vector3(-137, -1264, 0);
        //타워 정보창 한번 더 누를때까지 대기
        yield return new WaitUntil(() => check == true);
        clickObj.transform.position = new Vector3(slotParent.transform.GetChild(0).GetChild(0).position.x + 3, slotParent.transform.GetChild(0).GetChild(0).position.y - 3, 0); //클릭 오브젝트 위치변경 
        g_tutorialImg.gameObject.SetActive(true);
        g_tutorialImg.rectTransform.pivot = new Vector2(0, 0.5f);
        g_tutorialImg.sprite = tutorialSprites[spriteIdx++];    //타워클릭 이미지
        g_tutorialImg.transform.position = new Vector3(slotParent.transform.GetChild(0).GetChild(0).position.x, slotParent.transform.GetChild(0).GetChild(0).position.y + 25, 0);


        //첫번째 유닛 슬롯 클릭 활성화
        UnitSlot unitSlot = unitslotBtn[0].GetComponent<UnitSlot>();
        unitslotBtn[0].interactable = true;
        unitslotBtn[0].transform.GetChild(3).gameObject.SetActive(false);
        check = false;

        yield return new WaitUntil(() => unitSlot.clickCnt > 0);
        clickObj.SetActive(false);
        g_tutorialImg.gameObject.SetActive(false);

        yield return new WaitUntil(() => unitSlot.clickCnt >= 10);
        yield return new WaitForSeconds(20);

        g_tutorialImg.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        g_tutorialImg.gameObject.SetActive(true);  // 안내 이미지를 활성화.
        g_tutorialImg.sprite = tutorialSprites[spriteIdx++];    //다음 이미지 보여주기

        g_tutorialImg.rectTransform.localPosition = new Vector3(149, -694, 0);
        g_tutorialImg.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);

        yield return new WaitForSeconds(2f);
        tutorialParent.transform.GetChild(0).gameObject.SetActive(true);   //버튼 활성화

        yield return new WaitUntil(() => index.Equals(5));
        isReady = false;

        //갑옷입은 유닛슬롯으로 바꾸기
        Item tutorialItem = (Item)DBManager.Instance.itemDB.equipDB["NORMAL"]["5004"];
        GameObject tempParent = GameObject.Find("UnitContainer").gameObject;   //유닛담겨져있는 컨테이너

        DataController.Factories[0].equipedItems[2] = tutorialItem; //팩토리 1번째 유닛 갑옷장착

        Unit unit = new Unit();                             //새로운 유닛 인스턴스 만듬
        unit.parent = DataController.Factories[0];
        DataController.Factories[0].unit = unit;            //팩토리의 유닛 바꿈
        DataController.Factories[0].unit.InputStatInfo();   //유닛 스텟 초기화
        unitSlot.Init(DataController.Factories[0], 1);       //슬롯 초기화
        GameManager.Instance.curPop = 0;                    //인구수 초기화

        for (int i = 0; i < tempParent.transform.GetChild(0).childCount; i++)
        {
            tempParent.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);   //필드위에 나와있는애들 꺼줌
        }
        yield return new WaitUntil(() => index.Equals(6));
        isReady = false;

        //마법 획득
        g_tutorialImg.sprite = tutorialSprites[spriteIdx++];//마법 소개이미지

        PlayerMagic icehorn = Resources.Load<PlayerMagic>("ScripableObject/Magic/01_IceHorn");  //scriptableobject형식인 마법 로드
        icehorn.possessionCount = 50;               //소유개수 10개로 설정
        DataController.PlayerMagic.Add(icehorn);    //마법리스트 추가
        GameManager.Instance.magicController.SetMagicSlot();    //리스트 생성및 초기화

        yield return new WaitUntil(() => index.Equals(7));
        isReady = false;
        g_tutorialImg.sprite = tutorialSprites[spriteIdx++]; //마법 사용법이미지

        //새로운 이미지 생성
        GameObject magicTutoObj = new GameObject();
        magicTutoObj.transform.SetParent(GameObject.Find("Canvas").transform);  //오브젝트의 부모를 캔버스로 바꿈
        magicTutoObj.AddComponent<Image>(); //이미지 컴포넌트 추가

        Image magicTutoImg = magicTutoObj.GetComponent<Image>();
        tempParent = GameObject.Find("SkillButton");
        magicTutoImg.sprite = tutorialSprites[spriteIdx++];
        magicTutoImg.preserveAspect = true;
        magicTutoImg.transform.localScale = new Vector3(5f, 5f, 5f);       //scale 변경
        magicTutoImg.rectTransform.sizeDelta = new Vector2(100, 58.4f);  //넓이 높이 변경

        magicTutoObj.GetComponent<Image>().rectTransform.pivot = new Vector2(1, 0.5f);    //피벗 변경
        magicTutoObj.transform.position = new Vector3(tempParent.transform.position.x, tempParent.transform.position.y + 25, 0);

        yield return new WaitUntil(() => index.Equals(8));
        isReady = false;
        g_tutorialImg.sprite = tutorialSprites[spriteIdx++];
        magicTutoImg.gameObject.SetActive(false);
        magicTutoObj.SetActive(false);

        //마법사용 튜토
        yield return new WaitUntil(() => index.Equals(9));
        isReady = false;

        g_tutorialImg.sprite = tutorialSprites[spriteIdx++];
        g_tutorialImg.transform.position = new Vector3(tempParent.transform.position.x, tempParent.transform.position.y + 25);
        g_tutorialImg.transform.localScale = new Vector3(1, 1, 1);

        //클릭이미지 활성
        clickObj.SetActive(true);
        clickObj.transform.position = new Vector3(tempParent.transform.position.x + 3, tempParent.transform.position.y - 3, 0);
        tutorialParent.transform.GetChild(0).gameObject.SetActive(false);   //버튼 비활성화

        //마법 선택할떄까지 대기
        yield return new WaitUntil(() => tempParent.GetComponent<MagicSlot>().isSelected);

        clickObj.SetActive(false);
        longClickObj.transform.position = new Vector3(towerTr.position.x + 1, towerTr.position.y - 6f, 0);
        g_tutorialImg.sprite = tutorialSprites[spriteIdx++];
        g_tutorialImg.transform.position = new Vector3(towerTr.position.x, towerTr.position.y + 20f, 0);

        //처음사용
        yield return new WaitUntil(() => tempParent.GetComponent<MagicSlot>().clickCnt > 0);
        g_tutorialImg.sprite = tutorialSprites[spriteIdx++];
        g_tutorialImg.transform.position = new Vector3(tempParent.transform.position.x, tempParent.transform.position.y + 25);
        clickObj.SetActive(true);
        longClickObj.SetActive(false);

        //두번째 선택
        yield return new WaitUntil(() => tempParent.GetComponent<MagicSlot>().isSelected);
        clickObj.SetActive(false);
        longClickObj.SetActive(true);
        g_tutorialImg.sprite = tutorialSprites[spriteIdx++];
        g_tutorialImg.transform.position = new Vector3(towerTr.position.x, towerTr.position.y + 20f, 0);

        yield return new WaitUntil(() => tempParent.GetComponent<MagicSlot>().clickCnt > 1);
        longClickObj.transform.position = new Vector3(tempParent.transform.position.x + 3, tempParent.transform.position.y - 3, 0);
        g_tutorialImg.gameObject.SetActive(false);
        check = false;
        clickObj.SetActive(false);
        longClickObj.SetActive(false);
        foreach (var tower in towers)
        {
            tower.UnitBindFactor = 99;
            tower.damage = 0;
        }

        //속박
        yield return new WaitUntil(() => check == true);
        g_tutorialImg.gameObject.SetActive(true);
        g_tutorialImg.sprite = tutorialSprites[spriteIdx++];
        towerTr = GameObject.Find("NormalTower").transform;    //노말타워 위치
        g_tutorialImg.transform.position = new Vector3(towerTr.position.x + 10, towerTr.position.y - 20, 0);
        check = false;
        tutorialParent.transform.GetChild(0).gameObject.SetActive(true);   //버튼 활성화

        yield return new WaitUntil(() => index.Equals(10));
        isReady = false;
        g_tutorialImg.sprite = tutorialSprites[spriteIdx++];
        clickObj.SetActive(true);
        clickObj.transform.position = new Vector3(69.9f, 247.3f, 0); //클릭 오브젝트 위치변경
        tutorialParent.transform.GetChild(0).gameObject.SetActive(false);   //버튼 비활성화

        yield return new WaitUntil(() => check == true);
        clickObj.SetActive(false);
        g_tutorialImg.gameObject.SetActive(false);

        foreach (var tower in towers)
        {
            tower.UnitBindFactor = 5;
            tower.damage = 30;
        }

        StartCoroutine(ShowTutorial_4());
    }

    public Vector3 GetPosition(Transform _TR)
    {
        return new Vector3(_TR.position.x, _TR.position.y);
    }

    //튜토리얼 4(메인화면2)
    IEnumerator ShowTutorial_4()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "MainScene");
        check = false;
        GameObject gamePanel = GameObject.Find("GamePanel");
        Button exitButton = GameObject.Find("TopPanel").transform.Find("ExitPanel").GetComponent<Button>();

        gamePanel.transform.GetChild(0).GetComponent<Button>().interactable = false;    //게임시작버튼 비활성화
        firstTutoBtn = gamePanel.transform.GetChild(1).GetComponentsInChildren<Button>();

        //튜토리얼까지 버튼 비활성화
        for (int i = 0; i < firstTutoBtn.Length; i++)
        {
            firstTutoBtn[i].interactable = false;
            firstTutoBtn[i].transform.GetChild(2).gameObject.SetActive(true);
        }

        //튜토리얼 오브젝트 다시 연결
        GameObject canvas = GameObject.Find("Canvas"); //캔버스(제일 부모 찾기)
        tutorialParent = canvas.transform.Find("Tutorial").gameObject;
        tutorialImg = tutorialParent.transform.GetChild(1).GetComponent<Image>();


        //튜토리얼창 켜기
        tutorialParent.SetActive(true);
        //검정 배경끄기
        tutorialParent.transform.GetChild(0).gameObject.SetActive(false);

        tutorialImg.sprite = tutorialSprites[spriteIdx++];
        TweenParams tParms = new TweenParams().SetEase(Ease.Linear);
        yield return tutorialImg.transform.DOScale(new Vector3(1, 1, 1), 0.2f).SetAs(tParms).WaitForCompletion();

        RemoveButtonFadeImage(3);

        //말머리 아이템 인벤토리에 넣어놓음
        Item tutorialItem = (Item)DBManager.Instance.itemDB.headDB["NORMAL"]["1027"];

        //갑옷 아이템도
        LobbyManager.Instance.inventory.AddItem((Item)DBManager.Instance.itemDB.equipDB["NORMAL"]["5004"]);

        for (int i = 0; i < 2; i++)
        {
            LobbyManager.Instance.inventory.AddItem(tutorialItem);
        }
        // 인벤토리정렬해준다.
        LobbyManager.Instance.inventory.SortInventory();

        GameObject clickObj = Instantiate(clickPrefabs);    //클릭 오브젝트 만들어주기
        clickObj.transform.position = new Vector3(firstTutoBtn[3].transform.position.x + 0.06f, firstTutoBtn[3].transform.position.y - 0.11f, 0); //클릭 오브젝트 위치변경

        // 합성 튜토리얼
        GameObject combinePanel = canvas.transform.Find("CombinePanel").gameObject;
        //// 합성창 켜졌을때
        yield return new WaitUntil(() => combinePanel.activeSelf == true);
        exitButton.interactable = false; //나가기버튼 비활성화
        clickObj.SetActive(false);
        tutorialImg.sprite = tutorialSprites[spriteIdx++];

        yield return new WaitForSeconds(2f);

        tutorialImg.gameObject.SetActive(false);
        clickObj.SetActive(true);
        clickObj.transform.position = new Vector3(combinePanel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).transform.position.x + 0.113f, combinePanel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).transform.position.y - 0.11f, 0);

        //합성창에 아이템 등록될때까지 대기
        yield return new WaitUntil(() => CombineSystem.itemCount > 0);
        clickObj.SetActive(false);

        yield return new WaitUntil(() => CombineSystem.itemCount > 1);

        tutorialImg.gameObject.SetActive(true);
        tutorialImg.sprite = tutorialSprites[spriteIdx++];
        tutorialImg.transform.localScale = new Vector3(0.4f, 0.4f);

        clickObj.SetActive(true);
        clickObj.transform.position = new Vector3(combinePanel.transform.GetChild(6).position.x + 0.06f, combinePanel.transform.GetChild(6).position.y - 0.11f, 0);

        //조합버튼 누를때까지 대기
        yield return new WaitUntil(() => check);
        clickObj.transform.position = new Vector3(exitButton.transform.position.x - 0.06f, exitButton.transform.position.y - 0.15f, 0); ;
        tutorialImg.transform.localScale = new Vector3(1f, 1f);
        tutorialImg.gameObject.SetActive(false);
        exitButton.interactable = true;
        check = false;
        //합성창 끌때까지 대기
        yield return new WaitUntil(() => combinePanel.activeSelf == false);
        tutorialImg.gameObject.SetActive(true);
        tutorialImg.sprite = tutorialSprites[spriteIdx++];
        clickObj.transform.position = new Vector3(firstTutoBtn[4].transform.position.x + 0.06f, firstTutoBtn[4].transform.position.y - 0.11f, 0);
        RemoveButtonFadeImage(4);   //상점창 켜주기

        yield return new WaitUntil(() => LobbyManager.Instance.lobbyUI.currPanel.Count > 0 || canvas.transform.Find("ShopPanel").gameObject.activeSelf == true);
        clickObj.SetActive(false);
        tutorialImg.gameObject.SetActive(false);

        //상점패널 열릴떄까지 대기
        yield return new WaitUntil(() => canvas.transform.Find("ShopPanel").gameObject.activeSelf == true);
        tutorialImg.gameObject.SetActive(true);
        tutorialImg.sprite = tutorialSprites[spriteIdx++];
        exitButton.interactable = false;

        yield return new WaitForSeconds(5f);
        tutorialImg.gameObject.SetActive(false);
        exitButton.interactable = true;

        //상점패널 닫힐때까지 대기
        yield return new WaitUntil(() => canvas.transform.Find("ShopPanel").gameObject.activeSelf == false);
        tutorialImg.gameObject.SetActive(true);
        tutorialImg.sprite = tutorialSprites[spriteIdx++];

        clickObj.SetActive(true);
        clickObj.transform.position = new Vector3(firstTutoBtn[2].transform.position.x + 0.06f, firstTutoBtn[2].transform.position.y - 0.11f, 0);
        RemoveButtonFadeImage(2);   //인벤토리창 켜주기

        yield return new WaitUntil(() => LobbyManager.Instance.lobbyUI.currPanel.Count > 0 || canvas.transform.Find("InventoryUI").gameObject.activeSelf == true);
        clickObj.SetActive(false);
        tutorialImg.gameObject.SetActive(false);

        //인벤토리 패널 열릴떄까지 대기
        yield return new WaitUntil(() => canvas.transform.Find("InventoryUI").gameObject.activeSelf == true);
        Button invenExitBtn = GameObject.Find("ExitBtn").GetComponent<Button>();
        invenExitBtn.interactable = false;

        //아이템 클릭 유도
        Transform beforeParent = clickObj.transform.parent;
        clickObj.SetActive(true);
        clickObj.transform.parent = LobbyManager.Instance.inventoryUI.slots[0][0].transform;
        clickObj.transform.localPosition = new Vector3(LobbyManager.Instance.inventoryUI.slots[0][0].transform.position.x + 50, LobbyManager.Instance.inventoryUI.slots[0][0].transform.position.y - 68, 0);

        //아이템 클릭할때까지 대기
        yield return new WaitUntil(() => LobbyManager.Instance.inventory.selectedSlot != null);
        clickObj.SetActive(false);
        tutorialImg.gameObject.SetActive(true);
        tutorialImg.sprite = tutorialSprites[spriteIdx++];
        tutorialImg.transform.localScale = new Vector3(0.4f, 0.4f);
        tutorialImg.rectTransform.localPosition = new Vector3(-204, -232);

        yield return new WaitForSeconds(2f);
        tutorialImg.rectTransform.localPosition = new Vector3(198, -602);
        tutorialImg.sprite = tutorialSprites[spriteIdx++];

        yield return new WaitForSeconds(2f);
        tutorialImg.gameObject.SetActive(false);
        invenExitBtn.interactable = true;

        //인벤토리창 꺼질떄까지 대기
        yield return new WaitUntil(() => canvas.transform.Find("InventoryUI").gameObject.activeSelf == false);
        tutorialImg.gameObject.SetActive(true);
        tutorialImg.sprite = tutorialSprites[spriteIdx++];
        tutorialImg.transform.localScale = new Vector3(1, 1);
        tutorialImg.rectTransform.localPosition = new Vector3(0, 111.9f);

        RemoveButtonFadeImage(1);   //마법창 켜주기
        clickObj.SetActive(true);
        clickObj.transform.parent = beforeParent;
        clickObj.transform.position = new Vector3(firstTutoBtn[1].transform.position.x + 0.06f, firstTutoBtn[1].transform.position.y - 0.11f, 0);

        //마법창 켤때까지 기다리기
        yield return new WaitUntil(() => LobbyManager.Instance.lobbyUI.currPanel.Count > 0 || canvas.transform.Find("MagicPanel").gameObject.activeSelf == true);
        clickObj.SetActive(false);
        tutorialImg.gameObject.SetActive(false);

        yield return new WaitUntil(() => canvas.transform.Find("MagicPanel").gameObject.activeSelf == true);
        exitButton.interactable = false;
        //골드 소매넣기
        LobbyManager.Instance.Gold += 1000;

        Transform magicTransform = GameObject.Find("Magics").transform;
        clickObj.SetActive(true);
        clickObj.transform.position = new Vector3(magicTransform.position.x - 0.2f, 0.2f);

        tutorialImg.gameObject.SetActive(true);
        tutorialImg.sprite = tutorialSprites[spriteIdx++];
        tutorialImg.transform.localScale = new Vector3(.4f, .4f);
        tutorialImg.rectTransform.localPosition = new Vector3(clickObj.transform.position.x, clickObj.transform.position.y);

        //팝업창 켜질떄까지 대기
        yield return new WaitUntil(() => canvas.transform.Find("MagicPanel").Find("PopUp").gameObject.activeSelf == true);
        clickObj.SetActive(false);
        tutorialImg.rectTransform.localPosition = new Vector3(-243, 342);
        tutorialImg.sprite = tutorialSprites[spriteIdx++];

        yield return new WaitForSeconds(3f);
        clickObj.SetActive(true);
        GameObject popupPanel = GameObject.Find("PopUp");
        popupPanel.transform.GetChild(0).GetChild(12).gameObject.SetActive(false); //팝업 나가기버튼 비활성화
        clickObj.transform.position = new Vector3(popupPanel.transform.GetChild(1).GetChild(1).position.x, popupPanel.transform.GetChild(1).GetChild(1).position.y - 0.11f);
        tutorialImg.rectTransform.localPosition = new Vector3(50, -367);
        tutorialImg.sprite = tutorialSprites[spriteIdx++];

        PlayerMagicManager playerMagicManager = FindObjectOfType<PlayerMagicManager>();
        //개수 1개 늘릴떄까지 대기
        yield return new WaitUntil(() => playerMagicManager.purchase_Amount > 0);
        clickObj.transform.position = new Vector3(popupPanel.transform.GetChild(1).GetChild(5).position.x, popupPanel.transform.GetChild(1).GetChild(5).position.y - 0.11f);

        yield return new WaitUntil(() => check == true);
        clickObj.SetActive(false);
        tutorialImg.rectTransform.localPosition = new Vector3(-41, -66);
        tutorialImg.sprite = tutorialSprites[spriteIdx++];
        check = false;
        yield return new WaitForSeconds(3f);
        clickObj.SetActive(true);
        clickObj.transform.position = new Vector3(popupPanel.transform.GetChild(0).GetChild(7).position.x, popupPanel.transform.GetChild(0).GetChild(7).position.y - 0.11f);
        tutorialImg.rectTransform.localPosition = new Vector3(119, -230);
        tutorialImg.sprite = tutorialSprites[spriteIdx++];

        //장착버튼 누를때까지 대기
        yield return new WaitUntil(() => playerMagicManager.equipedMagic.Count > 0);
        clickObj.SetActive(false);
        exitButton.interactable = true;
        tutorialImg.gameObject.SetActive(false);
        popupPanel.transform.GetChild(0).GetChild(12).gameObject.SetActive(true);

        //마법패널 끌때까지 대기
        yield return new WaitUntil(() => canvas.transform.Find("MagicPanel").gameObject.activeSelf == false);

        RemoveButtonFadeImage(0);   //병영창 켜주기
        clickObj.SetActive(true);
        clickObj.transform.position = new Vector3(firstTutoBtn[0].transform.position.x + 0.06f, firstTutoBtn[0].transform.position.y - 0.11f, 0);

        tutorialImg.gameObject.SetActive(true);
        tutorialImg.transform.localScale = new Vector3(1, 1);
        tutorialImg.rectTransform.localPosition = Vector3.zero;
        tutorialImg.sprite = tutorialSprites[spriteIdx++];

        //팩토리 창 켤때까지 대기
        yield return new WaitUntil(() => LobbyManager.Instance.lobbyUI.currPanel.Count > 0 || canvas.transform.Find("Factories").gameObject.activeSelf == true);
        clickObj.SetActive(false);
        tutorialImg.gameObject.SetActive(false);
        exitButton.interactable = false;
        GameObject factoryPanel = canvas.transform.Find("Factories").gameObject;

        yield return new WaitUntil(() => factoryPanel.gameObject.activeSelf == true);
        clickObj.SetActive(true);
        clickObj.transform.position = new Vector3(factoryPanel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).position.x + 0.06f, factoryPanel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).position.y - 0.11f, 0);

        tutorialImg.gameObject.SetActive(true);
        tutorialImg.sprite = tutorialSprites[spriteIdx++];
        tutorialImg.transform.localScale = new Vector3(.4f, .4f);
        tutorialImg.rectTransform.localPosition = new Vector3(158, 197);

        //팩토리 장착버튼 누를때까지 대기
        yield return new WaitUntil(() => canvas.transform.Find("FactoryOptionPanel").gameObject.activeSelf == true);
        GameObject equipBtn = GameObject.Find("Armor_Info").transform.GetChild(4).gameObject;
        tutorialImg.rectTransform.localPosition = Vector3.zero;
        tutorialImg.sprite = tutorialSprites[spriteIdx++];
        clickObj.transform.position = new Vector3(equipBtn.transform.position.x + 0.06f, equipBtn.transform.position.y - 0.11f, 0);

        //인벤창 열릴때까지 대기
        yield return new WaitUntil(() => canvas.transform.Find("InventoryUI").gameObject.activeSelf == true);
        invenExitBtn.interactable = false;  //인벤토리 나가기버튼 비활성
        tutorialImg.gameObject.SetActive(false);    //안내이미지 비활성화
        clickObj.transform.position = new Vector3(equipBtn.transform.position.x + 0.06f, equipBtn.transform.position.y - 0.11f, 0);

        beforeParent = clickObj.transform.parent;
        clickObj.transform.SetParent(LobbyManager.Instance.inventoryUI.slots[2][0].transform);
        clickObj.transform.localPosition = new Vector3(LobbyManager.Instance.inventoryUI.slots[2][0].transform.position.x + 50, LobbyManager.Instance.inventoryUI.slots[2][0].transform.position.y - 68, 0);

        //아이템 클릭
        yield return new WaitUntil(() => LobbyManager.Instance.inventory.selectedSlot != null);
        clickObj.transform.SetParent(beforeParent);
        equipBtn = GameObject.Find("EquipButton");
        clickObj.transform.localPosition = new Vector3(equipBtn.transform.position.x + 0.06f, equipBtn.transform.position.y - 0.11f, 0);

        yield return new WaitUntil(() => check == true);
        invenExitBtn.interactable = true;  //인벤토리 나가기버튼 활성
        clickObj.SetActive(false);
        exitButton.interactable = true;

        yield return new WaitUntil(() => factoryPanel.gameObject.activeSelf == true);
        exitButton.interactable = false;
        tutorialImg.gameObject.SetActive(true);    //안내이미지 활성화
        tutorialImg.sprite = tutorialSprites[spriteIdx];
        tutorialImg.rectTransform.localPosition = new Vector3(135, -33);

        yield return new WaitForSeconds(3f);
        tutorialImg.gameObject.SetActive(false);
        exitButton.interactable = true;
        gamePanel.transform.GetChild(0).GetComponent<Button>().interactable = true;    //게임시작버튼 활성화

        //튜토리얼 완료
        UserData.Instance.userdata.isFinishTutorial = true;

        LobbyManager.Instance.lobbyUI.StartResourceEff(true, 1, 10000);
        LobbyManager.Instance.lobbyUI.StartResourceEff(false, 0, 200);
        LobbyManager.Instance.SaveData();
    }
}
