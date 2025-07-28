using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    GameManager _gameManager;
    TextMeshProUGUI _scoreText;
    

    // Start is called before the first frame update
    void Start()
    {
        _scoreText = GetComponent<TextMeshProUGUI>();
        _scoreText.text = _gameManager.ScoreDistance.ToString();
        _gameManager.ScoreUpdateEvent += HandleScoreUpdate;
        
    }

    private void HandleScoreUpdate(float score)
    {
        _scoreText.text = score.ToString(); 
    }



   
}
