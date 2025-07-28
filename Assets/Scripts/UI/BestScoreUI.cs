using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BestScoreUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _bestScoreTxt;
    [SerializeField]
    private TextMeshProUGUI _bestScoreTxtShadow;
    [SerializeField]
    private TextMeshProUGUI _bestScorerNameTxt;
    [SerializeField]
    private TextMeshProUGUI _bestScorerNameTxtShadow;
    [SerializeField]
    private TextMeshProUGUI _newBestScorerNameTxt;

    [SerializeField]
    private Image _bestScoreWreath;
    [SerializeField]
    private GameObject _newBestScoreWreath;

    private GameManager _gameManager;

    private void OnEnable()
    {
        _gameManager = new GameManager();

        //_gameManager.FinalScoreEvent += DisplayBestScoreOnGameOver;
        Debug.Log("DISLAYING BEST SCORE");
    }

    public void DisplayBestScore()
    {
        _bestScoreTxt.text = PlayerPrefs.GetFloat("best-score").ToString();
        _bestScoreTxtShadow.text = PlayerPrefs.GetFloat("best-score").ToString();
        _bestScorerNameTxt.text = PlayerPrefs.GetString("best-scorer");
        _bestScorerNameTxtShadow.text = PlayerPrefs.GetString("best-scorer");
    }

    public void DisplayBestScoreOnGameOver(float score)
    {
        if(score > PlayerPrefs.GetFloat("best-score"))
        {
            PlayerPrefs.SetFloat("best-score", score);
            _bestScorerNameTxt.gameObject.SetActive(false);
            _newBestScorerNameTxt.text = PlayerPrefs.GetString("best-scorer");
            _newBestScoreWreath.SetActive(true);
            _bestScoreTxt.enabled = false;
            _bestScoreWreath.enabled = false;
            Debug.Log("NEW BEST SCORE|| OLD: " + PlayerPrefs.GetFloat("best-score") + " NEW: " + score);
        }
        else
        {
            _bestScoreWreath.enabled = true;
            _bestScoreTxt.enabled = true;
            _bestScorerNameTxt.gameObject.SetActive(true);
            _bestScoreTxt.text = PlayerPrefs.GetFloat("best-score").ToString();
            _bestScorerNameTxt.text = PlayerPrefs.GetString("best-scorer");
            _newBestScoreWreath.SetActive(false);
        }
    }

}
