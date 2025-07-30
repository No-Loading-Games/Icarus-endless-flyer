using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsManager : MonoBehaviour
{
    #region Private & Public Variables
    private static SkinsManager instance;

    public static SkinsManager Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private SkinSO _defaultSkin;
    [SerializeField]
    private Animator _defaultSkinAnimator;

    [SerializeField]
    private AnimatorOverrideController _skinAnimatorOverrider;

    private SkinSO _currentSkin;
    private Animator _currentSkinAnimator;
    private AnimatorOverrideController _currentSkinAnimatorOverride;


    [SerializeField]
    private GameObject _skinPanel;
    [SerializeField]
    private DisplayPanelSkins _displayPanel;
    [SerializeField]
    private ItemDisplayUI _itemDisplayUI;

    // LIST OF HEADWEAR SCRIPTABLE OBJECTS -----------------------------------//
    [SerializeField]
    private List<SkinSO> _skins;

    private bool _skinIsEquipped;

    [SerializeField]
    private Skin _icarusStartSkin;
    [SerializeField]
    private Skin _icarusGameSkin;
    [SerializeField]
    private IcarusStart _icarusStart;


    [SerializeField]
    private GameObject _SKDisplayPrefab;
    [SerializeField]
    private GameObject _SKShopPrefab;

    [SerializeField]
    private List<string> _animationClipNames;

    /*[SerializeField]
    private List<KeyValuePair<AnimationClip, AnimationClip>> overrides;*/

    private GameManager _gameManager;
    private string _equippedSkin;

    public bool SkinIsEquipped
    {
        get { return _skinIsEquipped; }
        set { _skinIsEquipped = value; }
    }
    public Animator CurrentSkinAnimator
    {
        get { return _currentSkinAnimator; }
        set { _currentSkinAnimator = value; }
    }
    public AnimatorOverrideController CurrentSkinAnimatorOverride
    {
        get { return _currentSkinAnimatorOverride; }
        set { _currentSkinAnimatorOverride = value; }
    }
    public List<SkinSO> Skins { get { return _skins; } }

    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }


        bool[] equipState = { false, true };

        if (!PlayerPrefs.HasKey("skin-equipped"))
        {
            Debug.Log("Is equipped? " + equipState[PlayerPrefs.GetInt("skin-equipped")]);

            EquipDefaultSkin("Icarus");

            LockAllSkins();
        }

        UnlockSkin("Icarus");

        int i = PlayerPrefs.GetInt("skin-equipped");
        _equippedSkin = PlayerPrefs.GetString("equipped-skin");

    }
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();

        _currentSkin = _defaultSkin;
        _currentSkinAnimatorOverride = _defaultSkin.inGameAnimator;
        _currentSkinAnimator = _defaultSkinAnimator;

        LoadSkins();

        Debug.Log("Something equipped? " + _equippedSkin);
    }

    public void LoadSkins()
    {
        bool[] unlockState = { false, true };

        InitializeAnimatorController();

        int ctr = 0;

        // Check which skin is equipped and store it as the current skin if equipped
        foreach (SkinSO skin in _skins)
        {
            bool isUnlocked = unlockState[PlayerPrefs.GetInt(skin.skinNamePP)];

            if (isUnlocked == false)
            {
                skin.isUnlocked = false;

            }
            else
            {
                Debug.Log("(1) SOMETHING IS TRUE for " + skin.skinNamePP);
                skin.isUnlocked = true;

                if (_equippedSkin == skin.skinNamePP)
                {
                    skin.isEquipped = true;
                    StoreCurrentSkin(skin);
                    Debug.Log("SOMETHING IS TRUE for " + ctr);
                }

            }

        }


    }

    public void EquipDefaultSkin(string skinName)
    {
        foreach (SkinSO skin in _skins)
        {
            if (skin.skinName == skinName)
            {
                skin.isEquipped = true;
                _skinIsEquipped = true;
                _equippedSkin = skin.skinNamePP;
                _currentSkin = skin;
                PlayerPrefs.SetInt("skin-equipped", 1);
                PlayerPrefs.SetString("equipped-skin", skin.skinNamePP);
            }
        }
    }

    public void AssignSkins() //  Called when opening Skins UI --- Replace each item on the skin display UI
    {
        _displayPanel.itemIsSelected = false;

        //Scans the player prefs if each skin is unlocked or locked        
        _itemDisplayUI.AssignSkinSOs(_skins, _displayPanel);

    }

    public void UnlockSkin(string skinName)
    {
        foreach (SkinSO skin in _skins)
        {
            if (skin.skinName == skinName)
            {
                skin.isUnlocked = true;
                PlayerPrefs.SetInt(skin.skinNamePP, 1);
            }
        }
    }

    public void LockSkin(string skinName)
    {
        foreach (SkinSO skin in _skins)
        {
            if (skin.skinName == skinName)
            {
                skin.isUnlocked = false;
                skin.isEquipped = false;
                PlayerPrefs.SetInt(skin.skinNamePP, 0);
            }
        }

        Debug.Log("Locked skin: " + skinName);
    }

    public void LockAllSkins()
    {
        foreach (SkinSO skin in _skins)
        {
            if (skin.skinName == "Icarus")
                return;

            skin.isUnlocked = false;
            skin.isEquipped = false;
            PlayerPrefs.SetInt(skin.skinNamePP, 0);
        }

        Debug.Log("Locked skin");
    }

    public void StoreCurrentSkin(SkinSO skin)
    {
        _currentSkin = skin;

        InitiliazeAnimatorOverrider(skin.animationClips);

        PlayerPrefs.SetInt("skin-equipped", 1);
        PlayerPrefs.SetString("equipped-skin", skin.skinNamePP);
        _skinIsEquipped = true;

        _icarusStartSkin.gameObject.SetActive(SkinsManager.Instance.SkinIsEquipped);
        _icarusStart.ResetIcarusState(0);
        //Debug.Log("SOMETHING 2 IS NOW " + _currentSkinAnimatorOverride);
        _icarusStartSkin.InitializeAnimator();


        Debug.Log(_equippedSkin + " is equipped");
        //_gameManager.EquipSkin(_currentSkinAnimatorOverride);
    }

    public void UnequipCurrentSkin()
    {
        PlayerPrefs.SetInt("skin-equipped", 0);
        PlayerPrefs.SetString("equipped-skin", "");
        _skinIsEquipped = false;

        _icarusStartSkin.gameObject.SetActive(SkinsManager.Instance.SkinIsEquipped);
        _icarusStartSkin.InitializeAnimator();
        //_gameManager.UnequipSkin();
    }

    private void InitializeAnimatorController()
    {
        List<string> animClipNames = new List<string>();

        //Get the animation clip names of each animation in the default animator then assign them to the animation clip name string list
        foreach (AnimationClip clip in _defaultSkinAnimator.runtimeAnimatorController.animationClips)
        {

            if (!animClipNames.Contains(clip.name))
                animClipNames.Add(clip.name);
        }

        _animationClipNames = animClipNames;
    }

    private void InitiliazeAnimatorOverrider(List<AnimationClip> animationClips)
    {
        List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new();
        _skinAnimatorOverrider.GetOverrides(overrides);

        List<string> animClipNames = new();


        foreach (AnimationClip clip in animationClips)
        {
            animClipNames.Add(clip.name);
        }

        for (int i = 0; i < overrides.Count; i++)
        {
            AnimationClip ogAnimClip = overrides[i].Key;
            int index = animClipNames.IndexOf(ogAnimClip.name);

            if (index >= 0 && index < animationClips.Count)
            {
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(ogAnimClip, animationClips[index]);
                //Debug.Log("SOMETHING IS " + overrides[i]);
            }
        }

        _skinAnimatorOverrider.ApplyOverrides(overrides);

        _currentSkinAnimatorOverride = _skinAnimatorOverrider;
        Debug.Log("SOMETHING IS NOW " + _currentSkinAnimatorOverride);
    }

}
