using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BestScoreButton : MonoBehaviour
{
    [SerializeField]
    private GameObject _bestScoreUI;
    
    [SerializeField]
    private GameObject _homeUI;

    [SerializeField]
    private GameObject _startObjects;


    public void EnableBestScoreUI()
    {
        AudioManager.Instance.PlaySFX("UI Click", 0f);

        _bestScoreUI.SetActive(true);
        _homeUI.SetActive(false);

        _startObjects.transform.DOMoveY(_startObjects.transform.position.y - 10f, 1f);
    }

    public void DisableBestScoreUI()
    {
        AudioManager.Instance.PlaySFX("UI Click", 0f);

        _startObjects.transform.DOMoveY(0f, 1f);

        _bestScoreUI.SetActive(false);
        _homeUI.SetActive(true);
    }
}
