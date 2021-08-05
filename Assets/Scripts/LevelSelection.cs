using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    // Configs
    [Header("Level System")]
    [SerializeField] Button[] levelButtons;
    
    // Cache
    string CURRENT_LEVEL_KEY = "levelAt";
    int _nextSceneLoad;

    private void Start()
    {
        _nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
        Time.timeScale = 1;
        Debug.Log("Timescale is: " + Time.timeScale);
    }

    private void Update()
    {
        LevelSelectionSystem();
    }

    // Makes buttons interactable, depending on what level we are in.
    private void LevelSelectionSystem()
    {
        // Change the int value to whatever your level selection build index is on your build settings 
        int levelAt = PlayerPrefs.GetInt(CURRENT_LEVEL_KEY, 2); // default value was 2
        
        for (int i = 0; i < levelButtons.Length; i++) 
        {
            // if 
            if (i + 2 > levelAt) 
            {
                levelButtons[i].interactable = false;
            }
        }
    }
    
    // Updates the PlayerPrefs value, depending on what level we are at.
    public void UpdateCurrentLevelPlayerPref() 
    {
        if (_nextSceneLoad > PlayerPrefs.GetInt(CURRENT_LEVEL_KEY))
        {
            PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, _nextSceneLoad);
        }
    }
    
    
    // Resets PlayerPrefs to default value. Deletes all saved progress
    public void ResetPlayerPrefs() // Might use this later
    {
        // A Reset Game button is pressed or the player finishes the game
        PlayerPrefs.DeleteAll();
    }
}
