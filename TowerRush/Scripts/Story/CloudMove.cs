using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CloudMove : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] float targetPoint;
    [SerializeField] float reStartX;
    void Start()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        TweenParams tParms = new TweenParams().SetEase(Ease.Linear);       
        yield return transform.DOLocalMoveX(targetPoint, duration).SetAs(tParms).WaitForCompletion();
        transform.localPosition = new Vector3(reStartX,transform.localPosition.y, 0);
        yield return new WaitForSeconds(.5f);

        StartCoroutine(Move());
    }
}
