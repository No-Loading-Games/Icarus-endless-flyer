using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HeadWearDisplay : MonoBehaviour
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

    public DisplayPanelHW _displayPanel;

    public HeadWearDisplay _previousHeadWear;

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
        //Set up the Head Wear Icon Button
        if (headWear.isUnlocked)
        {
            _iconButton.image.sprite = headWear.unlockedIcon;
            _bgImage.sprite = _unlockedBGSprite;
        }
        else
        {
            _iconButton.image.sprite = headWear.lockedIcon;
            _bgImage.sprite = _lockedBGSprite;
        }

        if(headWear.isEquipped)
        {
            _bgImage.sprite = _equippedBGSprite;
        }

    }

    public void DisplayHeadWearDetails()
    {
        if (_displayPanel == null)
            return;

        _displayPanel.UpdateDisplay(this, headWear, headWear.isUnlocked);
    }

    public void InitializeDisplayPanel(DisplayPanelHW display)
    {
        _displayPanel = display;
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
