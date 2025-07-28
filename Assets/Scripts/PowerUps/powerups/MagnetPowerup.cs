using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPowerup : Powerup
{
    [SerializeField]
    private float _duration = 5f;

    [SerializeField]
    private Sprite _pSprite;

    [SerializeField]
    private Color _pColor;

    private Color _headWearColor;

    protected override void Update()
    {
        base.Update();
    }

    public override void ApplyPowerup()
    {
        _duration = PowerupUpgradeManager.Instance.MagnetDuration;

        _gameManager.SpawnPowerUpUI();
        _powerupManager.Magnet.SetActive(true);
        _powerupManager.Magnet.GetComponent<Animator>().SetLayerWeight(1, 1);
        _powerupManager.Magnet.GetComponent<Animator>().SetLayerWeight(0, 0);
        _powerupManager.Magnet.GetComponent<Animator>().CrossFade("idle", 0.1f, 1);

        _headWearColor = _powerupManager.GetComponent<PlayerController>()._headWear.GetComponent<SpriteRenderer>().color;
        _powerupManager.GetComponent<PlayerController>()._headWear.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);

        GameObject magnet = _powerupManager.Magnet;
        _powerupManager.powerUpGameObject = magnet;
        _powerupManager.powerUpAnimator = magnet.GetComponent<Animator>();

        StartCoroutine(TickDuration());
    }

    public override void CleanupPowerup()
    {
        _powerupManager.powerUpAnimator = null;
        _gameManager.DespawnPowerUpUI();
        _powerupManager.Magnet.SetActive(false);
        _powerupManager.GetComponent<PlayerController>()._headWear.GetComponent<SpriteRenderer>().color = _headWearColor;

    }

    public override void HandlePowerup()
    {
        //_powerupManager.Magnet.GetComponent<Animator>().CrossFade("idle", 0.1f, 1);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _animator.CrossFade("magnet", 0.1f);
    }

    private IEnumerator TickDuration()
    {
        float time = 0f;

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
