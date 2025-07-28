using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShopItem : MonoBehaviour
{
    // Start is called before the first frame update
    GameManager _gameManager;

    [SerializeField]
    UnityEvent _handleAfterPurchaseEvent;

    [SerializeField]
    private TMP_Text _itemPriceText;

    [SerializeField]
    private TMP_Text _itemName;

    [SerializeField]
    private Color _textColor;

    [SerializeField]
    private Button _purchaseButton;

    [SerializeField]
    private GameObject _popUpUI;

    [SerializeField]
    private GameObject _gameUI;

    private ConfirmationUI confirmationUI;

    private int _itemPrice;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();

        _itemPrice = int.Parse(_itemPriceText.text);

        _purchaseButton = GetComponent<Button>();

        _purchaseButton.interactable = CanPurchase(_itemPrice);
    }

    private void Update()
    {
        _purchaseButton.interactable = CanPurchase(_itemPrice);

    }

    public void OnPurchase()
    {
        if(!CanPurchase(_itemPrice))
        {
            return;
        }

        AudioManager.Instance.PlaySFX("UI Click", 0f);

        confirmationUI = Instantiate(_popUpUI, _gameUI.transform).GetComponent<ConfirmationUI>();
        confirmationUI.ChangeDescription("Buy <color=#" + ColorUtility.ToHtmlStringRGB(_textColor) + ">" + _itemName.text + "</color> ?");

        confirmationUI.ConfirmPurchaseEvent += ConfirmPurchase;
        
    }


    public void ConfirmPurchase(bool decision)
    {
        if(decision)
        {
            Debug.Log("CONFIRMED added");
            _purchaseButton.interactable = true;
            GoldHandler.Instance.HandleTotalGoldUpdate(-_itemPrice);
            SharedUI.Instance.UpdateGoldUIText();
            _handleAfterPurchaseEvent?.Invoke();
        }

        confirmationUI.ConfirmPurchaseEvent -= ConfirmPurchase;
        Destroy(confirmationUI.gameObject);
    }

    private bool CanPurchase(int price)
    {
        return _gameManager.TotalGold >= price;
    }
}
