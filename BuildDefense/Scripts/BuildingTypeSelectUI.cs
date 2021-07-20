using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuildingTypeSelectUI : MonoBehaviour
{
    [SerializeField] private Sprite arrowSprite;     //default�� ȭ��ǥ �̹���
    private Dictionary<BuildingTypeSo, Transform> btnTransformDic;
    private Transform arrowBtn; //ȭ��ǥ ��ư
    private void Awake()
    {
        Transform btnTemplate = transform.Find("ButtonTemplate");   //��ư ���ø��� ã�Ƽ� ���ش�.
        btnTemplate.gameObject.SetActive(false);

        BuildingTypeListSo buildingTypeList = Resources.Load<BuildingTypeListSo>(typeof(BuildingTypeListSo).Name);  //Resource�������� ����Ÿ�Ը���Ʈ ������ ������

        btnTransformDic = new Dictionary<BuildingTypeSo, Transform>();
        int index = 0;

        arrowBtn = Instantiate(btnTemplate, transform); //ȭ��ǥ(default ��ư)����
        arrowBtn.gameObject.SetActive(true);

        float offsetAmount = 220f;
        arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

        arrowBtn.Find("Image").GetComponent<Image>().sprite = arrowSprite;
        arrowBtn.Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -60);

        arrowBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuildingManager.Instance.ActiveBuildingType(null);
        });
        index++;


        foreach (BuildingTypeSo buildingType in buildingTypeList.buildingTypeList)
        {
            Transform btnTransform = Instantiate(btnTemplate, transform);
            btnTransform.gameObject.SetActive(true);

            offsetAmount = 220f;
            btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

            btnTransform.Find("Image").GetComponent<Image>().sprite = buildingType.sprite;

            btnTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuildingManager.Instance.ActiveBuildingType(buildingType);
            });

            btnTransformDic[buildingType] = btnTransform;
            index++;
        }
    }
    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += UpdateActiveBuildingTypeButton; //������Ʈ���� ������ ��ȿ���� ���� Ÿ���� ����ɶ��� ȣ��
        UpdateActiveBuildingTypeButton();
    }
    private void UpdateActiveBuildingTypeButton()
    {
        arrowBtn.Find("Selected").gameObject.SetActive(false);

        foreach (var buildingType in btnTransformDic.Keys)
        {
            Transform btnTransform = btnTransformDic[buildingType];
            btnTransform.Find("Selected").gameObject.SetActive(false);
        }

        BuildingTypeSo activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();

        if (activeBuildingType == null)
        {
            arrowBtn.Find("Selected").gameObject.SetActive(true);
        }
        else
        {
            btnTransformDic[activeBuildingType].Find("Selected").gameObject.SetActive(true);
        }

    }
}
