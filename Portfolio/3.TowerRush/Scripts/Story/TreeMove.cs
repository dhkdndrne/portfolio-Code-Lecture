using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TreeMove : MonoBehaviour
{
    [SerializeField] float targetScale;
    [SerializeField] float duration;
    [SerializeField] float delay;
    void Start()
    {
        StartCoroutine(TreeAnim1());
    }

    IEnumerator TreeAnim1()
    {
        TweenParams tParms = new TweenParams().SetDelay(delay).SetEase(Ease.Linear);
        yield return transform.DOScale(targetScale,duration).SetAs(tParms).WaitForCompletion();
    }
}
