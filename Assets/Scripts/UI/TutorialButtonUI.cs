using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialButtonUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _tutorialUI;

    [SerializeField]
    private GameObject _homeUI;

    [SerializeField]
    private GameObject _startObjects;



    public void EnableTutorialUI()
    {
        AudioManager.Instance.PlaySFX("UI Click", 0f);

        _tutorialUI.SetActive(true);
        _homeUI.SetActive(false);

        _startObjects.transform.DOMoveY(_startObjects.transform.position.y - 10f, 1f);
    }

    public void DisableTutorialUI()
    {
        AudioManager.Instance.PlaySFX("UI Click", 0f);
        
        _startObjects.transform.DOMoveY(0, 1f);

        _tutorialUI.SetActive(false);
        _homeUI.SetActive(true);
    }
}
