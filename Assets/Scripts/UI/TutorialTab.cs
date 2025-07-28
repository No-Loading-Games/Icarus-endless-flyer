using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTab : MonoBehaviour
{
    public string[] pageAnimationClipNames;
    public string pageTitle;

    public int tabIdentifier;

    private TutorialManager _tutorialManager;

    //public event Action<int, string, string[]> ChangeTabEvent;

    public void MakeCurrentTab()
    {
        _tutorialManager = FindObjectOfType<TutorialManager>();

        _tutorialManager.prevTab = _tutorialManager.currentTab;
        _tutorialManager.currentTab = this;

        if (_tutorialManager.prevTab.tabIdentifier == _tutorialManager.currentTab.tabIdentifier)
        {
            Debug.Log("TUTORIAL ERROR");
            return;
        }

        float prevTabX = _tutorialManager.prevTab.transform.position.x;
        _tutorialManager.prevTab.transform.DOMoveX(prevTabX + .14f, 0.3f);

        float currTabX = _tutorialManager.currentTab.transform.position.x;
        _tutorialManager.currentTab.transform.DOMoveX(currTabX - .14f, 0.3f);

        _tutorialManager.pageNum = 1;
        _tutorialManager.currentTabAnimationClipNames = pageAnimationClipNames;
        _tutorialManager.currentTabIdentifier = tabIdentifier;
        _tutorialManager.currentTabTitle = pageTitle;

        _tutorialManager.UpdatePage();

        Debug.Log("CURRENT TAB IS " + pageTitle);

        //ChangeTabEvent?.Invoke(tabIdentifier,pageTitle,pageAnimationClipNames);
    }
    
}
