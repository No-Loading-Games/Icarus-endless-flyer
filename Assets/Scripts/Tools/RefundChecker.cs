using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class RefundChecker : MonoBehaviour
{
    [SerializeField]
    private GameObject _popUpNotif;

    [SerializeField]
    private GameObject _goldFillUpVFX;

    [SerializeField]
    private GameObject _goldUI;

    [SerializeField]
    private GameObject _confirmButton;

    [SerializeField]
    private TextMeshProUGUI _refundText;

    const int refundVersion = 10;

    private int totalRefund = 0;

    private void Start()
    {
        
    }

    private void Update()
    {
    }

    public void CheckForRefund()
    {
        if (PlayerPrefs.GetInt("upgradeRefundedVersion", 0) < refundVersion)
        {
            //Instantiate Refund UI first
            //Confirm button is only done to animate refund

            ShowNotification(true);

        }
    }

    public void ShowNotification(bool status)
    {
        GiveUpgradeRefund();
        _refundText.text = $"{totalRefund} coins refunded";

        if(totalRefund > 0)
            _popUpNotif.SetActive(status);
    }

    public void GiveUpgradeRefund()
    {
        totalRefund = 0;
        int ctr = 0;

        string[] upgradeIds = { "artemis-upgrade-level", "zeus-upgrade-level", "magnet-upgrade-level", "hermes-upgrade-level" };
        int[] maxUpgradeLevels = { 6, 6, 6, 4 };

        foreach (string upgradeId in upgradeIds)
        {
            int level = PlayerPrefs.GetInt(upgradeId);

            //since level 1 means no upgrade was bought, then skip that power up
            if (level == 1)
                continue;

            //Get the total per power up
            int refund = CalculateRefund(level, maxUpgradeLevels[ctr]);

            Debug.Log("REFUND POWER UP: " + upgradeId + " || level: " + level + " || Refund: " + refund);

            ctr++;

            //Add the total of each power up to the total refund
            totalRefund += Mathf.Max(0, refund); // no negative refunds
            Debug.Log("REFUND TOTAL: " + totalRefund);
        }

        //int currentCoins = PlayerPrefs.GetInt("total-gold", 0) + totalRefund;
        //PlayerPrefs.SetInt("total-gold", currentCoins);
        Debug.Log($"Refunded player {totalRefund} coins due to upgrade rebalancing.");


        //Execute giving refund and updating upgradeRefundedVersion regardless 
        //if player confirms or not to make sure they receive the refund
        PlayerPrefs.SetInt("upgradeRefundedVersion", refundVersion);
        PlayerPrefs.Save();
    }

    public int CalculateRefund(int level, int maxLvl)
    {
        //If level = 2, levelFactor should remain 1'
        float levelFactor = 0;
        if (level > 2)
            levelFactor = PowerupUpgradeManager.Instance.MaxUpgradeLevel / maxLvl;
        else
            levelFactor = 1;

        int oldBasePrice = 2500;
        int newBasePrice = 700;

        float oldUpgradeCostFactor = (5f / 3f);
        float newUpgradeCostFactor = PowerupUpgradeManager.Instance.UpgradeCostFactor;

        int oldTotal = 0;
        int newTotal = 0;

        for (int i = 1; i <= level; i++)
        {
            if(i <= 2)
            {
                oldTotal = oldBasePrice;
                newTotal = newBasePrice;
                Debug.Log(i + " REFUND OLD: " + oldTotal);
                Debug.Log(i + " REFUND NEW: " + newTotal);
                continue;
            }

            oldTotal += Mathf.RoundToInt(Mathf.Floor((oldBasePrice * (1 + ((i-1) / oldUpgradeCostFactor)) / 1000)) * 1000 * levelFactor);
            Debug.Log(i + " REFUND OLD: " + oldTotal);
            newTotal += Mathf.RoundToInt(Mathf.Floor((Mathf.RoundToInt((newBasePrice * (i-1)) + (100 * (Mathf.Pow(i-1, PowerupUpgradeManager.Instance.UpgradeCostFactor)))) / 100) * levelFactor) * 100);
            Debug.Log(i + " REFUND NEW: " + newTotal);
        }

        return oldTotal - newTotal;
    }

    public void AnimateRefund()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        GameObject goldVFX = Instantiate(_goldFillUpVFX, gameManager.transform, true);
        gameManager.AnimateFillUp(goldVFX, _confirmButton.transform.position, _goldUI, 0.6f);
        transform.DOMoveX(transform.position.x, 0.6f).OnComplete(() =>
        {
            GoldHandler.Instance.HandleTotalGoldUpdate(totalRefund);
        }
            );
        _popUpNotif.SetActive(false);
        //GetComponentInParent<GameObject>().SetActive(false);
    }
}
