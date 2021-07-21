using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public Action OnResourceAmountChanged;  //�ڿ��� �����ɶ� ȣ��

    private Dictionary<ResourceTypeSo, int> resourceAmountDic;  //�ڿ� ����� ������ ��ųʸ�

    private void Awake()
    {
        Instance = this;
        resourceAmountDic = new Dictionary<ResourceTypeSo, int>();

        //Resource �������� ResourceTypeListSoŸ���� ������ ������
        ResourceTypeListSo resourceTypeList = Resources.Load<ResourceTypeListSo>(typeof(ResourceTypeListSo).Name);

        //��ųʸ��� ����Ʈ�� �ڿ����� �־���
        foreach (ResourceTypeSo resourceType in resourceTypeList.list)
        {
            resourceAmountDic[resourceType] = 0;
        }
    }

    //�ڿ��߰�
    public void AddResource(ResourceTypeSo resourceType,int amount)
    {
        resourceAmountDic[resourceType] += amount;
        OnResourceAmountChanged?.Invoke();
    }
    //�ڿ��� �����ö�
    public int GetResourceAmount(ResourceTypeSo resourceType)
    {
        return resourceAmountDic[resourceType];
    }
    private void Test()
    {
        foreach(ResourceTypeSo resourceType in resourceAmountDic.Keys)
        {
            Debug.Log(resourceType.nameString + ": " + resourceAmountDic[resourceType]);
        }
    }
}
