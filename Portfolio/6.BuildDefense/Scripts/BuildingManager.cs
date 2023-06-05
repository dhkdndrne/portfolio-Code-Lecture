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
            if (activeBuildingType != null && CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition()))
            {
                Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
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

    private bool CanSpawnBuilding(BuildingTypeSo buildingType, Vector3 position)
    {
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

        bool isAreaClear = collider2DArray.Length == 0;
        if (!isAreaClear) return false;

        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            //콜라이더가 건설가능 범위에 들어갔을때
           BuildingTypeHolder buildingTypeHolder =  collider2D.GetComponent<BuildingTypeHolder>();
        
            if(buildingTypeHolder != null)
            {
                //BuildingTypeHolder가 있으면
                if (buildingTypeHolder.buildingType == buildingType)
                {
                    //건설범위에 이미 같은 건물이 있다.
                    return false;
                }
            }
        }
        return true;
    }
}
