using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public enum GameState
{
    START,
    PLAY,
    END
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float TIME_TICK = .001f;

    private float tempTIME_TICK;

    [SerializeField]
    private GameObject _icarusPrefab;

    [SerializeField]
    private GameObject _obstacleSpawner;

    [SerializeField]
    private GameObject _cloudsTransition;
    [SerializeField]
    private GameObject _featherFillUpVFX;
    public AnimationCurve _fillUpVFXAnimCurve;
    private float _vfxTimer;

    [SerializeField]
    private GameObject _gameUI;
    [SerializeField]
    private GameObject _homeUI;
    [SerializeField]
    private GameObject _featherUI;
    [SerializeField]
    private GameObject _powerUpTimerUI;
    [SerializeField]
    private RectTransform _UIRect;
    [SerializeField]
    private RectTransform _pauseButtonUI;
    [SerializeField]
    private GameObject _gameOverUI;
    [SerializeField]
    private GameObject _newBestScorerNameInputFieldUI;
    [SerializeField]
    private BestScoreUI _gameOverBestScoreUI;
    [SerializeField]
    private GameObject _pauseMenuUI;
    [SerializeField]
    private NotificationBarUI _notificationBar;

    [SerializeField]
    private GameObject _postProcessParent;

    [SerializeField]
    private GameObject _midasPowerUpUI;
    [SerializeField]
    private int _powerupMaxChance = 10;
    [SerializeField]
    private int _powerupHitChance = 6;
    [SerializeField]
    private int _featherMaxChance = 10;
    [SerializeField]
    private int _featherHitChance = 4;
    [SerializeField]
    private int _heartSpawnTickSecs = 1;

    /*    [SerializeField]
        private GameObject _tutorial;*/

    [SerializeField]
    private float _globalDownwardSpeed = 1.75f;

    //Creates an animation curve used for the obstacle speed change when difficuly increases
    //[SerializeField]
    public AnimationCurve _obsSpeedCurve;

    [SerializeField]
    private float _obsSpeedCoefficient = 2f;

    [SerializeField]
    private float _scoreDistance = 0f;

    //ICARUS START GAMEOBJECTS
    [SerializeField]
    private GameObject _gameStartObject;
    [SerializeField]
    private IcarusStart _icarusStart;
    [SerializeField]
    private float _stamina = 1f;
    [SerializeField]
    private GameObject _tutorialUI;
    [SerializeField]
    private int _maxHeartUse = 1;
    [SerializeField]
    private int _maxAdsUse = 1;

    private int _heartUse = 0;
    private int _adsUse = 0;
    private int _currentGold = 0;
    [SerializeField]
    private int _totalGold = 0;
    private PlayerController _playerController;
    private Animator _icarusAnimator;
    private float _currentSpeed;
    private bool _tickStamina = true;
    private bool _tickScore = true;
    private bool _isInSun = false;
    private Vector2 _icarusSpawnPoint;

    private bool _featherFell75 = false;
    private bool _featherFell25 = false;

    private float _bestScore;
    private bool _isNewBestScore;

    public AudioSource coinPickedSFX;
    public AudioClip coinPickedSound;
    public bool isBurnt = false;
    private HeartSpawner _heartSpawner;
    //   public bool tutorialToggled = true;

    public int powerUpCounter = 5;
    public float multiplier = 1;
    public ArtemisMultiplier multiplierUI;
    public bool artemisActive = false;
    public bool isSpeeding = false;
    public bool midasIsOn = false;
    [SerializeField]
    private bool _tutorial = false;
    private GoogleRewardedAd _googleRewardedAd;

    public Transform playerTransformBeforeCollision;
    /*[SerializeField]
    private bool _headWearIsEnabled = false;*/

    public event Action<float> ScoreUpdateEvent;
    public event Action<float> FinalScoreEvent;
    public event Action<int> GoldUpdateEvent;
    public event Action<int> UpdateTotalGoldEvent;
    public event Action<float> StaminaUpdateEvent;
    public event Action<float, bool> StaminaDelayUpdateEvent;
    public event Action<float> StaminaFreezeEvent;
    public event Action<float, float, Sprite, Color> PowerUpTimerUpdateEvent;
    public event Action<int, Sprite> PowerUpCounterUpdateEvent;

    private float _setDownwardSpeed = 0f;

    private GameState _gameState;
    private Random _rand;

    #region PROPERTIES
    public GameState GameStateGetter
    {
        get { return _gameState; }
    }
    public int CurrentGold
    {
        get { return _currentGold; }
    }
    public int TotalGold
    {
        get { return _totalGold; }
        set { _totalGold = value; }
    }
    public float CurrentSpeed
    {
        get { return _currentSpeed; }
    }
    public bool IsInSun
    {
        get { return _isInSun; }
        set { _isInSun = value; }
    }

    /*public bool HeadWearIsEnabled
    {
        get { return _headWearIsEnabled; }
        set { _headWearIsEnabled = value; }
    }*/
    public float GlobalDownwardSpeed
    {
        get { return _globalDownwardSpeed; }
        set { _globalDownwardSpeed = value; }

    } public float SetDownwardSpeed
    {
        get { return _setDownwardSpeed; }
        set { _setDownwardSpeed = value; }
    }

    public float BestScore { get { return _bestScore; } set { _bestScore = value; } }
    public bool IsNewBestScore { get { return _isNewBestScore; } set { _isNewBestScore = value; } }
    public int PowerupMaxChance { get { return _powerupMaxChance; } }
    public int PowerupHitChance { get { return _powerupHitChance; } }
    public int FeatherMaxChance { get { return _featherMaxChance; } }
    public int FeatherHitChance { get { return _featherHitChance; } }
    public float ScoreDistance { get { return _scoreDistance; } }
    public Vector2 IcarusSpawnPoint { get { return _icarusSpawnPoint; } set { _icarusSpawnPoint = value; } }
    public int HeartUse { get { return _heartUse; } set { _heartUse = value; } }
    public int AdsUse { get { return _adsUse; } set { _adsUse = value; } }
    public int MaxHeartUse { get { return _maxHeartUse; } }
    public int MaxAdsUse { get { return _maxAdsUse; } }
    public bool Tutorial { get { return _tutorial; } set { _tutorial = value; } }
    public PlayerController PlayerController { set {_playerController = value; } }
    #endregion


    void Start()
    {
        //Screen.SetResolution(468, 988, true);

        _rand = new Random();
        DOTween.Init();
        PrepareSTART();

        _cloudsTransition.SetActive(true);

        if (!Tutorial)
        {
            SpawnCloudsTransition("Cloud Fade Out", 1);
            SpawnFlyUI(0);
        }

        _currentSpeed = _obsSpeedCurve.Evaluate(_scoreDistance) * _obsSpeedCoefficient;
        _setDownwardSpeed = _globalDownwardSpeed;
        _heartSpawner = FindObjectOfType<HeartSpawner>();
        _googleRewardedAd = FindObjectOfType<GoogleRewardedAd>();
        _postProcessParent.SetActive(true);



    }

    // Update is called once per frame
    void Update()
    {
        ProcessGameState();
    }

    private void ProcessGameState()
    {
        switch (_gameState)
        {
            case GameState.START:
                ProcessSTART();
                break;
            case GameState.PLAY:
                ProcessPLAY();
                break;
            case GameState.END:
                ProcessEND();
                break;
        }
    }

    private void ProcessEND() { }

    private void ProcessPLAY() { }

    private void ProcessSTART() { }

    private void PrepareSTART()
    {
        _gameState = GameState.START;
        _obstacleSpawner.SetActive(false);
        _gameUI.SetActive(false);
        _powerUpTimerUI.SetActive(false);
        _midasPowerUpUI.SetActive(false);

        Vibration.Init();
    }

    public void StartGame()
    {
        _gameState = GameState.PLAY;

        Debug.Log("STARTING GAME ");
        _homeUI.SetActive(false);
        _obstacleSpawner.SetActive(true);

        multiplier = PowerupUpgradeManager.Instance.ArtemisMultiplier;
        Debug.Log("MULTIPLIER: " + multiplier);
        multiplierUI.SetMultiplierValue();
        /*
                if (tutorialToggled)
                    StartTutorial();
        */
        AudioManager.Instance.PlayMusic("Flying");
        AudioManager.Instance.burningSfxHandler.volume = 0;

        //ArtemisMultiplierUI.Instance.InitializeMultiplierValue(multiplier);

        _adsUse = 0;
        _heartUse = 0;

        _globalDownwardSpeed = _setDownwardSpeed;

        _playerController = Instantiate(_icarusPrefab, _icarusSpawnPoint, Quaternion.identity).GetComponent<PlayerController>();
        _icarusAnimator = _playerController.GetComponent<Animator>();

        StaminaUpdateEvent?.Invoke(_stamina);

        _gameUI.SetActive(true);
        _powerUpTimerUI.SetActive(false);
        _midasPowerUpUI.SetActive(false);

        _UIRect.transform.position = new Vector2(_UIRect.transform.position.x, _UIRect.transform.position.y + 10f);
        _UIRect.transform.DOMoveY(_UIRect.transform.position.y - 10f, 1f);

        /*_pauseButtonUI.transform.position = new Vector2(
            _pauseButtonUI.transform.position.x,
            _pauseButtonUI.transform.position.y + 10f
        );
        _pauseButtonUI.transform.DOMoveY(_pauseButtonUI.transform.position.y - 10f, 1f);*/

        FireUI fireUI = FindObjectOfType<FireUI>();
        fireUI.SetUpReference(_playerController.GetComponentInChildren<SunCollisions>());

        _obstacleSpawner.GetComponent<ObstacleSpawner>().SpawnObstacle();
        StartStaminaTick();
        //StartCoroutine(TickScore());
        StartScoreTick();
        StartCoroutine(TickScoreAndStamina());
        StartCoroutine(TickSpawnHeartChance());
    }

    public void PrepareGameOver()
    {
        AudioManager.Instance.musicHandler.Stop();
        AudioManager.Instance.PlayBurningSFX(0.6f, 0);

        artemisActive = false;
        _pauseButtonUI.gameObject.SetActive(false);
        _isInSun = false;
        StopScoreTick();
        StopStaminaTick();
        StopAllCoroutines();
        PowerupManager pManager = _playerController?.GetComponent<PowerupManager>();
        List<Obstacle> obstacles = FindObjectsOfType<Obstacle>().ToList<Obstacle>();

        /*foreach (Obstacle obstacle in obstacles)
        {
            obstacle.DownwardSpeed = 0;
        }*/
        _globalDownwardSpeed = 0;
        _playerController.LockInput(true);
        _obstacleSpawner.SetActive(false);
        _powerUpTimerUI.SetActive(false);
        pManager.Zeus.SetActive(false);
        pManager.Magnet.SetActive(false);
        pManager.ArtemisArrow.SetActive(false);
        pManager.Hermes.SetActive(false);

        _playerController.FallDown();

        if (Tutorial)
        {
            transform.DOMoveX(transform.position.x, 1f).OnComplete(() =>
            {
                SpawnCloudsTransition("Cloud Fade In", .8f); 
            });
        }

            AudioManager.Instance.PlaySFX("Game Over SFX", 2f);
    }

    public void GameOver()
    {
        if(Tutorial)
        {
            FindObjectOfType<GameTutorial>().RestartTutorial();
        }

        if (AudioManager.Instance.burningSfxHandler.volume > 0)
            AudioManager.Instance.burningSfxHandler.volume = 0;

        StormCloudsManager stormclouds = FindObjectOfType<StormCloudsManager>();
        if (stormclouds.isStormCloud)
            stormclouds.DespawnStormClouds(1.5f);

        _playerController.StopFeatherFX();
        _gameUI.SetActive(false);
        _pauseButtonUI.gameObject.SetActive(true);
        Debug.Log("SCORE: " + _scoreDistance);
        FinalScoreEvent?.Invoke(_scoreDistance);

        //Update Total Gold of the player
        UpdateTotalGoldEvent += GoldHandler.Instance.HandleTotalGoldUpdate;
        UpdateTotalGoldEvent?.Invoke(_currentGold);
        UpdateTotalGoldEvent -= GoldHandler.Instance.HandleTotalGoldUpdate;

        _cloudsTransition.SetActive(false);

        if (_isNewBestScore)
        {
            AudioManager.Instance.PlaySFX("Best Score", 0);
            _newBestScorerNameInputFieldUI.SetActive(true);
        }
        else
        {
            ShowGameOverUI();
        }
    }

    public void ShowGameOverUI()
    {
        //AudioManager.Instance.PlaySFX("Falling Menu", 2f);
        AudioManager.Instance.PlayMusic("Flying");
        _gameOverUI.SetActive(true);
        _gameOverBestScoreUI.DisplayBestScoreOnGameOver(_scoreDistance);
    }

    public void Home()
    {
        StopAllCoroutines();
        _gameUI.SetActive(false);
        _pauseMenuUI.SetActive(false);
        _gameOverUI.SetActive(false);
        _gameStartObject.SetActive(true);

        AudioManager.Instance.PlayBurningSFX(0.6f, 0);

        SpawnCloudsTransition("Cloud Fade In", 1);

        ResetUITransform(); //Resets all UI Canvas to its original position

        //Clean up game state

        //Destroy Current obstacles
        List<Obstacle> obstacles = FindObjectsOfType<Obstacle>().ToList<Obstacle>();
        foreach (Obstacle obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        //Destroy current coins
        List<Coin> coins = FindObjectsOfType<Coin>().ToList<Coin>();
        foreach (Coin coin in coins)
        {
            Destroy(coin.gameObject);
        }

        List<Powerup> powerups = FindObjectsOfType<Powerup>().ToList<Powerup>();
        foreach (Powerup powerup in powerups)
        {
            Destroy(powerup.gameObject);
        }

        //Destroy Heart
        List<Heart> heart = FindObjectsOfType<Heart>().ToList<Heart>();
        foreach (Heart h in heart)
        {
            Destroy(h.gameObject);
        }

        StopScoreTick();
        StopStaminaTick();

        if(_playerController != null)
            _playerController.SetUpSpeed(1);


        isBurnt = false;
        _stamina = 1f;
        _currentGold = 0;
        _scoreDistance = 0;
        _adsUse = 0;
        _heartUse = 0;

        ScoreUpdateEvent?.Invoke(0);
        GoldUpdateEvent?.Invoke(0);
        StaminaUpdateEvent?.Invoke(1f);

        ObstacleSpawner obstacleSpawner = _obstacleSpawner.GetComponent<ObstacleSpawner>();
        obstacleSpawner.DisableSpawn();

        Destroy(_playerController.gameObject);


        _gameStartObject.SetActive(true);
        Debug.Log("TRANSFORM ICARUS START");
        //Use DOMove to make sure that the FLY button goes in when the Cloud Transition is almost done
        SpawnFlyUI(0.8f);

        _globalDownwardSpeed = _setDownwardSpeed;
        _googleRewardedAd.LoadRewardedAd();
        Time.timeScale = 1;
    }

    public void ReContinueGame()
    {
        StopAllCoroutines();
        _playerController.ResetIcarus();
        StartScoreTick();
        StartStaminaTick();
        _globalDownwardSpeed = _setDownwardSpeed;
        _stamina = 1;

        StaminaUpdateEvent?.Invoke(_stamina);

        //AudioManager.Instance.PlayMusic("Flying");

        FindObjectOfType<Sun>().gameObject.transform.position = new Vector2(0, 4.2f);

        isBurnt = false;
        _gameUI.SetActive(true);
        _gameOverUI.SetActive(false);
        _obstacleSpawner.SetActive(true);

        Time.timeScale = 1;
        //_playerController.EnableCollision(false);
        //_playerController.AddDisableReason("game over");
        //StartCoroutine(DelayPlayerCollisionActivate());
        StartPlayerCollisionDelay();
        StartCoroutine(TickScoreAndStamina());
        StartCoroutine(TickSpawnHeartChance());
        PowerupManager powerupManager = _playerController.GetComponent<PowerupManager>();
        powerupManager.CurrentPowerup = null;
        powerupManager.DisableAllPowerups();


        
        //StartPlayerCollisionDelay();
    }

    public IEnumerator Replay()
    {
        ResetUITransform(); //Resets all UI Canvas to its original position

        StormCloudsManager stormclouds = FindObjectOfType<StormCloudsManager>();
        if (stormclouds.isStormCloud)
            stormclouds.DespawnStormClouds(1.5f);

        List<Obstacle> obstacles = FindObjectsOfType<Obstacle>().ToList<Obstacle>();

        _playerController.SetUpSpeed(0);
        //_playerController.GetComponent<PowerupManager>().artemisSpeedTrailFX.Stop();
        ParticleSystem[] particles = _playerController.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles)
        {
            particle.Stop();
        }

        _globalDownwardSpeed = 0;
        _playerController.GetComponent<Animator>().speed = 0;
        StopScoreTick();
        StopStaminaTick();
        StopAllCoroutines();

        FindObjectOfType<Sun>().gameObject.transform.position = new Vector2(0, 4.2f);

        _googleRewardedAd.LoadRewardedAd();

        //SpawnCloudsTransition("Cloud Replay");

        /*        if (tutorialToggled)
                    StartTutorial();*/

        yield return new WaitForSeconds(1f);

        _playerController.SetUpSpeed(1);
        isBurnt = false;
        _gameOverUI.SetActive(false);
        _pauseMenuUI.SetActive(false);

        //_pauseButtonUI.gameObject.SetActive(false);
        //_pauseMenuUI.SetActive(false);
        _powerUpTimerUI.SetActive(false);
        _midasPowerUpUI.SetActive(false);
        _gameUI.SetActive(false);
        _scoreDistance = 0;
        _currentGold = 0;
        _stamina = 1f;
        ScoreUpdateEvent?.Invoke(_scoreDistance);
        GoldUpdateEvent?.Invoke(_currentGold);
        StaminaUpdateEvent?.Invoke(_stamina);
        _globalDownwardSpeed = _setDownwardSpeed;
        _adsUse = 0;
        _heartUse = 0;

        foreach (Obstacle obs in obstacles)
        {
            Destroy(obs.gameObject);
        }

        //Destroy current coins
        List<Coin> coins = FindObjectsOfType<Coin>().ToList<Coin>();
        foreach (Coin coin in coins)
        {
            Destroy(coin.gameObject);
        }

        //Destory powerups
        List<Powerup> powerups = FindObjectsOfType<Powerup>().ToList<Powerup>();
        foreach (Powerup powerup in powerups)
        {
            Destroy(powerup.gameObject);
        }

        //Destroy Heart
        List<Heart> heart = FindObjectsOfType<Heart>().ToList<Heart>();
        foreach (Heart h in heart)
        {
            Destroy(h.gameObject);
        }

        //_obstacleSpawner.SetActive(false);
        //_obstacleSpawner.GetComponent<ObstacleSpawner>().StartSpawn();
        //_obstacleSpawner.GetComponent<ObstacleSpawner>().SpawnObstacle();

        //AudioManager.Instance.PlayMusic("Flying");

        _icarusStart.ResetLocations();

        Destroy(_playerController.gameObject);

        _icarusStart.IcarusJumpToSling();
        //_playerController.ResetIcarus();
        //StartStaminaTick();
        //StartCoroutine(TickScore());
        //StartScoreTick();
        //StartCoroutine(TickScoreAndStamina());
        //StartCoroutine(TickSpawnHeartChance());
        //StopCoroutine(Replay());


    }

    public IEnumerator TickSpawnHeartChance()
    {
        Debug.Log("HEART TICK STARTING");
        bool spawned = false;
        int hit = 0;

        while (!spawned)
        {
            Debug.Log("HEART TICK LOOPING");

            hit = _rand.Next(0, 100);
            Debug.Log("HEART HIT #: " + hit);

            if (hit <= 10)
            {
                Debug.Log("SPAWNED HEART;");
                _heartSpawner.SpawnHeart();
                spawned = true;
            }

            Debug.Log("DIDNT SPAWN HEART");
            yield return new WaitForSeconds(_heartSpawnTickSecs);
        }
    }

    public IEnumerator TickScoreAndStamina()
    {
        float timePassed = 0f;


        while (true)
        {

            if (artemisActive)
            {
                tempTIME_TICK = TIME_TICK / multiplier;
                multiplierUI.gameObject.SetActive(true);
            }
            else if (!artemisActive)
            {
                tempTIME_TICK = TIME_TICK;
                multiplierUI.gameObject.SetActive(false);
                Debug.Log("TIME TICK: " + tempTIME_TICK);
            }

            timePassed = 0f;
            while (timePassed <= tempTIME_TICK)
            {
                timePassed += Time.deltaTime;
                yield return null;
            }

            if (_tickStamina)
            {
                if (_isInSun)
                {
                    _stamina -= 0.1f;
                    _playerController.CreateFeatherFX();

                    Vibration.VibratePeek();

                    AudioManager.Instance.PlayBurningSFX(1, .5f);
                    //Change feather sprite animation to burning sprite animation
                    /*int i = 0;
                    while(i <= 4)
                        _playerController.featherFX.textureSheetAnimation.SetSprite(i, _playerController.burningFeatherSprite[i]);*/
                }
                else
                {
                    _stamina -= 0.01f;

                    AudioManager.Instance.PlayBurningSFX(1, 0);
                    //Change feather sprite animation to default sprite animation
                    /*int i = 0;
                    while(i <= 4)
                        _playerController.featherFX.textureSheetAnimation.SetSprite(i, _playerController.defaultFeatherSprite[i]);*/
                }
                Debug.Log("STAMINA " + _stamina);

                StaminaDelayUpdateEvent?.Invoke(_stamina, _isInSun);
                StaminaUpdateEvent?.Invoke(_stamina);
                HandleFeatherCheck();
            }

            if (_tickScore)
            {
                _scoreDistance += 1;
                ScoreUpdateEvent?.Invoke(_scoreDistance);
                HandleDifficultyCheck();
            }
        }
    }

    private void HandleDifficultyCheck()
    {
        PowerupManager pManager = FindObjectOfType<PowerupManager>();

        if (_scoreDistance >= 30f && !isSpeeding)
        {
            //Changes the obstacle move speed to the value in the curve depending on the SCORE or TIME
            ChangeMoveSpeed(_obsSpeedCurve.Evaluate(_scoreDistance) * _obsSpeedCoefficient);
        }
        else if (_scoreDistance < 30f && !isSpeeding)
        {
            ChangeMoveSpeed(_setDownwardSpeed);
        }
    }

    private void ChangeMoveSpeed(float speed)
    {
        _currentSpeed = speed;

        _globalDownwardSpeed = speed;
    }

    public int HandleFeatherCheckPure()
    {
        if (isBurnt)
        {
            _icarusAnimator.SetLayerWeight(0, 0);
            _icarusAnimator.SetLayerWeight(4, 1);
            return 4;
        }

        if (_stamina <= .05f)
        {
            return 3;
        }

        if (_stamina <= 0.25f)
        {
            return 2;
        }
        else if (_stamina <= 0.75f)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public int HandleFeatherCheck()
    {
        PowerupManager pManager = FindObjectOfType<PowerupManager>();

        if (_stamina < .01f)
        {
            _playerController.CreateBurstFeatherFX();
            //_icarusAnimator.SetLayerWeight(0, 0);
            /*_icarusAnimator.SetLayerWeight(1, 0);
            _icarusAnimator.SetLayerWeight(2, 0);
            _icarusAnimator.SetLayerWeight(3, 1);*/
            _playerController.SetAnimatorLayerWeight(1, 0);
            _playerController.SetAnimatorLayerWeight(2, 0);
            _playerController.SetAnimatorLayerWeight(3, 1);

            _playerController.LockInput(true);
            _playerController.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _playerController.GetComponent<Rigidbody2D>().simulated = false;
            _globalDownwardSpeed = 0;
            _icarusAnimator.Play("falling-prep start");

            StartCoroutine(DeathBuffer(0.6f));

            _playerController.GetComponent<WingFlapSFXPlayer>().AdjustWingFlapSFX(3);

            return 3;
        }

        if (_stamina <= 0.25f)
        {
            //if has powerup skip animation change
            //if (pManager.CurrentPowerup != null)
            //    return 2;

            if (!_featherFell25)
            {
                _icarusAnimator.CrossFade("flying", 0.1f, 2);
                _playerController.CreateFeatherFX();
                _featherFell25 = true;
            }
            //_icarusAnimator.SetLayerWeight(0, 0);
            /*_icarusAnimator.SetLayerWeight(1, 0);
            _icarusAnimator.SetLayerWeight(2, 1);
            _icarusAnimator.SetLayerWeight(3, 0);*/
            _playerController.SetAnimatorLayerWeight(1, 0);
            _playerController.SetAnimatorLayerWeight(2, 1);
            _playerController.SetAnimatorLayerWeight(3, 0);

            _playerController.GetComponent<WingFlapSFXPlayer>().AdjustWingFlapSFX(2);

            return 2;
        }
        else if (_stamina <= 0.75f)
        {
            //if has powerup skip animation change
            //if (pManager.CurrentPowerup != null)
            //    return 1;
            if (!_featherFell75)
            {
                _icarusAnimator.CrossFade("flying", 0.1f, 1);
                _playerController.CreateFeatherFX();
                _featherFell75 = true;
            }

            /*_icarusAnimator.SetLayerWeight(1, 1);
            _icarusAnimator.SetLayerWeight(2, 0);
            _icarusAnimator.SetLayerWeight(3, 0);*/
            _playerController.SetAnimatorLayerWeight(1, 1);
            _playerController.SetAnimatorLayerWeight(2, 0);
            _playerController.SetAnimatorLayerWeight(3, 0);

            _playerController.GetComponent<WingFlapSFXPlayer>().AdjustWingFlapSFX(1);

            return 1;
        }
        else
        {
            _featherFell75 = false;
            _featherFell25 = false;
            _playerController.SetAnimatorLayerWeight(1, 0);
            _playerController.SetAnimatorLayerWeight(2, 0);
            _playerController.SetAnimatorLayerWeight(3, 0);

            _playerController.GetComponent<WingFlapSFXPlayer>().AdjustWingFlapSFX(0);

            return 0;
        }
    }

    public void StopStaminaTick()
    {
        _tickStamina = false;

        StaminaDelayUpdateEvent?.Invoke(_stamina, _isInSun);
        StaminaFreezeEvent?.Invoke(_stamina);
    }

    public void StartStaminaTick()
    {
        _tickStamina = true;

        StaminaUpdateEvent?.Invoke(_stamina);
        //StartCoroutine(TickScoreAndStamina());
    }

    public void StartScoreTick()
    {
        _tickScore = true;
        //StartCoroutine(TickScore());
    }

    public void StopScoreTick()
    {
        _tickScore = false;
        //StopCoroutine(TickScore());
    }

    public void AddStamina(float amount)
    {
        _stamina += amount;

        if (_stamina >= 1f)
            _stamina = 1f;

        //_featherUI.GetComponent<Animator>().enabled = true;

        GameObject featherVFX = Instantiate(_featherFillUpVFX, transform, true);
        featherVFX.transform.position = _playerController.transform.position;


        StartCoroutine(AnimateFillUpVFX(featherVFX, featherVFX.transform.position, _featherUI, 1));

        //_featherUI.GetComponent<Animator>().CrossFade("Feather Replenishing", 0.1f);
        StaminaUpdateEvent?.Invoke(_stamina);
    }

    public IEnumerator AnimateFillUpVFX(GameObject vfx, Vector2 startpos, GameObject endposGO, float duration)
    {
        _vfxTimer += Time.deltaTime;

        float t = 0;

        while(t <= duration)
        {
            Debug.Log("FEATHER FILLING UP " + endposGO.transform.position.x);
            float x = Mathf.Lerp(startpos.x, endposGO.transform.position.x, _fillUpVFXAnimCurve.Evaluate(t));
            float y = Mathf.Lerp(startpos.y, endposGO.transform.position.y, t);
            vfx.transform.position = new Vector2(x, y);

            t += Time.deltaTime;

            yield return null;
        }

        endposGO.transform.DOScale(1.2f, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            endposGO.transform.DOScale(1, 0.5f);
        });

            Destroy(vfx);
        StopCoroutine(AnimateFillUpVFX(null, startpos, endposGO, duration));
    }

    public void AnimateFillUp(GameObject vfx, Vector2 startpos, GameObject endposGO, float duration)
    {
        StartCoroutine(AnimateFillUpVFX(vfx, startpos, endposGO, duration));
    }


    public void AddGold()
    {
        _currentGold += 1;

        //coinPickedSFX.PlayOneShot(coinPickedSound);
        GoldUpdateEvent?.Invoke(_currentGold);
    }

    public void SpawnCloudsTransition(String anim, float newSpeed)
    {
        _cloudsTransition.SetActive(true);
        _cloudsTransition.GetComponent<Animator>().Play(anim);
        _cloudsTransition.GetComponent<Animator>().speed = newSpeed;
    }

    private void SpawnFlyUI(float buffer)
    {
        _icarusStart.gameObject.SetActive(false);

        transform.DOMoveY(transform.position.y, buffer).OnComplete(() =>
        {
            _icarusStart.gameObject.SetActive(true);
            _icarusStart.ReturnToInitLocations();

        });
    }

    public void SpawnPowerUpUI()
    {
        _powerUpTimerUI.SetActive(true);
    }

    public void DespawnPowerUpUI()
    {
        _powerUpTimerUI.SetActive(false);
    }

    public void StartPowerUpUITimer(float time, float duration, Sprite pSprite, Color pColor)
    {
        _powerUpTimerUI.SetActive(true);
        PowerUpTimerUpdateEvent?.Invoke(time, duration, pSprite, pColor);
    }

    public void StartPowerUpUICounter(int ctr, Sprite pSprite)
    {
        _powerUpTimerUI.SetActive(true);
        PowerUpCounterUpdateEvent?.Invoke(ctr, pSprite);
    }

    public void StartPlayerCollisionDelay()
    {
        //_playerController.EnableCollision(false);
        _playerController.AddDisableReason("invulnerable");
        StartCoroutine(DelayPlayerCollisionActivate());
    }

    public IEnumerator DelayPlayerCollisionActivate()
    {

        StopStaminaTick();

        float time = 0f;
        float alphaTimer = 0f;
        Debug.Log("DELAY Activating Delay");


        GameObject notif = Instantiate(_playerController.GetComponent<PowerupManager>().pickUpNotifVFX, _playerController.transform.position, Quaternion.identity, _playerController.transform);

        notif.GetComponent<Animator>().CrossFade("invul shield", 0.1f);

        while (time <= 3.5f)
        {
            Debug.Log("DELAY time " + time);
            time += Time.deltaTime;


            if (time < 0.375f || (time > 0.75f && time < 1.125f))
            {
                _playerController.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - alphaTimer);
                alphaTimer += 1.3f * Time.deltaTime;
            }
            else
            {
                _playerController.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                alphaTimer = 0f;
            }


            yield return null;
        }

        StartStaminaTick();
        notif.GetComponent<Animator>().Play("NoneAnim");
        Destroy(notif.gameObject);
        Debug.Log("is notif enabled? " + notif.name);

        //The game needs to check if the player has power that makes them invul so that invul won't be disabled in those cases
        /*if(!isSpeeding)
            _playerController.EnableCollision(true);*/

        _playerController.RemoveDisableReason("invulnerable");
        Debug.Log("DELAY Deactivated");
    }

    public void MidasPowerUpUIDisplay()
    {
        Debug.Log("MIDAS CHECK");
        if (midasIsOn)
        {
            Debug.Log("MIDAS on");
            _powerUpTimerUI.GetComponent<Animator>().SetBool("midasIsOn", true);
            _midasPowerUpUI.SetActive(true);
        }
        else
        {
            _midasPowerUpUI.SetActive(false);
            _powerUpTimerUI.GetComponent<Animator>().SetBool("midasIsOn", false);
        }
        //yield return null;

    }

    public IEnumerator DeathBuffer(float time)
    {
        yield return new WaitForSeconds(time);

        PrepareGameOver();
    }    

    public void ShowNotificationBar(Sprite sprite, string desc)
    {
        _notificationBar.gameObject.SetActive(true);
        _notificationBar.ShowNotification(sprite, desc);
    }

    public void DeactNotificationBar()
    {
        _notificationBar.gameObject.SetActive(false);
    }

    public bool CheckTutorial()
    {
        if (!PlayerPrefs.HasKey("tutorial"))
        {
            PlayerPrefs.SetInt("tutorial", 1);
            return true;
        }

        if(PlayerPrefs.GetInt("tutorial") == 1)
        {
            return true;
        } 
        else
        {
            return false;
        }
    }

    public void SetTutorial(bool tutorial)
    {
        PlayerPrefs.SetInt("tutorial", tutorial ? 1 : 0);
        Tutorial = tutorial;
        PlayerPrefs.SetInt("done-tutorial", tutorial ? 0 : 1);
    }

    private void ResetUITransform()
    {
        _gameOverUI.transform.position = Vector2.zero;
        _gameUI.transform.position = Vector2.zero;
        _pauseMenuUI.transform.position = Vector2.zero;
    }

    public void TickStaminaDamage(float damage)
    {
        _stamina -= damage;

        ChangeMoveSpeed(0.7f);
        //Add slowing down of CURRENT Global speed by a few for a few seconds

        _playerController.LockInput(true);
        StartCoroutine(HitTimer());

        HandleFeatherCheck();
        if (_stamina <= 0)
        {
            _stamina = 0;
            PrepareGameOver();
            StaminaUpdateEvent?.Invoke(_stamina);
        }
        else
        {

            StaminaUpdateEvent?.Invoke(_stamina);
            StartPlayerCollisionDelay();
        }

    }

    private IEnumerator HitTimer()
    {

        yield return new WaitForSeconds(1);

        _playerController._isHit = false;
        _playerController.LockInput(false);
        
        StopCoroutine(HitTimer());
    }

    public void TutorialObstacleHit()
    {
        ChangeMoveSpeed(0.7f);
        StartPlayerCollisionDelay();

        transform.DOMoveX(transform.position.x, 2.5f).OnComplete(() =>
        {
            ChangeMoveSpeed(_setDownwardSpeed);
        }
            );
    }

    public void ResetUpdate()
    {
        PlayerPrefs.SetInt("upgradeRefundedVersion", 0);
        PlayerPrefs.SetInt("updateVersion", 0);
        Debug.Log("UPDATE VERSION RESETTED TO " + PlayerPrefs.GetInt("updateVersion"));

        PlayerPrefs.SetInt("artemis-upgrade-level", 2);

        PlayerPrefs.SetInt("zeus-upgrade-level", 2);

        PlayerPrefs.SetInt("magnet-upgrade-level", 1);

        PlayerPrefs.SetInt("hermes-upgrade-level", 1);

        GoldHandler.Instance.HandleTotalGoldUpdate(-_totalGold + 90000);

    }


}
