using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InGameUI : MonoBehaviour
{
    public delegate void OnSaveSound();
    public OnSaveSound saveSoundCallback;

    [SerializeField] GameObject exitPanel;
    [SerializeField] Button exitButton;
    [SerializeField] Text stageText;
    [SerializeField] Text targetCountText;
    [SerializeField] Text populationText;
    [SerializeField] Image populationSlider;     // 인구수 슬라이드 이미지
    public Text timerText;
    public Image coolTimeImg;
    [Header("Result UI")]
    //--------------------------------------------------------------------
    Text goldText;
    Text lugaText;
    Text levelText;

    GameObject itemParent;
    Text itemNameText;
    Image f_itemImg;
    Image expBar;

    [SerializeField] GameObject[] resultPanel;  //결과창
    [SerializeField] GameObject skillSlotPrefab; //사용한 스킬창 프리팹
    [SerializeField] GameObject factorySlotPrefab;  //사용한 유닛창 프리팹
    [SerializeField] Transform skillSlotParent;
    [SerializeField] Transform factorySlotPraent;
    [SerializeField] GameObject f_GoldPos;   // 첫클리어
    [SerializeField] GameObject goldPos2;    // 그 후
    [SerializeField] GameObject f_LugaPos;   // 첫클리어
    [SerializeField] GameObject lugaPos2;    // 그 후
    [SerializeField] Button continueBtn;    // 계속하기 버튼 (월드창으로)
    [SerializeField] Button gotoMainBtn;    // 나가기 버튼 (메인창으로)
    [SerializeField] Button restartBtn;     // 다시하기 버튼
    [SerializeField] GameObject resultUnitslot; //결과창 유닛슬롯
    [SerializeField] GameObject stageResultPanel;
    //--------------------------------------------------------------------
    [Header("Option")]
    public GameObject optionPanel;
    public Button gameSpeedBtn;
    public Button optionBtn;
    public Text gameSpeedText;
    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
    void Start()
    {
        // 인게임 UI
        //---------------------------------
        stageText.text = DBManager.Instance.worldDB.DataBase[DataController.CurrentStage].stageName;
        populationText.text = GameManager.Instance.curPop.ToString() + " / " + GameManager.Instance.limitPop.ToString();
        targetCountText.text = GameManager.Instance.targetCount.ToString();
        InvokeRepeating("UpdateUI", 0, 0.1f);
        //------------------------------------
        // 결과창

        InitResultUI(stageResultPanel);

        continueBtn.onClick.AddListener(() =>
        {
            DataController.isContinue = true;
            DataController.checkGameLoad = true;
            GameManager.Instance.FadeInToMainScene();
            saveSoundCallback?.Invoke();
        });
        gotoMainBtn.onClick.AddListener(() =>
        {
            DataController.isContinue = false;
            DataController.checkGameLoad = true;
            GameManager.Instance.FadeInToMainScene();
            saveSoundCallback?.Invoke();
        });
        restartBtn.onClick.AddListener(() =>
        {
            DataController.isContinue = false;
            DataController.checkGameLoad = true;
            GameManager.Instance.FadeInToMainScene();
            saveSoundCallback?.Invoke();
        });
        exitButton.onClick.AddListener(() => { optionPanel.SetActive(false); exitPanel.SetActive(true); Time.timeScale = 0; });
        optionBtn.onClick.AddListener(() => optionPanel.SetActive(optionPanel.activeSelf == false ? true : false));

        if (!UserData.Instance.userdata.isFinishTutorial) exitButton.interactable = false;
    }

    void UpdateUI()
    {
        populationSlider.fillAmount = (float)GameManager.Instance.curPop / GameManager.Instance.limitPop * 100f * 0.01f;
        populationText.text = GameManager.Instance.curPop.ToString() + " / " + GameManager.Instance.limitPop.ToString();
        targetCountText.text = GameManager.Instance.targetCount.ToString();
    }
    //결과창 초기화
    void InitResultUI(GameObject _Parent)
    {
        goldText = _Parent.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>();
        lugaText = _Parent.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<Text>();
        levelText = _Parent.transform.GetChild(2).GetChild(6).GetComponent<Text>();
        itemParent = _Parent.transform.GetChild(2).GetChild(0).gameObject;
        itemNameText = itemParent.transform.GetChild(1).GetComponent<Text>();
        f_itemImg = itemParent.transform.GetChild(0).GetComponent<Image>();
        expBar = _Parent.transform.GetChild(2).GetChild(5).GetChild(0).GetComponent<Image>();
       // _Parent.SetActive(false);
    }

    //결과창 애니매이션
    IEnumerator ResultAnimation(GameObject _Panel, bool _Clear, bool _FirstClear)
    {
        itemParent.SetActive(false); 
        _Panel.transform.localScale = Vector3.zero;
        yield return _Panel.transform.DOScale(new Vector3(1, 1, 1), 0.3f).WaitForCompletion();
        yield return new WaitForSeconds(0.2f);

        var target = DBManager.Instance.worldDB.DataBase[DataController.CurrentStage];
        DataController.checkWorldClear = false;

        int gold = 0;
        int luga = 0;
        int level = UserData.Instance.userdata.level;

        levelText.text = "Lv " + level.ToString();
        
        //골드를 실제 유저 정보에 저장해야된다
        //첫 클리어
        if (_FirstClear)
        {
            itemParent.SetActive(true);
            lugaText.transform.parent.position = f_LugaPos.transform.position;
            goldText.transform.parent.position = f_GoldPos.transform.position;

            //첫보상 = 첫보상 + 보상
            gold = target.firstClear_Gold + Random.Range(target.clear_MinGold, target.clear_MaxGold + 1);
            luga = target.firstClear_Luga + Random.Range(target.clear_MinLuga, target.clear_MaxLuga + 1);

            StartCoroutine(ResourceTextAnim(gold, goldText));    // 골드 증가
            StartCoroutine(ResourceTextAnim(luga, lugaText));    // 다이아 증가
            if (level < 29) StartCoroutine(IncreaseExpAnim(true));              // 경험치 증가

            //튜토리얼만 아이템 확정 지급
            if(DataController.CurrentStage == 0)
            {
                GiveTutorialItem();
            }
            else
            {
                GetFirstClearItem();                                //아이템 
            }
           
            DBManager.Instance.worldDB.DataBase[DataController.CurrentStage].isClear = true;

            //0 ~ 9 9탄 
            if (DataController.CurrentStage / 9 > 0 && DataController.CurrentStage % 10 == 9) DataController.checkWorldClear = true;
        }
        // 첫 클리어 아닐때
        else if (_Clear)
        {
            gold = Random.Range(target.clear_MinGold, target.clear_MaxGold + 1);
            luga = Random.Range(target.clear_MinLuga, target.clear_MaxLuga + 1);

            goldText.transform.parent.position = goldPos2.transform.position;
            lugaText.transform.parent.position = lugaPos2.transform.position;

            StartCoroutine(ResourceTextAnim(gold, goldText));    // 골드 증가
            StartCoroutine(ResourceTextAnim(luga, lugaText));    // 다이아 증가

            if (level < 29) StartCoroutine(IncreaseExpAnim());              // 경험치 증가
        }
        //클리어 실패
        else
        {
            goldText.transform.parent.position = goldPos2.transform.position;
            lugaText.transform.parent.position = lugaPos2.transform.position;

            if (level < 29) expBar.fillAmount = UserData.Instance.userdata.exp / DBManager.Instance.expDB.ExpData[level][0] * 100f * 0.01f;
        }

        DataController.rewardGold += gold;
        DataController.rewardLuga += luga;

        StartCoroutine(ShowUseSlots());
    }
    //튜토리얼 아이템 지급
    void GiveTutorialItem()
    {
        Item tutorialItem = (Item)DBManager.Instance.itemDB.equipDB["NORMAL"]["7004"];
        DataController.getItem = tutorialItem;
        f_itemImg.sprite = tutorialItem.image;
        ItemColorName(tutorialItem);
    }


    //사용한 스킬 , 유닛슬롯 보여줌
    IEnumerator ShowUseSlots()
    {
        for (int i = 0; i < GameManager.Instance.magicController.MagicSlots.Count; i++)
        {
            if (GameManager.Instance.magicController.MagicSlots[i].gameObject.activeSelf == false) continue;
            GameObject obj = Instantiate(skillSlotPrefab, skillSlotParent);
            obj.transform.GetChild(1).GetComponent<Image>().sprite = GameManager.Instance.magicController.MagicSlots[i].MagicIcon.sprite;    //스킬이미지
            obj.transform.GetChild(2).GetComponent<Text>().text = GameManager.Instance.magicController.MagicSlots[i].MagicName.text;         //스킬이름
            obj.transform.GetChild(3).GetComponent<Text>().text = GameManager.Instance.magicController.MagicSlots[i].clickCnt.ToString();    //클릭회수
            yield return new WaitForSeconds(.1f);
        }
        resultUnitslot.SetActive(true);
        for (int i = 0; i < GameManager.Instance.UnitSlots.Count; i++)
        {
            GameObject obj = Instantiate(factorySlotPrefab, factorySlotPraent);
            obj.transform.GetChild(2).GetComponent<Text>().text = "공장 " + (i + 1);                                         //공장 번호
            obj.transform.GetChild(3).GetComponent<Text>().text = GameManager.Instance.UnitSlots[i].clickCnt.ToString();     //클릭회수        
            obj.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = GameManager.Instance.UnitSlots[i].shoeImg.sprite;   //신발
            obj.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = GameManager.Instance.UnitSlots[i].armorImg.sprite;   //몸통
            obj.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = GameManager.Instance.UnitSlots[i].headImg.sprite;   //머리
            yield return new WaitForSeconds(.1f);
        }
    }

    //경험치 증가효과 코루틴
    IEnumerator IncreaseExpAnim(bool _First = false)
    {
        float currentExp = UserData.Instance.userdata.exp;   //현재 경험치
        float targetExp = 0;
        levelText.text = "Lv " + UserData.Instance.userdata.level.ToString();

        var target = DBManager.Instance.worldDB.DataBase[DataController.CurrentStage];
        int targetLevelExp = DBManager.Instance.expDB.ExpData[UserData.Instance.userdata.level][0];     //레벨업에 필요한 경험치

        if (_First)
        {
            targetExp = (currentExp + target.firstClear_Exp + target.clear_Exp);    //애니메이션 목표 경험치
            UserData.Instance.userdata.exp += target.firstClear_Exp + target.clear_Exp;
        }
        else
        {
            targetExp = currentExp + target.clear_Exp;
            UserData.Instance.userdata.exp += target.clear_Exp;
        }
            
        float offset = targetExp - currentExp;
        
        while (currentExp < targetExp)
        {
            currentExp += offset * Time.deltaTime;
            expBar.fillAmount = currentExp / targetLevelExp * 100f * 0.01f;

            //렙업
            if (currentExp >= targetLevelExp)
            {
                expBar.fillAmount = 0;
                LevelUp(targetLevelExp);

                targetExp -= targetLevelExp;
                currentExp = 0;
                targetLevelExp = DBManager.Instance.expDB.ExpData[UserData.Instance.userdata.level][0]; //목표 최대 경험치량 변경           
            }
            yield return null;
        }
    }

    void LevelUp(int _TargetLevelExp)
    {
        UserData.Instance.userdata.level++;

        if (UserData.Instance.userdata.exp - _TargetLevelExp > 0) UserData.Instance.userdata.exp -= _TargetLevelExp;
        else UserData.Instance.userdata.exp = 0;
        levelText.text = "Lv " + UserData.Instance.userdata.level.ToString();
        //숫자 커졌다 작아졌다 하는 애니메이션
        TweenParams tParms = new TweenParams().SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad);
        levelText.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f).SetAs(tParms);
    }

    //자원 증가 애니메이션
    IEnumerator ResourceTextAnim(float _Resource, Text _Text)
    {
        float duration = 0.5f; // 카운팅에 걸리는 시간 설정. 
        float current = 0;
        float target = _Resource;
        float offset = (target - current) / duration;
        while (current < target)
        {
            current += offset * Time.deltaTime;
            _Text.text = ((int)current).ToString();
            yield return null;

        }
        current = target;
        _Text.text = ((int)current).ToString();
    }

    //결과창 보여주는 함 수
    public void ShowResult(bool _Clear, bool _FirstClear = false)
    {
        
        GameObject.Find("UnitContainer").gameObject.SetActive(false);   //유닛담겨져있는 컨테이너 꺼준다
        resultPanel[0].SetActive(true);
        if (_Clear) { resultPanel[1].SetActive(true); resultPanel[2].SetActive(false); }//클리어
        else { resultPanel[1].SetActive(false); resultPanel[2].SetActive(true); }  //실패

        if (GameManager.Instance.min > 0) timerText.text = GameManager.Instance.min + "분" + GameManager.Instance.sec + "초";
        else timerText.text = GameManager.Instance.sec + "초";

        if (!UserData.Instance.userdata.isFinishTutorial)
        {
            continueBtn.interactable = false;    // 계속하기 버튼 (월드창으로)
            restartBtn.interactable = false;     // 다시하기 버튼
            DataController.PlayerMagic.Clear();       //튜토리얼중이면 마법리스트를 클리어해줌 -> 장착마법으로 들어가지기 때문
        }
            
        StartCoroutine(ResultAnimation(resultPanel[0], _Clear, _FirstClear));
    }

    void GetFirstClearItem()
    {
        int randIdx = Random.Range(0,2);   // 랜덤 아이템 종류
        if (randIdx == 1) randIdx = 2;

        float[] itemPickRate = { 50, 30, 15, 4.99f, 0.01f }; //99.9
                                                             
        float pickRate = 0;
        // 뽑힐 확률값
        float weight = 0;
        int randRank = 0;
        for (int i = 0; i < itemPickRate.Length; i++)
        {
            pickRate += itemPickRate[i];
        }

        //총 확률
        weight = Random.Range(0, pickRate);

        //등급 index 선택함
        for (int i = 0; i < itemPickRate.Length; i++)
        {
            if (itemPickRate[i] >= weight)
            {
                randRank = i+1;
                break;
            }
            weight -= itemPickRate[i];
        }
        Item randomItem = DBManager.Instance.itemDB.GetRandomItem(randIdx, randRank,true);
        DataController.getItem = randomItem;
        f_itemImg.sprite = randomItem.image;

        ItemColorName(randomItem);
    }
    void ItemColorName(Item _RanomItem)
    {
        switch (_RanomItem.itemRank)
        {
            case Item.ItemRank.NORMAL:
                itemNameText.text = "<color=#000000>" + _RanomItem.itemName + "</color>";
                break;
            case Item.ItemRank.MAGIC:
                itemNameText.text = "<color=#0868ac>" + _RanomItem.itemName + "</color>";
                break;
            case Item.ItemRank.RARE:
                itemNameText.text = "<color=#0f8d35>" + _RanomItem.itemName + "</color>";
                break;
            case Item.ItemRank.EPIC:
                itemNameText.text = "<color=#4a017d>" + _RanomItem.itemName + "</color>";
                break;
            case Item.ItemRank.LEGEND:
                itemNameText.text = "<color=#aa0508>" + _RanomItem.itemName + "</color>";
                break;
        }
    }
    public void ExitGame(bool _True)
    {
        if (_True)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            DataController.checkGameLoad = true;
            saveSoundCallback?.Invoke();
            GameManager.Instance.FadeInToMainScene();
        }
        else { exitPanel.SetActive(false); Time.timeScale = GameManager.Instance.TimeScale; }
    }
}
