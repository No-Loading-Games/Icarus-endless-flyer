using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartItem : Item
{
    private HeartManager heartManager;

    [SerializeField]
    private string _pcNumber;

    void Start()
    {
        heartManager = FindObjectOfType<HeartManager>();

        InitializeItemData();
    }

    public void InitializeItemData()
    {
        name = _pcNumber + "x Daedalus' Trust";
    }

    private void Update()
    {
        numberOfPurchased = heartManager.GetHearts();
    }

}
