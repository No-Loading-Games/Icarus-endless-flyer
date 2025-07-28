using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ZeusEvent : MonoBehaviour
{
    [SerializeField]
    private GameObject _icarus;
    [SerializeField]
    private GameObject _bolt;
    [SerializeField]
    private ParticleSystem _lightning;


    private Animator _boltAnim;

    private PowerupManager _powerupManager;

    private Random _rand = new Random();

    // Start is called before the first frame update
    void Start()
    {
        _powerupManager = GetComponentInParent<PowerupManager>();
        _boltAnim = _bolt.GetComponent<Animator>();
        _boltAnim.SetLayerWeight(0, 1f);
    }

    private IEnumerator DelayDestruction(GameObject obstacle)
    {
        yield return new WaitForSeconds(0.4f);

        AudioManager.Instance.PlaySFX("Explosion", 0);
        obstacle.GetComponent<SpriteRenderer>().enabled = false;
        obstacle.GetComponent<PolygonCollider2D>().enabled = false;
        obstacle.GetComponent<Obstacle>().PlayExplosion();

        Vibration.Init();
        Vibration.VibratePeek();
        CameraShake.Shake(0.5f,0.7f);

        Debug.Log("Collider obstacle off");

        yield return new WaitForSeconds(10f);

        
        Destroy(obstacle);
        Debug.Log("Collider destroy");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // x Scale value for Bolt Prefab
        /*float x = -(Mathf.Sign(_powerupManager.transform.position.x)) * (_powerupManager.transform.localScale.x/Mathf.Abs(_powerupManager.transform.localScale.x));
        Debug.Log("BOLT X IS (player pos x is " + _powerupManager.transform.position.x +" )-" + (Mathf.Sign(_powerupManager.transform.position.x)) + " * " + (_powerupManager.transform.localScale.x / Mathf.Abs(_powerupManager.transform.localScale.x)));*/
        
        // x Scale value for Bolt Particle System
        float x = -(Mathf.Sign(_powerupManager.transform.position.x));
        Debug.Log("BOLT X IS (player pos x is " + _powerupManager.transform.position.x + " )-" + (Mathf.Sign(_powerupManager.transform.position.x)));

        Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();  
        if(obstacle == null)
        {
            return;
        }

        Debug.Log("LIGHTNING");
        //_bolt.transform.localScale = new Vector3(xScale[_rand.Next(2)], 1f, 1f);
        /*_bolt.transform.localScale = new Vector3(x, 1f, 1f);
        _boltAnim.SetTrigger("lightningTrigger");
        Debug.Log("BOLT SCALE IS: " + _bolt.transform.localScale);*/

        AudioManager.Instance.PlaySFX("Lightning", 0);
        _lightning.transform.localScale = new Vector3(x, 1f, 1f);
        _lightning.Play();
        StartCoroutine(DelayDestruction(obstacle.gameObject));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();

        if (obstacle == null)
            return;

        //_boltAnim.CrossFade("idle", 0.1f);
    }
}