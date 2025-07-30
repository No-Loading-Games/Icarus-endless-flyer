using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class PowerupUpgradeItem : MonoBehaviour
{
    GameManager _gameManager;

    [SerializeField]
    UnityEvent _handleAfterPurchaseEvent;

    [SerializeField]
    private Slider _upgradeLevelGauge;

    [SerializeField]
    private TMP_Text _itemPriceText;

    [SerializeField]
    private TMP_Text _itemName;

    [SerializeField]
    private Color _textColor;

    [SerializeField]
    private Button _purchaseButton;

    private int _itemPrice;
    [SerializeField]
    private int _basePrice;

    [SerializeField]
    private string _upgradeLevelID;

    [SerializeField]
    private GameObject _popUpUI;

    [SerializeField]
    private GameObject _gameUI;

    private ConfirmationUI confirmationUI;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _itemPrice = int.Parse(_itemPriceText.text);

        _purchaseButton = GetComponent<Button>();

        LoadUpgradeLevel();
    }

    private void Update()
    {

        if (!CanUpgrade(_itemPrice))
        {
            _purchaseButton.interactable = false;
        }
    }

    public void OnUpgrade()
    {

        if (!CanUpgrade(_itemPrice))
        {
            return;
        }

        AudioManager.Instance.PlaySFX("UI Click", 0f);

        //Disable all buttons
        List<Button> buttons = FindObjectsOfType<Button>().ToList<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }

        confirmationUI = Instantiate(_popUpUI, _gameUI.transform).GetComponent<ConfirmationUI>();
        confirmationUI.ChangeDescription("Upgrade <color=#"+ ColorUtility.ToHtmlStringRGB(_textColor) + ">" + _itemName.text + "</color> ?");

        confirmationUI.ConfirmPurchaseEvent += ConfirmPurchase;

    }
    public void ConfirmPurchase(bool decision)
    {
        if (decision)
        {
            Debug.Log("CONFIRMED added");
            _purchaseButton.interactable = true;
            GoldHandler.Instance.HandleTotalGoldUpdate(-_itemPrice);
            SharedUI.Instance.UpdateGoldUIText();
            _handleAfterPurchaseEvent?.Invoke();
            LoadUpgradeLevel();
        }

        //Enable all buttons
        List<Button> buttons = FindObjectsOfType<Button>().ToList<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }

        confirmationUI.ConfirmPurchaseEvent -= ConfirmPurchase;
        Destroy(confirmationUI.gameObject);
    }

    private void LoadUpgradeLevel()
    {
        _upgradeLevelGauge.value = PlayerPrefs.GetInt(_upgradeLevelID);

        float levelFactor = PowerupUpgradeManager.Instance.MaxUpgradeLevel / _upgradeLevelGauge.maxValue;
        //Level factor is essential for power ups like hermes where you can only upgrade until lvl 4 instead of 6

        if (_upgradeLevelGauge.value > 1)
        {
            Debug.Log("POWER PRICE: " + Mathf.Floor((Mathf.RoundToInt((_basePrice * (_upgradeLevelGauge.value)) + (100 * (Mathf.Pow(_upgradeLevelGauge.value, PowerupUpgradeManager.Instance.UpgradeCostFactor)))) / 100) * levelFactor) * 100);
            
            _itemPriceText.text = (Mathf.Floor((Mathf.RoundToInt((_basePrice * (_upgradeLevelGauge.value)) + (100 * (Mathf.Pow(_upgradeLevelGauge.value, PowerupUpgradeManager.Instance.UpgradeCostFactor))))/100) * levelFactor) * 100).ToString();
            _itemPrice = int.Parse(_itemPriceText.text);

        }

        if (_upgradeLevelGauge.value == _upgradeLevelGauge.maxValue)
        {
            _itemPriceText.text = "MAX";
            _itemPriceText.alignment = TextAlignmentOptions.Bottom;
        }
    }
    private bool CanUpgrade(int price)
    {
        return _gameManager.TotalGold >= price && PlayerPrefs.GetInt(_upgradeLevelID) < _upgradeLevelGauge.maxValue;
    }

}
