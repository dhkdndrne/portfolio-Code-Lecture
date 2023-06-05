using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    public Slider loadingBar;

    float time = 0;
    private void Start()
    {
        loadingBar.value = 0;
    }
    void Update()
    {
        time += 0.2f * Time.deltaTime;
        loadingBar.value = Mathf.Lerp(loadingBar.value, time, 5f * Time.deltaTime);

        if(loadingBar.value >=1)
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
