using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class ShopItem : MonoBehaviour
{
    private GameManager _gameManager;
    private ShopManager _shopManager;
    
    public Item _item;

    [SerializeField]
    private TextMeshProUGUI _itemPriceTag;

    [SerializeField]
    UnityEvent _handleDataInitialization;

    [SerializeField]
    UnityEvent _handleAfterPurchaseEvent;

    [SerializeField]
    private Button _purchaseButton;

    [SerializeField]
    private GameObject _popUpUI;

    private ConfirmationUI confirmationUI;

    public string confirmationText;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _shopManager = FindObjectOfType<ShopManager>();

        _purchaseButton = GetComponent<Button>();

        _itemPriceTag.text = _item.price.ToString(); //Shop Item Price will depend on what is set on the Item Script

        _purchaseButton.interactable = CanPurchase(_item.price);
    }

    private void Update()
    {
        if (!CanPurchase(_item.price))
        {
            _purchaseButton.interactable = false;
            return;
        }

    }

    public void OnPurchase()
    {
        confirmationText = "Buy";

        if(!CanPurchase(_item.price))
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

        confirmationUI = Instantiate(_popUpUI, _shopManager.transform).GetComponent<ConfirmationUI>();
        confirmationUI.ChangeDescription(confirmationText + " <color=#" + ColorUtility.ToHtmlStringRGB(_item.textColor) + ">" + _item.name + "</color> ?");

        confirmationUI.ConfirmPurchaseEvent += ConfirmPurchase;
        
    }


    public void ConfirmPurchase(bool decision)
    {
        if(decision)
        {
            Debug.Log("CONFIRMED added");
            GoldHandler.Instance.HandleTotalGoldUpdate(-_item.price);
            SharedUI.Instance.UpdateGoldUIText();
            _handleAfterPurchaseEvent?.Invoke();
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

    private bool CanPurchase(int price)
    {
        if (_item.numberOfPurchased >= _item.maxPurchase)
        {
            _itemPriceTag.text = "SOLD";
            return false;
        }

        return _gameManager.TotalGold >= price;
    }
}
