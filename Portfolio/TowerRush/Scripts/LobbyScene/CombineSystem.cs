using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public struct CombineRate
{
    float normal;
    float magic;
    float rare;
    float epic;
    float legend;
    public CombineRate(float _Normal, float _Magic, float _Rare, float _Epic, float _Legend)
    {
        normal = _Normal;
        magic = _Magic;
        rare = _Rare;
        epic = _Epic;
        legend = _Legend;
    }
    public float ReturnRate(int _Idx)
    {
        if (_Idx.Equals(0))
        {
            return normal;
        }
        else if (_Idx.Equals(1))
        {
            return magic;
        }
        else if (_Idx.Equals(2))
        {
            return rare;
        }
        else if (_Idx.Equals(3))
        {
            return epic;
        }
        else
        {
            return legend;
        }
    }
}

public class CombineSystem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject[] inventory;
    List<Item> combineItems = new List<Item>();
    [SerializeField] Image[] itemImages = new Image[3];
    [SerializeField] Sprite resultDefalutImg;
    [SerializeField] Text resultItemName;
    [SerializeField] Image effectImage;
    bool isReset;
    //버튼
    //-------------------------------
    public RectTransform[] combineTabs;
    Text[] buttonText = new Text[3];
    int targetIndex;

    // Canvas RayCast
    //--------------------------------
    Canvas canvas;
    GraphicRaycaster gr;
    PointerEventData ped;

    // 조합창 아이템 개수
    public static int itemCount;

    //합성확률 DB
    List<List<CombineRate>> combineRateDB = new List<List<CombineRate>>();
    float[] rankPickRate = new float[5];
    string[] colorText = new string[5];
    //----------
    float pressTime;
    bool isPressed;
    bool isClick;
    bool combined;

    void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        gr = canvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);

        //하단 버튼오브젝트 연결
        for (int i = 0; i < combineTabs.Length; i++)
        {
            int temp = i;
            combineTabs[temp].GetComponent<Button>().onClick.AddListener(() => ActiveIneventory(temp));
            buttonText[i] = combineTabs[i].gameObject.transform.GetChild(0).GetComponent<Text>();
        }

        //조합창 이미지오브젝트 초기화
        for (int i = 0; i < 3; i++)
        {
            itemImages[i].GetComponent<ImageOutLine>().Init();
        }

        List<Dictionary<string, object>> combineRateData = CSVReader.Read("DBData/CombineRate");

        int count = 0;
        for (int i = 0; i < combineRateData.Count; i += 5, count++)
        {
            combineRateDB.Add(new List<CombineRate>());
            for (int j = 0; j < 5; j++)
            {
                combineRateDB[count].Add(new CombineRate(
                       System.Convert.ToSingle(combineRateData[i + j]["Normal"]),
                       System.Convert.ToSingle(combineRateData[i + j]["Magic"]),
                       System.Convert.ToSingle(combineRateData[i + j]["Rare"]),
                       System.Convert.ToSingle(combineRateData[i + j]["Epic"]),
                       System.Convert.ToSingle(combineRateData[i + j]["Legend"])));
            }
        }

        colorText[0] = "<color=#000000>노말</color>";
        colorText[1] = "<color=#0868ac>매직</color>";
        colorText[2] = "<color=#0f8d35>레어</color>";
        colorText[3] = "<color=#4a017d>에픽</color>";
        colorText[4] = "<color=#aa0508>레전드</color>";
    }

    void OnEnable()
    {
        inventory[0].SetActive(true);
        for (int i = 1; i < inventory.Length; i++)
        {
            inventory[i].SetActive(false);
        }
        targetIndex = 0;

        //조합 아이템창을 다 꺼준다
        itemImages[2].sprite = resultDefalutImg;
        for (int i = 0; i < 2; i++)
        {
            itemImages[i].enabled = false;
        }
        resultItemName.gameObject.SetActive(false);
        //인벤토리의 정보를 옮긴다.
        GetInventroyItemInfo();

        //결과 아이템의 정보를 초기화
        itemImages[2].sprite = resultDefalutImg;
        //텍스트 끄기
        resultItemName.gameObject.SetActive(false);
        //결과 아이템 메테리얼 초기화
        itemImages[2].material = null;

        //결과 이펙트 효과 초기화
        Color color = effectImage.color;
        color.a = 0f;
        effectImage.color = color;

        combined = false;
        isPressed = false;
        isClick = false;
        pressTime = 0;
    }

    void Update()
    {
        if (isPressed && !combined)
        {
            pressTime += 1f * Time.deltaTime;
            if (pressTime >= 0.1f)
            {
                pressTime = 0.1f;
                isClick = true;
            }
        }

        if (isClick)
        {
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            if (results.Count > 0)
            {
                GameObject obj = results[0].gameObject;
                //클릭한 오브젝트가 슬롯일때
                if (obj.GetComponent<InventorySlot>() != null)
                {
                    InventorySlot tempSlot = obj.GetComponent<InventorySlot>();

                    //슬롯의 아이템이 있을때
                    if (tempSlot.item != null)
                    {
                        //슬롯이 조합이 눌리지 않았고 카운트가 2이하일때
                        if (!tempSlot.isCombined && itemCount < 2)
                        {
                            Item tempItem = tempSlot.item;
                            combineItems.Add(tempItem);
                            tempSlot.isCombined = true;
                            //장착된 아이템에 회색이미지를 켜준다
                            tempSlot.transform.GetChild(1).gameObject.SetActive(true);
                            //이미지 추가
                            itemImages[itemCount].enabled = true;
                            itemImages[itemCount].sprite = tempSlot.icon.sprite;
                            itemImages[itemCount].GetComponent<ImageOutLine>().SetOutLineColor(tempItem);

                            if (!isReset)
                            {
                                itemImages[2].sprite = resultDefalutImg;
                                itemImages[2].material = null;
                                //텍스트 끄기
                                resultItemName.gameObject.SetActive(false);
                                isReset = true;
                            }
                            //인덱스 증가
                            itemCount++;
                        }
                        //합성창에 올린 아이템을 한번 더 눌렀을때
                        else if (tempSlot.isCombined)
                        {
                            Item targetItem = tempSlot.item;
                            for (int i = 0; i < combineItems.Count; i++)
                            {
                                //리스트에서 해당 아이템을 찾고 지운다.
                                if (targetItem.Equals(combineItems[i]))
                                {
                                    obj.GetComponent<InventorySlot>().isCombined = false;
                                    tempSlot.transform.GetChild(1).gameObject.SetActive(false);
                                    combineItems.RemoveAt(i);
                                    break;
                                }
                            }

                            //// temp의 아이템정보를 합성창으로 옮김 이미지도 같이 옮겨준다.
                            for (int i = 0; i < itemImages.Length; i++)
                            {
                                if (i < combineItems.Count)
                                {
                                    itemImages[i].sprite = combineItems[i].image;
                                    itemImages[i].GetComponent<ImageOutLine>().SetOutLineColor(combineItems[i]);
                                }
                                else
                                {
                                    itemImages[i].enabled = false;
                                }
                            }
                            itemCount--;
                            itemImages[2].sprite = resultDefalutImg;
                            itemImages[2].material = null;
                        }
                        if (itemCount.Equals(2))
                            CalculateRate();

                        isPressed = false;
                        isClick = false;
                    }
                }
            }
        }
    }

    //인벤토리 아이템 정보 가져오는 함수
    void GetInventroyItemInfo()
    {
        //인벤토리 아이템정보 옮겨오기
        for (int i = 0; i < inventory.Length; i++)
        {
            InventorySlot[] tempSlots = inventory[i].transform.GetChild(0).GetChild(0).GetComponentsInChildren<InventorySlot>();
            int itemCount = LobbyManager.Instance.inventory.items[i].Count;
            for (int j = 0; j < tempSlots.Length; j++)
            {
                tempSlots[j].transform.GetChild(1).gameObject.SetActive(false);
                if (itemCount > j && LobbyManager.Instance.inventory.items[i][j] != null)
                {
                    tempSlots[j].AddItem(LobbyManager.Instance.inventory.items[i][j]);
                    tempSlots[j].icon.sprite = tempSlots[j].item.image;
                }
                else tempSlots[j].ClearSlot();
            }
        }
    }
    //합성 예상결과 계산하는함수
    void CalculateRate()
    {
        for (int i = 0; i < 5; i++)
            rankPickRate[i] = combineRateDB[(int)combineItems[0].itemRank-1][(int)combineItems[1].itemRank-1].ReturnRate(i);

        float max = -9999;
        float second = -9999;
        int maxIdx = 0;
        int secondIdx = 0;

        for (int i = 0; i < rankPickRate.Length; i++)
        {
            if (rankPickRate[i] > second)
            {
                if (rankPickRate[i] > max)
                {
                    second = max;
                    max = rankPickRate[i];
                    maxIdx = i;
                }
                else
                {
                    second = rankPickRate[i];
                    secondIdx = i;
                }

            }
        }
        LobbyManager.Instance.lobbyUI.combineText.text = "예상결과 : " + colorText[maxIdx] + " , " + colorText[secondIdx];
    }
    //조합
    public void Combine()
    {
        if (itemCount > 1)
        {
            if (!UserData.Instance.userdata.isFinishTutorial)
            {
                LobbyManager.Instance.tutorialManager.check = true;
            }

            combined = true;
            InventorySlot[] tempSlots = inventory[targetIndex].transform.GetChild(0).GetChild(0).GetComponentsInChildren<InventorySlot>();

            for (int i = 0; i < tempSlots.Length; i++)
            {
                //슬롯이 조합창에 등록되어있다면
                if (tempSlots[i].isCombined)
                {
                    for (int j = 0; j < LobbyManager.Instance.inventory.items[targetIndex].Count; j++)
                    {   //인벤토리에서 슬롯의 아이템과 같은 아이템을 삭제한다.
                        if (tempSlots[i].item.Equals(LobbyManager.Instance.inventory.items[targetIndex][j]))
                        {
                            LobbyManager.Instance.inventory.Remove(j);
                            break;
                        }
                    }
                }
                tempSlots[i].ClearSlot();
            }
            combineItems.Clear();
            float totalRate = 0;
            float weight = 0;

            //뽑을 랭크를 정한다.
            for (int i = 0; i < rankPickRate.Length; i++)
            {
                totalRate += rankPickRate[i];
            }

            weight = Random.Range(0f, totalRate);

            int targetRank = 0;
            for (int i = 0; i < rankPickRate.Length; i++)
            {
                if (weight <= rankPickRate[i])
                {
                    targetRank = i+1;
                    break;
                }
                weight -= rankPickRate[i];
            }

            LobbyManager.Instance.lobbyUI.combineText.text = "예상결과 : ";
            //뽑힌 등급의 아이템 중 하나를 인벤토리에 생성.

            StartCoroutine(FadeIn());
            PickRandomItem(targetIndex, targetRank);
            ClearCombinePanel();
            GetInventroyItemInfo();
            StartCoroutine(InitResultImage());
            FireBaseDataBase.Instance.UpdateInvenData(targetIndex);
        }

    }

    public void SortItem()
    {
        LobbyManager.Instance.inventory.SortInventory();
        GetInventroyItemInfo();

        //선택한 아이템들 초기화
        ClearCombinePanel();

    }

    //조합창에 등록된 아이템 삭제하는 함수
    public void ClearCombinePanel()
    {
        for (int i = 0; i < 2; i++)
        {
            itemImages[i].sprite = null;
            itemImages[i].enabled = false;
        }

        InventorySlot[] tempSlots = inventory[targetIndex].transform.GetChild(0).GetChild(0).GetComponentsInChildren<InventorySlot>();
        for (int j = 0; j < tempSlots.Length; j++)
        {
            tempSlots[j].transform.GetChild(1).gameObject.SetActive(false);
            tempSlots[j].isCombined = false;
        }
        combineItems.Clear();
        itemCount = 0;      
    }
    void PickRandomItem(int _ItemType, int _Rank)
    {
        List<Item> items = new List<Item>();
        string rank = DBManager.Instance.itemDB.ReturnRankToString(_Rank);

        switch (_ItemType)
        {
            case 0:
                foreach (var item in DBManager.Instance.itemDB.headDB[rank])
                {
                    items.Add((Item)item.Value);
                }
                break;

            case 1:
                foreach (var item in DBManager.Instance.itemDB.scrollDB[rank])
                {
                    items.Add((Item)item.Value);
                }
                break;

            case 2:
                foreach (var item in DBManager.Instance.itemDB.equipDB[rank])
                {
                    items.Add((Item)item.Value);
                }
                break;
        }

        float totalRate = 0;
        float weight = 0;

        for (int i = 0; i < items.Count; i++)
        {
            totalRate += items[i].pickRate;
        }

        weight = Random.Range(0f, totalRate + 1);

        for (int i = 0; i < items.Count; i++)
        {
            if (weight <= items[i].pickRate)
            {
                //아이템을 인벤토리에 추가
                LobbyManager.Instance.inventory.AddItem(items[i]);

                itemImages[2].sprite = items[i].image;

                if (items[i].itemRank.Equals(Item.ItemRank.LEGEND)) itemImages[2].material = LobbyManager.Instance.lobbyUI.pick_LegendMat;
                else itemImages[2].GetComponent<ImageOutLine>().SetOutLineColor(items[i]);

                itemImages[2].enabled = false;
                resultItemName.text = items[i].itemName;
                isReset = false;
                break;
            }
            weight -= items[i].pickRate;
        }
    }

    void ActiveIneventory(int _Num)
    {
        inventory[targetIndex].SetActive(false);
        ChangeButtonSize(false);
        targetIndex = _Num;
        LobbyManager.Instance.inventory.currIndex = _Num;
        ChangeButtonSize(true);
        inventory[targetIndex].SetActive(true);
        ClearCombinePanel();
    }

    void ChangeButtonSize(bool _Change)
    {
        if (!_Change)
        {
            combineTabs[targetIndex].sizeDelta = new Vector2(LobbyManager.Instance.lobbyUI.sButtonSize, combineTabs[targetIndex].sizeDelta.y);
            buttonText[targetIndex].fontSize = LobbyManager.Instance.lobbyUI.sTextSize;
        }
        else
        {
            combineTabs[targetIndex].sizeDelta = new Vector2(LobbyManager.Instance.lobbyUI.bButtonSize, combineTabs[targetIndex].sizeDelta.y);
            buttonText[targetIndex].fontSize = LobbyManager.Instance.lobbyUI.bTextSize;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressTime = 0f;
        isPressed = false;
        isClick = false;
    }

    IEnumerator FadeIn()
    {
        float currenTime = 0.0f;
        float percent = 0.0f;
        while (percent < 1)
        {
            currenTime += Time.deltaTime;
            percent = currenTime / 1.5f;

            Color tempC = effectImage.color;

            tempC.a = Mathf.Lerp(0, 1f, percent);

            effectImage.color = tempC;
            yield return null;
        }

        Color color = effectImage.color;
        color.a = 0f;
        effectImage.color = color;

        itemImages[2].enabled = true;
        resultItemName.gameObject.SetActive(true);
        
    }
    IEnumerator InitResultImage()
    {
        yield return new WaitForSeconds(3f);
        //결과창 이미지 바꾸기
        itemImages[2].sprite = resultDefalutImg;
        itemImages[2].material = null;
        //텍스트 끄기
        resultItemName.gameObject.SetActive(false);
        combined = false;
    }
}
