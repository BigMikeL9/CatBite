using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    
    // Cache
    int currentSceneIndex;
    int nextSceneLoad;


   
    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    public int GetCurrentSceneIndex()
    {
        return currentSceneIndex;
    }
    
    
     public int GetNextSceneLoad()
    {
        return nextSceneLoad;
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
        // Destroy(gameObject);

    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
