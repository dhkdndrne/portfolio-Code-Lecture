using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShowStory : MonoBehaviour
{
    [SerializeField] Button[] storys;
    [SerializeField] string[] narrations;
    [SerializeField] Text text;
    [SerializeField] RectTransform textBG;
    [SerializeField] GameObject nextPageText;
    [SerializeField] FadeController fader;
    System.Action action;
    int idx = 0;
    bool check = false;

    private void Start()
    {
        text.DOText(narrations[idx], 1);
        action += ()=>LoadingControl.LoadScene("MainScene");
    }
    private void Update()
    {
        textBG.sizeDelta = new Vector2(text.rectTransform.rect.width + 100, text.rectTransform.rect.height);
        if (text.text.Equals(narrations[idx]))
        {
            check = true;
            nextPageText.SetActive(true);
        }
    }

    //클릭하면 텍스트가 다 보여지고 한번더 누르면 장면이 넘어간다.
    public void SkipText()
    {
        if (!check)
        {
            text.DOComplete();
            nextPageText.SetActive(true);
            check = true;
        }
    }

    public void NextStroy()
    {
        if (check)
        {
            nextPageText.SetActive(false);
            if (idx < storys.Length - 1)
            {
                storys[idx].gameObject.SetActive(false);
                storys[++idx].gameObject.SetActive(true);

                text.text = string.Empty;
                text.DOText(narrations[idx], 1);
            }
            else
            {
                fader.FadeIn(0.5f, action);
            }
            check = false;
        }
        else
        {
            SkipText();
        }
    }
    private void OnDestroy()
    {
        storys = null;
        narrations = null;
        text = null;
        textBG = null;
        nextPageText = null;
        fader = null;

        Resources.UnloadUnusedAssets();
    }
}
