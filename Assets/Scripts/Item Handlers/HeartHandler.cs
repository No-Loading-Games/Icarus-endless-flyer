using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeartHandler : MonoBehaviour
{
    #region Singleton Instantiation
    private static HeartHandler instance;

    public static HeartHandler Instance
    {
        get { return instance; }
    }

    #endregion

    [SerializeField]
    private GameManager _gameManager;
    private TextMeshProUGUI _heartUI;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        _heartUI = GetComponent<TextMeshProUGUI>();
        //_gameManager.UpdateTotalGoldEvent += HandleTotalGoldUpdate;
        /*
        if (PlayerPrefs.HasKey("total-gold"))
        {
            LoadGold();
        }
        else
        {
            PlayerPrefs.SetInt("total-gold", 0);
            _gameManager.TotalGold = PlayerPrefs.GetInt("total-gold");
            _heartUI.text = _gameManager.TotalGold.ToString();
        }
        */
    }

    public void HandleTotalHeartUpdate(int heart)
    {
        //_gameManager.TotalGold += heart;
        //PlayerPrefs.SetInt("total-gold", _gameManager.TotalGold);
        Debug.Log("TOTAL HEART: +" + heart);

    }

    private void LoadHeart()
    {
        //_gameManager.TotalGold = PlayerPrefs.GetInt("total-gold");
        //_heartUI.text = _gameManager.TotalGold.ToString();
    }
}
