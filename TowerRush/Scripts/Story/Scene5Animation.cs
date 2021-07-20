using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Scene5Animation : MonoBehaviour
{
    [SerializeField] GameObject unit;
    [SerializeField] GameObject finalUnit;
    [SerializeField] GameObject scrollEffect;

    void Start()
    {
        StartCoroutine(StoryAnimation());
    }
    IEnumerator StoryAnimation()
    {
        yield return new WaitForSeconds(0.5f);

        unit.transform.DOLocalMove(new Vector3(-66, 172.58f,0), 2);
        yield return unit.transform.DOScale(1.4f, 2).WaitForCompletion();

        scrollEffect.SetActive(true);
        yield return new WaitForSeconds(4f);

        scrollEffect.SetActive(false);
        unit.transform.DOLocalMove(new Vector3(436f, -379f, 0), 2);
        yield return unit.transform.DOScale(2.3f, 2).WaitForCompletion();

        Color color = finalUnit.GetComponent<Image>().color;

        while(color.a < 1)
        {
            color.a += Time.deltaTime;
            finalUnit.GetComponent<Image>().color = color;
            yield return null;
        }
    }
}
