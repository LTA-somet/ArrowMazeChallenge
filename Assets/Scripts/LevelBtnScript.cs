using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelBtnScript : ProjectBehaviourScript
{
    [SerializeField] private GameObject lockObj, unlockObj;
    [SerializeField] private Image[] starArray;
    [SerializeField] private TextMeshProUGUI levelIndexText;
    [SerializeField] private Color lockColor, unlockColor;
    [SerializeField] private Button btn;
    


    private int levelIndex;
    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            OnButtonLevelClick();
        });
    }


    public void SetLevelButton(LevelItem value, int index, bool activeLevel)
    {
        if (value.unlocked)
        {
            
            levelIndex = index + 1;
            btn.interactable = true;
            lockObj.SetActive(false);
            unlockObj.SetActive(true);
            SetStar(value.startAchieved);
            levelIndexText.text = levelIndex.ToString();
        }
        else
        {
            btn.interactable = false;
            lockObj.SetActive(true);
            unlockObj.SetActive(false);
        }
    }
    private void SetStar(int starAchieved)
    {
        for (int i = 0; i < starArray.Length; i++)
        {
            if (i < starAchieved)
            {
                starArray[i].color = unlockColor;
            }
            else
            {
                starArray[i].color = lockColor;
            }
        }
    }
    private void OnButtonLevelClick()
    {
        SceneManager.LoadScene($"Level_{levelIndex}");
        LevelSystemManager.Instance.CurrentLevel = levelIndex - 1;
    }
}
