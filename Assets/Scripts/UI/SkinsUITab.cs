using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinsUITab : MonoBehaviour
{
    public string[] pageAnimationClipNames;
    public string pageTitle;

    public GameObject tabObject;

    public int tabIdentifier;

    private SkinsTabManager _skinsTabManager;

    //public event Action<int, string, string[]> ChangeTabEvent;

    public void MakeCurrentTab()
    {
        _skinsTabManager = FindObjectOfType<SkinsTabManager>();

        _skinsTabManager.prevTab = _skinsTabManager.currentTab;
        _skinsTabManager.prevTab.tabObject.SetActive(false);

        _skinsTabManager.currentTab = this;
        _skinsTabManager.currentTab.tabObject.SetActive(true);

        if (_skinsTabManager.prevTab.tabIdentifier == _skinsTabManager.currentTab.tabIdentifier)
        {
            Debug.Log("TUTORIAL ERROR");
            return;
        }

        float prevTabX = _skinsTabManager.prevTab.transform.position.x;
        _skinsTabManager.prevTab.transform.DOMoveX(prevTabX + .14f, 0.3f);

        float currTabX = _skinsTabManager.currentTab.transform.position.x;
        _skinsTabManager.currentTab.transform.DOMoveX(currTabX - .14f, 0.3f);

        //_skinsTabManager.pageNum = 1;
        //_skinsTabManager.currentTabAnimationClipNames = pageAnimationClipNames;
        _skinsTabManager.currentTabIdentifier = tabIdentifier;
        _skinsTabManager.currentTabTitle = pageTitle;

        _skinsTabManager.UpdatePage();

        Debug.Log("CURRENT TAB IS " + pageTitle);

        //ChangeTabEvent?.Invoke(tabIdentifier,pageTitle,pageAnimationClipNames);
    }
}
