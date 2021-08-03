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
    
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(currentSceneIndex);
        Time.timeScale = 1;
    }
    
    
    public void LoadNextScene()
    {
        SceneManager.LoadScene(currentSceneIndex + 1);
        Time.timeScale = 1;
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
