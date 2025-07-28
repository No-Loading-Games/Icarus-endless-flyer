using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DiveManager : MonoBehaviour
{
    public float diveConstant = 0.65f;

    //For Debugging
    /*public Slider diveSlider;
    public int tapCounts;*/

    private Touch inputTouch;
    private float touchPosition = 0f;

    private bool _onHold = false;

    private float _diveValue = 0;
    private float _diveTime = 0;


    [SerializeField]
    private PlayerController _controller;

    void Update()
    {
        CheckInput();
        if (!_controller.DoubleTapped)
            DecreaseValue();

        //tapCounts = inputTouch.tapCount;
        //diveSlider.value = _diveValue;
    }

    private void CheckInput()
    {
        if (Input.touches.Length == 0)
        {
            _onHold = false;
            //_diveIndex = 0;
            _controller.DoubleTapped = false;
            return;
        }

        inputTouch = Input.touches[0];

        if (inputTouch.phase != TouchPhase.Ended || inputTouch.phase != TouchPhase.Canceled)
        {
            touchPosition = inputTouch.position.x;

            if(!_onHold && inputTouch.tapCount <= 2)
            {
                _diveValue += diveConstant;

                _onHold = true;


            }

            Debug.Log("TAPPIES = " + inputTouch.tapCount + " Dash value is " + _diveValue + " on hold? " + _onHold);
            //increase the slider value, but slider value is constantly decreasing unless its value is 1
           
            if (_diveValue >= 1)
            {
                _diveValue = 0;

                if (inputTouch.tapCount == 2)
                {
                    _controller.DoubleTapped = true;

                    _onHold = true;
                }

                return;
            }

            

        }

        if (inputTouch.phase == TouchPhase.Ended)
        {
            //Check first the distance travelled, if the MINIMUM distance is not yet travelled, LOCK INPUT then continue moving the player UNTIL MINIMUM distance reached
            /*if (_controller.DoubleTapped)
            {
                _controller._isSnapping = true;
                _controller.CheckNearestDiveDistance();
            }*/

            _controller.DoubleTapped = false;
            _onHold = false;
            
        }


        Debug.Log("double tapper? " + _controller.DoubleTapped);
    }

    private void DecreaseValue()
    {
        if (_diveValue <= 0)
        {
            _diveValue = 0;
            return;
        }

        _diveValue -= Time.deltaTime;
    }


}
