using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsShopTabManager : MonoBehaviour
{

    [SerializeField]
    private List<HeadWearSO> _headWears;    
    [SerializeField]
    private List<SkinSO> _skins;

    [SerializeField]
    private DisplayPanelHW _displayPanelHW;
    [SerializeField]
    private DisplayPanelSkins _displayPanelSK;

    [SerializeField]
    private ItemDisplayUI _itemDisplayUI;

    [SerializeField]
    private HeadWearManager _headWearManager;
    [SerializeField]
    private SkinsManager _skinsManager;

    public void AssignHeadWears() //  Called when opening Skins UI --- Replace each item on the head wear display UI
    {
        _headWears = _headWearManager.HeadWears;

        Debug.Log("ASSIGNING HEADWEARS");
        //Scans the player prefs if each head wear is unlocked or locked        
        _itemDisplayUI.AssignForSaleHeadWearSOs(_headWears, _displayPanelHW);

    }
    public void AssignSkins() //  Called when opening Skins UI --- Replace each item on the skins display UI
    {
        _skins = _skinsManager.Skins;

        Debug.Log("ASSIGNING HEADWEARS");
        //Scans the player prefs if each skin is unlocked or locked        
        _itemDisplayUI.AssignForSaleSkinSOs(_skins, _displayPanelSK);

    }

}
