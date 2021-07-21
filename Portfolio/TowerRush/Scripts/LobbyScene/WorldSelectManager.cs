using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class WorldSelectManager : MonoBehaviour
{
    [SerializeField] List<GameObject> worlds = new List<GameObject>();
    public GameObject slotPrefab; //유닛 슬롯 프리팹

    [SerializeField] GameObject worldInfoPanel;  // 스테이지 정보 패널
    public GameObject WorldInfoPanel
    {
        get
        {
            return worldInfoPanel;
        }
    }
    [SerializeField] GameObject armyPanel;       // 유닛 편성 패널

    GameObject enterList; //입장 유닛 리스트
    public GameObject EnterList
    {
        get
        {
            return enterList;
        }
    }
    GameObject unitList; // 유닛 리스트 오브젝트
    public GameObject UnitList
    {
        get
        {
            return unitList;
        }
    }

    Text stageTxt;              // 스테이지 이름
    Text stageInfoTxt1;         // 제한시간
    Text stageInfoTxt2;         // 제한인구
    Text stageInfoTxt3;         // 클리어 인원수

    Button openButton;          // 유닛 편성 패널 열고닫는 버튼
    Button exitButton;          // 스테이지 정보 패널 나가기 함수
    public Button gameStartButton;     // 스테이지 입장 버튼

    bool isOpen;                // 군대 편집패널 열렸는지
    public bool IsOpen
    {
        get { return isOpen; }
    }

    GameObject tempArrowObj;
    List<GameObject> unitPool = new List<GameObject>(); // 유닛 슬롯 풀링

    //월드 이동
    [Header("world change")]
    [SerializeField] RectTransform list;
    bool isScroll;
    int worldIdx;                                 // 월드 인덱스
    [SerializeField] List<float> targetPos = new List<float>();             // 목표 위치
    [SerializeField] Button world_leftBtn;
    [SerializeField] Button world_RightBtn;
    [SerializeField] Text worldNameTxt;
    [SerializeField] string[] world_Name;
    void Awake()
    {
        //자식수만큼 월드를 추가한다
        for (int i = 0; i < transform.GetChild(1).GetChild(0).childCount; i++)
        {
            worlds.Add(transform.GetChild(1).GetChild(0).GetChild(i).gameObject);
        }

        stageTxt = worldInfoPanel.transform.GetChild(0).GetComponent<Text>();
        stageInfoTxt1 = worldInfoPanel.transform.GetChild(1).GetComponent<Text>();
        stageInfoTxt2 = worldInfoPanel.transform.GetChild(2).GetComponent<Text>();
        stageInfoTxt3 = worldInfoPanel.transform.GetChild(3).GetComponent<Text>();
        openButton = worldInfoPanel.transform.GetChild(4).GetComponent<Button>();
        exitButton = worldInfoPanel.transform.GetChild(5).GetComponent<Button>();

        enterList = armyPanel.transform.GetChild(0).GetChild(3).GetChild(0).gameObject;
        unitList = armyPanel.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).gameObject;

        exitButton.onClick.AddListener(() => ClosePanel());
        LobbyManager.Instance.onUnitCheckCallback += () => ChangeArmyPanelState(false);
        float tempPos = list.transform.localPosition.x;

        for (int i = 0; i < list.transform.childCount; i++)
        {
            targetPos.Add(tempPos);
            tempPos -= list.GetChild(i).GetComponent<RectTransform>().rect.width;
        }
    }

    void OnEnable()
    {
        if (tempArrowObj != null) tempArrowObj.SetActive(false);

        int ten = 0;
        int stage = 0;
        if (UserData.Instance.userdata.stage / 10 > 0)
        {
            ten = UserData.Instance.userdata.stage / 10;
            stage = UserData.Instance.userdata.stage % 10;
        }
        else stage = UserData.Instance.userdata.stage;

        //잠금 이미지 없애주기
        for (int i = 1; i <= ten; i++)
        {
            list.GetChild(i).GetChild(10).gameObject.SetActive(false);
        }

        tempArrowObj = worlds[ten].transform.GetChild(stage).GetChild(0).gameObject;
        tempArrowObj.SetActive(true);

        armyPanel.transform.localPosition = new Vector2(0, 1040f);
        openButton.gameObject.SetActive(true);
        armyPanel.transform.GetChild(0).gameObject.SetActive(false);

        isOpen = false;

        worldInfoPanel.SetActive(false);

        CreateFactorySlot();
        LobbyManager.Instance.armyCount = 0;

        if (ten > 0)
        {
            worldIdx = ten;
        }

        worldNameTxt.text = world_Name[worldIdx];
        list.localPosition = new Vector2(targetPos[worldIdx], list.localPosition.y);
    }

    public void SelectStage(int _Num)
    {
        //현재 플레이어가 클리어한 스테이지 이전 스테이지면 선택 가능
        if (_Num <= UserData.Instance.userdata.stage)
        {
            worldInfoPanel.SetActive(true);

            int ten = DBManager.Instance.worldDB.DataBase[_Num].limitTime / 60; //분
            string one = (DBManager.Instance.worldDB.DataBase[_Num].limitTime % 60).ToString();

            if (one.Equals("0")) one = "00";

            stageTxt.text = DBManager.Instance.worldDB.DataBase[_Num].stageName;
            stageInfoTxt1.text = ten + "분 " + one + "초";
            stageInfoTxt2.text = DBManager.Instance.worldDB.DataBase[_Num].limitPopulation.ToString();
            stageInfoTxt3.text = DBManager.Instance.worldDB.DataBase[_Num].clearCount.ToString();
            armyPanel.SetActive(true);
            DataController.CurrentStage = _Num;
        }
    }

    //병력 편성 패널 내려오는 함수
    public void ChangeArmyPanelState(bool _Open)
    {
        if (!_Open && !isOpen)
        {
            StartCoroutine(CheckOpen());
            armyPanel.transform.DOLocalMoveY(153f, 1).OnComplete(() => isOpen = true);
            openButton.gameObject.SetActive(false);
        }
        else
        {
            armyPanel.transform.GetChild(0).gameObject.SetActive(false);
            armyPanel.transform.DOLocalMoveY(1040f, 0.95f).OnComplete(() => isOpen = false);
            openButton.gameObject.SetActive(true);
        }
    }

    IEnumerator CheckOpen()
    {
        yield return new WaitUntil(() => isOpen);
        armyPanel.transform.GetChild(0).gameObject.SetActive(true);
        SortList();
        GetFactoryUnit();
    }

    //팩토리 유닛장착 아이템 이미지 가져오기
    void GetFactoryUnit()
    {
        for (int i = 0; i < LobbyManager.Instance.factoryManager.factoryCount; i++)
        {
            GameObject unitSlot = unitList.transform.GetChild(i).gameObject;

            //머리 (히로디온)
            if (LobbyManager.Instance.factoryManager.factories[i].equipedItems[0] != null)
            {
                unitSlot.transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = LobbyManager.Instance.factoryManager.factories[i].equipedItems[0].image;
            }
            //갑옷
            if (LobbyManager.Instance.factoryManager.factories[i].equipedItems[2] != null)
            {
                unitSlot.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = LobbyManager.Instance.factoryManager.factories[i].equipedItems[2].image;

            }
            //신발
            if (LobbyManager.Instance.factoryManager.factories[i].equipedItems[3] != null)
            {
                unitSlot.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = LobbyManager.Instance.factoryManager.factories[i].equipedItems[3].image;
            }
        }
    }

    //팩토리 수 만큼 슬롯을 생성한다
    void CreateFactorySlot()
    {
        for (int i = 0; i < LobbyManager.Instance.factoryManager.factoryCount; i++)
        {
            //팩토리의 개수가 슬롯의 개수보다 많으면 슬롯추가.
            if (unitPool.Count <= i)
            {
                GameObject obj = Instantiate(slotPrefab, unitList.transform);
                obj.name = i.ToString();
                unitPool.Add(obj);
            }

            if (unitPool[i].transform.parent != unitList.transform)
            {
                unitPool[i].transform.SetParent(unitList.transform);
            }
        }
    }

    void SortList()
    {
        //입장리스트에 있던 유닛슬롯들 원위치로 옮김
        while (enterList.transform.childCount > 0)
        {
            enterList.transform.GetChild(0).transform.SetParent(unitList.transform);
        }

        //번호순으로 정렬
        for (int i = 0; i < unitList.transform.childCount; i++)
        {
            unitList.transform.GetChild(i).SetSiblingIndex(int.Parse(unitList.transform.GetChild(i).name));
        }
    }
    void ClosePanel()
    {
        SortList();
        // 군대 편집 패널 트위닝 중지
        armyPanel.transform.DOKill();
        worldInfoPanel.SetActive(false);
        armyPanel.SetActive(false);

        // 군대 편집 패널을 위로 올려준다
        armyPanel.transform.localPosition = new Vector2(0, 1040f);

        //열기 버튼 활성화
        openButton.gameObject.SetActive(true);
        isOpen = false;
        LobbyManager.Instance.armyCount = 0;
    }

    public void ClickWorldBtn(bool _Direction)
    {
        bool move = false;
        ClosePanel();
        if (_Direction)
        {
            if (worldIdx > 0)
            {
                worldIdx--;
                move = true;
            }

        }
        else
        {
            if (worldIdx < list.childCount - 1)
            {
                worldIdx++;
                move = true;
            }

        }

        if (move)
        {
            isScroll = true;
            StartCoroutine(Scroll());
            worldNameTxt.text = world_Name[worldIdx];
        }

    }

    //편성 슬롯 이동효과 함수
    public void MoveSlot(bool _Right)
    {
        float pos = unitList.transform.position.x;
        if (unitList.transform.childCount > 5)
        {
            int charCount = unitList.transform.childCount - 5;
            if (_Right)
            {
                pos = (charCount * -113.2f);
                unitList.transform.DOLocalMoveX(pos, 1);
            }
            else
            {
                pos = (charCount * 113.2f) > 0 ? 0 : (charCount * 113.2f);
                unitList.transform.DOLocalMoveX(pos, 1);
            }
        }
    }

    //월드스크롤
    IEnumerator Scroll()
    {
        while (isScroll)
        {
            list.localPosition = Vector2.Lerp(list.localPosition, new Vector2(targetPos[worldIdx], list.localPosition.y), Time.deltaTime * 5);
            if (Vector2.Distance(list.localPosition, new Vector2(targetPos[worldIdx], list.localPosition.y)) < 0.1f)
            {
                isScroll = false;
                list.localPosition = new Vector2(targetPos[worldIdx], list.localPosition.y);

            }
            yield return null;
        }
    }
}
