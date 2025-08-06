using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HermesEvent : MonoBehaviour
{

    private ParticleSystem _dustFX;
    private GameManager _gameManager;
    private Obstacle _currentObstacle;
    private ObstacleSpawner _obstacleSpawner;
    private PowerupManager _powerupManager;

    private int _counter = 3;
    private float currentDownSpeed;
    private bool _isJumping = false;
    private const float JUMP_DELAY = 0.31f;

    private PlayerController _playerController;
    private Animator _animator;
    private Animator _hermesAnimator;

    private List<Obstacle> _jumpedObstacles;
    private Vector3 _dustPos;

    [SerializeField]
    private int _maxJumps;

    [SerializeField]
    private Sprite _pSprite;

    [SerializeField]
    private float _jumpSpeed = 10f;

    public bool IsJumping 
    {  
        get { return _isJumping; } 
    }

    /*public int MaxJumps 
    {
        get { return _maxJumps; } 
        set { MaxJumps = _maxJumps; } 
    }*/


    // Start is called before the first frame update
    void Start()
    {
        _jumpedObstacles = new List<Obstacle>();
        _gameManager = FindObjectOfType<GameManager>();
        _obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        _powerupManager = FindObjectOfType<PowerupManager>();

        _playerController = _powerupManager.gameObject.GetComponent<PlayerController>();
        _animator = _powerupManager.gameObject.GetComponent<Animator>();
        _hermesAnimator = GetComponent<Animator>();

        _dustFX = GetComponentInChildren<ParticleSystem>();
        _maxJumps = PowerupUpgradeManager.Instance.HermesMaxJumps;
        _counter = _maxJumps;
        _gameManager.StartPowerUpUICounter(_counter, _pSprite);
        currentDownSpeed = _obstacleSpawner.DownwardSpeed;
    }

    void OnEnable()
    {
        _obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        _counter = _maxJumps;
        _gameManager.StartPowerUpUICounter(_counter, _pSprite);
        _jumpedObstacles?.Clear();
        currentDownSpeed = _gameManager.CurrentSpeed;
        _isJumping = false;
    }

    // Update is called once per frame
    void Update() { }

    public void EndZoneHit()
    {
        if (!_isJumping)
            return;

        //SetAllInstancedObstacleSpeeds(currentDownSpeed);
        _gameManager.GlobalDownwardSpeed = currentDownSpeed;
        _gameManager.StartStaminaTick();
        _gameManager.isSpeeding = false;

        _obstacleSpawner.SetDownwardSpeed(_gameManager.CurrentSpeed);

        //_obstacleSpawner.SetPauseInterval(false);
        _playerController.PlayAnimation("flying", 0.1f, _gameManager.HandleFeatherCheckPure());
        _hermesAnimator.CrossFade("idle", 0.1f, 3);

        _currentObstacle.hermesDust.transform.localPosition = _dustPos;
        _isJumping = false;
        _playerController.LockInput(false);

        if (!_jumpedObstacles.Contains(_currentObstacle))
        {
            _counter--; 
            _gameManager.StartPowerUpUICounter(_counter, _pSprite);
            _jumpedObstacles.Add(_currentObstacle);
        }
        else return;

        Debug.Log("ENDZONE HIT COUNTER " + _counter);

        if (_counter <= 0)
        {
            Debug.Log("DELETE POWERUP");
            // Destroy all obstacles except this one
            // It will be destroyed naturally by ObjectDestroyer
            _currentObstacle.toBeDestroyed = false;
            List<Obstacle> obstacles = FindObjectsByType<Obstacle>(FindObjectsSortMode.None)
                .ToList<Obstacle>();
            obstacles.ForEach(obstacle =>
            {
                if (obstacle.toBeDestroyed)
                    Destroy(obstacle.gameObject);
            });
            _powerupManager.DeletePowerup();
        }
    }

    /*private void SetAllInstancedObstacleSpeeds(float speed)
    {
        List<Obstacle> obstacles = FindObjectsByType<Obstacle>(FindObjectsSortMode.None)
            .ToList<Obstacle>();

        obstacles.ForEach(obstacle => obstacle.DownwardSpeed = speed);
        _obstacleSpawner.SetDownwardSpeed(10f);
    }
*/
    private IEnumerator JumpAnimDelay()
    {
        float time = 0f;

        while (time <= JUMP_DELAY)
        {
            yield return null;
            time += Time.deltaTime;
        }


        _currentObstacle.hermesDust.Play();
        CameraShake.Shake(0.2f, 0.5f);

        //JUMP
        _playerController.PlayAnimation("slinging", 0.1f, _gameManager.HandleFeatherCheckPure());
        _hermesAnimator.CrossFade("slinging", 0.1f, 3);

        AudioManager.Instance.PlaySFX("Slinging", 0);

        //SetAllInstancedObstacleSpeeds(10f);
        _gameManager.isSpeeding = true;
        _gameManager.GlobalDownwardSpeed = 10f* _gameManager.CurrentSpeed;

        Debug.Log("NOTHING UP HERE");
        _playerController._isHit = false;
        _playerController.GetComponent<Rigidbody2D>().simulated = true;

        Debug.Log("NOTHING END ZONE Global Downward Speed = " + _gameManager.GlobalDownwardSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Obstacle obstacle = collision.GetComponent<Obstacle>();

        if (obstacle == null || _isJumping)
        {
            return;
        }

        _playerController._isHit = true;

        _currentObstacle = obstacle;
        _currentObstacle.toBeDestroyed = true;
        _isJumping = true;
        _currentObstacle.Indestructable = true;
        _playerController.LockInput(true);

        _gameManager.GlobalDownwardSpeed = 0;
        _gameManager.StopStaminaTick();

        _obstacleSpawner.SetDownwardSpeed(0);

        _dustPos = _currentObstacle.hermesDust.transform.localPosition;
        //_obstacleSpawner.SetPauseInterval(true);

        //Prep Jump Anim Here
        //...
        float x = Mathf.Sign(_playerController.transform.localPosition.x) * Mathf.Sign(_playerController.transform.localScale.x);
        _playerController.transform.localScale = new Vector2(x * -(_playerController.transform.localScale.x), 2);
        //transform.localScale = new Vector2(x * (transform.localScale.x)/2, 1);

        _dustFX.Play();
        CameraShake.Shake(0.15f, 0.3f);

        _playerController.PlayAnimation("hermes", 0.1f, _gameManager.HandleFeatherCheckPure());
        _hermesAnimator.CrossFade("prep", 0.1f, 3);
        AudioManager.Instance.PlaySFX("Hermes Prep", 0);

        _currentObstacle.hermesDust.transform.localPosition = _currentObstacle.hermesDust.transform.InverseTransformPoint(_dustFX.transform.TransformPoint(_dustFX.transform.localPosition));
        StartCoroutine(JumpAnimDelay());

        Debug.Log("NOTHING WRONG HERE");
    }
}
