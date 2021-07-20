using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/builingTypeList")]
public class BuildingTypeListSo : ScriptableObject
{
    //빌딩타입들을 가지고있는 리스트
    public List<BuildingTypeSo> buildingTypeList;
}
