using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleMobileAdsManager : MonoBehaviour
{
    // Start is called before the first frame update

    private GoogleRewardedAd _gRewarded;
    void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            Debug.Log("Google Mobile Ads SDK is initialized.");
            _gRewarded = FindObjectOfType<GoogleRewardedAd>();
            _gRewarded.LoadRewardedAd();
        });
    }
}
