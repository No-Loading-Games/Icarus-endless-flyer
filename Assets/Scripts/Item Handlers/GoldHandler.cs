using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldHandler : MonoBehaviour
{
    #region Singleton Instantiation
    private static GoldHandler instance;

    public static GoldHandler Instance
    {
        get { return instance; }
    }

    #endregion

    private int ctr = 0;

    [SerializeField]
    private GameManager _gameManager;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (PlayerPrefs.HasKey("total-gold"))
        {
            LoadGold();
        }
        else
        {
            PlayerPrefs.SetInt("total-gold", 0); 
            _gameManager.TotalGold = PlayerPrefs.GetInt("total-gold");
            SharedUI.Instance.UpdateGoldUIText();
        }

    }

    public void HandleTotalGoldUpdate(int gold)
    {
        _gameManager.TotalGold += gold;
        PlayerPrefs.SetInt("total-gold", _gameManager.TotalGold);
        Debug.Log(ctr + ") TOTAL GOLD: "+_gameManager.TotalGold);
        SharedUI.Instance.UpdateGoldUIText();
    }

    private void LoadGold()
    {
        _gameManager.TotalGold = PlayerPrefs.GetInt("total-gold");
        SharedUI.Instance.UpdateGoldUIText();
    }
}
