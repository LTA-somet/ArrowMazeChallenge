using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : ProjectBehaviourScript
{
    public Button playBtn;
    public GameObject selectLevel;

    private void Start()
    {
        playBtn.onClick.AddListener(() =>
        {
            selectLevel.SetActive(true);
        });
    }
}
