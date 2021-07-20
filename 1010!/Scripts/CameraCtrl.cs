using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    Camera camrea;
    [SerializeField]float boardUnit;
    void Start()
    {
        camrea = GetComponent<Camera>();

        camrea.orthographicSize = boardUnit / camrea.aspect;
    }


}
