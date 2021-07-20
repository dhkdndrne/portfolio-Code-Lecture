using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class LobbyUI : MonoBehaviour
{
    public delegate void OnInventoryClosed();
    public OnInventoryClosed onIventoryClosedCallback;

    public Image representCharImg;      //대표캐릭터 이미지

    public Image representHead;
    public Image representBody;
    public Image representLeft;
    public Image representRight;

    [SerializeField] Image expBar;
    [SerializeField] Text expText;
    [SerializeField] Text playerNameText;
    [SerializeField] Text playerLevelText;

    public Button startButton;
    [Header("Common")]
    public Button exitButton;

    [Header("TitlePanel")]
    public GameObject[] panels;
    public Button optionBtn;

    //[Header("TopPanel")]
    //public GameObject topPanel;

    [Header("GamePanel")]
    public GameObject gamePanel;

    [Header("WorldPanel")]
    public GameObject worldPanel;

    // resource
    //-----------------------------------------
    [Header("Resources")]
    public Text goldText;
    public Text diamondText;

    // factory
    //-----------------------------------------
    [Header("Factory")]
    public GameObject factoryImageParents;
    [HideInInspector] public List<Image> f_HeadImages = new List<Image>();
    [HideInInspector] public List<Image> f_BodyImages = new List<Image>();
    [HideInInspector] public List<Image> f_ShoeImages = new List<Image>();

    [HideInInspector] public List<Text> f_IDText = new List<Text>();
    [HideInInspector] public List<List<Text>> f_statText = new List<List<Text>>();
    [HideInInspector] public List<Button> f_RepresentBtn = new List<Button>();
    public List<Text> f_ScrollRankText = new List<Text>();
    public List<Text> f_ScrollNameText = new List<Text>();

    public List<Button> factoryBtn = new List<Button>();
    public GameObject factoryPanel;
    //팩토리 추가
    //-----------------------------------------------------------
    [Header("Factory Add")]
    public GameObject addFactoryPanel;
    public Button[] f_AddButtons = new Button[2];
    public Button open_F_PanelBtn;
    public Text f_AddPriceText;
    //-----------------------------------------

    // inventory
    //-----------------------------------------
    [Header("InventoryPanel")]
    public GameObject inventoryUI;      // 아이템 UI창

    public float bButtonSize;                  // 선택된 버튼 크기
    public float sButtonSize;                  // 선택되지 않은 버튼들 크기
    public int bTextSize;                    // 선택된 텍스트 사이즈
    public int sTextSize;                    // 선택되지 않은 텍스트 사이즈
    public Image alarmPanel;
    public Text alarmText;
    // 팩토리 아이템 정보
    //-----------------------------------------------------------
    [Header("FactoryItemInfo")]
    [SerializeField]
    Image[] itemImage;
    [SerializeField]
    Text[] itemName;
    [SerializeField]
    Text[] itemRank;
    [SerializeField]
    GameObject[] itemInfoParent;
    GameObject[,] infoIcons = new GameObject[4, 5];

    [SerializeField] Text scrollInfoText;
    public Sprite null_ItemImage;

    //-----------------------------------------------------------
    [Header("FactoryAnimUI")]
    public Image animHeadImage;
    public Image animBodyImage;
    public Image animShoeImage;

    public Sprite defaultHeadImage;
    public Sprite defaultShoeImage;
    public Sprite defaultShoeSingleImage;
    public Sprite defaultBodyImage;
    //-----------------------------------------------------------
    [Header("CombinePanel")]
    public Text combineText;
    //-----------------------------------------------------------
    [Header("Shop")]
    public Button[] headPickBtn = new Button[6];
    public Button[] scrollPickBtn = new Button[6];
    public Button[] equipmentPickBtn = new Button[6];

    public Button openGoldBtn;
    public Button openDiaBtn;
    //----------------------------------------------------
    [Header("ItemRankMat")]
    public Material[] itemRankMat;
    public Material pick_LegendMat;
    //-----------------------------------------------------------
    public GameObject magic_popUpPanel;   //마법 패널
    //장착된 마법
    public Image[] magic_EquipedMagic_Icon;
    public Text[] magic_EquipedMagic_Name;
    public Text[] magic_EquipedMagic_Count;

    public Image magic_Icon;
    public Text magic_Name_Text;
    public Text magic_Level_Text;
    public Text magic_LevelUpPrice_Text;
    public Text magic_Description_Text;
    public Text magic_PossessCount_Text;
    public Text magic_PerchasePrice_Text;
    public Text magic_PerchaseAmount_Text;
    public Text magic_StatText;
    public Text magic_NextLevelStatText;
    public Button magic_LevelUp_Btn;
    public Button magic_Purchase_Btn;
    public Button magic_Equip_Btn;
    public Button magic_UnEquip_Btn;
    public Button magic_IncreaseAmount_Btn;
    public Button magic_DecreaseAmount_Btn;
    //-----------------------------------------------------------
    [Header("WorldClear Panel")]
    public Button worldClearPanel;
    //------------------------------------------------------------

    public GameObject optionPanel;
    //열려있는 패널들 관리
    public Stack<GameObject> currPanel { get; private set; }

    [HideInInspector] public bool isInvenOpen;
    bool startCor;    //알람 코루틴 끝났는지 체크용

    [SerializeField] InventoryUI invenUI;

    [Header("ExitGame")]
    [SerializeField] GameObject exitPanel;
    [SerializeField] Button exitGameButton;
    [SerializeField] Button undoButton;

    //ResourceEffectPool
    List<GameObject> goldEffPool = new List<GameObject>();
    List<GameObject> lugaEffPool = new List<GameObject>();

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }

    private void Start()
    {
        if (DataController.isContinue)
        {
            PushInStack(worldPanel);
            worldPanel.SetActive(true);
        }
    }
    public void Init()
    {
        startButton.onClick.AddListener(() => PushInStack(worldPanel));
        startButton.onClick.AddListener(() => worldPanel.SetActive(true));

        for (int i = 0; i < itemInfoParent.Length; i++)
        {
            for (int j = 0; j < itemInfoParent[i].transform.childCount; j++)
            {
                infoIcons[i, j] = itemInfoParent[i].transform.GetChild(j).gameObject;
                infoIcons[i, j].SetActive(false);
            }
        }

        currPanel = new Stack<GameObject>();

        worldClearPanel.onClick.AddListener(() => worldClearPanel.gameObject.SetActive(false));
        if (DataController.checkWorldClear)
        {
            worldClearPanel.gameObject.SetActive(true);
        }
        //팩토리 구입 버튼
        f_AddButtons[0].onClick.AddListener(() =>
        {
            LobbyManager.Instance.factoryManager.AddFactory();
            StartCoroutine(PanelAnim(addFactoryPanel, true));
        });
        //팩토리 늘리기 나가기버튼
        f_AddButtons[1].onClick.AddListener(() =>
        {
            StartCoroutine(PanelAnim(addFactoryPanel, true));
        });

        open_F_PanelBtn.onClick.AddListener(() =>
        {
            StartCoroutine(PanelAnim(addFactoryPanel, false));
            f_AddPriceText.text = LobbyManager.Instance.factoryManager.factoryPrice.ToString();

        });

        optionBtn.onClick.AddListener(() => optionPanel.SetActive(optionPanel.activeSelf == false ? true : false));
        ShowPlayerLevel();

        //상단의 다이아, 골드 (+) 버튼 누르면 상점창 나타나게
        openGoldBtn.onClick.AddListener(() =>
        {
            GameObject shopPanel = GameObject.Find("Canvas").transform.Find("ShopPanel").gameObject;
            if (shopPanel.activeSelf == false)
            {
                MenuTabClick(3);
                shopPanel.transform.GetChild(1).GetChild(0).GetChild(0).localPosition = new Vector3(0, 1265);
            }
        });
        openDiaBtn.onClick.AddListener(() =>
        {
            GameObject shopPanel = GameObject.Find("Canvas").transform.Find("ShopPanel").gameObject;
            if (shopPanel.activeSelf == false)
            {
                MenuTabClick(3);
                shopPanel.transform.GetChild(1).GetChild(0).GetChild(0).localPosition = new Vector3(0, 2452);
            }
        });

        //게임종료 버튼
        exitGameButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.SaveData();   //저장
            Application.Quit();
        });

        undoButton.onClick.AddListener(() => StartCoroutine(PanelAnim(exitPanel, true)));


        //자원 이팩트 풀 초기화
        Transform lugaPool = GameObject.Find("LugaPool").transform;
        for (int i = 0; i < lugaPool.childCount; i++)
        {
            lugaEffPool.Add(lugaPool.GetChild(i).gameObject);
            lugaEffPool[i].SetActive(false);
        }

        Transform goldPool = GameObject.Find("GoldPool").transform;
        for (int i = 0; i < goldPool.childCount; i++)
        {
            goldEffPool.Add(goldPool.GetChild(i).gameObject);
            goldEffPool[i].SetActive(false);
        }

        StartCoroutine(CheckInveToryOpen());
    }
    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (isInvenOpen)
                {
                    onIventoryClosedCallback?.Invoke();
                }
                else if (currPanel.Count > 0) ExitPanel();
                else if (currPanel.Count.Equals(0))
                {
                    StartCoroutine(PanelAnim(exitPanel, false));
                    //나가시겠습니까? 팝업창
                }
            }
        }
    }

    void ShowPlayerLevel()
    {
        if (UserData.Instance.userdata.level / 10 == 0) playerLevelText.text = "Lv.0" + UserData.Instance.userdata.level.ToString();
        else playerLevelText.text = "Lv." + UserData.Instance.userdata.level.ToString();

        if (UserData.Instance.userdata.level == 30)
        {
            expText.text = string.Empty;
            expBar.fillAmount = 1f;
        }
        else
        {
            expText.text = UserData.Instance.userdata.exp.ToString() + "/" + DBManager.Instance.expDB.ExpData[UserData.Instance.userdata.level][0];
            expBar.fillAmount = (float)UserData.Instance.userdata.exp / DBManager.Instance.expDB.ExpData[UserData.Instance.userdata.level][0] * 100f * 0.01f;
        }

    }

    //팩토리창 이미지 설정
    public void SetFactoryUI(int _Idx)
    {
        f_IDText.Add(factoryImageParents.transform.GetChild(_Idx).GetChild(7).GetComponent<Text>());

        //팩토리 캐릭터 이미지들
        f_HeadImages.Add(factoryImageParents.transform.GetChild(_Idx).GetChild(0).GetChild(2).GetComponent<Image>());
        f_BodyImages.Add(factoryImageParents.transform.GetChild(_Idx).GetChild(0).GetChild(1).GetComponent<Image>());
        f_ShoeImages.Add(factoryImageParents.transform.GetChild(_Idx).GetChild(0).GetChild(0).GetComponent<Image>());

        f_statText.Add(new List<Text>());

        //스탯 아이콘 및 텍스트
        GameObject statParent = factoryImageParents.transform.GetChild(_Idx).GetChild(2).GetChild(1).gameObject;
        for (int j = 0; j < statParent.transform.childCount; j++)
        {
            f_statText[_Idx].Add(statParent.transform.GetChild(j).GetChild(0).GetComponent<Text>());
        }

        //스크롤 랭크
        f_ScrollRankText.Add(factoryImageParents.transform.GetChild(_Idx).GetChild(4).GetChild(1).GetChild(0).GetComponent<Text>());
        //스크롤 이름
        f_ScrollNameText.Add(factoryImageParents.transform.GetChild(_Idx).GetChild(4).GetChild(0).GetComponent<Text>());

        factoryBtn.Add(factoryImageParents.transform.GetChild(_Idx).GetChild(1).GetComponent<Button>());
        int temp = _Idx;
        factoryBtn[_Idx].onClick.AddListener(() => OpenFactorySetting(temp));

        //대표공장 버튼
        f_RepresentBtn.Add(factoryImageParents.transform.GetChild(_Idx).GetChild(3).GetComponent<Button>());
        f_RepresentBtn[_Idx].onClick.AddListener(() => LobbyManager.Instance.factoryManager.SetRepresntFactory(_Idx));

    }

    //각 팩토리 패널을 열어주는 함수
    public void OpenFactorySetting(int _Num)
    {
        if (LobbyManager.Instance.factoryManager.factories.Count > _Num)
        {
            factoryPanel.SetActive(true);
            PushInStack(factoryPanel);
            LobbyManager.Instance.factoryManager.FactorySetting(_Num);
            UpdateFactoryItemInfo(_Num);
            f_AddButtons[1].onClick?.Invoke();
        }
    }

    // 팩토리에 장착된 아이템 UI표시
    public void UpdateFactoryItemInfo(int _Index)
    {
        LobbyManager.Instance.factoryManager.currFactoryIndex = _Index;
        Item[] temp = LobbyManager.Instance.factoryManager.factories[_Index].equipedItems;
        if (temp.Length > 0)
        {
            for (int i = 0; i < 4; i++)
            {
                if (temp[i] != null)
                {
                    itemImage[i].sprite = temp[i].image;
                    itemName[i].text = temp[i].itemName;
                    itemRank[i].text = ChangeItemNameColor(temp[i].itemRank);

                    ActivateFactoyIcon(temp[i], i);
                }
                else
                {
                    itemImage[i].sprite = null_ItemImage;
                    itemName[i].text = null;
                    itemRank[i].text = null;
                    ActivateFactoyIcon(null, i);
                }
            }
        }
    }

    //열린 패널들 스택에 넣고 이전 패널 끄는 함수
    void PushInStack(GameObject _Obj)
    {
        if (exitButton.gameObject.activeSelf.Equals(false))
            exitButton.gameObject.SetActive(true);

        if (currPanel.Count > 0)
        {
            GameObject temp = currPanel.Peek();
            temp.SetActive(false);
        }
        optionBtn.gameObject.SetActive(false);
        currPanel.Push(_Obj);
    }

    //팩토리관리창 총 정보들
    public void UpdateFactoryUIInfo(int _Num)
    {
        f_IDText[_Num].text = (LobbyManager.Instance.factoryManager.factories[_Num].factoryId + 1).ToString();
        f_HeadImages[_Num].sprite = LobbyManager.Instance.factoryManager.factories[_Num].equipedItems[0]?.image ?? defaultHeadImage;
        f_BodyImages[_Num].sprite = LobbyManager.Instance.factoryManager.factories[_Num].equipedItems[2]?.image ?? defaultBodyImage;
        f_ShoeImages[_Num].sprite = LobbyManager.Instance.factoryManager.factories[_Num].equipedItems[3]?.image ?? defaultShoeImage;


        f_statText[_Num][0].text = (LobbyManager.Instance.factoryManager.factories[_Num].unit.unitStat.hp).ToString();
        f_statText[_Num][1].text = ((int)LobbyManager.Instance.factoryManager.factories[_Num].unit.unitStat.defense).ToString();
        f_statText[_Num][2].text = ((int)LobbyManager.Instance.factoryManager.factories[_Num].unit.unitStat.evade).ToString();
        f_statText[_Num][3].text = ((int)LobbyManager.Instance.factoryManager.factories[_Num].unit.unitStat.speed).ToString();
        f_statText[_Num][4].text = "";

        if (LobbyManager.Instance.factoryManager.factories[_Num].equipedItems[1] != null)
        {
            f_ScrollRankText[_Num].text = ChangeItemNameColor(LobbyManager.Instance.factoryManager.factories[_Num].equipedItems[1].itemRank);
            f_ScrollNameText[_Num].text = LobbyManager.Instance.factoryManager.factories[_Num].equipedItems[1].itemName;
        }
        else
        {
            f_ScrollRankText[_Num].text = "";
            f_ScrollNameText[_Num].text = "";
        }
    }

    //팩토리 아이템 정보 아이템 아이콘 활성
    void ActivateFactoyIcon(Item _Item, int _Idx)
    {
        if (_Item != null)
        {
            //스크롤일때
            if (_Item.itemType.Equals(Item.ItemType.CHARACTERISTIC))
            {
                infoIcons[_Idx, 0].SetActive(true);
                scrollInfoText.text = _Item.description;
            }
            //나머지 장비들
            else if (_Item.GetType().Equals(typeof(EquipMent)))
            {
                EquipMent temp = _Item as EquipMent;

                if (temp.hp != 0) { infoIcons[_Idx, 0].SetActive(true); infoIcons[_Idx, 0].GetComponentInChildren<Text>().text = temp.hp.ToString(); }
                else { infoIcons[_Idx, 0].SetActive(false); }

                if (temp.defense != 0) { infoIcons[_Idx, 1].SetActive(true); infoIcons[_Idx, 1].GetComponentInChildren<Text>().text = ((int)temp.defense).ToString(); }
                else { infoIcons[_Idx, 1].SetActive(false); }

                if (temp.evade != 0) { infoIcons[_Idx, 2].SetActive(true); infoIcons[_Idx, 2].GetComponentInChildren<Text>().text = ((int)temp.evade).ToString(); }
                else { infoIcons[_Idx, 2].SetActive(false); }

                if (temp.speed != 0) { infoIcons[_Idx, 3].SetActive(true); infoIcons[_Idx, 3].GetComponentInChildren<Text>().text = ((int)temp.speed).ToString(); }
                else { infoIcons[_Idx, 3].SetActive(false); }

            }
            else
            {
                Head temp = _Item as Head;

                if (temp.hp != 0) { infoIcons[_Idx, 0].SetActive(true); infoIcons[_Idx, 0].GetComponentInChildren<Text>().text = temp.hp.ToString(); }
                else { infoIcons[_Idx, 0].SetActive(false); }

                if (temp.defense != 0) { infoIcons[_Idx, 1].SetActive(true); infoIcons[_Idx, 1].GetComponentInChildren<Text>().text = temp.defense.ToString() + "%"; }
                else { infoIcons[_Idx, 1].SetActive(false); }

                if (temp.evade != 0) { infoIcons[_Idx, 2].SetActive(true); infoIcons[_Idx, 2].GetComponentInChildren<Text>().text = temp.evade.ToString() + "%"; }
                else { infoIcons[_Idx, 2].SetActive(false); }

                if (temp.speed != 0) { infoIcons[_Idx, 3].SetActive(true); infoIcons[_Idx, 3].GetComponentInChildren<Text>().text = temp.speed.ToString() + "%"; }
                else { infoIcons[_Idx, 3].SetActive(false); }

                if (temp.allStats != 0) { infoIcons[_Idx, 4].SetActive(true); infoIcons[_Idx, 4].GetComponentInChildren<Text>().text = temp.allStats.ToString(); }
                else { infoIcons[_Idx, 4].SetActive(false); }
            }

        }
        else
        {
            if (_Idx.Equals(1))
            {
                infoIcons[_Idx, 0].SetActive(false);
                scrollInfoText.text = null;
                return;
            }

            for (int i = 0; i < 5; i++)
                infoIcons[_Idx, i].SetActive(false);
        }
    }
    //메인화면 메뉴 버튼선택 함수
    public void MenuTabClick(int _N)
    {
        panels[_N].gameObject.SetActive(true);
        //optionBtn.gameObject.SetActive(false);
        PushInStack(panels[_N].gameObject);
        gamePanel.SetActive(false);

        if (_N.Equals(0)) addFactoryPanel.transform.localScale = Vector2.zero;
        else if (_N.Equals(1)) magic_popUpPanel.SetActive(false);
    }

    //패널들을 끄는 함수
    public void ExitPanel()
    {

        //열려있는 패널을 끈다
        GameObject closePanel = currPanel.Pop();
        closePanel.SetActive(false);

        if (currPanel.Count > 0)
        {
            //마지막 패널이 아니면 전의 패널을 킨다.
            GameObject openPanel = currPanel.Peek();
            openPanel.SetActive(true);
        }
        else
        {
            exitButton.gameObject.SetActive(false);
            optionBtn.gameObject.SetActive(true);
            gamePanel.SetActive(true);
        }

    }

    //아이템 등급에 따라 색깔 다르게 주기
    public string ChangeItemNameColor(Item.ItemRank _ItemRank)
    {
        string text = "";
        switch (_ItemRank)
        {
            case Item.ItemRank.NORMAL:
                text = "<color=#000000>노말</color>";
                break;
            case Item.ItemRank.MAGIC:
                text = "<color=#0868ac>매직</color>";
                break;
            case Item.ItemRank.RARE:
                text = "<color=#0f8d35>레어</color>";
                break;
            case Item.ItemRank.EPIC:
                text = "<color=#4a017d>에픽</color>";
                break;
            case Item.ItemRank.LEGEND:
                text = "<color=#aa0508>레전드</color>";
                break;
        }
        return text;
    }

    //팩토리에 장착한 아이템 애니메이션용 이미지 설정
    public void SetAnimItemImage(int _ItemType, Item _Item, bool _Exist)
    {
        if (_Exist)
        {
            if (_ItemType.Equals(0))
            {
                animHeadImage.sprite = _Item.image;
            }
            else if (_ItemType.Equals(2))
            {
                animBodyImage.sprite = _Item.image;

            }
            else if (_ItemType.Equals(3))
            {
                animShoeImage.sprite = _Item.image;
            }
        }
        else
        {
            if (_ItemType.Equals(0))
            {
                animHeadImage.sprite = defaultHeadImage;
            }
            else if (_ItemType.Equals(2))
            {
                animBodyImage.sprite = defaultBodyImage;

            }
            else if (_ItemType.Equals(3))
            {
                animShoeImage.sprite = defaultShoeImage;
            }
        }

    }

    //메세지 페이드 아웃 효과
    public void ShowEquipedAlarm(float _Start, float End)
    {
        if (!startCor)
        {
            alarmPanel.gameObject.SetActive(true);
            StartCoroutine(ShowAlarmPanel(_Start, End));
            startCor = true;
        }
    }

    public void ShowAlarm(string _String)
    {
        alarmText.text = _String;
        ShowEquipedAlarm(0, 1);
    }

    //자원증가 연출
    public void StartResourceEff(bool _IsGold, int _Rank, int _ResourceAmount)
    {
        int num = 0;
        if (_Rank.Equals(0)) num = 15;
        else if (_Rank.Equals(1)) num = 30;
        else if (_Rank.Equals(2)) num = 50;

        if (_IsGold)
        {
            for (int i = 0; i < num; i++)
            {
                goldEffPool[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < num; i++)
            {
                lugaEffPool[i].SetActive(true);
            }

        }
        StartCoroutine(IncreaseResourceAnim(_IsGold, _ResourceAmount));

    }

    //창 등장 애니메이션(나중에 수정 shop에도 같은 함수)
    public IEnumerator PanelAnim(GameObject _Target, bool _IsActive)
    {
        TweenParams tParms = new TweenParams().SetEase(Ease.Linear);

        //켜진상태
        if (_IsActive)
        {
            yield return _Target.transform.DOScale(new Vector3(0, 0, 0), 0.1f).SetAs(tParms).WaitForCompletion();
        }
        else
        {
            yield return _Target.transform.DOScale(new Vector3(1, 1, 1), 0.1f).SetAs(tParms).WaitForCompletion();
        }

    }
    IEnumerator CheckInveToryOpen()
    {
        yield return new WaitUntil(() => isInvenOpen);
        inventoryUI.SetActive(true);
        optionBtn.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);

        if (gamePanel.activeSelf)
            gamePanel.SetActive(false);

        if (currPanel.Count > 0)
        {
            GameObject temp = currPanel.Peek();
            temp.SetActive(false);
        }
        StartCoroutine(CheckInveToryClose());
    }

    IEnumerator CheckInveToryClose()
    {
        yield return new WaitUntil(() => !isInvenOpen);
        inventoryUI.SetActive(false);
        if (currPanel.Count > 0)
        {
            GameObject temp = currPanel.Peek();
            temp.SetActive(true);
            exitButton.gameObject.SetActive(true);
        }
        else
        {
            optionBtn.gameObject.SetActive(true);
            if (!gamePanel.activeSelf)
                gamePanel.SetActive(true);
        }

        StartCoroutine(CheckInveToryOpen());
    }

    IEnumerator ShowAlarmPanel(float _Start, float _End)
    {

        float currenTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currenTime += Time.deltaTime;
            percent = currenTime / 1f;

            Color color = alarmPanel.color;
            Color texColor = alarmText.color;

            color.a = Mathf.Lerp(_Start, _End, percent);
            texColor.a = Mathf.Lerp(_Start, _End, percent);

            alarmPanel.color = color;
            alarmText.color = texColor;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        alarmPanel.gameObject.SetActive(false);
        startCor = false;
    }

    //자원 증가 애니메이션
    public IEnumerator IncreaseResourceAnim(bool _IsGold, int _Resource)
    {
        yield return new WaitForSeconds(.5f);

        float duration = 0.4f; // 카운팅에 걸리는 시간 설정. 
        float current = _IsGold ? LobbyManager.Instance.Gold : LobbyManager.Instance.Luga;
        float target = _IsGold ? LobbyManager.Instance.Gold + _Resource : LobbyManager.Instance.Luga + _Resource;
        float offset = (target - current) / duration;
        Text text = _IsGold ? goldText : diamondText;

        while (current < target)
        {
            current += offset * Time.deltaTime;
            if (_IsGold) text.text = ((int)current).ToString();
            else text.text = ((int)current).ToString();

            yield return null;
        }
            
        if(_IsGold) LobbyManager.Instance.Gold += _Resource;
        else LobbyManager.Instance.Luga += _Resource;
    }
}
