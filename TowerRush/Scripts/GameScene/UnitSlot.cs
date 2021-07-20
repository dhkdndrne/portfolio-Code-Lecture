using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSlot : MonoBehaviour
{
    List<GameObject> unitContainer = new List<GameObject>();
    Button unitButton;
    public Image headImg { get; private set; }         // 히로디온
    public Image shoeImg { get; private set; }          // 신발
    public Image armorImg { get; private set; }         // 방어구
    Image coolTimeImg;      // 쿨타임 이미지
    Text Idtext;

    [SerializeField] float coolTime;         // 쿨타임
    public int clickCnt { get; private set; }
    int id;
    bool isCreate;
    int layer;              //이미지 레이어
    string shoeNumber;
    Unit unit;
    void Awake()
    {
        unitButton = GetComponent<Button>();
        headImg = transform.GetChild(0).GetChild(2).GetComponent<Image>();
        armorImg = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        shoeImg = transform.GetChild(0).GetChild(0).GetComponent<Image>();

        coolTimeImg = transform.GetChild(1).GetComponent<Image>();
        Idtext = transform.GetChild(2).GetComponent<Text>();
    }
    private void Start()
    {
        UnitPooling();
    }

    void UnitPooling()
    {
        GameObject parent = GameObject.Find("UnitContainer");
        GameObject newObj = new GameObject(id.ToString());
        newObj.transform.parent = parent.transform;
        for (int i = 0; i < 30; i++)
        {
            //나중에 오브젝트 풀링하기
            GameObject unitObj = Instantiate(GameManager.Instance.unitPrefab, newObj.transform);
            unitObj.name = (id + 1).ToString() + "번 유닛 " + (i + 1);
            unitObj.GetComponent<UnitAbillity>().InitStat(unit);
            unitObj.GetComponent<UnitAbillity>().InstanceSkillData(unit);
            unitObj.SetActive(false);
            unitContainer.Add(unitObj);
        }
    }
    public void Init(Factory _Factory, int i)
    {
        if (_Factory.equipedItems[0] != null) headImg.sprite = _Factory.equipedItems[0].image;
        if (_Factory.equipedItems[2] != null) armorImg.sprite = _Factory.equipedItems[2].image;
        if (_Factory.equipedItems[3] != null) { shoeImg.sprite = _Factory.equipedItems[3].image; shoeNumber = _Factory.equipedItems[3].id.ToString(); }

        id = _Factory.factoryId;
        unit = _Factory.unit;

        Idtext.text = i.ToString();
        coolTime = _Factory.coolTime <= 1 ? 1 : _Factory.coolTime / 4;

    }


    //유닛소환
    public void CreateUnit()
    {
        if (!isCreate && GameManager.Instance.summonReady && GameManager.Instance.curPop + 1 <= GameManager.Instance.limitPop)
        {
            coolTimeImg.fillAmount = 1f;
            isCreate = true;
            StartCoroutine(GameManager.Instance.UpdateCoolTime());
            StartCoroutine(UpdateCoolTime());

            GameObject obj = unitContainer.Find(unitobj => unitobj.activeSelf.Equals(false));
            //활성화 할 유닛이 없을때
            if (obj == null)
            {
                obj = unitContainer.Find(unitobj => unitobj.activeSelf.Equals(true));
                GameObject tempObj = null;

                for (int i = 0; i < 30; i++)
                {
                    tempObj = Instantiate(obj, Vector3.zero, Quaternion.identity, transform);
                    tempObj.SetActive(false);

                    unitContainer.Add(tempObj);
                }
                //킬 유닛 다시 찾는다.
                obj = unitContainer.Find(unitobj => unitobj.activeSelf.Equals(false));
            }
            
            obj.GetComponent<UnitAbillity>().InitStat(unit);
            obj.SetActive(true);

            //다리 이미지 게임용 이미지로 바꾸는 코드
            Sprite temp;
            if (shoeNumber != null) temp = Resources.Load<Sprite>("InGameShoe/" + shoeNumber);
            else temp = Resources.Load<Sprite>("InGameShoe/Basic");

            obj.GetComponent<UnitImage>().ImageChange(headImg.sprite, armorImg.sprite, temp);

            //유닛들이 겹쳐보이지 않게 이미지 레이어 설정
            if (layer > 0) layer--;
            else layer = 4;
            obj.GetComponent<UnitImage>().ChangeLayer(layer);

            GameManager.Instance.curPop++;
            clickCnt++;
        }
    }

    IEnumerator UpdateCoolTime()
    {
        while (isCreate)
        {
            coolTimeImg.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;

            if (coolTimeImg.fillAmount <= 0)
            {
                isCreate = false;
            }

            yield return null;
        }
    }

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
}
