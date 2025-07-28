using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartManager : MonoBehaviour
{

    [SerializeField]
    private int _initHeartsCost = 1;

    public int GetHearts()
    {
        if(!PlayerPrefs.HasKey("hearts"))
        {
            PlayerPrefs.SetInt("hearts", 3);
            return 3;
        }


        return PlayerPrefs.GetInt("hearts");
    }

    public void SetHearts(int amount)
    {
        if(PlayerPrefs.GetInt("hearts") == 3)
        {
            return;
        }
        PlayerPrefs.SetInt("hearts", amount);
        SharedUI.Instance.UpdateHeartUIText();
    }

    public void AddHearts(int amount)
    {
        int currentHearts = PlayerPrefs.GetInt("hearts");
        
        PlayerPrefs.SetInt("hearts", currentHearts + amount);

        SharedUI.Instance.UpdateHeartUIText();
    }

    public void ConsumeHeart(int amount)
    {
        int currentAmount = PlayerPrefs.GetInt("hearts");
        PlayerPrefs.SetInt("hearts", currentAmount - amount);
        SharedUI.Instance.UpdateHeartUIText();
    }

    public int GetHeartsCost()
    {
        return _initHeartsCost;
    }

}
