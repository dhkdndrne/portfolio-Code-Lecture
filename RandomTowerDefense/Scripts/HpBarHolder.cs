using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarHolder : MonoBehaviour
{
    Vector3 cameraPos;


    // Update is called once per frame
    void Update()
    {
        HoldHpBar();
    }

    void HoldHpBar()
    {
        cameraPos = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
        transform.LookAt(cameraPos);        
    }
}
