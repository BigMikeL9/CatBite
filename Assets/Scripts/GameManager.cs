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
    [SerializeField] float looseScreenCountDown = 3f;

    [Header("Score System")] 
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] int startingScore;

    [Header("Popup Canvases")] 
    [SerializeField] GameObject winCanvas;
    [SerializeField] GameObject looseCanvas;
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject settingsCanvas;

    [Header("Popup Canvases Transitions")] // delete later
    [SerializeField] float timeScaleToZeroDelay = 1.1f;
    [SerializeField] Animator winAnimator;
    [SerializeField] Animator loseAnimator;
     
    
    // Cache
    Enemy[] _enemies;
    CatHandler _catHandler;
    SceneController _sceneController;
    LevelSelection _levelSelection;
    int _currentSceneIndex;

    private void OnEnable()
    {
        _sceneController = FindObjectOfType<SceneController>();
        _levelSelection = FindObjectOfType<LevelSelection>();
        
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // adds all the enemies in the scene to the "_enemies" array
        _enemies = FindObjectsOfType<Enemy>();
    }
    

    private void Start()
    {
        _catHandler = FindObjectOfType<CatHandler>();
        
        // scoreText.text = startingScore.ToString();
    }

    private void Update()
    {
        WinCondition();
        StartCoroutine(LoseCondition());
        HeartsSystem();
    }
    

    public void UpdateNumberOfCats()
    {
        playerHealth--;

        if (playerHealth <= 0)
        {
            _catHandler.spawnCats = false;
        }
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
                StartCoroutine(SetTimeScaleToZero());
            }
        }
    }
    
    // This method will be used to set Timescale to 0 at the end popup canvases transitions, in the ANIMATOR
    IEnumerator SetTimeScaleToZero()
    {
        yield return new WaitForSeconds(timeScaleToZeroDelay);
        Time.timeScale = 0;
    }
    
    IEnumerator LoseCondition()
    {
        if (!AllEnemiesDead() && playerHealth <= 0)
        {
            yield return new WaitForSeconds(looseScreenCountDown);
            Debug.Log("YOU LOOOOSEE");
            Time.timeScale = 0;
            looseCanvas.SetActive(true);
        }
    }
    
    public void ScoreUpdate(int scoreValue)
    {
        startingScore += scoreValue;
        scoreText.text = startingScore.ToString();
    }
    
    public void PauseGame()
    {
        pauseCanvas.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void ContinueGame()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1;
    }
    
}
