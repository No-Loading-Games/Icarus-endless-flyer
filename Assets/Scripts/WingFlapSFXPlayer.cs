using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingFlapSFXPlayer : MonoBehaviour
{
    [SerializeField]
    private AnimationClip[] flyingAnimationClip;

    [SerializeField]
    private AnimationEvent playWingSFX;

    private void Start()
    {
        playWingSFX = new AnimationEvent();
        playWingSFX.functionName = "PlayWingFlap";
        playWingSFX.time = 0;
    }


    public void AdjustWingFlapSFX(int currentLayerIndex)
    {
        int x = 0;

        while(x < flyingAnimationClip.Length)
        {
            if (x == currentLayerIndex)
            {
                flyingAnimationClip[x].events = null;
                flyingAnimationClip[x].AddEvent(playWingSFX);
            }
            else
                flyingAnimationClip[x].events = null;

            x++;
        }
    }

    public void PlayWingFlap()
    {
        AudioManager.Instance.PlayWingFlapSFX();
    }

}
