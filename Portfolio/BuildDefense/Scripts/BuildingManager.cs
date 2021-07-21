using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    public Action OnActiveBuildingTypeChanged;
    private BuildingTypeSo activeBuildingType;      //���� ����Ÿ��
    private BuildingTypeListSo buildingTypeList;    //����Ÿ�� ����Ʈ

    private void Awake()
    {
        Instance = this;
        //Resource ������ �ִ� BuildingTypeListSoŸ���� ������ ������(��ũ���ͺ� ������Ʈ)
        buildingTypeList = Resources.Load<BuildingTypeListSo>(typeof(BuildingTypeListSo).Name);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //���� ������ �ǹ� Ÿ���� �������
            if(activeBuildingType !=null)
            {
                Instantiate(activeBuildingType.prefab,UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
            }
           
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            activeBuildingType = buildingTypeList.buildingTypeList[0];
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            activeBuildingType = buildingTypeList.buildingTypeList[1];
        }
    }

    public void ActiveBuildingType(BuildingTypeSo buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChanged?.Invoke(); //BuildingGhost�� �����ͼ� ���ԵǸ� Ŀ�ø��� �ʹ� ����
    }

    public BuildingTypeSo GetActiveBuildingType()
    {
        return activeBuildingType;
    }
}
