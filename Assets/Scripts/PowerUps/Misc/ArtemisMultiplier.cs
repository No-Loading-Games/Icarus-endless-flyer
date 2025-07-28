using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArtemisMultiplier : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _multiplier;

    [SerializeField]
    private List<TextMeshProUGUI> _multiplierShadows;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetMultiplierValue()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _multiplier.text = "x" + _gameManager.multiplier.ToString();

        for (int i = 0; i < _multiplierShadows.Count; i++)
        {
            _multiplierShadows[i].text = "x" + _gameManager.multiplier.ToString();
        }

        Debug.Log("ARTEMIS MULTIPLIER: " + _multiplier.text);
    }
}
