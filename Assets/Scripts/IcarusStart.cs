using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

enum IcarusState
{
    IDLE,
    WALKING,
    RUNNING,
    SLEEPING,
    JUMP_TO_SLING
}

public class IcarusStart : MonoBehaviour
{

    public bool gameStart = false;

    public ParticleSystem dust;
    public ParticleSystem speedTrail;

    //[SerializeField]
    //private float _launchDuration = 4;

    [SerializeField]
    private GameObject _speedTrailGO;
    [SerializeField]
    private bool _activateSpeedTrail;

    [SerializeField]
    private float _walkSpeed = 1f;

    [SerializeField]
    private float _runSpeed = 1.4f;

    [SerializeField]
    private Transform _swingTransform;

    [SerializeField]
    private GameObject _flyUI;

    [SerializeField]
    private GameObject _platform;

    [SerializeField]
    private GameObject _flockOfBirds;

    [SerializeField]
    private GameObject _pillars;

    [SerializeField]
    private GameObject _backPillar;

    [SerializeField]
    private GameObject _startObjects;

    [SerializeField]
    private GameObject _clouds;

    [SerializeField]
    private GameObject _mountains;

    [SerializeField]
    private GameObject _sun;
    [SerializeField]
    private GameObject _sunParent;
    [SerializeField]
    private GameObject _tutorialUI;
    [SerializeField]
    private GameTutorial _gameTutorial;

    [SerializeField]
    private GameObject _homeUI;
    /*
        [SerializeField]
        private GameObject _sunLight;*/

    [SerializeField]
    private GameObject _homeButtons;

    [SerializeField]
    private GameObject _headWear;
    [SerializeField]
    private Animator _headWearAnimator;

    [SerializeField]
    private GameObject _currencyUI;

    [SerializeField]
    private Animator _animator;
    private IcarusState _state;
    private Random _rand = new Random();
    private Camera _camera;
    private bool _doneState = false;
    private int _direction = 0;
    private bool _startGame = false;
    private GameManager _gameManager;

    [SerializeField]
    private CameraAutoZoom _camSettings;
    public event Action<bool> GameStartEvent;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        DOTween.Init();

        if (!PlayerPrefs.HasKey("done-tutorial") || PlayerPrefs.GetInt("done-tutorial") == 0)
        {
            //Do the following when TUTORIAL WAS NOT PLAYED yet.

            PlayerPrefs.SetInt("done-tutorial", 0);

            if (PlayerPrefs.GetInt("replay-tutorial") == 1)
                return;

            _startObjects.transform.position = new Vector2(_startObjects.transform.position.x, _startObjects.transform.position.y - 10f);

            _tutorialUI.SetActive(true);
            _homeUI.SetActive(false);
            return;

        }
        else
        {
            //Do the following when TUTORIAL WAS PLAYED

            _homeUI.SetActive(true);
            _tutorialUI.SetActive(false);
        }

    }

    public void Init()
    {
        _startGame = false;
        gameStart = false;
        _doneState = false;
        _state = IcarusState.WALKING;
        _camera = FindObjectOfType<Camera>();
        _gameManager = FindObjectOfType<GameManager>();

        //_camSettings = Camera.main.GetComponent<CameraAutoZoom>();

        InitializeHeadWear();

        Debug.Log("Screen Height: " + Screen.height);
        //Get the location of Sun relative to the screen height
        //float sunYPos = 7.41f * (_camSettings.referenceWidth / Screen.width * (Screen.height / _camSettings.referenceHeight));

        //_sunParent.transform.position = new Vector3(0, sunYPos, 0);
        
        CrossFadeAnimation("idle", 0.1f);
        _backPillar.GetComponent<Animator>().CrossFade("Back Pillar", 0.1f);
        GetComponent<TrailRenderer>().emitting = false;

        AudioManager.Instance.PlayMusic("Idle");
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("ICARUS UPDATE " + _state.ToString());
        HandleInput();
        ProcessIcarusState();
    }

    private void HandleInput()
    {
        if (!gameStart) //set gameStart = false whenever home button is pressed.
            return;

        _state = IcarusState.JUMP_TO_SLING;
    }

    private IEnumerator StartIdleCounter(float duration)
    {
        float time = 0f;
        while (time <= duration)
        {
            time += Time.deltaTime;
            yield return null;
        }

        //Debug.Log("State: " + _state + " is done after " + time + " seconds");
        _doneState = true;
    }

    public void ReturnToInitLocations()
    {
        ResetLocations();

        //Spawn FLY UI Button after spawning clouds
        _homeUI.SetActive(true);
        _flyUI.SetActive(true);
        FindObjectOfType<BirdBehaviour>().ResetPosition();

        _homeButtons.GetComponent<RectTransform>().anchoredPosition = new Vector2(-33, -124);
        _currencyUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -8);
    }

    public void ResetLocations()
    {

        Init();
        _startObjects.SetActive(true);
        _clouds.transform.DOKill();

        _clouds.transform.DOMoveY(-7.27f, 1f);
        _sun.transform.DOMoveY(7.41f, 1f);
        _pillars.transform.DOMoveY(-2.560884f, 1f);
        transform.position = new Vector2(2.108f, -3.920884f - 12f);
        transform.DOMoveY(-3.920884f, 1f);
        _platform.transform.DOMoveY(-5.239f, .75f);
    }

    public void IcarusJumpToSling()
    {
        CrossFadeAnimation("hop", 0.1f);

        GetComponent<SpriteRenderer>().sortingOrder = 5;
        _headWearAnimator.GetComponent<SpriteRenderer>().sortingOrder = 5;


        CreateDust();

        AudioManager.Instance.PlaySFX("Jumping", 0f);


        _flyUI.GetComponent<Animator>().CrossFade("fly button fly out", 0.1f);

        _homeButtons.transform.DOMoveX(_homeButtons.transform.localPosition.x + 2000f, 0.5f);
        _currencyUI.transform.DOMoveY(_currencyUI.transform.localPosition.y - 1f, 0.5f);

        _backPillar.GetComponent<Animator>().CrossFade("jump to sling", 0.1f);
        AudioManager.Instance.PlaySFX("Jump Finish", 1.2f);
        //startSFX.PlayDelayed(0.6f);
        transform.DOJump(new Vector2(0.2f, -3.90f), .45f, 1, 1.05f).OnComplete(() =>
        {
            IcarusWait(IcarusSwingBack);
        });

        _startGame = true;
    }

    private void IcarusWait(TweenCallback nextAction)
    {
        transform.DOMoveX(transform.position.x, 0.15f, false).OnComplete(nextAction);
    }

    private void IcarusSwingBack()
    {
        _backPillar.GetComponent<Animator>().CrossFade("swing back", 0.1f);
        CrossFadeAnimation("prep", 0.1f);

        //AudioManager.Instance.PlaySFX("Walking", 0f);
        AudioManager.Instance.PlaySFX("Winding", 0f);

        transform.localScale = new Vector2(2, 2);
        transform
            .DOMoveX(transform.position.x - .5f, .9f, false)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                AudioManager.Instance.sfxHandler.Stop();
                transform
                    .DOMoveX(transform.position.x, .8f, false)
                    .OnComplete(() =>
                    {
                        _backPillar.GetComponent<Animator>().CrossFade("launch", 0.1f);
                        CrossFadeAnimation("slinging", 0.1f);
                        IcarusWait(IcarusLaunch);
                    });
            });
    }

    private void IcarusLaunch()
    {
        Debug.Log("Icarus Launch!!");
        speedTrail.Play();
        _speedTrailGO.SetActive(_activateSpeedTrail);

        AudioManager.Instance.PlaySFX("Slinging", 0f);

        GetComponent<TrailRenderer>().emitting = true;

        #region For Game Trailer
        //Only for game trailer
        /*transform.DOMoveX(0.13f, _launchDuration/2).OnComplete(() =>
        {
            _gameManager.SpawnCloudsTransition("Cloud Start");
        });

        _mountains.transform.DOMoveY(-5.27f, _launchDuration/2);
        _sun.transform.DOMoveY(4.2f, _launchDuration);
        _clouds.transform.DOMoveY(-4.38f, (_launchDuration/2) + 3);
        transform.DOMoveY(0, _launchDuration).OnComplete(() =>
        {
            GetComponent<TrailRenderer>().emitting = false;
            _speedTrailGO.SetActive(false);
            StartCallback();
        });*/
        #endregion

        transform.DOMoveX(0.13f, 2);

        _mountains.transform.DOMoveY(-5.27f, 2);
        _sun.transform.DOMoveY(4.2f, 2);
        _clouds.transform.DOMoveY(-4.38f, 5);
        _gameManager.SpawnCloudsTransition("Cloud Start",1);
        transform.DOMoveY(0, 2).OnComplete(() =>
        {
            GetComponent<TrailRenderer>().emitting = false;
            _speedTrailGO.SetActive(false);
            StartCallback();
        });

        AudioManager.Instance.PlaySFX("Launching", 0f);

        _platform.transform.DOMoveY(_platform.transform.position.y - 12f, 0.5f);
        _pillars.transform.DOMoveY(_pillars.transform.position.y - 12f, 0.5f);
        _flockOfBirds.transform.DOMoveY(_pillars.transform.position.y - 12f, 0.5f);
    }

    private void StartCallback()
    {
        Debug.Log("CALLBACK START!");
        _startObjects.SetActive(false);


        _gameManager.IcarusSpawnPoint = this.transform.position;

        if(_gameTutorial.debugTutorial)
        {
            //_gameManager.Tutorial = true;
            _gameManager.SetTutorial(true);
            _gameTutorial.StartTutorial(_gameManager.IcarusSpawnPoint, 1);
            _gameTutorial.debugTutorial = false;
            return;
        }

        if (_gameManager.CheckTutorial())
        {
            Debug.Log("CALLED TUTORIAL");
            _gameManager.Tutorial = true;
            _gameTutorial.StartTutorial(_gameManager.IcarusSpawnPoint, 1);
        }
        else
        {
            _gameManager.StartGame();
        }

        //Temporary
        //Temporary call to tutorial 
        

    }

    private void IcarusWalk()
    {
        //_animator.CrossFade("walk", 0.2f);
        //AudioManager.Instance.PlaySFX("Walking", 0);

        if (_direction == 0)
        {

            if (_camera.WorldToScreenPoint(transform.position).x <= (Screen.width * 0.2f))
            {

                _doneState = true;
                _direction = 1;
                return;
            }
            _doneState = false;
            transform.localScale = new Vector2(2, 2);
            transform.Translate(Vector2.left * _walkSpeed * Time.deltaTime);
        }
        else if (_direction == 1)
        {
            if (_camera.WorldToScreenPoint(transform.position).x >= (Screen.width * 0.8f))
            {

                _doneState = true;
                _direction = 0;

                return;
            }
            _doneState = false;
            transform.localScale = new Vector2(-2, 2);
            //transform.localScale = new Vector2(-1, 1);
            transform.Translate(Vector2.right * _walkSpeed * Time.deltaTime);
        }
    }

    private void ProcessIcarusState()
    {
        //Debug.Log("ICARUS START GAME " + _startGame);
        //Debug.Log("ICARUS DONE STATE " + _doneState);

        if (_startGame)
            return;

        if (_doneState)
        {
            StopAllCoroutines();
            IcarusState state = (IcarusState)_rand.Next(0, 2);
            _state = state;
            _doneState = false;
            return;
        }

        //Debug.Log("ICARUS STATE " + _state);

        switch (_state)
        {
            case IcarusState.IDLE:
                _doneState = false;
                StartCoroutine(StartIdleCounter((float)(_rand.NextDouble() + _rand.Next(2, 5))));
                //_animator.CrossFade("idle", 0.1f);
                PlayAnimation("idle");
                break;
            case IcarusState.WALKING:
                //_animator.CrossFade("walk", .1f);
                PlayAnimation("walk");
                StartCoroutine(StartIdleCounter((float)(_rand.NextDouble() + _rand.Next(3, 6))));
                IcarusWalk();
                break;
            case IcarusState.JUMP_TO_SLING:
                //Debug.Log("STATE JUMP TO SLING");
                float x = Mathf.Sign(transform.localPosition.x - 0f) * Mathf.Sign(transform.localScale.x);
                transform.localScale = new Vector2(x * (transform.localScale.x), 2);
                GameStartEvent?.Invoke(gameStart);
                IcarusJumpToSling();
                break;
        }
    }

    private void CreateDust()
    {
        dust.Play();
    }

    public void PlayWalkSFX()
    {
        AudioManager.Instance.PlaySFX("Walking", 0.9f,1.1f);
    }

    private void CrossFadeAnimation(string animClipName, float transDuration)
    {
        _animator.CrossFade(animClipName, transDuration);

        if(HeadWearManager.Instance.HeadWearIsEquipped)
            _headWearAnimator.CrossFade(animClipName, transDuration);
    }
    private void PlayAnimation(string animClipName)
    {
        _animator.Play(animClipName);

        if(HeadWearManager.Instance.HeadWearIsEquipped)
            _headWearAnimator.Play(animClipName);
    }

    private void InitializeHeadWear()
    {
        _headWear.SetActive(HeadWearManager.Instance.HeadWearIsEquipped);
    }

    public void ResetIcarusState(int state)
    {
        _state = (IcarusState)state;
    }
}
