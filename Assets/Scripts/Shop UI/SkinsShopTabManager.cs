using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsShopTabManager : MonoBehaviour
{

    [SerializeField]
    private List<HeadWearSO> _headWears;
    [SerializeField]
    private DisplayPanelHW _displayPanel;
    [SerializeField]
    private ItemDisplayUI _itemDisplayUI;

    [SerializeField]
    private HeadWearManager _headWearManager;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AssignHeadWears() //  Called when opening Skins UI --- Replace each item on the head wear display UI
    {
        _headWears = _headWearManager.HeadWears;
        _displayPanel.itemIsSelected = false;

        //Scans the player prefs if each head wear is unlocked or locked        
        _itemDisplayUI.AssignForSaleHeadWearSOs(_headWears, _displayPanel);

    }

}
