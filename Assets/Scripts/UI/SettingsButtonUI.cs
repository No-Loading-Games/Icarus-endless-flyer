using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SettingsButtonUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _settingsUI;
    
    [SerializeField]
    private GameObject _homeUI;

    [SerializeField]
    private GameObject _startObjects;

    [SerializeField]
    private GameObject _flyUI;

    [SerializeField]
    private BackButtonUI _backButton;

    [SerializeField]
    private bool _isFromMainMenu;
    public void EnableSettingsUI()
    {
        AudioManager.Instance.PlaySFX("UI Click", 0f);

        _settingsUI.SetActive(true);
        _homeUI.SetActive(false);

        _backButton.hiddenUI = _settingsUI;
        _backButton.shownUI = _homeUI;
        _backButton.flyUI = _flyUI;
        _backButton.gameStartObjects = _startObjects;
        _backButton.isFromMainMenu = _isFromMainMenu;

        _startObjects.transform.DOMoveY(_startObjects.transform.position.y - 10f, 1f);
    }
    
}
