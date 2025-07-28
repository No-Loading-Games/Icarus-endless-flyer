using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermesPowerup : Powerup
{

    protected override void Update()
    {
        base.Update();
    }


    public override void ApplyPowerup()
    {
        //_playerController.EnableCollision(false);
        _playerController.AddDisableReason("hermes powerup");
        _powerupManager.Hermes.SetActive(true);
        _powerupManager.Hermes.GetComponent<Animator>().SetLayerWeight(3, 1);
        _powerupManager.Hermes.GetComponent<Animator>().SetLayerWeight(0, 0);

        GameObject hermes = _powerupManager.Hermes;
        _powerupManager.powerUpGameObject = hermes;
        _powerupManager.powerUpAnimator = hermes.GetComponent<Animator>();
    }

    public override void CleanupPowerup()
    {
        _gameManager.DespawnPowerUpUI();
        //_playerController.EnableCollision(true);
        _playerController.RemoveDisableReason("hermes powerup");
        _powerupManager.Hermes.SetActive(false);
    }

    public override void HandlePowerup() 
    {
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _animator.CrossFade("hermes", 0.1f);
    }

}
