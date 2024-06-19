using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : ProjectBehaviourScript
{
    public static GameManager Instance;
    public Button homeBtn;
    public Button resetBtn;
    public TextMeshProUGUI textTime;
    public float timeCountDown;
    public bool codition_1 = false;

    private int _currentSceneIndex;

    public GameObject winObject;
    public GameObject  loseObject;

    private void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
    }
    private void Start()
    {
        homeBtn.onClick.AddListener(() =>
        {
            OnHomeButtonClick();
        });
        resetBtn.onClick.AddListener(() =>
        {
            OnResetButtonClick();
        });
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        if(timeCountDown > 0)
        {
            timeCountDown -= Time.deltaTime;
        }
        else if(timeCountDown < 0)
        {
            timeCountDown = 0;
            textTime.color = Color.red;
            Invoke("LoseGame", 1f);
            LoseGame();
            
        }
        int minutes = Mathf.FloorToInt(timeCountDown / 60);
        int seconds = Mathf.FloorToInt(timeCountDown % 60);

        textTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);


    }

    public void OnHomeButtonClick()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void OnResetButtonClick()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Time.timeScale = 1;
    }
    public void OnNextButtonClick()
    {
        SceneManager.LoadScene(_currentSceneIndex + 1);
        Time.timeScale = 1;
    }
    
    public void WinGame()
    {
        Debug.Log("win game");
        winObject.SetActive(true);
        Time.timeScale = 0;
    }
    public void LoseGame()
    {
        Debug.Log("lose game");
        loseObject.SetActive(true);
        LevelSystemManager.Instance.LevelComplete(0);
    }

    #region
    protected override void LoadComponent()
    {
        homeBtn = GameObject.Find("HomeBtn").GetComponent<Button>();
        resetBtn = GameObject.Find("ResetBtn").GetComponent<Button>();
        textTime = GameObject.Find("TimeTxt").GetComponent<TextMeshProUGUI>();
        winObject = GameObject.Find("Wingame");
        loseObject = GameObject.Find("Losegame");

        timeCountDown = 45;
        codition_1 = false;
    }
    #endregion
}
