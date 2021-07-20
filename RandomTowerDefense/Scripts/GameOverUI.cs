using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI[] txt = new TextMeshProUGUI[3];
  
    void Start()
    {
        txt[0] = GameObject.Find("Round").GetComponent<TextMeshProUGUI>();
        txt[1] = GameObject.Find("KillCount").GetComponent<TextMeshProUGUI>();
        txt[2] = GameObject.Find("TowerCnt").GetComponent<TextMeshProUGUI>();
        ShowUI();
        GameDB.Instance.ResetData();
        DataController.Instance.DeleteData();
    }

    void ShowUI()
    {
        txt[0].text = "\t\t종료된 라운드 : " + GameDB.Instance.Round;
        txt[1].text = "\t\t해치운 적 : " + GameDB.Instance.KillCount;
        txt[2].text = "\t\t\t건설한 타워 수\n  \t\t노말타워 : " + GameDB.Instance.t_dictionary[RareList.NORMAL].Count + " <color=#0000ff>매직타워 </color>: " + GameDB.Instance.t_dictionary[RareList.MAGIC].Count +"\n"
            + "\t <color=#5F00FF>레어타워 </color>: " + GameDB.Instance.t_dictionary[RareList.RARE].Count + " <color=#FFBB00>유니크타워 </color>: " + 
            GameDB.Instance.t_dictionary[RareList.UNIQUE].Count + " <color=#1DDB16>에픽타워 </color>: " + GameDB.Instance.t_dictionary[RareList.EPIC].Count;
    }
    public void GoStartScene()
    {
        SceneManager.LoadScene("Title");
    }
}
