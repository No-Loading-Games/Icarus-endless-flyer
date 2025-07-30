using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopHeadWearButtonController : MonoBehaviour
{
    public HeadWearDisplay headWearDisplay;

    public GameObject uiElementToToggle;

    private void Start()
    {
        //uiElementToToggle = headWearDisplay._displayPanel.gameObject;
    }

    public void OnPointerDown(BaseEventData eventData)
    {        
        headWearDisplay.ShopDisplayHeadWearDetails();
        headWearDisplay.ActivateIndicator();
        //uiElementToToggle.SetActive(true);
        
    }

    public void OnPointerUp(BaseEventData eventData)
    {
        headWearDisplay.ShopDisplayDeactivate();
        headWearDisplay.DeactivateIndicator();
        //uiElementToToggle.SetActive(false);
        
    }

}
