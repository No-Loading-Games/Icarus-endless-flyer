using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;
    private TextMeshProUGUI _goldUI;



    // Start is called before the first frame update
    void Start()
    {
        _goldUI = GetComponent<TextMeshProUGUI>();  
        _goldUI.text = _gameManager.CurrentGold.ToString();
        _gameManager.GoldUpdateEvent += HandleGoldUpdate;
    }

    private void HandleGoldUpdate(int gold)
    {
        _goldUI.text = gold.ToString();
    }

}
