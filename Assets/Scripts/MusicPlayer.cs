using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    void Start()
    {
        SetUpSingleton();
    }
    
    private void SetUpSingleton()
    {
        var numOfMusicPlayers = FindObjectsOfType(GetType()).Length;

        if (numOfMusicPlayers > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
