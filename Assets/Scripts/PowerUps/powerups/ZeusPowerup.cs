using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeusPowerup : Powerup
{
    [SerializeField]
    private float _duration;

    [SerializeField]
    private AnimationClip _despawnAnim;

    [SerializeField]
    private GameObject _sun;

    [SerializeField]
    private GameObject _stormCloudParent;

    private StormCloudsManager _stormCloudManager;

    [SerializeField]
    private List<StormCloudsBehaviour> _stormClouds;

    [SerializeField]
    private Sprite _pSprite;

    [SerializeField]
    private Color _pColor;

    /*public float ZeusDuration
    {
        get { return _duration; }
        set { ZeusDuration = _duration; }
    }*/

    protected override void Update()
    {
        base.Update();
    }


    public override void ApplyPowerup()
    {
        _duration = PowerupUpgradeManager.Instance.ZeusDuration;

        _sun = FindObjectOfType<Sun>().gameObject;
        _stormCloudParent = Instantiate(_stormCloudParent, null);
        _stormCloudManager = _stormCloudParent.GetComponent<StormCloudsManager>();

        _stormCloudParent.transform.position = new Vector2(0f, 7.5f);
        _stormCloudManager.isStormCloud = true;

        _powerupManager.Zeus.SetActive(true);
        _powerupManager.Zeus.GetComponent<Animator>().SetBool("despawning", false);
        _powerupManager.Zeus.GetComponent<Animator>().CrossFade("spawning", 0.1f, 0);
        //_playerController.EnableCollision(false);
        _playerController.AddDisableReason("zeus powerup");

        _sun.transform.DOMoveY(7.4f, 1f);    //Moves the sun outside the screen
        _stormCloudManager.SpawnStormClouds(3.2f, 1f);

        GameObject zeus = _powerupManager.Zeus;
        _powerupManager.powerUpGameObject = zeus;
        _powerupManager.powerUpAnimator = zeus.GetComponent<Animator>();
        StartCoroutine(TickDespawning());
    }

    public override void CleanupPowerup()
    {
        _powerupManager.Zeus.SetActive(false);
        //_playerController.EnableCollision(true);
        _playerController.RemoveDisableReason("zeus powerup");
    }

    public override void HandlePowerup() 
    {
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _animator.CrossFade("zeus", 0.1f);
    }

    private IEnumerator TickDespawning()
    {
        float time = 0f;

        while (time <= _duration - _despawnAnim.length)
        {
            time += Time.deltaTime;
            _gameManager.StartPowerUpUITimer(time, _duration, _pSprite, _pColor);
            yield return null;
        }
        _powerupManager.Zeus.GetComponent<Animator>().SetBool("despawning", true);
        Debug.Log("ZEUS CLOUD DESPAWING");

        _sun.transform.DOMoveY(4.2f, 1f);     //Moves the Sun back inside the screen
        _stormCloudManager.DespawnStormClouds(_despawnAnim.length);

        StartCoroutine(TickDuration(time));
    }

    private IEnumerator TickDuration(float time)
    {

        while (time <= _duration)
        {
            time += Time.deltaTime;
            _gameManager.StartPowerUpUITimer(time, _duration, _pSprite, _pColor);
            yield return null;
        }

        _gameManager.DespawnPowerUpUI();
        _powerupManager.DeletePowerup();
    }


}
