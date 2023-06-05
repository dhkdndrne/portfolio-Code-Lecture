using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShopEffect : MonoBehaviour
{
    public float rotationSpeed;
    public float biggerTime;
    public bool isReverse;
    public bool isLateCreate;
    public float delayTime;
    bool fitScale;

    private void OnEnable()
    {
        fitScale = false;
        transform.localScale = new Vector3(0,0,0);
        StartCoroutine(StartShopEffect());
    }
    IEnumerator StartShopEffect()
    {
        if (isLateCreate) yield return new WaitForSeconds(delayTime);
        
        while(true)
        {
            if (fitScale)
            {
                if (!isReverse) transform.Rotate(0, 0, Time.deltaTime * rotationSpeed, Space.Self);
                else transform.Rotate(0, 0, -Time.deltaTime * rotationSpeed, Space.Self);
            }
            else
            {
                transform.DOScaleX(1f, biggerTime);
                transform.DOScaleY(1f, biggerTime);
                fitScale = true;
            }
            yield return null;
        }
    }
}
