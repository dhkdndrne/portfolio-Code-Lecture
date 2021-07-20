using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private GameObject spriteGameObjcet;
    private void Awake()
    {
        spriteGameObjcet = transform.Find("Sprite").gameObject;
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingTypeChanged;
    }
    
    private void Update()
    {
        transform.position = UtilsClass.GetMouseWorldPosition();
    }
    private void Show(Sprite ghostSprite)
    {
        spriteGameObjcet.SetActive(true);
        spriteGameObjcet.GetComponent<SpriteRenderer>().sprite = ghostSprite;
            
    }

    private void Hide()
    {
        spriteGameObjcet.SetActive(false);
    }

    private void BuildingTypeChanged()
    {
        BuildingTypeSo buildingType = BuildingManager.Instance.GetActiveBuildingType();
        if(buildingType ==null)
        {
            Hide();
        }
        else
        {
            Show(buildingType.sprite);
        }
    }
}
