using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Random = System.Random;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField]
    private List<Powerup> _powerupList;
    [SerializeField]
    private Coin _coinPrefab;
    [SerializeField]
    private FeatherPowerup _featherPrefab;

    private GameManager _gameManager;
    private List<Powerup> _spawnedPowerups;
    private List<Coin> _spawnedCoins;

    Random random;

    public List<Powerup> SpawnedPowerups { get { return _spawnedPowerups; } }
    public List<Coin> SpawnedCoins {  get { return _spawnedCoins; } }

    void Start()
    {
        Debug.Log("POWERUP MAX CHANCE " +  _gameManager.PowerupMaxChance);
        _powerupList = FindObjectOfType<ObstacleSpawner>().PowerUpList;
        Debug.Log("POWER UP LIST IS " + _powerupList);
    }

    private void Awake()
    {
        //_powerupList = FindObjectOfType<ObstacleSpawner>().PowerUpList;

        random = new Random();
        _spawnedPowerups = new List<Powerup>();
        _spawnedCoins = new List<Coin>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private bool SpawnPowerup(Transform spawnPoint, Transform tempParent)
    {
        int spawnRes = random.Next(10);

        Debug.Log("Spawn Res is " + spawnRes);

        if(spawnRes > 7)
        {
            Powerup _powerup = Instantiate(_powerupList[random.Next(_powerupList.Count)], spawnPoint.position, Quaternion.identity, tempParent.transform);
            //Powerup _powerup = Instantiate(_powerupList[random.Next(1)], spawnPoint.position, Quaternion.identity, tempParent.transform); 
            //Powerup _powerup = Instantiate(_powerupList[0], spawnPoint.position, Quaternion.identity, tempParent.transform); // 0-artemis ; 1-hermes ; 2-magnet ; 3-midas ; 4-zeus
            _powerup.transform.parent = null;
            _spawnedPowerups.Add(_powerup);
            return true;
        }

        return false;
    }


    private bool SpawnCoin(Transform spawnPoint, Transform tempParent)
    {
        int spawnRes = random.Next(100);
        if (spawnRes <= 85)
        {
            var coin = Instantiate(_coinPrefab, spawnPoint.position, Quaternion.identity, tempParent.transform);
            coin.transform.parent = null;
            _spawnedCoins.Add(coin);
            return true;
        }

        return false;
    }

    private bool SpawnFeather(Transform spawnPoint, Transform tempParent)
    {
        int spawnRes = random.Next(2);
        if(spawnRes == 1)
        {
            var feather = Instantiate(_featherPrefab, spawnPoint.position, Quaternion.identity, tempParent.transform);
            feather.transform.parent = null;    
            _spawnedPowerups.Add(feather);
            return true;
        }

        return false;
    }


    public void Spawn(List<Transform> _spawnPoints, Transform tempParent, bool spawnCoinOnly = false)
    {
        bool spawnFeather = false;
        bool spawnPowerup = false;

        int spawnPowerupRoll = random.Next(_gameManager.PowerupMaxChance);
        int spawnFeatherRoll = random.Next(_gameManager.FeatherMaxChance);

        if (spawnPowerupRoll <= _gameManager.PowerupHitChance) spawnPowerup = true;
        if(spawnFeatherRoll <= _gameManager.FeatherHitChance + (_gameManager.HandleFeatherCheck()*1.5f)) spawnFeather = true;

        for (int i = 0;  i < _spawnPoints.Count; i++)
        {

            //If feather spawned toggle off spawnFeather and skip loop
            if (spawnFeather && !spawnCoinOnly)
            {
                spawnFeather = !SpawnFeather(_spawnPoints[i], tempParent);
                continue;
            }


            //If powerup spawn 
            if(spawnPowerup && !spawnCoinOnly)
            {
                spawnPowerup = !SpawnPowerup(_spawnPoints[i], tempParent);
                continue;
            }


            //Else spawn Coin
            SpawnCoin(_spawnPoints[i], tempParent);
        }
    }

    public void SpawnTutorial(List<Transform> spawnPoints, Transform tempParent)
    {
        Debug.Log("SPAWN TUTORIAL!!!");
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if(i == 3)
            {
                Powerup _powerup = Instantiate(_powerupList[2], spawnPoints[i].position, Quaternion.identity, tempParent.transform); // 0-artemis ; 1-hermes ; 2-magnet ; 3-midas ; 4-zeus
                _powerup.transform.parent = null;
                _spawnedPowerups.Add(_powerup);
                continue;
            }

            SpawnCoin(spawnPoints[i], tempParent);
        }
    }
}
