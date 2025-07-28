using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherPowerup : Powerup
{
    [SerializeField, Range(0, 1f)]
    private float _staminaAmount = 0.2f;

    protected override void Start()
    {
        base.Start();

        _animator.CrossFade("Feather", 0.1f);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void ApplyPowerup()
    {
        _gameManager.AddStamina(_staminaAmount);
        _powerupManager.GetComponent<PlayerController>().AddFeatherVFX();

        Debug.Log("FEATHER ADDED");

        //_powerupManager.DeletePowerup();
    }

    public override void HandlePowerup()
    {
    }

    public override void CleanupPowerup()
    {
    }
}
