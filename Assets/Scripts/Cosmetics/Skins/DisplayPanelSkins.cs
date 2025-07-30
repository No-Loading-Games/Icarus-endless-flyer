using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayPanelSkins : MonoBehaviour
{
    public AnimationClip flyingAnimation;
    public AnimatorOverrideController animator;

    public TextMeshProUGUI description;
    public TextMeshProUGUI skinName;

    public Button equipButton;
    public TextMeshProUGUI equipButtonTMP;
    public bool itemIsSelected;

    [SerializeField]
    private Image _previewIcon;
    [SerializeField]
    private Animator _previewAnimator;
    [SerializeField]
    private AnimationClip _previewDefaultAnimation;
    [SerializeField]
    private AnimatorOverrideController _previewAnimatorOverrider;

    private GameManager _gameManager;
    private SkinSO _skin;

    private SkinDisplay _prevHWDisplay;
    private SkinDisplay _currHWDisplay;
    private SkinDisplay _prevEquippedHW;
    private SkinDisplay _currEquippedHW;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();

        _previewAnimator = _previewIcon.GetComponent<Animator>();
        //_previewAnimatorOverrider = (AnimatorOverrideController)_previewAnimator.runtimeAnimatorController;

        itemIsSelected = false;
    }

    // Update is called once per frame
    public void CheckIfSkinIsWorn()
    {
        if (SkinsManager.Instance.SkinIsEquipped)
        {
            equipButtonTMP.GetComponent<RectTransform>().localPosition = new Vector2(0, 4.49f);
            equipButtonTMP.color = new Color(.91f, .86f, 0.38f);
            return;
        }
        else
        {
            skinName.text = "Fearless Icarus";
            description.text = "\"Never regret thy fall, O Icarus of the fearless flight\" - Oscar Wilde";

            equipButton.interactable = false;
            equipButtonTMP.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
            equipButtonTMP.color = new Color(0.83f, .6f, 0.43f);
            Debug.Log("Something 3 is " + equipButton.interactable);
        }

    }

    public void UpdateDisplay(SkinDisplay skinDisplay,SkinSO skin, bool isUnlocked)
    {
        itemIsSelected = true;
        if(_currHWDisplay != null)
        {
            _prevHWDisplay = _currHWDisplay;
            if (_prevHWDisplay.skin.isEquipped)
                _prevEquippedHW = _prevHWDisplay;
            _prevHWDisplay.DeactivateIndicator();
        }

        _currHWDisplay = skinDisplay;
        _currHWDisplay.ActivateIndicator();



        _skin = skin;
        skinName.text = skin.skinName;
        description.text = skin.desription;

        //Update Flying animation preview
        flyingAnimation = skin.previewAnimation;

        Debug.Log("Something is " + isUnlocked);

        ChangePreviewAnimation();

        if (equipButton == null) // Check if there is an equip button. If there's none, it means the player is accessing the function using the shop
            return;

        CheckEquipButtonState(isUnlocked);

        if (_skin.isEquipped)
        {
            _currEquippedHW = _currHWDisplay;
            equipButtonTMP.text = "Unequip";
        }
        else
            equipButtonTMP.text = "Equip";

    }

    public void CheckIfEquipped()
    {
        if (_skin.isEquipped)
        {
            equipButtonTMP.text = "Equip";
            UnequipSkin();
        }
        else
        {
            equipButtonTMP.text = "Unequip";
            EquipSkin();
        }
    }

    public void EquipSkin()
    {
        if(_prevEquippedHW != null)
        {
            _prevEquippedHW.UnshowEquippedBG();
            _prevEquippedHW.skin.isEquipped = false;
        }

        _prevEquippedHW = _currEquippedHW;
        _currEquippedHW = _currHWDisplay;

        _skin.isEquipped = true;

        //change EQUIP Button Image Sprite

        _currEquippedHW.ShowEquippedBG();
        SkinsManager.Instance.StoreCurrentSkin(_skin);
    }

    public void UnequipSkin()
    {
        _skin.isEquipped = false;
        _currEquippedHW.UnshowEquippedBG();
        _prevEquippedHW = _currHWDisplay;

        SkinsManager.Instance.UnequipCurrentSkin();
    }

    public void CheckEquipButtonState(bool isUnlocked)
    {
        equipButton.interactable = isUnlocked;

        Debug.Log("Something 2 is " + isUnlocked);
        if (isUnlocked)
        {
            equipButtonTMP.GetComponent<RectTransform>().localPosition = new Vector2(0, 4.49f);
            equipButtonTMP.color = new Color(.91f, .86f, 0.38f);
        }
        else
        {
            equipButtonTMP.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
            equipButtonTMP.color = new Color(0.83f, .6f, 0.43f);
        }

    }


    private void ChangePreviewAnimation()
    {
        List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new();
        _previewAnimatorOverrider.GetOverrides(overrides);

        for (int i = 0; i < overrides.Count; i++)
        {
            if (overrides[i].Key == _previewDefaultAnimation)
            {
                // Replace with the new clip (even if it has a different name)
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, flyingAnimation);
                break;
            }

        }

        _previewAnimatorOverrider.ApplyOverrides(overrides);

        _previewAnimator.runtimeAnimatorController = _previewAnimatorOverrider;
    }


}
