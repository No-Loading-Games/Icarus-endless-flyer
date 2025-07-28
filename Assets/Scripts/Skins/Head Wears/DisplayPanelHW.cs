using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayPanelHW : MonoBehaviour
{
    public AnimationClip flyingAnimation;
    public AnimatorOverrideController animator;

    public TextMeshProUGUI description;
    public TextMeshProUGUI headWearName;

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
    private HeadWearSO _headWear;

    private HeadWearDisplay _prevHWDisplay;
    private HeadWearDisplay _currHWDisplay;
    private HeadWearDisplay _prevEquippedHW;
    private HeadWearDisplay _currEquippedHW;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();

        _previewAnimator = _previewIcon.GetComponent<Animator>();
        //_previewAnimatorOverrider = (AnimatorOverrideController)_previewAnimator.runtimeAnimatorController;

        itemIsSelected = false;
    }

    // Update is called once per frame
    public void CheckIfHeadWearIsWorn()
    {
        if (HeadWearManager.Instance.HeadWearIsEquipped)
        {
            equipButtonTMP.GetComponent<RectTransform>().localPosition = new Vector2(0, 4.49f);
            equipButtonTMP.color = new Color(.91f, .86f, 0.38f);
            return;
        }
        else
        {
            headWearName.text = "Fearless Icarus";
            description.text = "\"Never regret thy fall, O Icarus of the fearless flight\" - Oscar Wilde";

            equipButton.interactable = false;
            equipButtonTMP.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
            equipButtonTMP.color = new Color(0.83f, .6f, 0.43f);
            Debug.Log("Something 3 is " + equipButton.interactable);
        }

    }

    public void UpdateDisplay(HeadWearDisplay headWearDisplay,HeadWearSO headWear, bool isUnlocked)
    {
        itemIsSelected = true;
        if(_currHWDisplay != null)
        {
            _prevHWDisplay = _currHWDisplay;
            if (_prevHWDisplay.headWear.isEquipped)
                _prevEquippedHW = _prevHWDisplay;
            _prevHWDisplay.DeactivateIndicator();
        }

        _currHWDisplay = headWearDisplay;
        _currHWDisplay.ActivateIndicator();



        _headWear = headWear;
        headWearName.text = headWear.headWearName;
        description.text = headWear.desription;

        //Update Flying animation preview
        flyingAnimation = headWear.flyingAnimation;

        Debug.Log("Something is " + isUnlocked);

        CheckEquipButtonState(isUnlocked);

        if (_headWear.isEquipped)
        {
            _currEquippedHW = _currHWDisplay;
            equipButtonTMP.text = "Unequip";
        }
        else
            equipButtonTMP.text = "Equip";

        ChangePreviewAnimation();

    }

    public void CheckIfEquipped()
    {
        if (_headWear.isEquipped)
        {
            equipButtonTMP.text = "Equip";
            UnequipHeadWear();
        }
        else
        {
            equipButtonTMP.text = "Unequip";
            EquipHeadWear();
        }
    }

    public void EquipHeadWear()
    {
        if(_prevEquippedHW != null)
        {
            _prevEquippedHW.UnshowEquippedBG();
            _prevEquippedHW.headWear.isEquipped = false;
        }

        _prevEquippedHW = _currEquippedHW;
        _currEquippedHW = _currHWDisplay;

        _headWear.isEquipped = true;

        //change EQUIP Button Image Sprite

        _currEquippedHW.ShowEquippedBG();
        HeadWearManager.Instance.StoreCurrentHeadWear(_headWear);
    }

    public void UnequipHeadWear()
    {
        _headWear.isEquipped = false;
        _currEquippedHW.UnshowEquippedBG();
        _prevEquippedHW = _currHWDisplay;

        HeadWearManager.Instance.UnequipCurrentHeadWear();
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
