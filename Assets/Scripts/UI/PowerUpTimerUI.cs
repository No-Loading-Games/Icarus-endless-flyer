using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpTimerUI : MonoBehaviour
{
    [SerializeField]
    private Image _timerFill;
    [SerializeField]
    private Image _timerBG;
    [SerializeField]
    private Image _powerUpSprite;
    [SerializeField]
    private TextMeshProUGUI _powerUpCounter;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void OnEnable()
    {
        _gameManager = FindObjectOfType<GameManager>();

        _gameManager.PowerUpTimerUpdateEvent += HandlePowerUpTimerUpdate;
        _gameManager.PowerUpCounterUpdateEvent += HandlePowerUpCounterUpdate;
        _timerFill.color = new Color(1, 1, 1);
    }

    private void HandlePowerUpTimerUpdate(float time, float duration, Sprite pSprite, Color pColor)
    {
        _timerFill.gameObject.SetActive(true);
        _timerBG.gameObject.SetActive(true);
        _powerUpCounter.gameObject.SetActive(false);

        _timerFill.fillAmount = (duration - time) / duration;
        _timerFill.color = pColor;

        _powerUpSprite.sprite = pSprite;
    }
    private void HandlePowerUpCounterUpdate(int uses, Sprite pSprite)
    {
        _timerFill.gameObject.SetActive(false);
        _timerBG.gameObject.SetActive(false);
        _powerUpCounter.gameObject.SetActive(true);
        
        Debug.Log("BUG NOT FOUND");

        _powerUpSprite.sprite = pSprite;
        _powerUpCounter.text = uses.ToString();
    }
}
