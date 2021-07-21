using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public RectTransform list;          // 인벤토리 슬라이더
    public RectTransform[] buttonRect;  // 슬라이더 버튼들
    public float[] pos;                 // 슬라이더 위치들
    public float targetPos;             // 목표 위치
    public float distance;              // 타겟간 거리 설정

    public Text[] itemStatsText;
    public Text[] buttonTexts;
    public Image itemInfoImage;
    public Text itemInfoNameText;
    public Text itemInfoRankText;
    public Text itemDescriptonsText;

    public Sprite itemNullImage;

    [SerializeField] bool isScroll;      // 스크롤중인지 아닌지
    int targetIndex;                    // 목표 인덱스
    [Header("    ")]
    public Transform[] itemsParent;     // 인벤토리창 ( 몸통, 특성, 장비)

    public List<List<InventorySlot>> slots = new List<List<InventorySlot>>();
    public Button equipButton;
    public Button sellButton;
    public Button exitButton;

    [Header("Sell Panel")]
    public GameObject sell_PopUpPanel;
    public Text sellPriceText;
    public Button sell_ConfirmBtn;
    public Button sell_ExitBtn;

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
    public void InIt()
    {
        itemStatsText = GameObject.Find("Canvas").transform.Find("InventoryUI").GetChild(1).GetChild(1).GetChild(5).GetComponentsInChildren<Text>();
        for (int i = 0; i < itemStatsText.Length; i++)
        {
            itemStatsText[i].transform.parent.gameObject.SetActive(false);
        }

        LobbyManager.Instance.inventory.onItemChangedCallback += UpdateUI;
        for (int i = 0; i < itemsParent.Length; i++)
        {
            slots.Add(new List<InventorySlot>());
            InventorySlot[] temp = itemsParent[i].GetComponentsInChildren<InventorySlot>();
            for (int j = 0; j < LobbyManager.Instance.inventory.space; j++)
            {
                slots[i].Add(temp[j]);
                slots[i][j].yIndex = j;
            }
        }
        pos[0] = distance;

        for (int i = 1; i < pos.Length; i++)
        {
            pos[i] = pos[i - 1] - distance;
        }

        equipButton.onClick.AddListener(() =>
        {
            if (LobbyManager.Instance.inventory.selectedSlot != null && LobbyManager.Instance.inventory.selectedSlot.item != null) LobbyManager.Instance.inventory.EquipItem();
        });
        sellButton.onClick.AddListener(() => StartCoroutine(LobbyManager.Instance.lobbyUI.PanelAnim(sell_PopUpPanel, false)));
        sell_ExitBtn.onClick.AddListener(() => StartCoroutine(LobbyManager.Instance.lobbyUI.PanelAnim(sell_PopUpPanel, true)));
        sellButton.interactable = false;

        LobbyManager.Instance.lobbyUI.onIventoryClosedCallback += CloseInventory;
    }

    // 팩토리 인덱스별로 열려있는 창 다르게
    // 메인화면에서 열면 첫번째 창을 보여준다
    public void OpenInventory(int _Num)
    {
        int num = _Num;
        //메인 화면에서 들어갔을때
        if (_Num.Equals(-1))
        {
            equipButton.gameObject.SetActive(false);
            sellButton.gameObject.SetActive(true);
            num = 0;
        }
        else
        {
            // 메인화면에서 아이템창 들어가면 장착버튼 비활성화 / 팔기버튼 활성화
            equipButton.gameObject.SetActive(true);
            sellButton.gameObject.SetActive(false);
            FactoryAnimation facAnim = FindObjectOfType<FactoryAnimation>();
            facAnim.scrollEffect.SetActive(false);
            facAnim.scrollEffect2.SetActive(false);

            LobbyManager.Instance.inventory.IsFactoryManage = true;
        }

        exitButton.gameObject.SetActive(true);
        LobbyManager.Instance.lobbyUI.isInvenOpen = true;
        LobbyManager.Instance.inventory.currIndex = num;
        targetIndex = num;

        for (int i = 0; i < itemsParent.Length; i++)
        {
            itemsParent[i].GetChild(0).GetChild(0).localPosition = Vector2.zero;
        }

        ChangeButtonSize();
        list.localPosition = new Vector2(pos[targetIndex], 0);
        UpdateUI();
    }
    void ChangeButtonSize()
    {
        for (int i = 0; i < pos.Length; i++)
        {
            buttonRect[i].sizeDelta = new Vector2(i == targetIndex ? LobbyManager.Instance.lobbyUI.bButtonSize : LobbyManager.Instance.lobbyUI.sButtonSize, buttonRect[i].sizeDelta.y);
            buttonTexts[i].fontSize = (i == targetIndex ? LobbyManager.Instance.lobbyUI.bTextSize : LobbyManager.Instance.lobbyUI.sTextSize);
        }
    }
    //인벤토리 닫기
    public void CloseInventory()
    {
        exitButton.gameObject.SetActive(false);
        LobbyManager.Instance.lobbyUI.isInvenOpen = false;
        SetItemInfo(null);
        LobbyManager.Instance.inventory.IsFactoryManage = false;
        LobbyManager.Instance.inventory.selectedSlot = null;
    }

    //아이템 슬롯창 UI갱신
    void UpdateUI()
    {
        //모든 인벤토리를 순회
        for (int tempIdx = 0; tempIdx < itemsParent.Length; tempIdx++)
        {
            for (int i = 0; i < LobbyManager.Instance.inventory.space; i++)
            {
                if (i < LobbyManager.Instance.inventory.items[tempIdx].Count)
                    slots[tempIdx][i].AddItem(LobbyManager.Instance.inventory.items[tempIdx][i]);   //추가
                else
                {
                    //빈 슬롯들은 초기화
                    slots[tempIdx][i].ClearSlot();

                }
            }
        }

    }

    //스크롤될 매뉴버튼 클릭
    public void TabClick(int _N)
    {
        int tempIdx = targetIndex;
        targetIndex = _N;
        targetPos = pos[_N];
        LobbyManager.Instance.inventory.currIndex = _N;
        isScroll = true;
        StartCoroutine(LobbyManager.Instance.lobbyUI.PanelAnim(sell_PopUpPanel, true));
        StartCoroutine(Scroll());

        // 아이템 정보 UI 초기화
        SetItemInfo(null);
        // 이전 인벤토리를 처음위치로 바꾼다.
        itemsParent[tempIdx].GetChild(0).GetChild(0).localPosition = Vector2.zero;
        ChangeButtonSize();
        UpdateUI();
    }
    //인벤토리에서 선택한 아이템 정보UI 설정
    public void SetItemInfo(InventorySlot _Slot)
    {
        if (_Slot != null)
        {
            LobbyManager.Instance.inventory.selectedSlot = _Slot;
            if (_Slot.item != null)
            {
                switch (_Slot.item.itemRank)
                {
                    case Item.ItemRank.NORMAL:
                        sellPriceText.text = "500";
                        break;

                    case Item.ItemRank.MAGIC:
                        sellPriceText.text = "1000";
                        break;

                    case Item.ItemRank.RARE:
                        sellPriceText.text = "3000";
                        break;

                    case Item.ItemRank.EPIC:
                        sellPriceText.text = "5000";
                        break;

                    case Item.ItemRank.LEGEND:
                        sellPriceText.text = "10000";
                        break;
                }
            }
            itemInfoImage.sprite = _Slot.item.image;
            itemInfoNameText.text = _Slot.item.itemName;
            itemInfoRankText.text = LobbyManager.Instance.lobbyUI.ChangeItemNameColor(_Slot.item.itemRank);

            if (_Slot.item.GetType().Equals(typeof(Head)))
            {
                Head tempHead = _Slot.item as Head;
                if (tempHead.hp != 0)
                {
                    itemStatsText[0].transform.parent.gameObject.SetActive(true);
                    itemStatsText[0].text = "체력 : " + tempHead.hp;
                }
                else itemStatsText[0].transform.parent.gameObject.SetActive(false);

                if (tempHead.defense != 0)
                {
                    itemStatsText[1].transform.parent.gameObject.SetActive(true);
                    itemStatsText[1].text = "방어 : " + tempHead.defense + "%";
                }
                else itemStatsText[1].transform.parent.gameObject.SetActive(false);

                if (tempHead.evade != 0)
                {
                    itemStatsText[2].transform.parent.gameObject.SetActive(true);
                    itemStatsText[2].text = "회피 : " + tempHead.evade + "%";
                }
                else itemStatsText[2].transform.parent.gameObject.SetActive(false);

                if (tempHead.speed != 0)
                {
                    itemStatsText[3].transform.parent.gameObject.SetActive(true);
                    itemStatsText[3].text = "속도 : " + tempHead.speed + "%";
                }
                else itemStatsText[3].transform.parent.gameObject.SetActive(false);

                if (tempHead.allStats != 0)
                {
                    itemStatsText[4].transform.parent.gameObject.SetActive(true);
                    itemStatsText[4].text = "올스텟 : " + tempHead.allStats;
                }
                else itemStatsText[4].transform.parent.gameObject.SetActive(false);

                if (tempHead.addOption != 0 || tempHead.addOptionPer != 0)
                {
                    itemStatsText[5].transform.parent.gameObject.SetActive(true);
                    itemStatsText[5].text = "추가 : " + tempHead.addOption;

                }
                else itemStatsText[5].transform.parent.gameObject.SetActive(false);
            }
            else if (_Slot.item.GetType().Equals(typeof(Scroll)))
            {
                Scroll temp = _Slot.item as Scroll;

                itemDescriptonsText.text = temp.description;

                SkillInfoData tempData = DBManager.Instance.itemDB.SkillInfoDB[temp.id.ToString()][(int)temp.itemRank - 1];
                //스크롤 쿨타임

                if(tempData.coolTime != 0)
                {
                    itemStatsText[6].transform.parent.gameObject.SetActive(true);
                    itemStatsText[6].text = "쿨타임 : " + tempData.coolTime + "초";
                }

                //스크롤 능력
                itemStatsText[7].transform.parent.gameObject.SetActive(true);

                if (tempData.healFactor != 0)
                {
                    itemStatsText[7].text = "효과 : " + tempData.healFactor + "%";
                }
                else if (tempData.speedFactor != 0)
                {
                    itemStatsText[7].text = "효과 : " + tempData.speedFactor + "%";
                }
                else if (tempData.shieldFactor != 0)
                {
                    itemStatsText[7].text = "효과 : " + tempData.shieldFactor;
                }
                else if (tempData.evadeFactor != 0)
                {
                    itemStatsText[7].text = "효과 : " + tempData.evadeFactor + "%";
                }
                else if (tempData.defenseFactor != 0)
                {
                    itemStatsText[7].text = "효과 : " + tempData.defenseFactor + "%";
                }
                else if (tempData.invincible == true)
                {
                    itemStatsText[7].text = "효과 : " + tempData.duration + "초 무적";
                }
            }
            else
            {
                EquipMent tempEquipment = _Slot.item as EquipMent;
                if (tempEquipment.hp != 0)
                {
                    itemStatsText[0].transform.parent.gameObject.SetActive(true);
                    itemStatsText[0].text = "체력 : " + tempEquipment.hp;
                }
                else itemStatsText[0].transform.parent.gameObject.SetActive(false);

                if (tempEquipment.defense != 0)
                {
                    itemStatsText[1].transform.parent.gameObject.SetActive(true);
                    itemStatsText[1].text = "방어 : " + (int)tempEquipment.defense + "%";
                }
                else itemStatsText[1].transform.parent.gameObject.SetActive(false);

                if (tempEquipment.evade != 0)
                {
                    itemStatsText[2].transform.parent.gameObject.SetActive(true);
                    itemStatsText[2].text = "회피 : " + (int)tempEquipment.evade + "%";
                }
                else itemStatsText[2].transform.parent.gameObject.SetActive(false);

                if (tempEquipment.speed != 0)
                {
                    itemStatsText[3].transform.parent.gameObject.SetActive(true);
                    itemStatsText[3].text = "속도 : " + (int)tempEquipment.speed + "%";
                }
                else itemStatsText[3].transform.parent.gameObject.SetActive(false);

                if (tempEquipment.addOption != 0 || tempEquipment.addOptionPer != 0)
                {
                    itemStatsText[5].transform.parent.gameObject.SetActive(true);
                    itemStatsText[5].text = "추가 : " + tempEquipment.addOption;

                }
                else itemStatsText[5].transform.parent.gameObject.SetActive(false);


            }
            itemDescriptonsText.text = _Slot.item.description;
            sellButton.interactable = true;
        }
        else
        {
            itemInfoImage.sprite = itemNullImage;
            itemInfoNameText.text = null;
            itemInfoRankText.text = null;
            itemDescriptonsText.text = null;

            if (itemStatsText.Length > 0)
            {
                for (int i = 0; i < itemStatsText.Length; i++)
                {
                    itemStatsText[i].transform.parent.gameObject.SetActive(false);
                }
                    
            }

            //장착버튼 비활성화
            equipButton.interactable = false;
            sellButton.interactable = false;
        }
    }
    //스크롤
    IEnumerator Scroll()
    {
        while (isScroll)
        {
            list.localPosition = Vector2.Lerp(list.localPosition, new Vector2(targetPos, 0), Time.deltaTime * 5);
            if (Vector2.Distance(list.localPosition, new Vector2(targetPos, 0)) < 0.1f)
            {
                isScroll = false;
                list.localPosition = new Vector2(targetPos, 0);
            }
            yield return null;
        }
    }

}
