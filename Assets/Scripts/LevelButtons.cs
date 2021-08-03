using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtons : MonoBehaviour
{
    [SerializeField] int sceneIndex;


    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(LevelLoader);
    }

    public void LevelLoader()
    {
        SceneManager.LoadScene(sceneIndex);
    }
    
}
