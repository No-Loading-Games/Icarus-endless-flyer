using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckUpdate : MonoBehaviour
{
    [SerializeField]
    private GameObject _updateUIGroup;

    [SerializeField]
    private RefundChecker _refundChecker;

    const int updateVersion = 15;

    private void Start()
    {
        if (PlayerPrefs.GetInt("updateVersion", 0) < updateVersion)
        {
            //Activate Update UI if there is an update
            //Confirm button will check if you need a refund

            _updateUIGroup.SetActive(true);


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

}
