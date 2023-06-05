using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResourcesUI : MonoBehaviour
{
    private ResourceTypeListSo resourceTypeList;
    private Dictionary<ResourceTypeSo, Transform> resourceTypeTransformDic;

    private void Awake()
    {
        resourceTypeList = Resources.Load<ResourceTypeListSo>(typeof(ResourceTypeListSo).Name);

        resourceTypeTransformDic = new Dictionary<ResourceTypeSo, Transform>();

        Transform resourceTemlate = transform.Find("ResourceTemplate");
        resourceTemlate.gameObject.SetActive(false);

        int index = 0;
        foreach (ResourceTypeSo resourceType in resourceTypeList.list)
        {
            Transform resourceTransform = Instantiate(resourceTemlate, transform);
            resourceTransform.gameObject.SetActive(true);

            float offsetAmount = -160f;
            resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

            resourceTransform.Find("Image").GetComponent<Image>().sprite = resourceType.sprite;
            resourceTypeTransformDic[resourceType] = resourceTransform;

            index++;
        }
    }

    private void UpdateResourceAmount()
    {
        foreach (ResourceTypeSo resourceType in resourceTypeList.list)
        {         
            Transform resourceTransform = resourceTypeTransformDic[resourceType];
            int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType);

            resourceTransform.Find("Text").GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString());
        }
    }
    private void Start()
    {
        UpdateResourceAmount();
        ResourceManager.Instance.OnResourceAmountChanged += UpdateResourceAmount;
    }
}
