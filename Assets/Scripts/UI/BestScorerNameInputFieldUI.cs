using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BestScorerNameInputFieldUI : MonoBehaviour
{
    [SerializeField]
    private string _inputText;

    [SerializeField]
    private TMP_InputField _bestScorer;

    [SerializeField]
    private GameObject _parentUI;

    private GameManager _gameManager;

    public void GrabFromInputField(string input)
    {
        //char[] inputChars = input.ToCharArray();
        if (input.Equals("") || !input.All(char.IsLetterOrDigit))
            _inputText = "Unnamed";
        else
            _inputText = input;
        //PlayerPrefs.SetString("best-scorer", _inputText);

        _gameManager = FindObjectOfType<GameManager>();

        //LeaderboardManager.Instance.AddEntry(_inputText, (int)_gameManager.ScoreDistance);
        LeaderBoardUI.Instance.AddEntryAndRefresh(_inputText, (int)_gameManager.ScoreDistance);

        Debug.Log("NAME: " + _inputText);
        _parentUI.SetActive(false);
    }

    public void EnterCurrentName()
    {
        _bestScorer.onEndEdit?.Invoke(_bestScorer.text);
    }


}
