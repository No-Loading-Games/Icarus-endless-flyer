using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Search;
using TMPro;
using UnityEngine;

public class MidasPowerup : Powerup
{

    /*[SerializeField]
    private Sprite _pSprite;
*/

    protected override void Start()
    {
        base.Start();
        _animator.CrossFade("midas", 0.1f);
    }

    protected override void Update()
    {
        base.Update();
    }


    public override void ApplyPowerup()
    {
        //_gameManager.StartPowerUpUICounter(1,_pSprite);
        _gameManager.midasIsOn = true;
        _gameManager.MidasPowerUpUIDisplay();
        _powerupManager.Midas.SetActive(true);

        
    }

    public override void CleanupPowerup()
    {
        //_gameManager.DespawnPowerUpUI();
        //_gameManager.midasIsOn = false;
        _powerupManager.Midas.SetActive(false);
        _gameManager.midasIsOn = false;
        _gameManager.MidasPowerUpUIDisplay();
    }

    public override void HandlePowerup()
    {
    }
}
