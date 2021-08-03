using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    // Configs
    [Header("Level System")]
    [SerializeField] Button[] levelButtons;
    
    // Cache
    string CURRENT_LEVEL_KEY;
    SceneController _sceneController;

    private void Start()
    {
        _sceneController = FindObjectOfType<SceneController>();
    }

    private void Update()
    {
        LevelSelectionSystem();
    }

    // Makes buttons interactable, depending on what level we are in.
    private void LevelSelectionSystem()
    {
        // Change the int value to whatever your level selection build index is on your build settings 
        int levelAt = PlayerPrefs.GetInt(CURRENT_LEVEL_KEY, 2); 
        
        for (int i = 0; i < levelButtons.Length; i++) 
        {
            // if 
            if (i + 3 > levelAt) 
            {
                levelButtons[i].interactable = false;
            }
        }
    }
    
    // Updates the PlayerPrefs value, depending on what level we are at.
    public void UpdateCurrentLevelPlayerPref() // there might be a bug here **********************
    {
        if (_sceneController.GetCurrentSceneIndex() > PlayerPrefs.GetInt(CURRENT_LEVEL_KEY))
        {
            PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, _sceneController.GetCurrentSceneIndex());
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
}
