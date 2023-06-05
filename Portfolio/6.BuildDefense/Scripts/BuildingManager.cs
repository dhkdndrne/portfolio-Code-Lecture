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
        OnActiveBuildingTypeChanged?.Invoke(); //BuildingGhost�� �����ͼ� ���ԵǸ� Ŀ�ø��� �ʹ� ����
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
            //�ݶ��̴��� �Ǽ����� ������ ������
           BuildingTypeHolder buildingTypeHolder =  collider2D.GetComponent<BuildingTypeHolder>();
        
            if(buildingTypeHolder != null)
            {
                //BuildingTypeHolder�� ������
                if (buildingTypeHolder.buildingType == buildingType)
                {
                    //�Ǽ������� �̹� ���� �ǹ��� �ִ�.
                    return false;
                }
            }
        }
        return true;
    }
}
