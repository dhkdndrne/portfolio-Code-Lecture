using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using DarkTonic.MasterAudio;
using DG.Tweening;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    /*
     물건 순서
     히로디온(스킨)
     스크롤
     장비
     골드
     다이아
         */

    //다이아 가격
    [SerializeField] int[] price_Dia = new int[3];
    //골드 구매 가격
    [SerializeField] int[] price_Gold = new int[3];
    [Header("CheckPanel")]
    [SerializeField] GameObject checkPanel;
    [SerializeField] Sprite[] itemImages;
    [SerializeField] Image packageImg;
    [SerializeField] Text priceText;
    [SerializeField] Text packageNameTxt;
    [SerializeField] Button confirmButton;  // 아이템 구매 확인 버튼
    [SerializeField] Button exitButton;     // 아이템 구매 취소 버튼
    [SerializeField] Button exitButton2;     // 다른공간 눌렀을때 꺼지게 하는 버튼
    [SerializeField] Button effectSkipBtn;     // 이펙트 스킵버튼

    [Header("ShopEffect")]
    public Sprite[] itemImage;
    public GameObject effectPanel;
    public Image[] effectImg = new Image[2];    //연출용 이미지(뒤에 빛 돌아가는거)
    public Image effectItem;        //연출용 이미지   (상자)
    public Image effectBackImg;     //연출용 배경이미지(화면 밝아지는거)

    public GameObject resultItem;   //뽑기 결과 아이템
    public Text resultText;

    public GameObject resultTenItems; //10개 뽑기 결과 아이템
    public Image[] resultItems = new Image[10];
    public Text[] resultItemTexts = new Text[10];

    public Button closeRButton;     // 뽑은 결과 닫는 버튼
    public Button closeRTButton;    // "10개짜리"
    public Button removeAD_Button;
    List<Item> pickItems = new List<Item>();
    bool isTen;

    void Start()
    {
        InitButton();

        closeRButton.onClick.AddListener(CloseResultPanel);
        closeRTButton.onClick.AddListener(CloseResultPanel);

        resultItem.transform.GetChild(0).GetComponent<ImageOutLine>().Init();

        for (int i = 0; i < resultItems.Length; i++)
        {
            resultItems[i].transform.GetComponent<ImageOutLine>().Init();
        }

        exitButton.onClick.AddListener(() => StartCoroutine(ConfirmPanelAnim(true)));
        exitButton2.onClick.AddListener(() => StartCoroutine(ConfirmPanelAnim(true)));

    }

    void InitButton()
    {
        for (int i = 0; i < 6; i++)
        {
            int temp = i;
            LobbyManager.Instance.lobbyUI.headPickBtn[i].onClick.AddListener(() => BuyHerodion(temp));
            LobbyManager.Instance.lobbyUI.scrollPickBtn[i].onClick.AddListener(() => BuyScroll(temp));
            LobbyManager.Instance.lobbyUI.equipmentPickBtn[i].onClick.AddListener(() => BuyEquipment(temp));
        }

        if (UserData.Instance.userdata.isBuydeleteAD) removeAD_Button.interactable = false;
    }

    public void CheckBuyGold(int _Num)
    {
        if (_Num.Equals(0))
        {
            SetConfirmUI(itemImages[9], "소형 골드주머니", 0, 0, false, true);
        }
        else if (_Num.Equals(1))
        {
            SetConfirmUI(itemImages[10], "대형 골드주머니", 0, 1, false, true);
        }
        else if (_Num.Equals(2))
        {
            SetConfirmUI(itemImages[11], "초대형 골드상자", 0, 2, false, true);
        }
    }
    void BuyGold(int _index)
    {
        if (LobbyManager.Instance.Luga >= price_Gold[_index])
        {
            if (_index.Equals(0)) LobbyManager.Instance.lobbyUI.StartResourceEff(true, _index, 10000);
            else if (_index.Equals(1)) LobbyManager.Instance.lobbyUI.StartResourceEff(true, _index, 100000);
            else if (_index.Equals(2)) LobbyManager.Instance.lobbyUI.StartResourceEff(true, _index, 500000);

            LobbyManager.Instance.Luga -= price_Gold[_index];
        }
        else
        {
            LobbyManager.Instance.lobbyUI.ShowAlarm("비용이 부족합니다..");
        }
    }

    void BuyHerodion(int _Idx)
    {
        StartCoroutine(ConfirmPanelAnim(false));
        switch (_Idx)
        {
            case 0:
                SetConfirmUI(itemImages[0], "인턴 제작 히로디온", 0, 0);
                break;

            case 1:
                SetConfirmUI(itemImages[0], "인턴 제작 히로디온", 0, 0, true);
                break;

            case 2:
                SetConfirmUI(itemImages[1], "수제자 제작 히로디온", 0, 1);
                break;

            case 3:
                SetConfirmUI(itemImages[1], "수제자 제작 히로디온", 0, 1, true);
                break;

            case 4:
                SetConfirmUI(itemImages[2], "장인 제작 히로디온", 0, 2);
                break;

            case 5:
                SetConfirmUI(itemImages[2], "장인 제작 히로디온", 0, 2, true);
                break;
        }
    }
    void BuyScroll(int _Idx)
    {
        StartCoroutine(ConfirmPanelAnim(false));
        switch (_Idx)
        {
            case 0:
                SetConfirmUI(itemImages[3], "인턴 제작 스크롤", 1, 0);
                break;

            case 1:
                SetConfirmUI(itemImages[3], "인턴 제작 스크롤", 1, 0, true);
                break;

            case 2:
                SetConfirmUI(itemImages[4], "수제자 제작 스크롤", 1, 1);
                break;

            case 3:
                SetConfirmUI(itemImages[4], "수제자 제작 스크롤", 1, 1, true);
                break;

            case 4:
                SetConfirmUI(itemImages[5], "장인 제작 스크롤", 1, 2);
                break;

            case 5:
                SetConfirmUI(itemImages[5], "장인 제작 스크롤", 1, 2, true);
                break;
        }
    }
    void BuyEquipment(int _Idx)
    {
        StartCoroutine(ConfirmPanelAnim(false));
        switch (_Idx)
        {
            case 0:
                SetConfirmUI(itemImages[6], "인턴 제작 장비", 2, 0);
                break;

            case 1:
                SetConfirmUI(itemImages[6], "인턴 제작 장비", 2, 0, true);
                break;

            case 2:
                SetConfirmUI(itemImages[7], "수제자 제작 장비", 2, 1);
                break;

            case 3:
                SetConfirmUI(itemImages[7], "수제자 제작 장비", 2, 1, true);
                break;

            case 4:
                SetConfirmUI(itemImages[8], "장인 제작 장비", 2, 2);
                break;

            case 5:
                SetConfirmUI(itemImages[8], "장인 제작 장비", 2, 2, true);
                break;
        }
    }

    void CloseResultPanel()
    {
        resultItem.SetActive(false);
        resultTenItems.SetActive(false);
        effectPanel.SetActive(false);
    }
    //단품 사기
    void GetRandomItem(int _ItemType, int _BuyRank)
    {
        isTen = false;

        if (LobbyManager.Instance.inventory.CheckIsFull(_ItemType, 1))
        {
            if (LobbyManager.Instance.Luga >= price_Dia[_BuyRank])
            {
                pickItems.Add(DBManager.Instance.itemDB.GetRandomItem(_ItemType, _BuyRank));
                LobbyManager.Instance.inventory.AddItem(pickItems[0]);
                StartShopEffect();
                LobbyManager.Instance.Luga -= price_Dia[_BuyRank];
            }
        }
        //히로디온
        if (_ItemType.Equals(0)) effectItem.sprite = itemImage[0];
        //스크롤
        else if (_ItemType.Equals(1)) effectItem.sprite = itemImage[1];
        //장비
        else effectItem.sprite = itemImage[_ItemType + _BuyRank];
    }
    //10개 사기
    void GetRandomItems(int _Price, int _ItemType, int _BuyRank)
    {
        isTen = true;
        if (LobbyManager.Instance.Luga < _Price)
        {
            LobbyManager.Instance.lobbyUI.ShowAlarm("비용이 부족합니다.");
            return;
        }
        if (!LobbyManager.Instance.inventory.CheckIsFull(_ItemType, 10)) return;

        for (int i = 0; i < 10; i++)
        {
            pickItems.Add(DBManager.Instance.itemDB.GetRandomItem(_ItemType, _BuyRank));
            LobbyManager.Instance.inventory.AddItem(pickItems[i]);
        }
        StartShopEffect();
        //히로디온
        if (_ItemType.Equals(0)) effectItem.sprite = itemImage[0];
        //스크롤
        else if (_ItemType.Equals(1)) effectItem.sprite = itemImage[1];
        //장비
        else effectItem.sprite = itemImage[_ItemType + _BuyRank];

        LobbyManager.Instance.Luga -= _Price;
    }


    //상점 이펙트 시작
    void StartShopEffect()
    {
        effectPanel.SetActive(true);
        for (int i = 0; i < effectImg.Length; i++)
        {
            effectImg[i].gameObject.SetActive(true);
        }

        effectItem.gameObject.SetActive(true);
        effectBackImg.gameObject.SetActive(true);
        StartCoroutine("ShopEffectFadeIn");
    }
    void ShowResultItem(bool _IsTen)
    {
        if (_IsTen)
        {
            resultTenItems.SetActive(true);
            for (int i = 0; i < pickItems.Count; i++)
            {
                resultItems[i].sprite = pickItems[i].image;
                resultItemTexts[i].text = pickItems[i].itemName;

                if (pickItems[i].itemRank.Equals(Item.ItemRank.LEGEND)) resultItems[i].material = LobbyManager.Instance.lobbyUI.pick_LegendMat;
                else resultItems[i].transform.GetComponent<ImageOutLine>().SetOutLineColor(pickItems[i]);

            }
        }
        else
        {
            resultItem.SetActive(true);
            resultItem.transform.GetChild(0).GetComponent<Image>().sprite = pickItems[0].image;

            if (pickItems[0].itemRank.Equals(Item.ItemRank.LEGEND)) resultItem.transform.GetChild(0).GetComponent<Image>().material = LobbyManager.Instance.lobbyUI.pick_LegendMat;
            else resultItem.transform.GetChild(0).GetComponent<ImageOutLine>().SetOutLineColor(pickItems[0]);


            resultText.text = pickItems[0].itemName;
        }
        pickItems.Clear();
    }

    bool check;
    //상점뽑기 연출효과
    IEnumerator ShopEffectFadeIn()
    {
        check = false;
        float currenTime = 0.0f;
        float percent = 0.0f;

        yield return new WaitForSeconds(3f);
        while (percent < 1)
        {
            currenTime += Time.deltaTime;
            percent = currenTime / 1.5f;

            Color color = effectBackImg.color;

            color.a = Mathf.Lerp(0, 1f, percent);

            effectBackImg.color = color;

            if (!check && effectBackImg.color.a > 0.6f)
            {
                effectSkipBtn.gameObject.SetActive(false);
                check = true;
            }
            yield return null;
        }

        StartCoroutine("ShopEffectFadeOut");
        for (int i = 0; i < effectImg.Length; i++)
        {
            effectImg[i].gameObject.SetActive(false);
        }
        effectItem.gameObject.SetActive(false);

    }

    IEnumerator ShopEffectFadeOut()
    {

        float currenTime = 0.0f;
        float percent = 0.0f;
        ShowResultItem(isTen);
        while (percent < 1)
        {
            currenTime += Time.deltaTime;
            percent = currenTime / 1.5f;

            Color color = effectBackImg.color;

            color.a = Mathf.Lerp(1f, 0, percent);
            effectBackImg.color = color;

            yield return null;
        }
        effectSkipBtn.gameObject.SetActive(true);
        effectBackImg.gameObject.SetActive(false);
    }

    public void Skip()
    {
        StopCoroutine("ShopEffectFadeIn");
        StopCoroutine("ShopEffectFadeOut");

        Color color = effectBackImg.color;
        color.a = 0f;
        effectBackImg.color = color;

        for (int i = 0; i < effectImg.Length; i++)
        {
            effectImg[i].gameObject.SetActive(false);
        }
        effectItem.gameObject.SetActive(false);
        ShowResultItem(isTen);

        effectBackImg.gameObject.SetActive(false);

        MasterAudio.StopAllOfSound("Crash-Cymbal");
    }

    //구매 확인창 UI수정
    public void SetConfirmUI(Sprite _ItemImage, string _PackageName, int _ItemType, int _Rank, bool _IsTen = false, bool _IsGold = false)
    {
        StartCoroutine(ConfirmPanelAnim(false));

        packageImg.sprite = _ItemImage;
        packageNameTxt.text = _PackageName;
        confirmButton.onClick.RemoveAllListeners();
        int price = 0;
        if (!_IsGold)
        {
            if (_IsTen)
            {
                price = (int)((price_Dia[_Rank] * 10) - price_Dia[_Rank] * 10 * 0.1);
                priceText.text = (price).ToString();
                confirmButton.onClick.AddListener(() => GetRandomItems(price, _ItemType, _Rank));

            }
            else
            {
                priceText.text = price_Dia[_Rank].ToString();
                confirmButton.onClick.AddListener(() => GetRandomItem(_ItemType, _Rank));
            }
        }
        else
        {
            priceText.text = price_Gold[_Rank].ToString();
            confirmButton.onClick.AddListener(() => BuyGold(_Rank));
        }
        confirmButton.onClick.AddListener(() => StartCoroutine(ConfirmPanelAnim(true)));
    }
    public IEnumerator ConfirmPanelAnim(bool _Active)
    {
        TweenParams tParms = new TweenParams().SetEase(Ease.Linear);

        //켜진상태
        if (_Active)
        {
            yield return checkPanel.transform.DOScale(new Vector3(0, 0, 0), 0.1f).SetAs(tParms).WaitForCompletion();
            exitButton2.gameObject.SetActive(false);
        }
        else
        {
            exitButton2.gameObject.SetActive(true);
            yield return checkPanel.transform.DOScale(new Vector3(1, 1, 1), 0.1f).SetAs(tParms).WaitForCompletion();
        }

    }

}
