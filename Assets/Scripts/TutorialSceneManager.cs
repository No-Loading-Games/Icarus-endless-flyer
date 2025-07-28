using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSceneManager : MonoBehaviour
{
    public float GlobalDownwardSpeed = 1.75f;
    public bool Tutorial = true;

    public bool CheckTutorial()
    {
        if (!PlayerPrefs.HasKey("tutorial"))
        {
            PlayerPrefs.SetInt("tutorial", 1);
            return true;
        }

        if (PlayerPrefs.GetInt("tutorial") == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetTutorial(bool tutorial)
    {
        PlayerPrefs.SetInt("tutorial", tutorial ? 1 : 0);
    }
}
