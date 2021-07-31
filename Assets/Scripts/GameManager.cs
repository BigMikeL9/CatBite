using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    // Configs 
    [Header("Health System")]
    [Tooltip("The max number of cats that will spawn in level.")]
    [SerializeField] int numberOfCats = 3;
    
    [Header("Score System")] 
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] int startingScore;

    [Header("Level System")]
    [SerializeField] float looseScreenCountDown = 3f;
    [SerializeField] Button[] levelButtons;
    
    
    // Cache
    Enemy[] _enemies;
    string CURRENT_LEVEL_KEY;
    int currentSceneIndex;
    CatHandler _catHandler;


    private void OnEnable()
    {
        // adds all the enemies in the scene to the "_enemies" array
        _enemies = FindObjectsOfType<Enemy>();
    }

    private void Awake()
    {
        SetupSingleton();
    }

    private void Start()
    {
        _catHandler = FindObjectOfType<CatHandler>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // scoreText.text = startingScore.ToString();
    }

    private void Update()
    {
        WinCondition();
        StartCoroutine(LoseCondition());
        LevelSelectionSystem();
    }
    
    
    private void SetupSingleton()
    {
        int numOfGameSessions = FindObjectsOfType(GetType()).Length;

        if (numOfGameSessions > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void UpdateNumberOfCats()
    {
        numberOfCats--;

        if (numberOfCats <= 0)
        {
            _catHandler.spawnCats = false;
        }
    }

    // Makes buttons interactable, depending on what level we are in.
    private void LevelSelectionSystem()
    {
        // Change the int value to whatever your level selection build index is on your build settings 
        int levelAt = PlayerPrefs.GetInt(CURRENT_LEVEL_KEY, 2); 
        
        for (int i = 0; i < levelButtons.Length; i++) 
        {
            // if 
            if (i + 2 > levelAt) 
            {
                levelButtons[i].interactable = false;
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
        if (AllEnemiesDead() && numberOfCats >= 0)
        {
            if (currentSceneIndex == 10) // MAX LEVEL
            {
                Debug.Log("YOU WON THE GAAAAMEE!");
                // Show Win game screen or more coming soon screen
            }
            else
            {
                Debug.Log("You Won the level!!");
                // Time.timeScale = 0;
                // Show win screen with next level button
            }
        }
    }
    
    IEnumerator LoseCondition()
    {
        if (!AllEnemiesDead() && numberOfCats <= 0)
        {
            yield return new WaitForSeconds(looseScreenCountDown);
            Debug.Log("YOU LOOOOSEE");
            Time.timeScale = 0;
            // SHOW LOse screen with restart level button
        }
    }


    // Updates the PlayerPrefs value, depending on what level we are at.
    private void UpdateCurrentLevelPlayerPref() // there might be a bug here **********************
    {
        if (currentSceneIndex > PlayerPrefs.GetInt(CURRENT_LEVEL_KEY))
        {
            PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, currentSceneIndex);
        }
    }
    
    
    // Resets PlayerPrefs to default value. Deletes all saved progress
    private void DeletePlayerPrefs() // Might use this later
    {
        if (true) // A Reset Game button is pressed or the player finishes the game
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(currentSceneIndex);
        Destroy(gameObject); // Resets the GameManager Script
        // Time.timeScale = 1;
    }
    
    
    public void LoadNextScene()
    {
        SceneManager.LoadScene(currentSceneIndex + 1);
        // Time.timeScale = 1;
        Destroy(gameObject);
        
    }
    
    public void Quit()
    {
        Application.Quit();
    }
    
    public void ScoreUpdate(int scoreValue)
    {
        startingScore += scoreValue;
        scoreText.text = startingScore.ToString();
    }
}
