using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopTabs : MonoBehaviour
{
    public string[] pageAnimationClipNames;
    public string pageTitle;

    public GameObject tabObject;

    public int tabIdentifier;

    private ShopManager _shopManager;

    //public event Action<int, string, string[]> ChangeTabEvent;

    public void MakeCurrentTab()
    {
        _shopManager = FindObjectOfType<ShopManager>();

        _shopManager.prevTab = _shopManager.currentTab;
        _shopManager.prevTab.tabObject.SetActive(false);

        _shopManager.currentTab = this;
        _shopManager.currentTab.tabObject.SetActive(true);

        if (_shopManager.prevTab.tabIdentifier == _shopManager.currentTab.tabIdentifier)
        {
            Debug.Log("TUTORIAL ERROR");
            return;
        }

        float prevTabX = _shopManager.prevTab.transform.position.x;
        _shopManager.prevTab.transform.DOMoveX(prevTabX + .14f, 0.3f);

        float currTabX = _shopManager.currentTab.transform.position.x;
        _shopManager.currentTab.transform.DOMoveX(currTabX - .14f, 0.3f);

        //_shopManager.pageNum = 1;
        //_shopManager.currentTabAnimationClipNames = pageAnimationClipNames;
        _shopManager.currentTabIdentifier = tabIdentifier;
        _shopManager.currentTabTitle = pageTitle;

        _shopManager.UpdatePage();

        Debug.Log("CURRENT TAB IS " + pageTitle);

        //ChangeTabEvent?.Invoke(tabIdentifier,pageTitle,pageAnimationClipNames);
    }
}
