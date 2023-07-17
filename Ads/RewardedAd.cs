using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public string androidAdUnitId;
    string adUnitId;

    void Awake()
    {
        adUnitId = androidAdUnitId;
        LoadAd();
    }

    public void LoadAd()
    {
        print("Loading Rewarded!!");
        Advertisement.Load(adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId.Equals(adUnitId))
        {
            print("Rewarded loaded!!");
            ShowAd();
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        print("Rewarded failed to load");
    }



    public void ShowAd()
    {
        print("showing Rewarded ad!!");
        Advertisement.Show(adUnitId, this);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        print("Rewarded clicked");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(adUnitId) && showCompletionState.Equals(UnityAdsCompletionState.COMPLETED))
        {
            print("Rewarded show complete , Distribute the rewards");
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        print("Rewarded show failure");

    }

    public void OnUnityAdsShowStart(string placementId)
    {
        print("Rewarded show start");

    }
}