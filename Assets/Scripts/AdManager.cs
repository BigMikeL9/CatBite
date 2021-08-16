using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
   [Tooltip("Turn off to play real Ads")]
   [SerializeField] private bool testMode = true;
   
   public static AdManager Instance;
 
   
   // Game IDs from Services --> Ads
#if UNITY_ANDROID
   private string gameId = "4258705";
#elif UNITY_IOS
   private string gameId = "4258704";
#endif
   
   
   private SceneController _sceneController;

   private void Awake()
   {
      if (Instance != null && Instance != this)
      {
         Destroy(gameObject);
      }
      else
      {
         Instance = this;
         DontDestroyOnLoad(gameObject);
         
         Advertisement.AddListener(this);
         Advertisement.Initialize(gameId, testMode);
      }
   }

  
   // Starts the Ad. Parameter should be the class that will call this "ShowAd" method 
   // Parameter should be cached
   public void ShowAd(SceneController sceneController)
   {
      this._sceneController = sceneController; // storing reference to the gameManager in order to use it below when we finish the ad.
      
      Advertisement.Show("startLevel");
   }
   
   
   public void OnUnityAdsDidError(string message)
   {
      Debug.Log($"Unity Ads Error: {message}");
   }
   
   public void OnUnityAdsReady(string placementId)
   {
      Debug.Log("Unity Ads Ready");
   }

   public void OnUnityAdsDidStart(string placementId)
   {
      Debug.Log("Unity Ads Started");
   }

   public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
   {
      switch (showResult)
      {
         case ShowResult.Finished:
            _sceneController.RestartLevel();
            break;
         case ShowResult.Skipped:
            // Ad was skipped
            break;
         case ShowResult.Failed:
            Debug.LogWarning("Ad Failed");
            break;
      }
   }
}
