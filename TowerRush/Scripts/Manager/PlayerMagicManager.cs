using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMagicManager : MonoBehaviour
{
    [SerializeField] GameObject magicsParent;
    [SerializeField] Button popUpExitBtn;
    PlayerMagic currPlayerMagic; // 현재 선택한 마법
    List<PlayerMagic> magicList = new List<PlayerMagic>();      // 마법 리스트
    public List<PlayerMagic> MagicList { get { return magicList; } }

    public List<PlayerMagic> equipedMagic = new List<PlayerMagic>();    // 장착된 마법
    Button[] magicListBtn;
    [SerializeField] Button[] equipMagicListBtn;

    [HideInInspector] public int purchase_Amount { get; private set; }
    int purchase_Price;

    public void Init()
    {
        LobbyManager.Instance.lobbyUI.magic_LevelUp_Btn.onClick.AddListener(LevelUpMagic);
        LobbyManager.Instance.lobbyUI.magic_Purchase_Btn.onClick.AddListener(PurchaseMagic);
        LobbyManager.Instance.lobbyUI.magic_IncreaseAmount_Btn.onClick.AddListener(() => ChangeAmount(1));
        LobbyManager.Instance.lobbyUI.magic_DecreaseAmount_Btn.onClick.AddListener(() => ChangeAmount(0));
        LobbyManager.Instance.lobbyUI.magic_Equip_Btn.onClick.AddListener(EquipMagic);
        LobbyManager.Instance.lobbyUI.magic_UnEquip_Btn.onClick.AddListener(UnEquipMagic);
        popUpExitBtn.onClick.AddListener(() => LobbyManager.Instance.lobbyUI.magic_popUpPanel.SetActive(false));

        magicListBtn = magicsParent.GetComponentsInChildren<Button>();
        LoadMagics();

        for (int i = 0; i < 3; i++)
        {
            LobbyManager.Instance.lobbyUI.magic_EquipedMagic_Icon[i].sprite = equipedMagic.Count > i ? equipedMagic[i].icon : LobbyManager.Instance.lobbyUI.null_ItemImage;
            LobbyManager.Instance.lobbyUI.magic_EquipedMagic_Name[i].text = equipedMagic.Count > i ? equipedMagic[i].magicName : "";
            LobbyManager.Instance.lobbyUI.magic_EquipedMagic_Count[i].text = equipedMagic.Count > i ? equipedMagic[i].possessionCount.ToString() : "";
        }
        LimitMagicLevel();
    }
    //마법 로드
    void LoadMagics()
    {
        PlayerMagic[] magicObj = Resources.LoadAll<PlayerMagic>("ScripableObject/Magic/");
        Button[] skillBtns = magicsParent.GetComponentsInChildren<Button>();
        List<PlayerMagic> tempList = new List<PlayerMagic>();
        for (int i = 0; i < magicObj.Length; i++)
        {
            int temp = i;
            magicList.Add(Instantiate(magicObj[i]));
            skillBtns[i].transform.GetChild(0).GetComponent<Image>().sprite = magicObj[i].icon;
            magicListBtn[i].onClick.AddListener(() => OpenPopUpPanel(temp, false));
            skillBtns[i].interactable = false;

            //세이브가 되었으면
            if (UserData.Instance.userdata.isSaved)
            {
                magicList[i].level = UserData.Instance.magicList[i].level;
                magicList[i].possessionCount = UserData.Instance.magicList[i].possessionCount;
                magicList[i].levelUp_Price = UserData.Instance.magicList[i].levelUp_Price;
                magicList[i].isEquip = UserData.Instance.magicList[i].isEquip;

                if (UserData.Instance.magicList[i].isEquip)
                {
                    tempList.Add(magicList[i]);
                }
            }
        }


        if (DataController.checkGameLoad)
        {
            foreach (var dcMagic in DataController.PlayerMagic)
            {
                currPlayerMagic = dcMagic;
                EquipMagic();
            }
        }
        else //게임씬 로딩이 아닐떄 (게임을 켰을때 첫 로딩)
        {
            foreach (var magic in UserData.Instance.equipedMagicList)
            {
                foreach (var list in tempList)
                {
                    if (list.magicName.Equals(magic.name))
                    {
                        currPlayerMagic = list;
                        EquipMagic();
                        break;
                    }
                }
            }
        }


        for (int i = 0; i < 3; i++)
        {
            int temp = i;
            equipMagicListBtn[i].onClick.AddListener(() => OpenPopUpPanel(temp, true));
        }
    }

    //마법 장착
    void EquipMagic()
    {
        //1개 이상이어야지 장착
        if (currPlayerMagic.possessionCount > 0)
        {
            //이미 있는 마법이면 개수만 갱신
            for (int i = 0; i < equipedMagic.Count; i++)
            {
                if (equipedMagic[i].name.Equals(currPlayerMagic.name))
                {
                    LobbyManager.Instance.lobbyUI.magic_EquipedMagic_Count[i].text = currPlayerMagic.possessionCount.ToString();
                    return;
                }
            }

            //마법 장착슬롯이 꽉차면 마지막거를 지운다 
            if (equipedMagic.Count == 3)
            {
                equipedMagic[2].isEquip = false;
                equipedMagic.RemoveAt(2);
            }
            equipedMagic.Add(currPlayerMagic);
            currPlayerMagic.isEquip = true;
            LobbyManager.Instance.lobbyUI.magic_EquipedMagic_Icon[equipedMagic.Count - 1].sprite = currPlayerMagic.icon;
            LobbyManager.Instance.lobbyUI.magic_EquipedMagic_Name[equipedMagic.Count - 1].text = currPlayerMagic.magicName;
            LobbyManager.Instance.lobbyUI.magic_EquipedMagic_Count[equipedMagic.Count - 1].text = currPlayerMagic.possessionCount.ToString();
            LobbyManager.Instance.lobbyUI.magic_popUpPanel.SetActive(false);
        }
    }

    //마법 장착 해제
    void UnEquipMagic()
    {
        for (int i = 0; i < equipedMagic.Count; i++)
        {
            if (equipedMagic[i].Equals(currPlayerMagic))
            {
                currPlayerMagic.isEquip = false;
                equipedMagic.RemoveAt(i);
                break;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            LobbyManager.Instance.lobbyUI.magic_EquipedMagic_Icon[i].sprite = equipedMagic.Count > i ? equipedMagic[i].icon : LobbyManager.Instance.lobbyUI.null_ItemImage;
            LobbyManager.Instance.lobbyUI.magic_EquipedMagic_Name[i].text = equipedMagic.Count > i ? equipedMagic[i].magicName : "";
            LobbyManager.Instance.lobbyUI.magic_EquipedMagic_Count[i].text = equipedMagic.Count > i ? equipedMagic[i].possessionCount.ToString() : "";
        }
        LobbyManager.Instance.lobbyUI.magic_popUpPanel.SetActive(false);
    }

    private void Update()
    {
        if (currPlayerMagic != null)
        {
            if (LobbyManager.Instance.lobbyUI.magic_popUpPanel.activeSelf) UpdateUI();

            purchase_Price = currPlayerMagic.perchasePrice * purchase_Amount;
        }
    }

    void OpenPopUpPanel(int _Idx, bool _isEquiped)
    {
        if (_isEquiped)
        {
            //장착한 마법이 1개 이상이고 장착마법리스트의 숫자가 인덱스보다 높을때
            if (!equipedMagic.Count.Equals(0) && equipedMagic.Count > _Idx) currPlayerMagic = equipedMagic[_Idx];
            else return;
        }
        else currPlayerMagic = magicList[_Idx];

        purchase_Price = currPlayerMagic.perchasePrice;
        LobbyManager.Instance.lobbyUI.magic_popUpPanel.SetActive(true);
        LobbyManager.Instance.lobbyUI.magic_Icon.sprite = currPlayerMagic.icon;
    }

    void UpdateUI()
    {
        LobbyManager.Instance.lobbyUI.magic_Name_Text.text = currPlayerMagic.magicName;                        //이름
        LobbyManager.Instance.lobbyUI.magic_Level_Text.text = "Lv." + currPlayerMagic.level.ToString();        //레벨
        LobbyManager.Instance.lobbyUI.magic_PossessCount_Text.text = currPlayerMagic.possessionCount.ToString(); //보유개수

        LobbyManager.Instance.lobbyUI.magic_LevelUpPrice_Text.text = currPlayerMagic.levelUp_Price.ToString(); // 레벨업 비용
        LobbyManager.Instance.lobbyUI.magic_PerchasePrice_Text.text = purchase_Price.ToString();               // 구매 비용

        LobbyManager.Instance.lobbyUI.magic_Description_Text.text = currPlayerMagic.description;               //설명
        LobbyManager.Instance.lobbyUI.magic_PerchaseAmount_Text.text = purchase_Amount.ToString();             //구매량

        if (currPlayerMagic.level < 10)
        {
            //현재 마법정보
            string s_coolTime = currPlayerMagic.magicStat[currPlayerMagic.level].coolTime == 0 ? string.Empty : "쿨타임 : " + currPlayerMagic.magicStat[currPlayerMagic.level].coolTime.ToString();
            string s_radious = currPlayerMagic.magicStat[currPlayerMagic.level].radius == 0 ? string.Empty : "범위 : " + currPlayerMagic.magicStat[currPlayerMagic.level].radius.ToString();
            string s_duration = currPlayerMagic.magicStat[currPlayerMagic.level].duration == 0 ? string.Empty : "지속시간 : " + currPlayerMagic.magicStat[currPlayerMagic.level].duration.ToString();
            string s_statFactor = currPlayerMagic.magicStat[currPlayerMagic.level].statFactor == 0 ? string.Empty : "효과 : " + currPlayerMagic.magicStat[currPlayerMagic.level].statFactor.ToString() + "%";

            LobbyManager.Instance.lobbyUI.magic_StatText.text = s_coolTime + "\n" + s_radious + "\n" + s_duration + "\n" + s_statFactor;

            //다음레벨 마법정보
            s_coolTime = currPlayerMagic.magicStat[currPlayerMagic.level + 1].coolTime == 0 ? string.Empty : "쿨타임 : " + currPlayerMagic.magicStat[currPlayerMagic.level + 1].coolTime.ToString();
            s_radious = currPlayerMagic.magicStat[currPlayerMagic.level + 1].radius == 0 ? string.Empty : "범위 : " + currPlayerMagic.magicStat[currPlayerMagic.level + 1].radius.ToString();
            s_duration = currPlayerMagic.magicStat[currPlayerMagic.level + 1].duration == 0 ? string.Empty : "지속시간 : " + currPlayerMagic.magicStat[currPlayerMagic.level + 1].duration.ToString();
            s_statFactor = currPlayerMagic.magicStat[currPlayerMagic.level + 1].statFactor == 0 ? string.Empty : "효과 : " + currPlayerMagic.magicStat[currPlayerMagic.level + 1].statFactor.ToString() + "%";

            LobbyManager.Instance.lobbyUI.magic_NextLevelStatText.text = s_coolTime + "\n" + s_radious + "\n" + s_duration + "\n" + s_statFactor;
        }
        else
        {
            string s_coolTime = currPlayerMagic.magicStat[currPlayerMagic.level].coolTime == 0 ? string.Empty : "쿨타임 : " + currPlayerMagic.magicStat[currPlayerMagic.level].coolTime.ToString();
            string s_radious = currPlayerMagic.magicStat[currPlayerMagic.level].radius == 0 ? string.Empty : "범위 : " + currPlayerMagic.magicStat[currPlayerMagic.level].radius.ToString();
            string s_duration = currPlayerMagic.magicStat[currPlayerMagic.level].duration == 0 ? string.Empty : "지속시간 : " + currPlayerMagic.magicStat[currPlayerMagic.level].duration.ToString();
            string s_statFactor = currPlayerMagic.magicStat[currPlayerMagic.level].statFactor == 0 ? string.Empty : "효과 : " + currPlayerMagic.magicStat[currPlayerMagic.level].statFactor.ToString() + "%";

            LobbyManager.Instance.lobbyUI.magic_StatText.text = s_coolTime + "\n" + s_radious + "\n" + s_duration + "\n" + s_statFactor;
            LobbyManager.Instance.lobbyUI.magic_NextLevelStatText.text = string.Empty;
        }

        // 현재 가지고있는 돈이 레벨업보다 적으면 버튼 비활성화
        if (LobbyManager.Instance.Gold < currPlayerMagic.levelUp_Price) LobbyManager.Instance.lobbyUI.magic_LevelUp_Btn.interactable = false;
        else LobbyManager.Instance.lobbyUI.magic_LevelUp_Btn.interactable = true;

        // 현재 가지고있는 돈이 구매할 마법의 가격 보다 적으면 버튼 비활성화
        if (purchase_Amount.Equals(0) || LobbyManager.Instance.Gold < currPlayerMagic.perchasePrice * purchase_Amount) LobbyManager.Instance.lobbyUI.magic_Purchase_Btn.interactable = false;
        else LobbyManager.Instance.lobbyUI.magic_Purchase_Btn.interactable = true;

        // 현재 가지고있는 마법이 0개일때 장착/해제 버튼 비활성화
        if (currPlayerMagic.possessionCount.Equals(0)) LobbyManager.Instance.lobbyUI.magic_Equip_Btn.interactable = false;
        else LobbyManager.Instance.lobbyUI.magic_Equip_Btn.interactable = true;

        if (currPlayerMagic.possessionCount.Equals(0)) LobbyManager.Instance.lobbyUI.magic_UnEquip_Btn.interactable = false;
        else LobbyManager.Instance.lobbyUI.magic_UnEquip_Btn.interactable = true;

        //최대 레벨 도달하면 가격텍스트를 바꿈
        if (currPlayerMagic.level.Equals(10))
        {
            LobbyManager.Instance.lobbyUI.magic_LevelUpPrice_Text.text = "완료";
            LobbyManager.Instance.lobbyUI.magic_LevelUp_Btn.interactable = false;
        }
    }

    //마법 구매량 결정
    void ChangeAmount(int _Num)
    {
        // 감소
        if (_Num.Equals(0))
        {
            if (purchase_Amount > 0) purchase_Amount--;
        }
        //증가
        else
        {
            if (purchase_Amount + currPlayerMagic.possessionCount < 999) purchase_Amount++;
        }
    }

    //마법 레벨업
    void LevelUpMagic()
    {
        if (currPlayerMagic.level <= 10 && LobbyManager.Instance.Gold >= currPlayerMagic.levelUp_Price)
        {
            LobbyManager.Instance.Gold -= currPlayerMagic.levelUp_Price;
            currPlayerMagic.levelUp_Price *= currPlayerMagic.level + 1;
            currPlayerMagic.level++;
        }
    }

    //마법 구매하기
    void PurchaseMagic()
    {
        if (LobbyManager.Instance.Gold >= currPlayerMagic.perchasePrice * purchase_Amount)
        {
            currPlayerMagic.possessionCount += purchase_Amount;
            purchase_Amount = 0;
            purchase_Price = currPlayerMagic.perchasePrice;

            if (!UserData.Instance.userdata.isFinishTutorial)
            {
                LobbyManager.Instance.tutorialManager.check = true;
            }

            LobbyManager.Instance.Gold -= currPlayerMagic.perchasePrice * purchase_Amount;

            for (int i = 0; i < equipedMagic.Count; i++)
            {
                if (equipedMagic[i].name.Equals(currPlayerMagic.name))
                {
                    LobbyManager.Instance.lobbyUI.magic_EquipedMagic_Count[i].text = currPlayerMagic.possessionCount.ToString();
                    break;
                }
            }
        }
    }

    //레벨에 따라 마법 제한
    void LimitMagicLevel()
    {
        int limit = 0;

        if (UserData.Instance.userdata.level >= 30) limit = 10;
        else if (UserData.Instance.userdata.level >= 20) limit = 8;
        else if (UserData.Instance.userdata.level >= 10) limit = 6;
        else if (UserData.Instance.userdata.level >= 5) limit = 4;
        else if (UserData.Instance.userdata.level >= 1) limit = 2;
        Button[] skillBtns = magicsParent.GetComponentsInChildren<Button>();
        for (int i = 0; i < limit; i++)
        {
            skillBtns[i].interactable = true;
        }
    }
}
