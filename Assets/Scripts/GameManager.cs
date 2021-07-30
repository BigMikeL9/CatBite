using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] Button[] levelButtons;
    
    // Cache
    Enemy[] _enemies;
    

    
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
        scoreText.text = startingScore.ToString();
    }

    private void Update()
    {
        EnemyTracker();
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

    
    /* WIN CONDITIONS
       Player wins when he kills all chickens in level
           or if he kills more than half and have no cats left
           */

    
    
    // counts the number of enemies in the level
    private void EnemyTracker() //change method name later *****************
    {
        foreach (var enemy in _enemies)
        {
            // if enemy is not dead yet
            if (enemy != null)
            {
                // Dont do anything
                return;
            }
            
            Debug.Log("You Killed all Enemies!!");
            // Show win screen with next level button
        }
    }
    
    // Handles lose
    private void LoseCondition()
    {
        // if there is no more cats but there are still enemies in the scene
        if (numberOfCats <= 0 && _enemies != null)
        {
            // trigger lose message and retry button
                // if retry button clicked reload scene
        }
    }
    
    // Restarts level when the restart level button is pressed
    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Destroy(gameObject);
    }

    // Loads next level when the player wins and next level button is clicked
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    
    public void ScoreUpdate(int scoreValue)
    {
        startingScore += scoreValue;
        scoreText.text = startingScore.ToString();
    }
    
    private void LevelSelectionSystem()
    {
        // Change the int value to whatever your level selection build index is on your build settings 
        int levelAt = PlayerPrefs.GetInt("levelAt", 2); 
        
        for (int i = 0; i < levelButtons.Length; i++) 
        {
            // if 
            if (i + 2 > levelAt) 
            {
                levelButtons[i].interactable = false;
            }
        }
    }
    
   
}
