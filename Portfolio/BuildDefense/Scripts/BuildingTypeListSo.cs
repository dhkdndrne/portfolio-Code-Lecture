using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/builingTypeList")]
public class BuildingTypeListSo : ScriptableObject
{
    //����Ÿ�Ե��� �������ִ� ����Ʈ
    public List<BuildingTypeSo> buildingTypeList;
}
