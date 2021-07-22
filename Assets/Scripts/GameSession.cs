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
    [SerializeField] int maxNumOfHealth = 3; // when this is equal to 3, I should have only 3 heart containers visible in my scene view
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;


    [Header("Score System")] 
    [SerializeField] TextMeshPro scoreText;
    [SerializeField] int startingScore = 0;

    private void Start()
    {
        scoreText.text = startingScore.ToString();
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
    
    private void HeartSystem()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth) // controls which type of heart is displayed
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            
            if (i < maxNumOfHealth) // controls how many hearts should be displayed in relation to
                // the max allowed health that the player can have
            {                       
                hearts[i].enabled = true;  
            }
            else
            {
                hearts[i].enabled = false; // This hides any extra hearts in the array that we have, that is more than maxNumOfHealth.
            }
        }
    }
    
    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    
}
