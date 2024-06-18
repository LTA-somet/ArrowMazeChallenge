using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class SaveLoadData : ProjectBehaviourScript
{

    private static SaveLoadData instance;

    public static SaveLoadData Instance
    {
        get => instance;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause == true)
        {
            SaveData();
        }
    }

    public void Initialize()
    {
        if (PlayerPrefs.GetInt("GameStartedFirstTime") == 1)
        {
            LoadData();
        }
        else
        {
            SaveData();
            PlayerPrefs.SetInt("GameStartedFirstTime", 1);
        }
    }

    public void SaveData()
    {
        string levelDataString = JsonUtility.ToJson(LevelSystemManager.Instance.levelData);
        try
        {
            System.IO.File.WriteAllText(Application.persistentDataPath + "/LevelData.json", levelDataString);
            Debug.Log("data saved");
        }
        catch (System.Exception e)
        {
            Debug.Log("err save" + e);
            throw;
        }
    }
    public void LoadData()
    {
        try
        {
            string levelDataString = System.IO.File.ReadAllText(Application.persistentDataPath + "/LevelData.json");
            LevelData levelData = JsonUtility.FromJson<LevelData>(levelDataString);
            if (levelData != null)
            {
                LevelSystemManager.Instance.levelData.levelItemArray = levelData.levelItemArray;
                LevelSystemManager.Instance.levelData.lastUnlockedLevel = levelData.lastUnlockedLevel;
            }

            Debug.Log("data loaded");
        }
        catch (System.Exception e)
        {
            Debug.Log("err load" + e);
            throw;
        }
    }
}
