using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinsTabManager : MonoBehaviour
{
    public SkinsUITab currentTab;
    public SkinsUITab prevTab;
    public int currentTabIdentifier = 0;
    public int prevTabIdentifier = 0;
    public string currentTabTitle = "Head Wears";
    [SerializeField]
    private List<Sprite> _tabNameSprites;
    [SerializeField]
    private Image _tabName;

    [SerializeField]
    private TextMeshProUGUI _pageTitleTMP;

    private void OnEnable()
    {
        UpdatePage();
    }

    public void UpdatePage()
    {
        prevTab.enabled = false;
        currentTab.enabled = true;


        prevTabIdentifier = currentTabIdentifier;

        //_tabName.sprite = _tabNameSprites[currentTabIdentifier];
        _pageTitleTMP.text = currentTabTitle;

        //_pageNumTMP.text = "(" + pageNum.ToString() + " " + currentTabAnimationClipNames.Length.ToString() + ")";

    }
}
