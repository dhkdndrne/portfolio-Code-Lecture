using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    public Action OnActiveBuildingTypeChanged;
    private BuildingTypeSo activeBuildingType;      //현재 빌딩타입
    private BuildingTypeListSo buildingTypeList;    //빌딩타입 리스트

    private void Awake()
    {
        Instance = this;
        //Resource 폴더에 있는 BuildingTypeListSo타입의 에셋을 가져옴(스크립터블 오브젝트)
        buildingTypeList = Resources.Load<BuildingTypeListSo>(typeof(BuildingTypeListSo).Name);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //현재 선택한 건물 타입이 있을경우
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
        OnActiveBuildingTypeChanged?.Invoke(); //BuildingGhost를 가져와서 쓰게되면 커플링이 너무 심함
    }

    public BuildingTypeSo GetActiveBuildingType()
    {
        return activeBuildingType;
    }
}
