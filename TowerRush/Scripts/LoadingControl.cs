using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingControl : MonoBehaviour
{
    static string nextScene;
    [SerializeField]
    string[] tips;

    [SerializeField]
    Text tipText;

    [SerializeField]
    Text percentTxt;

    [SerializeField]
    Image unit;
    [SerializeField]
    GameObject unitParent;

    [SerializeField]
    Sprite[] unitSprite;

    float timer;
    int idx;
    float value;
    public FadeController fader;

    float tipTimer;

    public static void LoadScene(string _sceneName)
    {
        nextScene = _sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    private void Update()
    {
        if (timer <= 0)
        {
            unit.sprite = unitSprite[idx];
            timer = .2f;
            if (idx.Equals(1)) idx = 0;
            else idx++;
        }

        if (tipTimer <= 0)
        {
            tipText.text = tips[Random.Range(0, tips.Length)];

            tipTimer = 2f;
        }

        tipTimer -= Time.deltaTime;
        timer -= Time.deltaTime;
    }

    IEnumerator LoadSceneProcess()
    {
        //yield return null;
        fader.FadeOut(.5f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);
        // 90퍼 정도에서 로딩이 멈춤 (로딩씬 잠깐 깜빡 하는거 막기위해)
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            yield return null;

            if (operation.progress < 0.9f)
            {
                //value = operation.progress;
                value = Mathf.MoveTowards(value, 1f, Time.deltaTime);
            }
            else
            {
                value = Mathf.MoveTowards(value, 1f, Time.deltaTime);

                if (value >= 1f)
                {
                    if (fader.FadeIn(1f))
                    {
                        operation.allowSceneActivation = true;
                        percentTxt.text = "100%";
                        yield break;
                    }
                }
            }
            
            CharWalkAnim(value);
            percentTxt.text = ((int)(value * 100f)).ToString() + "%";         

        }
    }

    //로딩 캐릭터 걷는효과
    void CharWalkAnim(float _Value)
    {
        unitParent.transform.localPosition = new Vector3(0, (_Value * 100) * (369 * 0.01f), 0);
        unit.transform.localScale = new Vector3(1 - _Value, 1 - _Value, 1 - _Value);

        if (unitParent.transform.localPosition.y >= 369) unitParent.transform.localPosition = new Vector3(0, 369, 0);
    }
}
