using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIManager : ProjectBehaviourScript
{
    [SerializeField] private GameObject levelBtnHolder;
    [SerializeField] private LevelBtnScript levelBtnPrefab;

    private void Start()
    {
        InitializeUI();
    }
    public void InitializeUI()
    {
        LevelItem[] levelItems = LevelSystemManager.Instance.levelData.levelItemArray;

        for (int i = 0; i < levelItems.Length; i++)
        {
            LevelBtnScript levelBtn = Instantiate(levelBtnPrefab, levelBtnHolder.transform);
            levelBtn.SetLevelButton(levelItems[i], i, i == LevelSystemManager.Instance.levelData.lastUnlockedLevel);
        }
    }
}
