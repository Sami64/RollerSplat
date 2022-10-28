using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    GroundPiece[] allGroundPieces;
    public bool isGameStart;
    [CanBeNull]
    public TextMeshProUGUI timerText;
    float timer = 60;

    [CanBeNull]
    public GameObject titleScreen;
    


    // Start is called before the first frame update
    void Start()
    {
        isGameStart = true;
        timer = 60;
        timerText.text = " " + timer;
        SetupNewLevel();
    }



    //public void StartGame(bool isDynamic)
    //{
    //    isGameStart = true;
    //    titleScreen.SetActive(false);
    //    if (isDynamic)
    //        NextLevel(1);
    //    SetupNewLevel();

    //}

    // Update is called once per frame
    void Update()
    {
       if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            
            StartTimer();
        }
    }

    void StartTimer() {
       
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            updateTimer(timer);
        } else
        {
            timer = 0;
            GameOver();
        }
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void GameOver() {
        titleScreen.SetActive(true);
        isGameStart = false;
    }

    public void RestartGame()
    {
        NextLevel(true);
    }

    void SetupNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>();
    }

    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        } else if(singleton != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetupNewLevel();
    }

    public void CheckComplete()
    {
        bool isFinished = true;
        for(int i = 0; i < allGroundPieces.Length; i++)
        {
            if (allGroundPieces[i].isColored == false)
            {
                isFinished = false;
                break;
            }
        }

        if (isFinished)
        {
           NextLevel(false);
        }
    }

    void NextLevel(bool isGameOver)
    {
        if (isGameOver)
        {
            SceneManager.LoadScene(0);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
