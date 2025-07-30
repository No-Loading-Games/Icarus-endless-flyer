using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadWearManager : MonoBehaviour // Used to manage ALL head wears inside the Skins UI and the Shop UI
{

    #region Private & Public Variables
    private static HeadWearManager instance;

    public static HeadWearManager Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private HeadWearSO _defaultHeadWear;
    [SerializeField]
    private Animator _defaultHeadWearAnimator;

    [SerializeField]
    private AnimatorOverrideController _headWearAnimatorOverrider;

    private HeadWearSO _currentHeadWear;
    private Animator _currentHeadWearAnimator;
    private AnimatorOverrideController _currentHeadWearAnimatorOverride;


    [SerializeField]
    private GameObject _headWearPanel;
    [SerializeField]
    private DisplayPanelHW _displayPanel;
    [SerializeField]
    private ItemDisplayUI _itemDisplayUI;

    // LIST OF HEADWEAR SCRIPTABLE OBJECTS -----------------------------------//
    [SerializeField]
    private List<HeadWearSO> _headWears;

    private bool _headWearIsEquipped;

    [SerializeField]
    private HeadWear _icarusStartHeadWear;
    [SerializeField]
    private HeadWear _icarusGameHeadWear;
    [SerializeField]
    private IcarusStart _icarusStart;


    [SerializeField]
    private GameObject _HWDisplayPrefab;
    [SerializeField]
    private GameObject _HWShopPrefab;

    [SerializeField]
    private List<string> _animationClipNames;

    /*[SerializeField]
    private List<KeyValuePair<AnimationClip, AnimationClip>> overrides;*/

    private GameManager _gameManager;
    private string _equippedHeadWear;

    public bool HeadWearIsEquipped
    {
        get { return _headWearIsEquipped; }
        set { _headWearIsEquipped = value; }
    }
    public Animator CurrentHeadWearAnimator
    {
        get { return _currentHeadWearAnimator; }
        set { _currentHeadWearAnimator = value; }
    }
    public AnimatorOverrideController CurrentHeadWearAnimatorOverride
    {
        get { return _currentHeadWearAnimatorOverride; }
        set { _currentHeadWearAnimatorOverride = value; }
    }
    public List<HeadWearSO> HeadWears { get { return _headWears; } }

    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }


        bool[] equipState = { false, true };

        if (!PlayerPrefs.HasKey("head-wear-equipped"))
        {
            //_gameManager.HeadWearIsEnabled = false;
            Debug.Log("Is equipped? " + equipState[PlayerPrefs.GetInt("head-wear-equipped")]);
            _headWearIsEquipped = false;
            PlayerPrefs.SetInt("head-wear-equipped", 0);
            PlayerPrefs.SetString("equipped-head-wear", "");

            LockAllHeadWears();
        }


        int i = PlayerPrefs.GetInt("head-wear-equipped");
        _equippedHeadWear = PlayerPrefs.GetString("equipped-head-wear");

    }
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();

        _currentHeadWear = _defaultHeadWear;
        _currentHeadWearAnimatorOverride = _defaultHeadWear.headWearAnimator;
        _currentHeadWearAnimator = _defaultHeadWearAnimator;

        LoadHeadWears();

        Debug.Log("Something equipped? " + _equippedHeadWear);
    }

    public void LoadHeadWears()
    {
        bool[] unlockState = { false, true };

        InitializeAnimatorController();

        int ctr = 0;

        // Check which head wear is equipped and store it as the current head wear if equipped
        foreach (HeadWearSO headWear in _headWears)
        {
            bool isUnlocked = unlockState[PlayerPrefs.GetInt(headWear.headWearNamePP)];

            if (isUnlocked == false)
            {
                headWear.isUnlocked = false;
                
            }
            else
            {
                headWear.isUnlocked = true;

                if (_equippedHeadWear == headWear.headWearNamePP)
                {
                    headWear.isEquipped = true;
                    StoreCurrentHeadWear(headWear);
                    Debug.Log("SOMETHING IS TRUE for " + ctr);
                }

            }

            Debug.Log(ctr++);
        }


    }

    public void AssignHeadWears() //  Called when opening Skins UI --- Replace each item on the head wear display UI
    {
        _displayPanel.itemIsSelected = false;

        //Scans the player prefs if each head wear is unlocked or locked        
        _itemDisplayUI.AssignHeadWearSOs(_headWears, _displayPanel);

    }

    public void UnlockHeadWear(string headWearName)
    {
        foreach (HeadWearSO headWear in _headWears)
        {
            if (headWear.headWearName == headWearName)
            {
                headWear.isUnlocked = true;
                PlayerPrefs.SetInt(headWear.headWearNamePP, 1);
            }
        }
    }

    public void LockHeadWear(string headWearName)
    {
        foreach (HeadWearSO headWear in _headWears)
        {
            if (headWear.headWearName == headWearName)
            {
                headWear.isUnlocked = false;
                headWear.isEquipped = false;
                PlayerPrefs.SetInt(headWear.headWearNamePP, 0);
            }
        }

        Debug.Log("Locked head wear: " + headWearName);
    }

    public void LockAllHeadWears()
    {
        foreach (HeadWearSO headWear in _headWears)
        {
            headWear.isUnlocked = false;
            headWear.isEquipped = false;
            PlayerPrefs.SetInt(headWear.headWearNamePP, 0);
        }

        Debug.Log("Locked head wears");
    }

    public void StoreCurrentHeadWear(HeadWearSO headwear)
    {
        _currentHeadWear = headwear;

        InitiliazeAnimatorOverrider(headwear.animationClips);

        PlayerPrefs.SetInt("head-wear-equipped", 1);
        PlayerPrefs.SetString("equipped-head-wear", headwear.headWearNamePP);
        _headWearIsEquipped = true;

        _icarusStartHeadWear.gameObject.SetActive(HeadWearManager.Instance.HeadWearIsEquipped);
        _icarusStart.ResetIcarusState(0);
        //Debug.Log("SOMETHING 2 IS NOW " + _currentHeadWearAnimatorOverride);
        _icarusStartHeadWear.InitializeAnimator();

        Debug.Log(_equippedHeadWear + " is equipped");
        //_gameManager.EquipHeadWear(_currentHeadWearAnimatorOverride);
    }

    public void UnequipCurrentHeadWear()
    {
        PlayerPrefs.SetInt("head-wear-equipped", 0);
        PlayerPrefs.SetString("equipped-head-wear", "");
        _headWearIsEquipped = false;

        _icarusStartHeadWear.gameObject.SetActive(HeadWearManager.Instance.HeadWearIsEquipped);
        _icarusStartHeadWear.InitializeAnimator();
        //_gameManager.UnequipHeadWear();
    }

    private void InitializeAnimatorController()
    {
        List<string> animClipNames= new List<string>();

        //Get the animation clip names of each animation in the default animator then assign them to the animation clip name string list
        foreach (AnimationClip clip in _defaultHeadWearAnimator.runtimeAnimatorController.animationClips)
        {

            if(!animClipNames.Contains(clip.name))
                animClipNames.Add(clip.name);
        }

        _animationClipNames = animClipNames;
    }

    private void InitiliazeAnimatorOverrider(List<AnimationClip> animationClips)
    {
        List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new();
        _headWearAnimatorOverrider.GetOverrides(overrides);

        List<string> animClipNames = new();


        foreach (AnimationClip clip in animationClips)
        {
            animClipNames.Add(clip.name);
        }

        for(int i = 0; i < overrides.Count; i++)
        {
            AnimationClip ogAnimClip = overrides[i].Key;
            int index = animClipNames.IndexOf(ogAnimClip.name);

            if (index >= 0 && index < animationClips.Count)
            {
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(ogAnimClip, animationClips[index]);
                //Debug.Log("SOMETHING IS " + overrides[i]);
            }
        }

        _headWearAnimatorOverrider.ApplyOverrides(overrides);

        _currentHeadWearAnimatorOverride = _headWearAnimatorOverrider;
        Debug.Log("SOMETHING IS NOW " + _currentHeadWearAnimatorOverride);
    }


}
