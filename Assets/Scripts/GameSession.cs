using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [Header("Health System")]
    [SerializeField] int playerHealth = 3;
    
    [Header("Score System")] 
    [SerializeField] TextMeshPro scoreText;
    [SerializeField] int startingScore = 0;
    
    // Cache
    Enemy[] _enemies;

    
    private void OnEnable()
    {
        // adds all the enemies in the scene to the "_enemies" array
        _enemies = FindObjectsOfType<Enemy>();
    }

    
    private void Start()
    {
        scoreText.text = startingScore.ToString();
    }

    private void Update()
    {
        enemyTracker();
    }

    // counts the number of enemies in the level
    private void enemyTracker() //change method name later *****************
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
    public void ScoreUpdate(int scoreValue)
    {
        startingScore += scoreValue;
        scoreText.text = startingScore.ToString();
    }
    
    private void LoseCondition()
    {
        if (playerHealth <= 0)
        {
            // trigger lose message and retry button
                // if retry button clicked reload scene
        }
    }
    
    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void loadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    
}
