using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystemManager : ProjectBehaviourScript
{
    private static LevelSystemManager instance;
    [SerializeField] public LevelData levelData;

    private SaveLoadData saveLoadData;

    private int currentLevel;
    public int CurrentLevel
    {
        get => currentLevel;
        set => currentLevel = value;
    }
    public static LevelSystemManager Instance { get => instance; }


    private void Awake()
    {
        saveLoadData = GameObject.Find("SaveLoadData").GetComponent<SaveLoadData>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        saveLoadData.Initialize();
    }


    public void LevelComplete(int starAchieved)
    {
        levelData.levelItemArray[currentLevel].startAchieved = starAchieved;
        if (levelData.lastUnlockedLevel < currentLevel + 1 && starAchieved > 0)
        {
            levelData.lastUnlockedLevel = currentLevel + 1;
            levelData.levelItemArray[levelData.lastUnlockedLevel].unlocked = true;
        }
        SaveLoadData.Instance.SaveData();
    }

}

[System.Serializable]
public class LevelData
{
    public int lastUnlockedLevel = 0;
    public LevelItem[] levelItemArray;
}



[System.Serializable]
public class LevelItem
{
    public bool unlocked;
    public int startAchieved;
}
