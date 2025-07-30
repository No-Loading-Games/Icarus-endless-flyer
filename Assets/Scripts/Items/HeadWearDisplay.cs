using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class HeadWearDisplay : Item
{
    public HeadWearSO headWear;
    
    [SerializeField]
    private Button _iconButton;

    [SerializeField]
    private Image _bgImage;
    [SerializeField]
    private Sprite _unlockedBGSprite;
    [SerializeField]
    private Sprite _lockedBGSprite;
    [SerializeField]
    private Sprite _equippedBGSprite;

    [SerializeField]
    private GameObject _selectIndicator;

    //public int price;
    [SerializeField]
    private TextMeshProUGUI _priceTag;

    public DisplayPanelHW _displayPanel;

    public HeadWearDisplay _previousHeadWear;

    //public Item item;

    // Start is called before the first frame update
    void Start()
    {
        InitializeData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeData()
    {
        InitializeItemData();

        //Set up the Head Wear Icon Button
        if (headWear.isUnlocked)
        {
            numberOfPurchased = 1;
            _iconButton.image.sprite = headWear.unlockedIcon;
            _bgImage.sprite = _unlockedBGSprite;
        }
        else
        {
            numberOfPurchased = 0;
            _iconButton.image.sprite = headWear.lockedIcon;
            _bgImage.sprite = _lockedBGSprite;
        }

        if(headWear.isEquipped)
        {
            _bgImage.sprite = _equippedBGSprite;
        }

        if(headWear.isPurchasable)
        {
            price = headWear.price;
            //_priceTag.text = price.ToString();
        }

        this.gameObject.name = headWear.headWearNamePP;

    }

    public void DisplayHeadWearDetails()
    {
        if (_displayPanel == null)
            return;

        _displayPanel.UpdateDisplay(this, headWear, headWear.isUnlocked);
    }
    public void ShopDisplayHeadWearDetails()
    {
        if (_displayPanel == null)
            return;


        _displayPanel.gameObject.SetActive(false);
        //_displayPanel.gameObject.SetActive(false);

        //ActivateIndicator();

        _displayPanel.transform.position = new Vector2 (0, this.transform.position.y);
        _displayPanel.gameObject.SetActive(true);

        _displayPanel.UpdateDisplay(this, headWear, headWear.isUnlocked);
    }

    public void ShopDisplayDeactivate()
    {
        _displayPanel.gameObject.SetActive(false);

    }

    public void InitializeDisplayPanel(DisplayPanelHW display)
    {
        _displayPanel = display;
    }

    public void InitializeItemData()
    {
        name = headWear.headWearName;
        namePP = headWear.headWearNamePP;
        price = headWear.price;
        //textColor = new Color(66, 127, 195, 255);
        maxPurchase = 1;

    }

    public void ActivateIndicator()
    {
        _selectIndicator.SetActive(true);
    }
    
    public void DeactivateIndicator()
    {
        _selectIndicator.SetActive(false);
    }

    public void ShowEquippedBG()
    {
        _bgImage.sprite = _equippedBGSprite;
    }
    public void UnshowEquippedBG()
    {
        _bgImage.sprite = _unlockedBGSprite;
    }

}
