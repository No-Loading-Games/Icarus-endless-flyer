using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{

    private Image _staminaFill;
    private StaminaFillDelayUI _staminaFillDelayUI;
    private GameManager _gameManager;

    [SerializeField]
    private Material _defaultMaterial;
    [SerializeField]
    private Material _burningMaterial;

    [SerializeField]
    private GameObject _staminaFillDelay;

    [SerializeField]
    private Sprite _defaultStamina;
    [SerializeField]
    private Sprite _fullStamina;
    [SerializeField]
    private Sprite _mediumStamina;
    [SerializeField]
    private Sprite _smallStamina;

    [SerializeField]
    private Sprite _fullFeather;
    [SerializeField]
    private Sprite _mediumFeather;
    [SerializeField]
    private Sprite _smallFeather;
    [SerializeField]
    private Image _featherUI;
    

    // Start is called before the first frame update
    void Start()
    {
        _staminaFill = GetComponent<Image>();
        _gameManager = FindObjectOfType<GameManager>();
        _staminaFillDelayUI = _staminaFillDelay.GetComponent<StaminaFillDelayUI>();

        //_playerController = FindObjectOfType<PlayerController>();

        _gameManager.StaminaUpdateEvent += HandleStaminaUpdate;
        _gameManager.StaminaDelayUpdateEvent += HandleStaminaDelayUpdate;
        _gameManager.StaminaFreezeEvent += FreezeStaminaUpdate;
        //_staminaFill.color = new Color(0.3764706f, 0.772549f, 0.3019608f);
    }

    private void HandleStaminaUpdate(float stamina)
    {
        float tickPercentage = 1 - stamina;
        _staminaFill.fillAmount = stamina;
        _staminaFill.color = new Color(1,1,1);

        if (stamina > 0)
            _featherUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(114.5f - (Mathf.Round((213.5f * tickPercentage) * 100) / 100), _featherUI.GetComponent<RectTransform>().anchoredPosition.y);
        else
            _featherUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(114.5f - 213.5f, _featherUI.GetComponent<RectTransform>().anchoredPosition.y);
        
        //Debug.Log("feather UI pos: " + _featherUI.gameObject.transform.position);

        if (stamina <= 0.25f)
        {
            _featherUI.overrideSprite = _smallFeather;
            _staminaFill.overrideSprite = _smallStamina;

            _gameManager.powerUpCounter = 9;
            //_staminaFill.color = new Color(0.9058823f, 0.258823f, 0.172549f);
            //_playerController.CreateFeatherFX();
        }
        else if (stamina <= 0.75f)
        {
            _featherUI.overrideSprite = _mediumFeather;
            _staminaFill.overrideSprite = _mediumStamina;

            _gameManager.powerUpCounter = 7;
            //_staminaFill.color = Color.yellow;
            //_playerController.CreateFeatherFX();
        }
        
        else
        {
            _featherUI.overrideSprite = _fullFeather;
            _staminaFill.overrideSprite = _fullStamina;

            _gameManager.powerUpCounter = 5;
            //_staminaFill.color = new Color(1,1,1f);
            //_staminaFill.color = new Color(0.4352941176f, 0.9058823529f, 0.3019608f);
        }
        

    }

    private void FreezeStaminaUpdate(float stamina)
    {
        _staminaFill.fillAmount = stamina;
        _staminaFill.overrideSprite = _defaultStamina;
        _staminaFill.color = new Color(0.7019607843f,0.6901960784f,0.66666667f);
    }

    private void HandleStaminaDelayUpdate(float stamina, bool inSun)
    {
        if (!inSun)
        {
            _staminaFill.GetComponent<Image>().material = _defaultMaterial;
            _staminaFillDelayUI.GetStamina(stamina);
        }
        else
            _staminaFill.GetComponent<Image>().material = _burningMaterial;
    }


}
