using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    public CanvasGroup firstPanel;
    public CanvasGroup secondPanel;
    public new Camera camera;
    float targetX = -50f;
    bool selectLevel = false;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        firstPanel = GameObject.Find("Group1").GetComponent<CanvasGroup>();
        secondPanel = GameObject.Find("LevelSelectGroup").GetComponent<CanvasGroup>();

        OnStateOpen(true);
        OnStateOpen2(false);
    }

    //시작창
    public void OnStateOpen(bool isOpened)
    {
        firstPanel.alpha = (isOpened) ? 1.0f : 0.0f;
        firstPanel.interactable = isOpened;
        firstPanel.blocksRaycasts = isOpened;
    }
    //난이도 고르는창
    public void OnStateOpen2(bool isOpened)
    {
        secondPanel.alpha = (isOpened) ? 1.0f : 0.0f;
        secondPanel.interactable = isOpened;
        secondPanel.blocksRaycasts = isOpened;
    }

    public void GameStart()
    {
        OnStateOpen(false);
        selectLevel = true;
    }

    
    private void Update()
    {
        if (selectLevel)
        {
            if(targetX < 10)
            {
                targetX += 20f * Time.deltaTime;
                camera.transform.rotation = Quaternion.Euler(targetX, camera.transform.rotation.y, camera.transform.rotation.z);              
                camera.transform.position = Vector3.Lerp(camera.transform.position, new Vector3(35, 10,-0.5f),Time.deltaTime);
            }
            else
            {
                OnStateOpen2(true);
                selectLevel = false;
            }
           
        }
    }

    public void Continue()
    {
        DataController.Instance.LoadGameData();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void SelectEasy()
    {

        GameDB.Instance.SelectDifficulty("EASY");
        SceneManager.LoadScene("LoadingScene");
    }

    public void SelectNormal()
    {
        GameDB.Instance.SelectDifficulty("NORMAL");
        SceneManager.LoadScene("LoadingScene");
    }

    public void SelectHard()
    {
        GameDB.Instance.SelectDifficulty("HARD");
        SceneManager.LoadScene("LoadingScene");
    }
}
