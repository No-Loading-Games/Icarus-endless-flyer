using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class FireUI : MonoBehaviour
{
    // Start is called before the first frame update
    private SunCollisions _sunCollisions;
/*
    private GameManager _gameManager;
    private bool _isPlaying;*/

    [SerializeField]
    private Volume _burningVignetteFX;

    public void SetUpReference(SunCollisions sunCollisions)
    {
        _sunCollisions = sunCollisions;
        _sunCollisions.FireUIEvent += HandleFireUIEvent;
    }

    private void HandleFireUIEvent(bool playFire)
    {
        if(playFire)
        {
            GetComponent<Image>().enabled = true;
            GetComponent<Animator>().CrossFade("fire start", 0.1f);
            GetComponent<Animator>().SetBool("despawn", false);
            _burningVignetteFX.GetComponent<Animator>().SetBool("isInSun", true);

            //Play fire sfx
        }
        else
        {
            //GetComponent<Animator>().CrossFade("none", 0.1f);
            GetComponent<Animator>().SetBool("despawn", true);
            _burningVignetteFX.GetComponent<Animator>().SetBool("isInSun", false);
            //GetComponent<Image>().enabled = false;

        }
            
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
