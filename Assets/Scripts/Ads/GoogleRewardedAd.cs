using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GoogleRewardedAd : MonoBehaviour
{

    private RewardedAd _rewardedAd;

    //google test ad default
    [SerializeField]
    private string _adUnitId;
    [SerializeField]
    private GameObject _adButton;
    private GameManager _gameManager;

    void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            DestroyAd();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
            });

        //_adButton?.SetActive(true);
        _adButton.GetComponent<Button>().interactable = true;
    }

    /// <summary>
    /// Shows the ad.
    /// </summary>
    public void ShowAd()
    {
        AudioManager.Instance.PlaySFX("UI Click", 0f);
        AudioManager.Instance.musicHandler.Pause();

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            Debug.Log("Showing rewarded ad.");
            _rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(String.Format("Rewarded ad granted a reward: {0} {1}",
                                        reward.Amount,
                                        reward.Type));


                //Recontinue game
                _gameManager.AdsUse += 1;

                AudioManager.Instance.musicHandler.UnPause();
                _gameManager.ReContinueGame();
            });
        }
        else
        {

            AudioManager.Instance.musicHandler.UnPause();
            Debug.LogError("Rewarded ad is not ready yet.");
        }

        // Inform the UI that the ad is not ready.
        //_adButton?.SetActive(false);
        _adButton.GetComponent<Button>().interactable = false;
    }
    public void ShowAdForHeart()
    {
        AudioManager.Instance.PlaySFX("UI Click", 0f);
        AudioManager.Instance.musicHandler.Pause();

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            Debug.Log("Showing rewarded ad.");
            _rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(String.Format("Rewarded ad granted a reward: {0} {1}",
                                        reward.Amount,
                                        reward.Type));


                //Recontinue game
                _gameManager.AdsUse += 1;

                AudioManager.Instance.musicHandler.UnPause();
                _gameManager.ReContinueGame();
            });
        }
        else
        {

            AudioManager.Instance.musicHandler.UnPause();
            Debug.LogError("Rewarded ad is not ready yet.");
        }

        // Inform the UI that the ad is not ready.
        //_adButton?.SetActive(false);
        _adButton.GetComponent<Button>().interactable = false;
    }

    /// <summary>
    /// Destroys the ad.
    /// </summary>
    public void DestroyAd()
    {
        if (_rewardedAd != null)
        {
            Debug.Log("Destroying rewarded ad.");
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        // Inform the UI that the ad is not ready.
        //_adButton?.SetActive(false);
        _adButton.GetComponent<Button>().interactable = false;
    }
}
