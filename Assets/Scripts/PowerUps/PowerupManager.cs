using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerupManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _artemisArrow;

    [SerializeField]
    private GameObject _magnet;

    [SerializeField]
    private GameObject _zeus;

    [SerializeField]
    private GameObject _midas;

    [SerializeField]
    private GameObject _hermes;

    public ParticleSystem artemisSpeedTrailFX;
    public ParticleSystem artemisCloudTrailFX;

    public List<float> pickUpNotifRotation;

    public GameObject pickUpNotifVFX;
    public AnimationClip pickUpNotifAnim;

    public Animator powerUpAnimator;

    public GameObject powerUpGameObject = null;

    public GameObject ArtemisArrow
    {
        get { return _artemisArrow; }
    }
    public GameObject Magnet
    {
        get { return _magnet; }
    }
    public GameObject Zeus
    {
        get { return _zeus; }
    }
    public GameObject Midas
    {
        get { return _midas; }
    }
    public GameObject Hermes
    {
        get { return _hermes; }
    }

    private Powerup _currentPowerup;

    public Powerup CurrentPowerup
    {
        get { return _currentPowerup; }
        set
        {
            DeletePowerup();

            _currentPowerup = value;
            //_currentPowerup.GetComponent<BoxCollider2D>().enabled = false;
            _currentPowerup.ApplyPowerup();
        }
    }

    // Start is called before the first frame update
    void Start() { }

    public void DisableAllPowerups()
    {
        _artemisArrow.SetActive(false);
        _zeus.SetActive(false);
        _magnet.SetActive(false);
        _midas.SetActive(false);
        _hermes.SetActive(false);
    }

    public void SetPowerup(Powerup powerup)
    {
        _currentPowerup = powerup;
    }

    private void HandlePowerup()
    {
        if (_currentPowerup == null)
            return;

        _currentPowerup.HandlePowerup();
    }

    public void DeletePowerup()
    {
        //DisableAllPowerups();
        DisableAllPowerups();

        Debug.Log("DELETING POWERUP");
        
        if( _currentPowerup != null )
        {
            _currentPowerup.CleanupPowerup();
            Destroy(_currentPowerup.gameObject);
        }
        
        _currentPowerup = null;
    }

    // Update is called once per frame
    void Update()
    {
        HandlePowerup();
    }
}
