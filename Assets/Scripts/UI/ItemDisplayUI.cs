using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDisplayUI : MonoBehaviour
{
    //make a list of gameObjects for list - this is manually set by dev
    public List<HeadWearDisplay> itemsHW;

    public List<SkinDisplay> itemsSK;


#region This Region Is Curently Not In Use
    //ask for num of rows and columns
    [SerializeField]
    private int _numOfItems;
    [SerializeField]
    private int _numOfColums;
    private int _numOfRows;
    private int _totalNumOfItems;
    //make multiple panels depending of number of rows

    [SerializeField]
    private DisplayPanelHW _displayPanelHW;

    [SerializeField]
    private DisplayPanelSkins _displayPanelSkins;

#endregion

    [SerializeField]
    private GameObject _itemPrefab;
    
    private List<GameObject> _displayRows;

    //put each gameobject from list to each row with max of # of columns per row


    // Start is called before the first frame update
    void Start()
    {
    }

    public void AssignHeadWearSOs(List<HeadWearSO> headWears, DisplayPanelHW displayPanel)
    {
        int ctr = 0;

        foreach (HeadWearDisplay hwDisplay in itemsHW)
        {
            Debug.Log("Displayed " + ctr + "/" + itemsHW.Count() + " items");
            hwDisplay.InitializeDisplayPanel(displayPanel);

            if (ctr < headWears.Count && headWears[ctr] != null)
            {
                hwDisplay.headWear = headWears[ctr];
                hwDisplay.InitializeData();

                if (hwDisplay.headWear.isEquipped || ctr == 0)
                {
                    hwDisplay.DisplayHeadWearDetails();
                }

                ctr++;
            }
        }
    }

    public void AssignForSaleHeadWearSOs(List<HeadWearSO> headWears, DisplayPanelHW displayPanel)
    {
        int ctr = 0;
        Debug.Log("Headwears to be Displayed");

        if (itemsHW == null)
            return;

        foreach (HeadWearSO headwear in headWears)
        {
            Debug.Log("Displayed " + ctr + "/" + itemsHW.Count() + " items");
            if (headwear.isPurchasable)
            {
                itemsHW[ctr].InitializeDisplayPanel(displayPanel);
                itemsHW[ctr].headWear = headwear;
                itemsHW[ctr].InitializeData();
                ctr++;

            }            
        }

        foreach(HeadWearDisplay item in itemsHW)
        {
            item.InitializeDisplayPanel(displayPanel);
            if(displayPanel != null)
                Debug.Log("DISPLAYING PANEL ");

            if (!item.headWear.isPurchasable)
                Destroy(item.gameObject);
        }
        
    }


    public void AssignSkinSOs(List<SkinSO> skins, DisplayPanelSkins displayPanel)
    {
        int ctr = 0;

        foreach (SkinDisplay skDisplay in itemsSK)
        {
            Debug.Log("Displayed " + ctr + "/" + itemsSK.Count() + " items");
            skDisplay.InitializeDisplayPanel(displayPanel);

            if (ctr < skins.Count && skins[ctr] != null)
            {
                skDisplay.skin = skins[ctr];
                skDisplay.InitializeData();

                if (skDisplay.skin.isEquipped || ctr == 0)
                {
                    skDisplay.DisplaySkinDetails();
                }

                ctr++;
            }
        }
    }
    public void AssignForSaleSkinSOs(List<SkinSO> skins, DisplayPanelSkins displayPanel)
    {
        int ctr = 0;
        Debug.Log("Skins to be Displayed");

        if (itemsSK == null)
            return;

        foreach (SkinSO skin in skins)
        {
            if (skin.isPurchasable)
            {
                itemsSK[ctr].InitializeDisplayPanel(displayPanel);
                itemsSK[ctr].skin = skin;
                itemsSK[ctr].InitializeData();
                ctr++;

            Debug.Log("Displayed " + ctr + "/" + itemsSK.Count() + " items " + itemsSK[ctr].skin.skinName);
            }            
        }

        foreach(SkinDisplay item in itemsSK)
        {
            item.InitializeDisplayPanel(displayPanel);
            if(displayPanel != null)
                Debug.Log("DISPLAYING PANEL ");

            if (!item.skin.isPurchasable)
                Destroy(item.gameObject);
        }
        
    }
}
