using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AccessoryShopItem : ShopItem
{
    public void UpdateHeadWears()
    {
        //Item does not update at the moment
        HeadWearManager.Instance.UnlockHeadWear(_item.name);

        Debug.Log("Headwear bought " + _item.namePP + " || Unlocked? " + _item.maxPurchase);
        _item.GetComponent<HeadWearDisplay>().InitializeData();
    }
    
    public void UpdateSkins()
    {
        //Item does not update at the moment
        SkinsManager.Instance.UnlockSkin(_item.name);

        Debug.Log("Skin bought " + _item.namePP + " || Unlocked? " + _item.maxPurchase);
        _item.GetComponent<SkinDisplay>().InitializeData();
    }
}
