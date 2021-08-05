using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    
    // Configs
    [SerializeField] Animator animator;
    [SerializeField] float sceneTransitionTimer = 1f;
    
    // Cache
    int _currentSceneIndex;
    
    
    private void Start()
    {
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Time.timeScale = 1;
    }
    
    //
    // private void SetUpSingleton()
    // {
    //     var numOfSceneController = FindObjectsOfType(GetType()).Length;
    //
    //     if (numOfSceneController > 1)
    //     {
    //         gameObject.SetActive(false);
    //         Destroy(gameObject);
    //     }
    //     else
    //     {
    //         DontDestroyOnLoad(gameObject);
    //     }
    // }
    
    
    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(_currentSceneIndex);
    }
    
    
    public void LoadNextScene()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadLevel(_currentSceneIndex + 1));
    }

    IEnumerator LoadLevel(int sceneIndex)
    {
        // Play Animation
        animator.SetTrigger("Start");
        // Wait 
        yield return new WaitForSeconds(sceneTransitionTimer);
        // Load Scene
        SceneManager.LoadScene(sceneIndex);
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
