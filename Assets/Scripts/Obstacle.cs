using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _downwardSpeed = 1f;

    [SerializeField]
    private Transform _spawnPointsGroup;
    [SerializeField]
    private Transform _bufferSpawnPointsGroup;

    [SerializeField]
    private Coin _coinPrefab;

    [SerializeField]
    private FeatherPowerup _featherPowerup;


    //private int[] _testVals = { 0 , 3};
    private int _pupCtr;

    [SerializeField]
    private GameObject _midasFlashVFX;

    [SerializeField]
    private ParticleSystem _explosionPS;

    [SerializeField]
    private SpriteRenderer _bgObstacleSprite;

    private GameManager _gameManager;
    private MidasSpawn _midasSpawn;
    //private Powerup _powerup;

    //private List<Coin> _spawnedCoins;
    //private List<Powerup> _spawnedPowerups;
    private PickupSpawner _pickupSpawner;
    private List<Transform> _spawnPoints;
    private List<Transform> _bufferSpawnPoints;

    Random _rand = new Random();
    int[] _xScale = { -2, 2 };

    //private bool _spawnedPowerup = false;
    //private bool _spawnedFeather = false;

    public bool Indestructable = false;
    public bool toBeDestroyed = false;
    public ParticleSystem hermesDust;
    public static bool StopObstacle;

    public float DownwardSpeed
    {
        get { return _downwardSpeed; }
        set { _downwardSpeed = value; }
    }

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _midasSpawn = GetComponentInChildren<MidasSpawn>();
        _pickupSpawner = GetComponent<PickupSpawner>();
        
        transform.localScale = new Vector2(_xScale[_rand.Next(_xScale.Length)], 2);
        InitObstacle();
    }

    public void InitObstacle()
    {
        //POPULATE SPAWN POINTS-------------
        _spawnPoints = _spawnPointsGroup.GetComponentsInChildren<Transform>().ToList();
        //Pop first transform since it is the Group Transform
        _spawnPoints.RemoveAt(0);

        //Do the same for the buffer spawn points
        _bufferSpawnPoints = _bufferSpawnPointsGroup.GetComponentsInChildren<Transform>().ToList();
        _bufferSpawnPoints.RemoveAt(0);
        StopObstacle = _gameManager.Tutorial;
        Debug.Log("INIT OBSTACLE TUT " + _gameManager.Tutorial );

        //Spawn Pickups
        if(_gameManager.Tutorial)
        {
            Debug.Log("Spawn before tut!");
            _pickupSpawner.SpawnTutorial(_spawnPoints, this.gameObject.transform);
            Debug.Log("Spawn after tut!");
        }
        else
            _pickupSpawner.Spawn(_spawnPoints, this.gameObject.transform, false);
        
        //Spawn buffer coins
        _pickupSpawner.Spawn(_bufferSpawnPoints, this.gameObject.transform, true);
    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log("STOPOBS " + StopObstacle);
        if(!StopObstacle)
        {
            Debug.Log("OBSTACLE MOVING");
            transform.Translate(_gameManager.GlobalDownwardSpeed * Time.deltaTime * Vector2.down);
        }
        else
        {
            Debug.Log("OBSTACLE NOT MOVING");
            transform.Translate(Vector2.zero);
        }
        
        _pupCtr = _gameManager.powerUpCounter;
        //Debug.Log("PU COUNTER " + _pupCtr);
        /*Debug.Log(
            "speed is: "
                + _downwardSpeed
                + " with translation: "
                + (_downwardSpeed * Time.deltaTime * Vector2.down)
        );*/
    }

    public void PlayExplosion()
    {
        _explosionPS.Play();
    }


    public void Midas()
    {
       // List<Coin> coinsLeft = new List<Coin>(GetComponentsInChildren<Coin>());
        _pickupSpawner.SpawnedCoins.ForEach(coin =>
        {
            if(coin != null)
                Destroy(coin.gameObject);
        });

        //List<Powerup> powerUP = new List<Powerup>(GetComponentsInChildren<Powerup>());
        _pickupSpawner.SpawnedPowerups.ForEach(pUp =>
        {
            if(pUp != null)
                Destroy(pUp.gameObject);
        });

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;
        _bgObstacleSprite.enabled = false;

        _midasFlashVFX.GetComponent<Animator>().CrossFade("Midas Flash Fade In", 0.1f);

        _midasSpawn.SpawnMidasCoins(_coinPrefab, this.gameObject);
        Debug.Log("SPAWNED MIDAS COINS");

        _gameManager.midasIsOn = false;
        _gameManager.MidasPowerUpUIDisplay();
        FindObjectOfType<PowerupManager>().Midas.SetActive(false);
    }
}
