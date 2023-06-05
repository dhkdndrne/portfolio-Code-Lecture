using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ResourceGeneratorData
{
    //자원 타입과 생산량을 유연하게 지정해 줄 수 있다.
    public float timerMax;
    public ResourceTypeSo resourceType;
    public float resourceDetectionRadius;
    public int maxResourceAmount;
}
