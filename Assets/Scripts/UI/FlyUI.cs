using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyUI : MonoBehaviour
{
    [SerializeField]
    private IcarusStart _icarusStart;

    public void StartGame()
    {
        Vibration.Init();
        Vibration.VibratePop();

        _icarusStart.gameStart = true;
    }

}
