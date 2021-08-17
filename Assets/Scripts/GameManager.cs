using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // Configs 
    [Header("Health System")]
    [Tooltip("The max number of cats that will spawn in level.")]
    [SerializeField] int playerHealth = 3;
    [SerializeField] int maxNumOfHealth = 3; // when this is equal to 3, I should have only 3 heart containers visible in my scene view
    [SerializeField] Image[] hearts;
    
    [Header("Level System")]
    [SerializeField] float looseScreenCountDown = 3;

    [Header("Score System")] 
    [Tooltip("This is the Starting score value")]
    [SerializeField] int score = 0;
    // [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI winPopupScoreText;
    [SerializeField] float scoreUpdateSpeed = 0.3f;
    

    [Header("Timer")] 
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject timerGameObject;

    [Header("Popup Canvases")] 
    [SerializeField] GameObject winCanvas;
    [SerializeField] GameObject looseCanvas;
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject settingsCanvas;

    [Header("Popup Canvases Transitions")] // delete later
    [SerializeField] float delayToZeroTimeScale = 1.1f;
    [SerializeField] float delayToOneTimeScale = 1.1f;
    // [SerializeField] Animator winAnimator;
    // [SerializeField] Animator loseAnimator;
     
    
    // Cache
    Enemy[] _enemies;
    CatHandler _catHandler;
    LevelSelection _levelSelection;
    int _currentSceneIndex;
    float _secondsLeft;
    float _displayScore = 0;
    // public bool isPauseCanvasEnable;

    private void OnEnable()
    {
        _levelSelection = FindObjectOfType<LevelSelection>();

        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // adds all the enemies in the scene to the "_enemies" array
        _enemies = FindObjectsOfType<Enemy>();
    }
    

    private void Start()
    {
        _catHandler = FindObjectOfType<CatHandler>();
        
        // scoreText.text = score.ToString();
        StartCoroutine(IncrementalScoreUpdate());

        timerText.text = _secondsLeft.ToString();
        _secondsLeft = looseScreenCountDown;
    }

    private void Update()
    {
        WinCondition();
        HeartsSystem();
        
        StartCoroutine(LoseCondition());
    }
    
    
    private void HeartsSystem()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth) /* controls how many hearts should be displayed in relation to
                                       the max allowed health that the player can have */
            {                       
                hearts[i].enabled = true;  
            }
            else
            {
                hearts[i].enabled = false; // This hides any extra hearts in the array that we have, that is more than maxNumOfHealth.
            }
        }
    }
    
    // This method is called in the CatHandler class
    public void UpdateNumberOfCats()
    {
        playerHealth--;

        if (playerHealth <= 0)
        {
            _catHandler.spawnCats = false;
        }
    }
    
    // Checks if all enemies are dead in the scene
    private bool AllEnemiesDead() 
    {
        foreach (var enemy in _enemies)
        {
            // if there are enemies
            if (enemy != null)
            {
                
                // trigger lose message and retry button
                // if retry button clicked reload scene
                return false;
            }
        }
        return true;
    }
    
    
    private void WinCondition()
    {
        if (AllEnemiesDead() && playerHealth >= 0)
        {
            if (_currentSceneIndex == 10) // MAX LEVEL
            {
                Debug.Log("YOU WON THE GAAAAMEE!");
                // Show Win game screen or more coming soon screen
            }
            else
            {
                Debug.Log("You Won the level!!");
                _levelSelection.UpdateCurrentLevelPlayerPref();
                winCanvas.SetActive(true);
                // StartCoroutine(SetTimeScaleToZeroCoroutine());
                SetTimeScaleToZero();
            }
        }
    }
    
    
    IEnumerator LoseCondition()
    {
        if (!AllEnemiesDead() && playerHealth <= 0 && winCanvas.activeSelf == false)
        {
            LoseCountDownStart();
            yield return new WaitForSeconds(looseScreenCountDown);
            Debug.Log("YOU LOOOOSEE");
            looseCanvas.SetActive(true);
            StartCoroutine(SetTimeScaleToZeroCoroutine());
        }
    }

    private void LoseCountDownStart()
    {
        if (looseScreenCountDown >= 0)
        {
            timerGameObject.SetActive(true);
            
            looseScreenCountDown -= Time.deltaTime;
            _secondsLeft = Mathf.FloorToInt((looseScreenCountDown));
            Debug.Log(_secondsLeft);

            if (_secondsLeft >= 0)
            {
                timerText.text = _secondsLeft.ToString();
            }
        }
    }

    public void PauseGame()
    {
        pauseCanvas.SetActive(true);
        StartCoroutine(SetTimeScaleToZeroCoroutine());
    }
    
    
    public void ContinueGame()
    {
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
    }
    
    
    // This method will set the Timescale to 0 at the end popup canvases transitions, in the ANIMATOR
    IEnumerator SetTimeScaleToZeroCoroutine()
    {
        yield return new WaitForSeconds(delayToZeroTimeScale);
        Time.timeScale = 0;
    }

    // Sets the timescale to 0, when all the score is added incrementally to the win screen
    private void SetTimeScaleToZero()
    {
        if (winPopupScoreText.text == score.ToString())
        {
            Debug.Log("Set timescale to 0 NOWWWWWW");
            Time.timeScale = 0;
        }
    }
    
    // This method will reset the TIMESCALE back to 1
    IEnumerator SetTimeScaleToOne()
    {
        yield return new WaitForSeconds(delayToOneTimeScale);
        Time.timeScale = 1;
    }
    
    public void ScoreUpdate(int scoreValue)
    {
        score += scoreValue;
        
    }

    // Updates the score by increments of 1
    IEnumerator IncrementalScoreUpdate()
    {
        while (true)
        {
            if (_displayScore < score)
            {
                _displayScore++; // updates by 1
                // scoreText.text = _displayScore.ToString();
                winPopupScoreText.text = _displayScore.ToString();
            }

            yield return new WaitForSeconds(scoreUpdateSpeed); // decrease the argument to make the score update faster.
        }
    }
}
