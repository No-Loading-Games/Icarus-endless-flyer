using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PowerupSpawn : MonoBehaviour
{
    [SerializeField]
    protected Powerup _powerup;

    protected Animator _animator;
    protected Obstacle _obstacle;
    protected List<float> _notifRotation;
    protected PowerupManager _powerupManager;
    protected Random _rand = new Random();
    protected float _downwardSpeed = 1f;

    // Start is called before the first frame update
    protected virtual void Start() { }

    // Update is called once per frame
    protected virtual void Update() { }

    public virtual void InitPowerup() { }

    public void SetDownwardSpeed(float speed)
    {
        _downwardSpeed = speed;
    }

    public void SetObstacle(Obstacle obstacle)
    {
        _obstacle = obstacle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Powerup Hit");
        if (collision.gameObject.tag != "Player")
            return;

        //_playerController.PowerUpPickedVFX();


        Debug.Log("Player Hit");
        _powerupManager = collision.gameObject.GetComponentInParent<PowerupManager>();
        _notifRotation = _powerupManager.pickUpNotifRotation;
        _powerupManager.CurrentPowerup = _powerup;
        Debug.Log("");

        StartCoroutine(ActivatePickUpNotif());
    }

    private IEnumerator ActivatePickUpNotif()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        float rotation = _notifRotation[_rand.Next(_notifRotation.Count)];
        Debug.Log("pick up notif rotation: " + rotation);

        GameObject notif = Instantiate(
            _powerupManager.pickUpNotifVFX,
            transform.position,
            Quaternion.Euler(0f, 0f, rotation),
            this.transform
        );

        notif.GetComponent<Animator>().CrossFade("power-up notif", 0.1f);

        yield return new WaitForSeconds(_powerupManager.pickUpNotifAnim.length);

        Destroy(notif);
        Debug.Log("notif destroyed");
    }
}
