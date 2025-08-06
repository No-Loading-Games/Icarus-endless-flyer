using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool debugMode = false;

    #region Variables
    private const float TAP_TIME_THRESHOLD = 0.15f;

    public ParticleSystem featherFX;
    public ParticleSystem burstFeatherFX;
    public GameObject addVFX;

    public Material defaultMaterial;
    public Material burningMaterial;

    public AudioSource playerSFX;
    public AudioClip hitSFX;

    //public AudioSource powerUpPickedSFX;
    public AudioClip powerUpPickedSound;

    [SerializeField]
    private float upSpeed = 4f;

    [SerializeField]
    private float diveSpeed = 5f;

    [SerializeField]
    private float horizontalSpeed = 1f;

    [SerializeField]
    private float _doubleTapSpeed = 1.5f;

    [SerializeField]
    private GameObject _playerCollision;

    [SerializeField]
    private ParticleSystem _ashFalling;

    [SerializeField]
    private GameObject _sunColliderObject;

    [SerializeField]
    private TrailRenderer _dashTrail;

    public HeadWear _headWear;
    public Animator _headWearAnimator;
    public Animator _frontWingAnimator;

    //[SerializeField]
    //private AudioSource _fallingSFX;

    private float touchPosition = 0f;
    private Touch inputTouch;
    private bool _tapped = false;
    private bool _doubleTapped = false;
    private Rigidbody2D _rb2D;
    private GameManager _gameManager;
    private Animator _animator;
    private float _timeLastTap = 0f;
    private bool _lockInput = false;
    private PowerupManager _powerupManager;

    private float _diveOrigin = 0f;
    private int _diveIndex = 1;
    private float[] _diveDistances = { 1f, 2f, 3f};
    [SerializeField]
    private float _diveDistance = 1.4f;

    public HashSet<string> disableReasons = new HashSet<string>();

    private Vector2 _targetPosition;
    public bool _isHit = false;

    public PlayerCollision PlayerCollision { get { return _playerCollision.GetComponent<PlayerCollision>(); } }
    public ParticleSystem AshFalling { get { return _ashFalling; } }
    public bool DoubleTapped { get { return _doubleTapped; } set { _doubleTapped = value; } }
    public int DiveDistanceIndex { get { return _diveIndex; } set { _diveIndex = value; } }

    #endregion

    void Start()
    {
        Vibration.Init();

        _rb2D = GetComponent<Rigidbody2D>();
        _gameManager = FindObjectOfType<GameManager>();
        _animator = GetComponent<Animator>();
        _powerupManager = GetComponent<PowerupManager>();
        _headWearAnimator = _headWear.GetComponent<Animator>();

        InitializeHeadWear();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfDebugMode();

        if ((transform.position.y >= 3.6f)) //previous value = 4.725f
        {
            if(_gameManager.Tutorial)
            {
                transform.position = new Vector2(transform.position.x, 3.59f);
                return;
            }

            Debug.Log("REACHED HIGH EDGE");
            _gameManager.isBurnt = true;

            _gameManager.PrepareGameOver();
            return;
        }

        HandleInputCopy();
        HandleDownwardMovement();
    }

    public void SetUpSpeed(float speed)
    {
        upSpeed = speed;
    }

    public void LockInput(bool lockInput)
    {
        _lockInput = lockInput;
    }

    public void EnableCollision(bool enable)
    {
        if (_powerupManager.CurrentPowerup == null)
            enable = false;
        _playerCollision.GetComponent<PolygonCollider2D>().enabled = enable;
    }

    public void AddDisableReason(string reason)
    {
        disableReasons.Add(reason);
        UpdateCollider();
    }

    public void RemoveDisableReason(string reason)
    {
        disableReasons.Remove(reason);
        UpdateCollider();
    }

    public void UpdateCollider()
    {
        PolygonCollider2D col = _playerCollision.GetComponent<PolygonCollider2D>();
        col.enabled = disableReasons.Count == 0; //collider is enabled if there are no disableReasons
        Debug.Log("Collider Enabled? " + col.enabled + " | Reasons: " + string.Join(",", disableReasons));
    }

    private void HandleInput()
    {
        SetAnimatorBool("dive", false);
        if (_powerupManager.CurrentPowerup != null)
            _powerupManager.powerUpAnimator.SetBool("diving", false);

        _dashTrail.emitting = false;


        if (_lockInput)
            return;

        if (Input.touches.Length == 0)
        {
            _tapped = false;
            _doubleTapped = false;
            return;
        }

        inputTouch = Input.touches[0];

        if (inputTouch.phase != TouchPhase.Ended || inputTouch.phase != TouchPhase.Canceled)
        {
            touchPosition = inputTouch.position.x;

            //Debug.Log("TAPPIES = " + inputTouch.tapCount);

            if (inputTouch.tapCount == 1)
            {
                _timeLastTap = Time.time;

                _doubleTapped = false;

            }
            else if (inputTouch.tapCount == 2)
            {
                if (Time.time - _timeLastTap <= 0.275f)
                {
                    _doubleTapped = true;
                    //Debug.Log("DOUBLE TAPPED " + (Time.time - _timeLastTap));

                    SetAnimatorBool("dive", true);

                    _dashTrail.emitting = true;

                    if (_powerupManager.CurrentPowerup != null)
                    {
                        _powerupManager.powerUpAnimator.SetBool("diving", _doubleTapped);
                        //Debug.Log("POWER UP DIVING");
                    }
                    //Debug.Log("POWER UP DIVING??" + _doubleTapped);

                    //StartCoroutine(DashTimer());
                }
                else
                {
                    SetAnimatorBool("dive", false);
                    _doubleTapped = false;


                    if (_powerupManager.CurrentPowerup != null)
                        _powerupManager.powerUpAnimator.SetBool("diving", _doubleTapped);
                    //Debug.Log("POWER UP DIVING??" + _doubleTapped);
                }
            }
            else
            {
            }

        }
        else if (inputTouch.phase == TouchPhase.Ended)
        {
            //StopCoroutine(DashTimer());
            _tapped = false;
            _doubleTapped = false;
            //_onHold = false;
            return;
        }


        AudioManager.Instance.PlayDiveSFX(_doubleTapped);
        //StopCoroutine(DashTimer());
        _tapped = true;
    }

    private void HandleInputCopy()
    {
        SetAnimatorBool("dive", false);
        if (_powerupManager.CurrentPowerup != null)
            _powerupManager.powerUpAnimator.SetBool("diving", false);

        _dashTrail.emitting = false;


        if (_lockInput)
            return;

        if (Input.touches.Length == 0)
        {
            _tapped = false;
            return;
        }

        inputTouch = Input.touches[0];

        if (inputTouch.phase != TouchPhase.Ended || inputTouch.phase != TouchPhase.Canceled)
        {
            Vector3 screenPos = new Vector3(inputTouch.position.x, inputTouch.position.y, 0);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            touchPosition = screenPos.x;

        }

        AudioManager.Instance.PlayDiveSFX(_doubleTapped);
        SetAnimatorBool("dive", _doubleTapped);
        _dashTrail.emitting = _doubleTapped;

        if (_powerupManager.CurrentPowerup != null)
        {
            _powerupManager.powerUpAnimator.SetBool("diving", _doubleTapped);
            //StopCoroutine(DashTimer());
        }
        
        _tapped = true;

    }

    private void HandleDownwardMovement()
    {
        if (_lockInput && !_doubleTapped)
            return;

        if (!_tapped)
        {
            _rb2D.MovePosition(_rb2D.position + (Vector2.up * upSpeed) * Time.deltaTime);
            return;
        }

        Debug.Log("Lock input is " + _lockInput + " and tapped is " + _tapped);

        /*float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        Debug.Log("CAM WIDTH = " + camWidth);*/
        if (touchPosition < Screen.width / 2 || Input.GetKey(KeyCode.A))
        {
            MoveDown(Vector2.left);
        }
        else if (touchPosition > Screen.width / 2 || Input.GetKey(KeyCode.D))
        {
            MoveDown(Vector2.right);
        }
        else
        {
            MoveDown(Vector2.right);
        }

        //If controls depended on Camera instead of Screen touch position
        /*if (touchPosition < camWidth/2|| Input.GetKey(KeyCode.A))
        {
            MoveDown(Vector2.left);
        }
        else if (touchPosition > camWidth / 2 || Input.GetKey(KeyCode.D))
        {
            MoveDown(Vector2.right);
        }
        else
        {
            MoveDown(Vector2.right);
        }*/
    }

    //DoDash function will use MovePosition to move _rb2D to a specific distance depending on how long the player hold the dash

    public void CheckNearestDiveDistance()
    {
        float currentX = transform.position.x;

        _targetPosition = new Vector2(_diveDistances.OrderBy(x => Mathf.Abs(currentX - x)).First(), transform.position.y);
    }

    private void MoveDown(Vector2 direction)
    {
        if (_isHit)
        {
            _rb2D.velocity = Vector2.zero;
            _rb2D.simulated = false;
            Debug.Log("ICARUS IS HIT");
            return;
        }
        _rb2D.simulated = true;

        float doubleSpeed = _doubleTapped ? _doubleTapSpeed * 1.85f : 1f;

        #region ScreenDependentConstraints
        //Check if the player reaches the edges of the screen
        /*if (direction.x > 0 && (transform.position.x >= Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x - 0.25f))
        {
            Debug.Log("REACHED RIGHT EDGE");
            return;
        }

        if (direction.x < 0 && (transform.position.x <= Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x + 0.25f))
        {
            Debug.Log("REACHED LEFT EDGE");
            return;
        }

        if ((transform.position.y <= Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y + 0.25f)) //previous value = +1f
        {
            Debug.Log("REACHED LOW EDGE");
            return;
        }*/
        #endregion

        //Check if the player is on the edge of the camera
        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        float leftEdge = cam.transform.position.x - camWidth / 2f;
        float rightEdge = cam.transform.position.x + camWidth / 2f;
        float bottomEdge = cam.transform.position.y - camHeight / 2f;

        if (direction.x > 0 && (transform.position.x >= rightEdge - 0.25f))
        {
            Debug.Log("REACHED RIGHT EDGE");
            return;
        }

        if (direction.x < 0 && (transform.position.x <= leftEdge + 0.25f))
        {
            Debug.Log("REACHED LEFT EDGE");
            return;
        }

        if ((transform.position.y <= bottomEdge + 0.25f)) //previous value = +1f
        {
            Debug.Log("REACHED LOW EDGE");
            return;
        }

        if (_doubleTapped && _diveOrigin == 0)
        {
            _diveOrigin = transform.position.x;
        }


        //Debug.Log("Double Speed = " + doubleSpeed + " with Direction = " + direction);

        _rb2D.MovePosition(_rb2D.position + (direction * horizontalSpeed * doubleSpeed + Vector2.down * diveSpeed * doubleSpeed) * Time.deltaTime);

        /*Vector2 newPos = Vector2.MoveTowards(_rb2D.position, _rb2D.position + (direction + Vector2.down), diveSpeed * doubleSpeed * Time.fixedDeltaTime);
        _rb2D.MovePosition(newPos);*/

        transform.localScale = new Vector2(-2 * -direction.normalized.x, transform.localScale.y);

        if (_doubleTapped && (Mathf.Abs(transform.position.x - _diveOrigin) >= _diveDistance))
        {
            _doubleTapped = false;
            _diveOrigin = 0;
            Debug.Log("DIVE FINISHED. Distance traveled: " + Mathf.Abs(transform.position.x - _diveOrigin));

        }
        // x1 = 0.468, y1 = 0.533 ; x2 = -0.62, y2 = -0.307

        //transform.localScale = new Vector2(-1 * -direction.normalized.x, transform.localScale.y);
    }

    public void FallDown()
    {
        _gameManager.StopAllCoroutines();
        _gameManager.StopStaminaTick();
        _gameManager.StopScoreTick();
        _rb2D.velocity = Vector2.zero;
        _rb2D.simulated = false;
        //playerSFX.PlayDelayed(0.5f);


        GetComponentInChildren<SunCollisions>().gameObject.SetActive(false);
        SetAnimatorBool("alive", false);
        PlayAnimation("falling-prep", 0.1f, _gameManager.HandleFeatherCheckPure());
        GetComponent<SpriteRenderer>().sortingOrder = 20;

        long[] vibratePattern = {0,200,200,200,200,800 };

        Vibration.Vibrate();

        if (_powerupManager.CurrentPowerup != null)
            _powerupManager.CurrentPowerup.StopAllCoroutines();

        if (_gameManager.isBurnt)
        {
            _ashFalling.Play();

            //AudioManager.Instance.PlaySFX("Game Over SFX", 0);
            transform
                .DOMoveY(transform.position.y - .2f, 0.4f)
                .OnComplete(() =>
                {
                    _animator.enabled = false;
                    GetComponent<SpriteRenderer>().enabled = false;
                    transform
                        .DOMoveY(transform.position.y - 10f, 1f)
                        .OnComplete(() => _gameManager.GameOver());
                });
        }
        else
        {
            AudioManager.Instance.PlaySFX("Falling", 2.4f);

            //AudioManager.Instance.PlaySFX("Game Over SFX", 0);
            transform
                .DOMoveY(transform.position.y + 0.75f, .5f)
                .OnComplete(() =>
                {
                    transform
                        .DOMoveY(transform.position.y - 10f, 1.65f)
                        .OnComplete(() =>
                        {
                            _gameManager.GameOver();
                           
                        });   

                });
        }
    }

    public void ResetIcarus()
    {
        _gameManager.StopAllCoroutines();
        _gameManager.StopStaminaTick();
        _gameManager.StopScoreTick();
        transform.position = new Vector2(0.13f, 0);
        //_gameManager.AddStamina(1);
        _animator.enabled = true;
        _headWearAnimator.enabled = true;
        _animator.speed = 1;
        _headWearAnimator.speed = 1;
        _sunColliderObject.SetActive(true);

        SetAnimatorBool("alive", true);
        PlayAnimation("flying", 0.1f, _gameManager.HandleFeatherCheckPure());
        GetComponent<SpriteRenderer>().enabled = true;

        GetComponent<Rigidbody2D>().simulated = true;
        List<Collider2D> colliders = GetComponentsInChildren<Collider2D>().ToList();
        _powerupManager.DeletePowerup();
        _powerupManager.DisableAllPowerups();
        _lockInput = false;

        colliders.ForEach(
            (collider) =>
            {
                collider.enabled = true;
            }
        );
        GetComponentInChildren<PolygonCollider2D>().enabled = true;

        StopFeatherFX();

        _animator.SetLayerWeight(0, 1);
        _animator.SetLayerWeight(1, 0);
        _animator.SetLayerWeight(2, 0);
        _animator.SetLayerWeight(3, 0);
        _animator.SetLayerWeight(4, 0);

        if (HeadWearManager.Instance.HeadWearIsEquipped)
        {
            _headWearAnimator.SetLayerWeight(0, 1);
            _headWearAnimator.SetLayerWeight(1, 0);
            _headWearAnimator.SetLayerWeight(2, 0);
            _headWearAnimator.SetLayerWeight(3, 0);
            _headWearAnimator.SetLayerWeight(4, 0);
        }
    }

    public void CreateFeatherFX()
    {
        featherFX.Play();
        Debug.Log("feather FALLING!");
    }

    public void CreateBurstFeatherFX()
    {
        burstFeatherFX.Play();
        Debug.Log("feather FALLING!");
    }

    public void StopFeatherFX()
    {
        featherFX.Stop();
    }

    public void AddFeatherVFX()
    {
        addVFX.GetComponent<Animator>().SetTrigger("addFeatherTrigger");
    }
    public void AddHeartVFX()
    {
        addVFX.GetComponent<Animator>().SetTrigger("addHeartTrigger");
    }

    public void PlayAnimation(string animationName, float transDuration)
    {
        _animator.CrossFade(animationName, transDuration);
        _frontWingAnimator.CrossFade(animationName, transDuration);
        if (HeadWearManager.Instance.HeadWearIsEquipped)
            _headWearAnimator.CrossFade(animationName, transDuration);
    }
    public void PlayAnimation(string animationName, float transDuration, int layer)
    {
        _animator.CrossFade(animationName, transDuration, layer);
        _frontWingAnimator.CrossFade(animationName, transDuration, layer);
        if (HeadWearManager.Instance.HeadWearIsEquipped)
            _headWearAnimator.CrossFade(animationName, transDuration, layer);
    }
    public void SetAnimatorBool(string boolName, bool value)
    {
        _animator.SetBool(boolName, value);
        _frontWingAnimator.SetBool(boolName, value);
        if (HeadWearManager.Instance.HeadWearIsEquipped)
            _headWearAnimator.SetBool(boolName, value);
    }
    public void SetAnimatorLayerWeight(int layer, int weight)
    {
        _animator.SetLayerWeight(layer, weight);
        _frontWingAnimator.SetLayerWeight(layer, weight);
        if (HeadWearManager.Instance.HeadWearIsEquipped)
            _headWearAnimator.SetLayerWeight(layer, weight);
    }
    
    private void InitializeHeadWear()
    {
        _headWear.InitializeAnimator();
        _headWearAnimator = _headWear._animator;

        SetAnimatorBool("alive", true);
        PlayAnimation("flying", 0.1f, _gameManager.HandleFeatherCheckPure());

        _headWear.gameObject.SetActive(HeadWearManager.Instance.HeadWearIsEquipped);
    }

    private void CheckIfDebugMode()
    {
        if (debugMode)
            EnableDebugMode();
        else
            DisableDebugMode();
    }

    private void EnableDebugMode()
    {
        _playerCollision.GetComponent<PolygonCollider2D>().enabled = false;
        _sunColliderObject.GetComponent<BoxCollider2D>().enabled = false;
        _gameManager.StopScoreTick();
        _gameManager.StopStaminaTick();
    }

    private void DisableDebugMode()
    {
        //_playerCollision.GetComponent<PolygonCollider2D>().enabled = true;
        _sunColliderObject.GetComponent<BoxCollider2D>().enabled = true;
        if(disableReasons.Count == 0)
        {
            _gameManager.StartStaminaTick();
            _gameManager.StartScoreTick();
        }
    }

}