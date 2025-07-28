using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidasEvent : MonoBehaviour
{

    private PowerupManager _powerupManager;

    // Start is called before the first frame update
    void Start()
    {
        _powerupManager = FindObjectOfType<PowerupManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
        if (obstacle == null)
        {
            return;
        }
        FindObjectOfType<GameManager>().midasIsOn = false;
        FindObjectOfType<GameManager>().MidasPowerUpUIDisplay();

        Debug.Log("MIDAS OBSTACLE");
        obstacle.Midas();

        //FindObjectOfType<GameManager>().DespawnPowerUpUI();
       

    }
}
