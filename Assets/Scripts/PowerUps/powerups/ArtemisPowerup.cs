using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArtemisPowerup : Powerup
{
    [SerializeField]
    private float _duration;

    [SerializeField]
    private AnimationClip _despawnAnim;

    [SerializeField]
    private float _newSpawnIntervalQuoefficient = .75f;

    [SerializeField]
    private float _newDownwardSpeedQuoefficient = 20f;

    [SerializeField]
    private ParticleSystem _speedTrailFX;
    private ObstacleSpawner _obstacleSpawner;

    private ObstacleSpawnTrigger _obstacleSpawnTrigger;
    private List<Obstacle> _currentObstacles;

    [SerializeField]
    private Sprite _pSprite;

    [SerializeField]
    private Color _pColor;

    /*public float ArtemisDuration
    {
        get { return _duration; }
        set { ArtemisDuration = _duration; }
    }*/


    protected override void Start()
    {
        base.Start();

        _obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        _obstacleSpawnTrigger = FindObjectOfType<ObstacleSpawnTrigger>();

        _animator.CrossFade("artemis", 0.1f);
    }

    protected override void Update()
    {
        base.Update();
    }


    public void HideObstacles() { }

    public override void ApplyPowerup()
    {
        Debug.Log("Apply Artemis!");

        GameObject artemis = _powerupManager.ArtemisArrow;

        if(AudioManager.Instance.burningSfxHandler.volume > 0)
            AudioManager.Instance.PlayBurningSFX(1, 0);

        _powerupManager.powerUpGameObject = artemis;
        _powerupManager.powerUpAnimator = artemis.GetComponent<Animator>();

        //_obstacleSpawner.StopAllCoroutines();
        _currentObstacles = FindObjectsByType<Obstacle>(FindObjectsSortMode.None)
            .ToList<Obstacle>();

        //_playerController.EnableCollision(false);
        _playerController.AddDisableReason("artemis powerup");

        _powerupManager
            .GetComponentInChildren<CoinCollision>()
            .gameObject.GetComponent<BoxCollider2D>()
            .enabled = false;

        _powerupManager.GetComponent<PlayerController>().LockInput(true);
        _powerupManager.ArtemisArrow.SetActive(true);
        _powerupManager.ArtemisArrow.GetComponent<Animator>().SetLayerWeight(2, 1);
        _powerupManager.ArtemisArrow.GetComponent<Animator>().SetLayerWeight(0, 0);
        _powerupManager.ArtemisArrow.GetComponent<Animator>().CrossFade("spawning", 0.1f, 2);

        _powerupManager.transform.DOMoveX(0.13f, 0.3f);
        _powerupManager.transform
            .DOMoveY(-0.5f, 0.3f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _powerupManager.transform.DOMoveY(0f, 0.3f);

                _gameManager.GlobalDownwardSpeed = _gameManager._obsSpeedCurve.Evaluate(_gameManager.ScoreDistance) * _newDownwardSpeedQuoefficient;

                _powerupManager.GetComponent<PlayerController>().SetUpSpeed(0);

                _powerupManager.artemisSpeedTrailFX.Play();
                _powerupManager.artemisCloudTrailFX.Play();
                _gameManager.StopStaminaTick();
                _gameManager.artemisActive = true;
                _gameManager.isSpeeding = true;

                _powerupManager
                    .GetComponent<PlayerController>()
                    .PlayAnimation("artemis power-up", 0.1f, _gameManager.HandleFeatherCheckPure());

                //StartCoroutine(DelaySpawnTimer());

                _gameManager.SpawnPowerUpUI();

                AudioManager.Instance.PlaySFX("Artemis SFX", 0);

                StartCoroutine(TickArtemisDuration());
            });

    }

    private IEnumerator TickArtemisDuration()
    {
        float time = 0f;

        Vector3 tempScale = new Vector3(0, 0, 0);

        while (time <= (_duration - _despawnAnim.length))
        {
            time += Time.deltaTime;
            _gameManager.StartPowerUpUITimer(time, _duration, _pSprite, _pColor);

            yield return null;
        }

        _powerupManager.ArtemisArrow.GetComponent<Animator>().CrossFade("despawning", 0.1f, 2);
        _powerupManager.GetComponent<PlayerController>().PlayAnimation("flying", 0.1f, layer: _gameManager.HandleFeatherCheckPure());

        _powerupManager.artemisSpeedTrailFX.Stop();
        _powerupManager.artemisCloudTrailFX.Stop();

        //StartCoroutine(DelaySpawnTimer());
        //stops the obstacle spawner from spawning to ensure no obstacle collision with player after power up duration

        yield return SecondHalfDespawn(time);

        //Once the duration runs out, the obstacle that holds the powerup can now be destroyed.
       
    }

    private IEnumerator SecondHalfDespawn(float time)
    {
        Debug.Log("ARTEMIS DURATION: " + time + "/" + _duration);
        while (time <= _duration)
        {
            time += Time.deltaTime;
            _gameManager.StartPowerUpUITimer(time, _duration, _pSprite, _pColor);

            //_obstacleSpawner.StopAllCoroutines();
            yield return null;
        }

        foreach (var obstacles in _currentObstacles)
        {
            if (obstacles.toBeDestroyed)
                Destroy(obstacles.gameObject);
        }

        //StopCoroutine(DelaySpawnTimer());



        Debug.Log("ARTEMIS DURATION: " + time + "/" + _duration);

        _gameManager.DespawnPowerUpUI();

        //Delay Player Collision to make sure he is clear of obstacles when ending powerup
        //_gameManager.StartPlayerCollisionDelay();


        _powerupManager.DeletePowerup();

        _playerController.RemoveDisableReason("artemis powerup");
        _gameManager.StartPlayerCollisionDelay();
    }

    private IEnumerator DeletePowerupAfterDelay()
    {
        yield return _gameManager.DelayPlayerCollisionActivate();

    }

    public override void HandlePowerup() 
    {
    }

    public override void CleanupPowerup()
    {
        //_obstacleSpawner.SetDownwardSpeed(_gameManager.CurrentSpeed);
        _currentObstacles = FindObjectsByType<Obstacle>(FindObjectsSortMode.None)
            .ToList<Obstacle>();
/*        foreach (var obstacles in _currentObstacles)
        {
            obstacles.DownwardSpeed = _gameManager.CurrentSpeed;
            obstacles.GetComponent<PolygonCollider2D>().enabled = true;
            obstacles.Indestructable = false;
            Debug.Log("ENDED");
        }*/

        _gameManager.GlobalDownwardSpeed = _gameManager.CurrentSpeed;
        _powerupManager
            .GetComponent<PlayerController>()
            .PlayAnimation("flying", 0.1f, _gameManager.HandleFeatherCheckPure());
        _powerupManager.ArtemisArrow.SetActive(false);
        _powerupManager.GetComponent<PlayerController>().SetUpSpeed(1f);

        /*_powerupManager
            .GetComponentInChildren<CoinCollision>()
            .gameObject.GetComponent<BoxCollider2D>()
            .enabled = true;*/

        _powerupManager
            .GetComponentInChildren<CoinCollision>()
            .gameObject.GetComponent<BoxCollider2D>()
            .enabled = true;

        //Collision handled in delay
        //_playerController.EnableCollision(true);

        _gameManager.StartStaminaTick();
        _gameManager.artemisActive = false;
        _gameManager.isSpeeding = false;

        
        Debug.Log("GAME OBS SPEED: " + _gameManager.CurrentSpeed);
        //_obstacleSpawner.SpawnObstacle();

        //_obstacleSpawnTrigger.GetComponent<BoxCollider2D>().enabled = true;

        _powerupManager.GetComponent<PlayerController>().LockInput(false);
    }

    
}
