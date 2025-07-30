using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public ShopTabs currentTab;
    public ShopTabs prevTab;
    public int currentTabIdentifier = 0;
    public int prevTabIdentifier = 0;
    public string currentTabTitle = "Market";
    //public GameObject[] currentTabGameObject;

    //public int pageNum = 1;
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
        //Change sprite to DISABLED when interactable = false
        //Animator pageAnimator = _pageContent.GetComponent<Animator>();
        prevTab.enabled = false;
        currentTab.enabled = true;

        /*pageAnimator.SetLayerWeight(prevTabIdentifier, 0);
        pageAnimator.SetLayerWeight(currentTabIdentifier, 1);*/

        prevTabIdentifier = currentTabIdentifier;

        _tabName.sprite = _tabNameSprites[currentTabIdentifier];
        //_pageTitleTMP.text = currentTabTitle;

        //_pageNumTMP.text = "(" + pageNum.ToString() + " " + currentTabAnimationClipNames.Length.ToString() + ")";

    }

}
