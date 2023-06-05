using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
public class DataController : MonoBehaviour
{ 
    static GameObject _container;
    static GameObject Container
    {
        get
        {
            return _container;
        }
    }

    static DataController instance;
    public static DataController Instance
    {
        get
        {
            if(!instance)
            {
                _container = new GameObject();
                _container.name = "DataController";
                instance = _container.AddComponent(typeof(DataController)) as DataController;
                DontDestroyOnLoad(_container);
            }
            return instance;
        }
    }
    public string GameDataFileName = "/gameSaveFile.json";
    public GameData _gameData;
    public GameData gameData
    {
        get
        {
            if (_gameData == null)
            {
                SaveGameData();
                LoadGameData();
                _gameData = new GameData();

            }
            return _gameData;
        }
    }

    public void DeleteData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;
        
        if(File.Exists(filePath))
        {           
            Debug.Log("불러오기 성공");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
            _gameData.SetLoadDataInfo();
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            Debug.Log("새로운 파일 생성");
            _gameData = new GameData();
        }
    }

    public void SaveGameData()
    {
        if(_gameData !=null)
        {
            if (SceneManager.GetActiveScene().name == "MainScene")
            {
                _gameData.SaveInfo();
                _gameData.SaveTileInfo();
            }

            string ToJsonData = JsonUtility.ToJson(gameData);
            File.WriteAllText(Application.persistentDataPath + GameDataFileName, ToJsonData);

            Debug.Log(Application.persistentDataPath + GameDataFileName);
            Debug.Log("저장성공");
        }
        else
        {
            _gameData = new GameData();

            if (SceneManager.GetActiveScene().name == "MainScene" )
            {
                _gameData.SaveInfo();
                _gameData.SaveTileInfo();
            }

            string ToJsonData = JsonUtility.ToJson(gameData);
            File.WriteAllText(Application.persistentDataPath + GameDataFileName, ToJsonData);

            Debug.Log(Application.persistentDataPath + GameDataFileName);
            Debug.Log("저장성공");
        }
        
    }

    //게임이 종료되면 실행되는 함수
    //private void OnApplicationQuit()
    //{
    //    SaveGameData();
    //}

}
