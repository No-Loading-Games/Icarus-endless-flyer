using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HermesEndZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ENDZONE TriggerEnter Hermes");
        HermesEvent hermesEvent = collision.gameObject.GetComponent<HermesEvent>();
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (hermesEvent == null)
        {
            return;
        }

        if (!hermesEvent.IsJumping)
            return;

        Debug.Log("ENDZONE HIT END ZONE");

        gameManager.isSpeeding = false;
        hermesEvent.EndZoneHit();
        //this.gameObject.SetActive(false);
    }
}
