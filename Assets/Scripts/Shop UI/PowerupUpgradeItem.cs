using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

        _purchaseButton.interactable = CanUpgrade(_itemPrice);
    }

    public void OnUpgrade()
    {

        if (!CanUpgrade(_itemPrice))
        {
            return;
        }

        AudioManager.Instance.PlaySFX("UI Click", 0f);

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

        confirmationUI.ConfirmPurchaseEvent -= ConfirmPurchase;
        Destroy(confirmationUI.gameObject);
    }

    private void LoadUpgradeLevel()
    {
        _upgradeLevelGauge.value = PlayerPrefs.GetInt(_upgradeLevelID);

        float levelFactor = PowerupUpgradeManager.Instance.MaxUpgradeLevel / _upgradeLevelGauge.maxValue;

        if (_upgradeLevelGauge.value > 1)
        {
            _itemPriceText.text = (Mathf.Floor((_basePrice * (1 + (_upgradeLevelGauge.value / PowerupUpgradeManager.Instance.UpgradeCostFactor))) / 1000) * 1000 * (levelFactor)).ToString();
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
