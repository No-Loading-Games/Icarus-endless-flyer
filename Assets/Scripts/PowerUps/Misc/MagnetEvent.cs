using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetEvent : MonoBehaviour
{

    [SerializeField]
    private GameObject _icarus;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Coin coin = collision.gameObject.GetComponent<Coin>();

        if ( coin == null)
            return;

        Debug.Log("Coin Collided!");
        coin.ActivateCoinMagnet(_icarus);
    }

}
