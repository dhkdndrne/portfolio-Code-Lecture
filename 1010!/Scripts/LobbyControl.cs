using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyControl : MonoBehaviour
{
    [SerializeField]Text bestScore;
    [SerializeField]Button GameStartBtn;

    void Start()
    {
        if(PlayerPrefs.HasKey("bestScore"))
        bestScore.text = PlayerPrefs.GetInt("bestScore").ToString();

        GameStartBtn.onClick.AddListener(() => SceneManager.LoadSceneAsync(1));
    }

}
