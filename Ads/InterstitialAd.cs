using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class InterstitialAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public string androidAdUnitId;
    string adUnitId;

    void Awake()
    {
        adUnitId = androidAdUnitId;
        //LoadAd();
    }

    public void LoadAd()
    {
        print("Loading interstitial!!");
        Advertisement.Load(adUnitId, this);
        //ShowAd();
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        print("interstitial loaded!!");
        ShowAd();
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        print("interstitial failed to load");
    }



    public void ShowAd()
    {
        print("showing ad!!");
        Advertisement.Show(adUnitId, this);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        print("interstitial clicked");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        print("interstitial show complete");

    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        print("interstitial show failure");

    }

    public void OnUnityAdsShowStart(string placementId)
    {
        print("interstitial show start");

    }
}