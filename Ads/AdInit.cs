using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdInit : MonoBehaviour, IUnityAdsInitializationListener
{
    public string androidGameId;
    string gameId;
    public bool isTestingMode;

    private void Awake()
    {
        InitializeAds();
    }

    void InitializeAds()
    {
        gameId = androidGameId;

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, isTestingMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Ad Initialized");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Ad Initialisation Failed");
    }
}
