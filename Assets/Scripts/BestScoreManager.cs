using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestScoreManager : MonoBehaviour
{
    private GameManager _gameManager;
    public void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();

        //_gameManager.FinalScoreEvent += CompareBestScore;

        if (!PlayerPrefs.HasKey("best-score"))
        {
            PlayerPrefs.SetFloat("best-score", 0);
            PlayerPrefs.SetString("best-scorer", "Unnamed");
        }

        _gameManager.BestScore = PlayerPrefs.GetFloat("best-score");
        Debug.Log("NEW Bestie score: " + _gameManager.BestScore);
    }

    public void CompareBestScore(float score)
    {
        if (score > PlayerPrefs.GetFloat("best-score"))
        {
            //PlayerPrefs.SetFloat("best-score", score);
            _gameManager.IsNewBestScore = true;
        }
        else
            _gameManager.IsNewBestScore = false;

        _gameManager.BestScore = PlayerPrefs.GetFloat("best-score");
    }


}
