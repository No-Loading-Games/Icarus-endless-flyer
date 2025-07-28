using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupCollision : MonoBehaviour
{
    [SerializeField]
    private PowerupManager _powerupManager;
    private HeartSpawner _heartSpawner;
    private HeartManager _heartManager;
    private GameManager _gameManager;


    // Start is called before the first frame update
    void Start()
    {
        Vibration.Init();
        _heartSpawner = FindObjectOfType<HeartSpawner>();
        _heartManager = FindObjectOfType<HeartManager>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Heart heart = collision.GetComponent<Heart>();

        if (heart != null)
        {
            Vibration.VibratePop();

            _heartManager.AddHearts(1);
            _powerupManager.GetComponent<PlayerController>().AddHeartVFX();

            Destroy(heart.gameObject);

            AudioManager.Instance.PlaySFX("Power Up Pick Up", 0f);
            AudioManager.Instance.PlaySFX("Power Up Apply 2", 0.2f);
            return;
        }


        Powerup powerup = collision.GetComponent<Powerup>();

        if (powerup == null)
        {
            return;
        }

        //If Feather Powerup or Midas Powerup
        if(powerup.GetComponent<FeatherPowerup>() != null || powerup.GetComponent<MidasPowerup>() != null)
        {
            Vibration.VibratePop();

            powerup.GetComponent<SpriteRenderer>().enabled = false;
            powerup.GetComponent<BoxCollider2D>().enabled = false;
            powerup.ApplyPowerup();

            AudioManager.Instance.PlaySFX("Power Up Pick Up", 0f);
            AudioManager.Instance.PlaySFX("Power Up Apply 2", 0.2f);
            //AudioManager.Instance.PlaySFX("Gauge Up", 0.2f);
            return;
        }

        //else if there is still powerup 
        if(_powerupManager.CurrentPowerup != null) 
        {
          
            Debug.Log(_powerupManager.CurrentPowerup);
            Debug.Log(powerup);
            Debug.Log("ThERE is sitll powerup");
            return;
        }


        Debug.Log(powerup);
        Debug.Log(_powerupManager.CurrentPowerup);
        Debug.Log("NO POWERUP ACTIVATE THE PICKUP"); 
        powerup.GetComponent<SpriteRenderer>().enabled = false;
        powerup.GetComponent<BoxCollider2D>().enabled = false;

        Debug.Log("PICKUP POWERUP! " + powerup.name);

        AudioManager.Instance.PlaySFX("Power Up Pick Up", 0f);
        AudioManager.Instance.PlaySFX("Power Up Apply", 0.2f);

        StopCoroutine(_gameManager.DelayPlayerCollisionActivate());
        _powerupManager.CurrentPowerup = powerup;

        Vibration.VibratePop();
        StartCoroutine(ActivatePickUpNotif(powerup));
    }

    private IEnumerator ActivatePickUpNotif(Powerup powerup)
    {
        powerup.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        powerup.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        float rotation = 0f;
        Debug.Log("pick up notif rotation: " + rotation);

        GameObject notif = Instantiate(_powerupManager.pickUpNotifVFX, transform.position, Quaternion.Euler(0f, 0f, rotation), powerup.transform); 

        notif.GetComponent<Animator>().CrossFade("power-up notif", 0.1f);

        yield return new WaitForSeconds(_powerupManager.pickUpNotifAnim.length);

        Destroy(notif);
        Debug.Log("notif destroyed");
    }
}
