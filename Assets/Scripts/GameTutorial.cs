using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameTutorial : MonoBehaviour
{
    public bool debugTutorial = true;

    [SerializeField]
    private GameObject _icarusPrefab;
    [SerializeField]
    private GameObject _cursor;
    [SerializeField]
    private ObstacleSpawner _obstacleSpawner;
    [SerializeField]
    private ObstacleDestroyer _obstacleDestroyer;
    [SerializeField]
    private ObstacleSpawnTrigger _obstacleSpawnTrigger;
    [SerializeField]
    private GameObject _dangerText;
    [SerializeField]
    private GameObject _tryAgainText;
    [SerializeField]
    private ParticleSystem _ashFalling;
    [SerializeField]
    private IcarusStart _icarusStart;
    [SerializeField]
    private FireUI _fireUI;
    [SerializeField]
    private GameObject _inGameUICanvas;
    [SerializeField]
    private GameObject _inGameUIGroup;

    [SerializeField]
    private GameObject _rightSquare;
    [SerializeField]
    private GameObject _leftSquare;

    private GameManager _gameManager;
    private GameObject _icarusInstance;
    private Animator _cursorAnimator;
    private Obstacle _spawnedObstacle;
    private int _currentStep = 1;
    private bool _startTut = false;
    private PlayerController _playerController;
    private Animator _dangerAnimator;
    private Animator _icarusAnimator;
    private Vector2 _spawnPoint;

    [SerializeField]
    private GameObject _popUpUI;

    private ConfirmationUI confirmationUI;

    private AsyncOperation loadingOperation;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _cursorAnimator = _cursor.GetComponent<Animator>();
        _dangerAnimator = _dangerText.GetComponent<Animator>();

    }

    public void StartTutorial(Vector2 spawnPoint, int step)
    {
        _currentStep = step;
        Debug.Log("TUTORIAL STARTED");
        _startTut = true;
        _spawnPoint = spawnPoint;
        _icarusInstance = Instantiate(_icarusPrefab, spawnPoint, Quaternion.identity);
        _playerController = _icarusInstance.GetComponent<PlayerController>();
        _icarusAnimator = _icarusInstance.GetComponent<Animator>();
        _icarusInstance.GetComponentInChildren<HeadWear>().gameObject.SetActive(false);

        //_gameManager.GlobalDownwardSpeed = _gameManager.SetDownwardSpeed;
        _gameManager.GlobalDownwardSpeed = 1.75f;
        _gameManager.PlayerController = _playerController;

        _obstacleDestroyer.gameObject.SetActive(false);
        _obstacleSpawnTrigger.gameObject.SetActive(false);
        _spawnedObstacle = _obstacleSpawner.SpawnTutorialObstacle();

    }

    void Update()
    {
        //Debug.Log("START TUT " + _startTut);
        if(!_startTut) { return; }

        //Debug.Log("GAME STATE " +  _currentStep);
        switch (_currentStep) 
        {
            case 1 :
                Step1();
                break;
            case 2 :
                Step2();
                break;
            case 3 :
                Step3(); 
                break;
            case 4 :
                Step4();
                break;
            case 5 :
                Step5();
                break;
            case 6 :
                Step6();
                break;
            default:
                break;
        }
    }

    private void Step1() // HOLD RIGHT TO MOVE RIGHT
    {
        _cursor.SetActive(true);
        _cursorAnimator.CrossFade("hold", 0.1f);
        _cursor.transform.DOMoveX(1.35f, 2f);
        _cursor.transform.DOMoveY(.25f, 2f);

        _rightSquare.SetActive(true);

        if(_icarusInstance.transform.position.x >= 0.7)
        {
            _currentStep += 1;
        }
    }

    private void Step2() // HOLD LEFT TO MOVE LEFT
    {
        _cursorAnimator.CrossFade("hold", 0.1f);
        _cursor.transform.DOMoveX(-1.35f, 1.5f);
        _cursor.transform.DOMoveY(.25f, 1.5f);

        _rightSquare.SetActive(false);
        _leftSquare.SetActive(true);

        if (_icarusInstance.transform.position.x <= -0.1f) 
        {
            _currentStep += 1;  
        }
    }

    private void Step3() // CURSOR DISAPPEARS
    {
        Debug.Log("SPAWN TUTORIAL OBSTACLE");

        _leftSquare.SetActive(false);

        _cursor.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 1.25f).OnComplete(() =>
        {
            _cursor.SetActive(false);
        });
        _currentStep += 1;
    }

    private void Step4() // MOVE OBSTACLE
    {
        Obstacle.StopObstacle = false;
        _inGameUICanvas.SetActive(true);
        //_inGameUIGroup.SetActive(false);

        //_gameManager.GlobalDownwardSpeed = 1.75f;
        if(_playerController != null)
            _fireUI.SetUpReference(_playerController.GetComponentInChildren<SunCollisions>());

        if (_spawnedObstacle == null)
        {
            _spawnedObstacle = FindObjectOfType<Obstacle>();
        }
        Debug.Log("SPAWNED OBS " + _spawnedObstacle);
        Debug.Log("SPAWNED OBS Y POS" + _spawnedObstacle.transform.position.y);
        if (_spawnedObstacle.transform.position.y <= -5.5)
        {
            //Destroy(_spawnedObstacle.gameObject);
            _currentStep += 1;
        }
    }

    private void Step5() // ICARUS MOVE TO THE SUN
    {
        _playerController.LockInput(true);
        _icarusInstance.transform.DOMoveX(0, 6f);
        _icarusInstance.transform.DOMoveY(3.65f, 6f);
        _gameManager.Tutorial = false;

        _dangerAnimator.CrossFade("Danger", 0.1f);  

        if (_icarusInstance.transform.position.y >=0.75f)
        {
            _dangerText.gameObject.SetActive(true);
            _dangerAnimator.CrossFade("Danger", 0.1f);
            _dangerAnimator.transform.DOScale(Vector2.one * 4f, 1.5f).OnComplete(() =>
            {
                _dangerAnimator.transform.DOScale(Vector2.one * 1.5f, 1.5f);
            });
        }

        if(_icarusInstance.transform.position.y >= 3.25f)
        {
            _currentStep += 1;
        }
    }

    private void Step6() // ICARUS BURNS AND TUTORIAL ENDS
    {
        _dangerText.SetActive(false);
        FallDown();
        _startTut = false;

    }

    private void FallDown()
    {
        _icarusAnimator.SetBool("alive", false);
        _icarusAnimator.SetLayerWeight(0, 0);
        _icarusAnimator.SetLayerWeight(4, 1);
        _icarusAnimator.CrossFade("falling-prep", 0.1f, 4);

        //Load Game Scene
        loadingOperation = SceneManager.LoadSceneAsync("GameScene");
        loadingOperation.allowSceneActivation = false;

        _icarusInstance.GetComponent<PlayerController>().AshFalling.Play();
        _icarusInstance.GetComponentInChildren<SunCollisions>().gameObject.SetActive(false);

        _icarusInstance.transform
                .DOMoveY(_icarusInstance.transform.position.y - .2f, 0.4f)
                .OnComplete(() =>
                {
                    _icarusAnimator.enabled = false;
                    _icarusInstance.GetComponent<SpriteRenderer>().enabled = false;
                    _icarusInstance.transform
                        .DOMoveY(_icarusInstance.transform.position.y - 10f, 1f)
                        .OnComplete(() =>
                        {
                            //_inGameUIGroup.SetActive(true);
                            _inGameUICanvas.SetActive(false);
                            Destroy(_icarusInstance.gameObject);
                            TryAgain();
                        });
                });
    }

    private void TryAgain()
    {
        _tryAgainText.SetActive(true);


        _tryAgainText.transform.DOMoveY(1.5f, 1f).OnComplete(() =>
        {
            _tryAgainText.transform.DOMove(_tryAgainText.transform.position, 1.25f).OnComplete(() =>
            {
                _tryAgainText.transform.DOMoveY(-6, 1f).OnComplete(() =>
                {
                    _tryAgainText.SetActive(false);
                    _gameManager.SetTutorial(false);

                    _icarusStart.ReturnToInitLocations();
                    _obstacleDestroyer.gameObject.SetActive(true);
                    _obstacleSpawnTrigger.gameObject.SetActive(true);

                    loadingOperation.allowSceneActivation = true;
                });
            });
        });
    }

    public void RestartTutorial()
    {
        Destroy(_icarusInstance);
        Destroy(_spawnedObstacle.gameObject);
        _gameManager.Tutorial = true;
        _cursor.transform.localScale = Vector3.one * 2;

        var coins = FindObjectsByType<Coin>(FindObjectsSortMode.None);

        foreach (Coin coin in coins)
        {
            Destroy(coin.gameObject);
        }

        var powerups = FindObjectsByType<Powerup>(FindObjectsSortMode.None);

        foreach (Powerup powerup in powerups) { Destroy(powerup.gameObject); }

        StartTutorial(_spawnPoint, _currentStep);
    }

    

    public void RestartTutorial(bool restarting)
    {
        _gameManager.GlobalDownwardSpeed = 0;
        _obstacleSpawner.gameObject.SetActive(false);
        _gameManager.StopAllCoroutines();
        _playerController.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _playerController.GetComponent<Rigidbody2D>().angularVelocity = 0;

        Debug.Log("SPEEDS " + _playerController.GetComponent<Rigidbody2D>().velocity);
        _playerController.GetComponent<Rigidbody2D>().simulated = false;
        _playerController.GetComponent<Rigidbody2D>().Sleep();
        _playerController.SetUpSpeed(0);
        _playerController.LockInput(true);

        _icarusAnimator.SetBool("alive", false);
        _icarusAnimator.CrossFade("falling-prep", 0.1f, 0);
        _playerController.GetComponent<SpriteRenderer>().sortingOrder = 20;

        if (_icarusInstance.GetComponent<PowerupManager>().CurrentPowerup != null)
        {
            _icarusInstance.GetComponent<PowerupManager>().CurrentPowerup.StopAllCoroutines();
            _icarusInstance.GetComponent<PowerupManager>().Magnet.SetActive(false);
        }

        AudioManager.Instance.PlaySFX("Falling", 1f);

        _icarusInstance.transform
        .DOMoveY(transform.position.y, .5f)
        .OnComplete(() =>
        {
            _gameManager.SpawnCloudsTransition("Cloud Fade In", 1);
            Debug.Log("CLOUDS TRANSITIONING");
            _icarusInstance.transform
            .DOMoveY(-6, 1f)
            .OnComplete(() =>
            {

                Destroy(_icarusInstance);
                _gameManager.Tutorial = true;
                _cursor.transform.localScale = Vector3.one * 2;
                Destroy(_spawnedObstacle.gameObject);

                var coins = FindObjectsByType<Coin>(FindObjectsSortMode.None);

                foreach (Coin coin in coins)
                {
                    Destroy(coin.gameObject);
                }

                var powerups = FindObjectsByType<Powerup>(FindObjectsSortMode.None);

                foreach (Powerup powerup in powerups) { Destroy(powerup.gameObject); }

                StartTutorial(_spawnPoint, _currentStep);
            });
        });

    }

    public void DontMoveUp()
    {

        if ((transform.position.y >= Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y + 0.25f)) //previous value = +1f
        {
            Debug.Log("REACHED LOW EDGE");
            return;
        }
    }

    public void EndTutorial()
    {
        _startTut = false;

        confirmationUI = Instantiate(_popUpUI, _inGameUIGroup.transform).GetComponent<ConfirmationUI>();
        confirmationUI.ChangeDescription("Skip Tutorial?");

        confirmationUI.ConfirmPurchaseEvent += ConfirmPurchase;

    }

    public void ConfirmPurchase(bool decision)
    {
        if (decision)
        {
            //Load Game Scene
            loadingOperation = SceneManager.LoadSceneAsync("GameScene");
            loadingOperation.allowSceneActivation = true;

            _gameManager.SetTutorial(false);
            SceneManager.UnloadSceneAsync("TutorialScene");
        }

        confirmationUI.ConfirmPurchaseEvent -= ConfirmPurchase;
        Destroy(confirmationUI.gameObject);
    }
}
