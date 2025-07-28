using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDisplayUI : MonoBehaviour
{
    //make a list of gameObjects for list - this is manually set by dev
    public List<HeadWearDisplay> items;


#region This Region Is Curently Not In Use
    //ask for num of rows and columns
    [SerializeField]
    private int _numOfItems;
    [SerializeField]
    private int _numOfColums;
    private int _numOfRows;
    private int _totalNumOfItems;
    //make multiple panels depending of number of rows
#endregion

    [SerializeField]
    private DisplayPanelHW _displayPanel;
    [SerializeField]
    private GameObject _itemPrefab;
    
    private List<GameObject> _displayRows;

    //put each gameobject from list to each row with max of # of columns per row


    // Start is called before the first frame update
    void Start()
    {
        //CreateItems();
    }

    public void CreateItems()
    {

        Debug.Log("ITEMS DESTROYED");
        _numOfRows = (int)Mathf.Ceil(_numOfItems/ 3f);
        _totalNumOfItems = _numOfRows * _numOfColums;


        foreach (HeadWearDisplay item in items)
        {
            Destroy(item.gameObject);
        }

        List<HeadWearDisplay> newItems = new();

        for(int i = 0; i < _totalNumOfItems; i++)
        {
            newItems.Add(Instantiate(_itemPrefab, transform).GetComponent<HeadWearDisplay>());
            newItems[i].InitializeDisplayPanel(_displayPanel);
        }

        items = newItems;

    }

    public void AssignHeadWearSOs(List<HeadWearSO> headWears, DisplayPanelHW displayPanel)
    {
        int ctr = 0;

        foreach (HeadWearDisplay hwDisplay in items)
        {
            Debug.Log("Displayed " + ctr + "/" + items.Count() + " items");
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

        foreach (HeadWearSO headwear in headWears)
        {
            Debug.Log("Displayed " + ctr + "/" + items.Count() + " items");
            if (headwear.isPurchasable)
            {
                items[ctr].InitializeDisplayPanel(displayPanel);
                items[ctr].headWear = headwear;
                items[ctr].InitializeData();
                ctr++;

            }

            
        }
    }
}
