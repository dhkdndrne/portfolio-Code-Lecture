using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text bestScoreText;
    public void ShowScore(int num)
    {
        scoreText.text = num.ToString();
    }
    public void ShowBestScore(int num)
    {
        bestScoreText.text = num.ToString();
    }
}
