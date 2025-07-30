using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupUpgradeManager : MonoBehaviour
{
    #region Singleton Instantiation
    private static PowerupUpgradeManager instance;

    public static PowerupUpgradeManager Instance
    {
        get { return instance; }
    }

    #endregion

    [SerializeField]
    private int _maxUpgradeLevel;

    [SerializeField]
    private float _upgradeCostFactor;

    [Header("DEFAULT Property Values of Powerups")]
    [SerializeField]
    private float _defArtemisMultiplier;
    [SerializeField]
    private float _defZeusDuration;
    [SerializeField]
    private float _defMagnetDuration;
    [SerializeField]
    private int _defHermesMaxJumps;

    [Header("CURRENT Property Values of Powerups")]
    [SerializeField]
    private float _artemisMultiplier;
    [SerializeField]
    private float _zeusDuration;
    [SerializeField]
    private float _magnetDuration;
    [SerializeField]
    private int _hermesMaxJumps;

    [Header("CURRENT Upgrade Level of Powerups")]
    [SerializeField]
    private int _artemisUpgradeLevel;
    [SerializeField]
    private int _zeusUpgradeLevel;
    [SerializeField]
    private int _magnetUpgradeLevel;
    [SerializeField]
    private int _hermesUpgradeLevel;

    #region Properties
    public float MaxUpgradeLevel
    {
        get { return _maxUpgradeLevel; }
        set { MaxUpgradeLevel = _maxUpgradeLevel; }
    }
    public float ArtemisMultiplier
    {
        get { return _artemisMultiplier; }
        set { ArtemisMultiplier = _artemisMultiplier; }
    }
    public float ZeusDuration
    {
        get { return _zeusDuration; ; }
        set { ZeusDuration = _zeusDuration; ; }
    }
    public float MagnetDuration
    {
        get { return _magnetDuration; }
        set { MagnetDuration = _magnetDuration; }
    }
    public int HermesMaxJumps
    {
        get { return _hermesMaxJumps; }
        set { HermesMaxJumps = _hermesMaxJumps; }
    }
    public float UpgradeCostFactor
    {
        get { return _upgradeCostFactor; }
        set { _upgradeCostFactor = value; }
    }

#endregion
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (PlayerPrefs.HasKey("powerups-inputted"))
        {
            LoadPowerUpProperties();
        }
        else
            SetPowerUpProperties();

    }

    private void SetPowerUpProperties()
    {
        _artemisMultiplier = _defArtemisMultiplier;
        _zeusDuration = _defZeusDuration;
        _magnetDuration = _defMagnetDuration;
        _hermesMaxJumps = _defHermesMaxJumps;

        PlayerPrefs.SetInt("powerups-inputted", 1);


        PlayerPrefs.SetFloat("artemis-duration", _artemisMultiplier);
        PlayerPrefs.SetInt("artemis-upgrade-level", 1);

        PlayerPrefs.SetFloat("zeus-duration", _zeusDuration);
        PlayerPrefs.SetInt("zeus-upgrade-level", 1);

        PlayerPrefs.SetFloat("magnet-duration", _magnetDuration);
        PlayerPrefs.SetInt("magnet-upgrade-level", 1);

        PlayerPrefs.SetInt("hermes-max-jump", _hermesMaxJumps);
        PlayerPrefs.SetInt("hermes-upgrade-level", 1);
    }

    private void LoadPowerUpProperties()
    {

        _artemisMultiplier = PlayerPrefs.GetFloat("artemis-duration");
        _artemisUpgradeLevel = PlayerPrefs.GetInt("artemis-upgrade-level");

        _zeusDuration = PlayerPrefs.GetFloat("zeus-duration");
        _zeusUpgradeLevel = PlayerPrefs.GetInt("zeus-upgrade-level");

        _magnetDuration = PlayerPrefs.GetFloat("magnet-duration");
        _magnetUpgradeLevel = PlayerPrefs.GetInt("magnet-upgrade-level");

        _hermesMaxJumps = PlayerPrefs.GetInt("hermes-max-jump");
        _hermesUpgradeLevel = PlayerPrefs.GetInt("hermes-upgrade-level");
    }

    public void SetArtemisMultiplier(float addOn)
    {
        /*if (_artemisUpgradeLevel > _maxUpgradeLevel)
            return;*/
        _artemisUpgradeLevel++;
        PlayerPrefs.SetFloat("artemis-duration", _artemisMultiplier += addOn);
        PlayerPrefs.SetInt("artemis-upgrade-level", _artemisUpgradeLevel);
    }
    public void SetZeusDuration(float addOn)
    {
        /*if (_zeusUpgradeLevel > _maxUpgradeLevel)
            return;*/
        _zeusUpgradeLevel++;
        PlayerPrefs.SetFloat("zeus-duration", _zeusDuration += addOn);
        PlayerPrefs.SetInt("zeus-upgrade-level", _zeusUpgradeLevel);
    }
    public void SetMagnetDuration(float addOn)
    {
        /*if (_magnetUpgradeLevel > _maxUpgradeLevel)
            return;*/
        _magnetUpgradeLevel++;
        PlayerPrefs.SetFloat("magnet-duration", _magnetDuration += addOn);
        PlayerPrefs.SetInt("magnet-upgrade-level", _magnetUpgradeLevel);
    }
    public void SetHermesMaxDuration(int addOn)
    {
        /*if (_hermesUpgradeLevel > _maxUpgradeLevel)
            return;*/
        _hermesUpgradeLevel++;
        PlayerPrefs.SetInt("hermes-max-jump", _hermesMaxJumps += addOn);
        PlayerPrefs.SetInt("hermes-upgrade-level", _hermesUpgradeLevel);
    }



}
