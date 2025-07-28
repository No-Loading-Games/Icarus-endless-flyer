using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private GameManager _gameManager;

    private bool _isMagnet = false;
    private GameObject _playerTarget;

/*    [SerializeField]
    private GameObject _pickUpNotifVFX;

    [SerializeField]
    private AnimationClip _pickUpAnim;*/

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        GetComponent<Animator>().CrossFade("coin", 0.1f);
    }

    private void Update()
    {
        if (Obstacle.StopObstacle) return;

        if (_isMagnet)
            transform.Translate(5f * Time.deltaTime * (_playerTarget.transform.position - transform.position).normalized);
         else
            transform.Translate(_gameManager.GlobalDownwardSpeed * Time.deltaTime * Vector2.down);
    }


    public void ActivateCoinMagnet(GameObject playerTarget)
    {
        _playerTarget = playerTarget.GetComponentInParent<PlayerController>().gameObject;
        _isMagnet = true;/*
        transform.DOMoveX(_playerTarget.transform.position.x, 0.65f);
        transform.DOMoveY(_playerTarget.transform.position.y, 0.65f);*/

    }

}
