using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    private GameManager _gameManager;
    private GameTutorial _gameTutorial;

    // Start is called before the first frame update
    void Start()
    {
        Vibration.Init();
        _gameManager = FindObjectOfType<GameManager>();
        _gameTutorial = FindObjectOfType<GameTutorial>();
    }

    // Update is called once per frame
    void Update()
    {      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();

        if (obstacle == null)
            return;

        //GetComponentInChildren<ParticleSystem>().GetComponent<Transform>().localScale = GetComponentInParent<Transform>().localScale;
        GetComponentInChildren<ParticleSystem>().Play();
        AudioManager.Instance.PlaySFX("Hit", 0f);
        CameraShake.Shake(0.15f, 0.5f);

        Debug.Log("OBSTACLE COLLIDEDZZ WITH PLAYER!");
        //_gameManager.PrepareGameOver();

        Vibration.VibratePeek();
        _gameManager.TickStaminaDamage(0.45f);
    }


}
