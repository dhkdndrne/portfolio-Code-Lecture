using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public Action OnResourceAmountChanged;  //자원이 증가될때 호출

    private Dictionary<ResourceTypeSo, int> resourceAmountDic;  //자원 목록을 저장할 딕셔너리

    private void Awake()
    {
        Instance = this;
        resourceAmountDic = new Dictionary<ResourceTypeSo, int>();

        //Resource 폴더에서 ResourceTypeListSo타입의 에셋을 가져옴
        ResourceTypeListSo resourceTypeList = Resources.Load<ResourceTypeListSo>(typeof(ResourceTypeListSo).Name);

        //딕셔너리에 리스트의 자원들을 넣어줌
        foreach (ResourceTypeSo resourceType in resourceTypeList.list)
        {
            resourceAmountDic[resourceType] = 0;
        }
    }

    //자원추가
    public void AddResource(ResourceTypeSo resourceType,int amount)
    {
        resourceAmountDic[resourceType] += amount;
        OnResourceAmountChanged?.Invoke();
    }
    //자원량 가져올때
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
