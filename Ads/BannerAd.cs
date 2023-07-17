using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAd : MonoBehaviour
{
    public string androidAdUnitId;
    string adUnitId;

    BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;

    private void Start()
    {
        adUnitId = androidAdUnitId;
        Advertisement.Banner.SetPosition(bannerPosition);

        LoadBanner();
    }

    public void LoadBanner()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerLoadError
        };

        Advertisement.Banner.Load(adUnitId, options);
    }

    void OnBannerLoaded()
    {
        //Do something when the banner is loaded.
        ShowBannerAd();
    }

    void OnBannerLoadError(string error)
    {
        //Do something when the banner loading throws any exception
    }

    public void ShowBannerAd()
    {
        BannerOptions options = new BannerOptions
        {
            showCallback = OnBannerShow,
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden
        };
        Advertisement.Banner.Show(adUnitId, options);
    }

    void OnBannerShow()
    {

    }

    void OnBannerClicked()
    {

    }

    void OnBannerHidden()
    {
        
    }

    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }
}
