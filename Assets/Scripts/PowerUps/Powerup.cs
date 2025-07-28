using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Timeline.Actions;
using UnityEngine;
using Random = System.Random;

public abstract class Powerup  : MonoBehaviour
{
    protected PowerupManager _powerupManager;
    protected GameManager _gameManager;
    protected Animator _animator;
    protected PlayerController _playerController;

    private List<float> _notifRotation;
    private Random _rand = new Random();
    //public event Action<float, float, Sprite, Color> PowerUpTimerUpdateEvent;
    //protected PowerUpTimerUI powerupTimerUI;

    protected virtual void Start()
    {
        _gameManager = FindObjectOfType<GameManager>(); 

        _animator = GetComponent<Animator>();
        _playerController = FindObjectOfType<PlayerController>();
        _powerupManager = FindObjectOfType<PowerupManager>();
    }


    protected virtual void Update()
    {
        if(!Obstacle.StopObstacle)
            transform.Translate(_gameManager.GlobalDownwardSpeed * Time.deltaTime * Vector2.down);
    }

    public abstract void HandlePowerup();
    public abstract void ApplyPowerup();
    public abstract void CleanupPowerup();


   /* protected void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Powerup Hit");
        if (collision.gameObject.tag != "Player")
            return;

        //_playerController.PowerUpPickedVFX();


        Debug.Log("Player Hit");
        _powerupManager = collision.gameObject.GetComponentInParent<PowerupManager>();
        _notifRotation = _powerupManager.pickUpNotifRotation;
        AudioManager.Instance.PlaySFX("Power Up Pick Up", 0f);
        AudioManager.Instance.PlaySFX("Power Up Apply", 0.2f);

        _powerupManager.CurrentPowerup = this;
        Debug.Log("");

        StartCoroutine(ActivatePickUpNotif());

    }

    private IEnumerator ActivatePickUpNotif()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        float rotation = _notifRotation[_rand.Next(_notifRotation.Count)];
        Debug.Log("pick up notif rotation: " + rotation);

        GameObject notif = Instantiate(_powerupManager.pickUpNotifVFX, transform.position, Quaternion.Euler(0f,0f,rotation), this.transform);;
        
        notif.GetComponent<Animator>().CrossFade("power-up notif", 0.1f);

        yield return new WaitForSeconds(_powerupManager.pickUpNotifAnim.length);

        Destroy(notif);
        Debug.Log("notif destroyed");
    }  */  
}
