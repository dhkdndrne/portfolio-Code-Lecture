using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    static GameManager instance = null;
    public static GameManager Instance { get { return instance; } }

    [SerializeField] Transform[] posGroup;
    public Transform[] PosGroup { get { return posGroup; } }

    BlockSpwaner blockSpwaner = new BlockSpwaner();
    UIManager uiManager;
    int score;
    int count = 3;
    int bestScore;
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        blockSpwaner.SpawnBlocks(posGroup);
        uiManager = FindObjectOfType<UIManager>();
        bestScore = PlayerPrefs.HasKey("bestScore") ? PlayerPrefs.GetInt("bestScore") : 0;
        uiManager.ShowBestScore(bestScore);
    }
    public void GetScore(int num)
    {
        score += num;
        uiManager.ShowScore(score);

    }

    public void GameOver()
    {
        if (bestScore < score)
        {
            PlayerPrefs.SetInt("bestScore", score);
        }

        SceneManager.LoadScene(0);
    }
    public void CheckBlockCount()
    {
        if (count > 0) count--;

        if (count == 0)
        {
            count = 3;
            blockSpwaner.SpawnBlocks(posGroup);
        }
    }

}
