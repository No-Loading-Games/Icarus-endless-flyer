using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCollisions : MonoBehaviour
{
    private bool _playerCollided = false;

    public event Action<bool> FireUIEvent;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TickStaminaDown()
    {
        float time = 0f;

        while (time <= 0.75f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        _gameManager.AddStamina(-0.10f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Sun sun = collision.gameObject.GetComponent<Sun>();
        PlayerController player = GetComponentInParent<PlayerController>();

        Debug.Log("TRIGGER SUN");

        if (sun == null)
            return;

        if (_playerCollided)
            return;

        if (_gameManager.Tutorial)
            return;

        Debug.Log("Entered Sun!");

        player.GetComponent<SpriteRenderer>().material = player.burningMaterial;

        GetComponentInChildren<ParticleSystem>().Play();
        FireUIEvent?.Invoke(true);

        _gameManager.IsInSun = true;
        _playerCollided = true;

        //StartCoroutine(CheckIfBurning(_playerCollided));
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        Sun sun = collision.gameObject.GetComponent<Sun>();
        PlayerController player = GetComponentInParent<PlayerController>();

        if (sun == null)
            return;

        //AudioManager.Instance.PlayBurningSFX(0);

        //StartCoroutine(CheckIfBurning());

        //StopCoroutine(TickStaminaDown());
        player.GetComponent<SpriteRenderer>().material = player.defaultMaterial;
        GetComponentInChildren<ParticleSystem>().Stop();
        _gameManager.IsInSun = false;
        FireUIEvent?.Invoke(false);


        Debug.Log("Exit Sun!");
        _playerCollided = false;

        //StartCoroutine(CheckIfBurning(_playerCollided));

    }

    private IEnumerator CheckIfBurning()
    {
        yield return new WaitForSeconds(2);

        if (AudioManager.Instance.burningSfxHandler.isPlaying)
        {
            AudioManager.Instance.burningSfxHandler.Stop();
        }

        Debug.Log("Burning SFX Stopped");
        StopAllCoroutines();
    }

}
