using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SharedUI : MonoBehaviour
{
    #region Singleton: SharedUI
    private static SharedUI instance;

    public static SharedUI Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;

        _gameManager = FindObjectOfType<GameManager>();
        _heartManager = FindObjectOfType<HeartManager>();
    }

    #endregion

    private GameManager _gameManager;
    [SerializeField] TMP_Text[] goldUIText;
    [SerializeField] TMP_Text[] heartsUIText;
    private HeartManager _heartManager;

    private void Start()
    {
        //_gameManager.UpdateTotalGoldEvent += UpdateGoldUIText;

        UpdateGoldUIText();
        UpdateHeartUIText();
    }


    public void UpdateGoldUIText()
    {
        int gold = PlayerPrefs.GetInt("total-gold");
        for (int i=0; i < goldUIText.Length; i++)
        {
            SetItemText(goldUIText[i], gold);
        }
        //PlayerPrefs.SetInt("total-gold", gold);
        _gameManager.TotalGold = gold;
    }
    
    public void UpdateHeartUIText()
    {
        int heart = _heartManager.GetHearts();

        for (int i = 0; i < heartsUIText.Length; i++)
        {
            SetItemText(heartsUIText[i], heart);
        }
        //PlayerPrefs.SetInt("total-gold", heart);
        //_gameManager.TotalGold = gold;
    }

    public void UpdateBestScoreUIText(int score)
    {

    }

    private void SetItemText(TMP_Text itemTMP, int value)
    {
        itemTMP.text = value.ToString();
    }
}
