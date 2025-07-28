using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class FlockSpawner : MonoBehaviour
{
    Random random;

    [SerializeField]
    private ParticleSystem _flockOfBirdsParticle;

    [Header("Rate out of 10")]
    [SerializeField]
    private int _rateOfSpawn;

    [SerializeField]
    private float _spawnTimer;

    [SerializeField]
    private bool _birdsAreSpawned;
    [SerializeField]
    private bool _waitingSpawn;

    [SerializeField]
    private int spawnRes;

    [SerializeField]
    private bool _willSpawnInGame;


    private GameManager _gameManager;
    private float _durationTimer;

    private void Awake()
    {
        random = new Random();
        _birdsAreSpawned = false;
        _waitingSpawn = false;
        _durationTimer = _flockOfBirdsParticle.main.duration;
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckIfInGame();
        SpawnFlockOfBirds();
    }

    private void CheckIfInGame()
    {
        if (_gameManager.GameStateGetter == GameState.PLAY)
            transform.position = new Vector2(-3.21f, 0.7599998f);
    }

    private void SpawnFlockOfBirds()
    {
        if (_birdsAreSpawned || _waitingSpawn)
            return;

        spawnRes = random.Next(10);

        if (spawnRes > _rateOfSpawn)
        {
            _waitingSpawn = true;
            StartCoroutine(TickSpawnTimer());
            return;
        }

        StartCoroutine(TickDurationTimer());
        _flockOfBirdsParticle.transform.position = new Vector2(_flockOfBirdsParticle.transform.position.x, random.Next(-17, 25) / 10);
        _flockOfBirdsParticle.Play();
        _birdsAreSpawned = true;
    }

    private IEnumerator TickSpawnTimer()
    {
        yield return new WaitForSeconds(_spawnTimer);

        _waitingSpawn = false;
        //_flockOfBirdsParticle.Stop();
        StopCoroutine(TickSpawnTimer());
    }

    private IEnumerator TickDurationTimer()
    {
        yield return new WaitForSeconds(_durationTimer);

        _birdsAreSpawned = false;
        _flockOfBirdsParticle.Stop();
        StopCoroutine(TickDurationTimer());
    }
}
