using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaFillDelayUI : MonoBehaviour
{
    private Image _staminaFillDelay;

    private float _stamina;

    public float fillSmoothness; 
    // Start is called before the first frame update
    void Start()
    {
        _staminaFillDelay = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float prevFill = _staminaFillDelay.fillAmount;
        float currFill = _stamina;
        if (currFill > prevFill)
            prevFill = currFill;
        else if (currFill < prevFill)
        {
            Debug.Log("STAMINA DELAYING");
            prevFill = Mathf.Max(prevFill - fillSmoothness, currFill);
        }
        _staminaFillDelay.fillAmount = prevFill;
    }

    public void GetStamina(float stamina)
    {
        _stamina = stamina;
    }

}
