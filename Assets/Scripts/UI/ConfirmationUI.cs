using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ConfirmationUI : MonoBehaviour
{
    [SerializeField]
    private bool _confirm;

    public bool Confirmed
    {
        get { return _confirm; }
    }

    [SerializeField]
    private TMP_Text _popUpDescription;

    [SerializeField]
    public event Action<bool> ConfirmPurchaseEvent;

    public UnityEvent Confirm = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeDescription(string description)
    {
        _popUpDescription.text = description;
    }

    public void Yes()
    {
        AudioManager.Instance.PlaySFX("UI Click", 0f);

        _confirm = true;
        ConfirmPurchaseEvent?.Invoke(_confirm);

        //Debug.Log("CONFIRMED YES");
        gameObject.SetActive(false);
    }

    public void No()
    {
        AudioManager.Instance.PlaySFX("UI Click", 0f);

        _confirm = false;
        ConfirmPurchaseEvent?.Invoke(_confirm);

        //Debug.Log("CONFIRMED NO");
        gameObject.SetActive(false);
    }    

    public bool CheckIfYes()
    {
        return _confirm;
    }

    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
