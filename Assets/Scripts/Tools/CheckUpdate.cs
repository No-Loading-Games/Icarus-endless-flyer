using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckUpdate : MonoBehaviour
{
    [SerializeField]
    private GameObject _updateUIGroup;

    [SerializeField]
    private RefundChecker _refundChecker;

    [SerializeField]
    private Toggle dontShowToggle;
    [SerializeField]
    private GameObject toggleGroup;

    const int updateVersion = 2; //RealValue: 2; Fake value:218

    private void Start()
    {
        if(PlayerPrefs.GetInt("dontShowUpdates") > 0)
        {
            dontShowToggle.isOn = true;
            _updateUIGroup.SetActive(false);
        }

        if (PlayerPrefs.GetInt("updateVersion", 0) < updateVersion)
        {
            //Activate Update UI if there is an update
            //Confirm button will check if you need a refund

            _updateUIGroup.SetActive(true);
            dontShowToggle.isOn = false;

            PlayerPrefs.SetInt("updateVersion", updateVersion);
            PlayerPrefs.Save();
        }
    }
    private void Update()
    {
    }
    public void CheckRefund()
    {
        _refundChecker.CheckForRefund();
    }

    public void DeactivateUI()
    {
        _updateUIGroup.SetActive(false);
    }
    public void ActivateUI()
    {
        toggleGroup.SetActive(false);
        _updateUIGroup.SetActive(true);
    }

    public void TickNotToShowUI(Toggle tickValue)
    {
        if (tickValue.isOn)
            PlayerPrefs.SetInt("dontShowUpdates", 1);
        else
            PlayerPrefs.SetInt("dontShowUpdates", 0);
    }

}
