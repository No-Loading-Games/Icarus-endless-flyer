using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BirdBehaviour : MonoBehaviour
{
    Random _rand = new Random();

    private Animator _animator;
    private Rigidbody2D _rigidBody;

    [SerializeField]
    private IcarusStart _icarusStart;

    [SerializeField]
    private Transform _birdPlatform;

    private readonly int[] _idleStateIdentifier = { 0,0,0,0, 1, 2, 3 };
    private bool _gameStart;

    [SerializeField]
    private bool _isIdle;
    private float _idleTimer = 3f;

    // Start is called before the first frame update
    void OnEnable()
    {
        _gameStart = false;
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _icarusStart.GameStartEvent += FlyAway;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!_isIdle && !_gameStart)
        {
            _animator.CrossFade("flying", 0.1f);
            _rigidBody.velocity = (new Vector2(3.2972f, -1f)).normalized;
        }
        else if(_gameStart)
        {

            transform.Translate(2f * Time.fixedDeltaTime * (new Vector3(2.84f, -1.38f, 0f) - transform.position).normalized);
        }
        else
        {
            transform.Translate(new Vector3(0, 0, 0));
        }

    }

    public void ResetPosition()
    {
        transform.position = new Vector2(-3.17f, -1.46f);
        _rigidBody.velocity = (new Vector2(3.2972f, -1f)).normalized;
        _rigidBody.bodyType = RigidbodyType2D.Dynamic;
        _isIdle = false;
        _animator.CrossFade("flying", 0.1f);

        //_birdPlatform.transform.position = new Vector2(0.555f, -2.855f);

    }

    private void FlyAway(bool gameStart)
    {
        _isIdle = !gameStart;
        _gameStart = gameStart;
        transform.DOPause();
        StopAllCoroutines();
        //_animator.CrossFade("idle", 0.1f);
        _animator.SetBool("fly", true);
        _rigidBody.bodyType = RigidbodyType2D.Static;
        //Debug.Log("idle state : FLYING ");

    }

    private IEnumerator IdleStateHandler()
    {
        float totalTime = 0f;
        int idleState = _idleStateIdentifier[_rand.Next(6)]; ;

        transform.DOPause();
        //Debug.Log("idle state #: " + idleState);

        _animator.SetInteger("idleStateIdentifier", idleState);
        _idleTimer = _animator.GetCurrentAnimatorClipInfo(0).Length;
        while (totalTime <= _idleTimer)
        {
                totalTime += Time.deltaTime;
            yield return null;
        }

        StopAllCoroutines();
        StartCoroutine(IdleStateHandler());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision == null)
        {
            return;
        }

        _isIdle = true;
        _animator.CrossFade("idle", 0.1f);
        StartCoroutine(IdleStateHandler());
    }

    private IEnumerator SleepTimer()
    {
        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
    }
}
