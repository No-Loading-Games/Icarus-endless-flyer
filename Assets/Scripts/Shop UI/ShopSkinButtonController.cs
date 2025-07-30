using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSkinButtonController : MonoBehaviour
{
    public SkinDisplay skinDisplay;

    public GameObject uiElementToToggle;

    private void Start()
    {
        //uiElementToToggle = headWearDisplay._displayPanel.gameObject;
    }

    public void OnPointerDown(BaseEventData eventData)
    {
        skinDisplay.ShopDisplaySkinDetails();
        skinDisplay.ActivateIndicator();
        //uiElementToToggle.SetActive(true);

    }

    public void OnPointerUp(BaseEventData eventData)
    {
        skinDisplay.ShopDisplayDeactivate();
        skinDisplay.DeactivateIndicator();
        //uiElementToToggle.SetActive(false);

    }
}
