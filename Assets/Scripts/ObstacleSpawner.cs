using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField]
    private List<Obstacle> _obstacles;

    [SerializeField]
    private float _downwardSpeed = 1f;

    //Animation curve used to calculate the current spawn interval
    [SerializeField]
    private AnimationCurve _interval;

    //constant quoefficient on the spawn interval equation
    [SerializeField]
    private float _spawnIntervalCoefficient = 10f;

    //the spawn interval time variable changes depending on the SCORE or TIME
    [SerializeField]
    private float _spawnInterval = 0f;
    
/*    [SerializeField]
    private float totalTime = 0f;*/

    Random _rand = new Random();

    private GameManager _gameManager;

    [SerializeField]
    private bool _pauseInterval = false;

    [SerializeField]
    private List<Powerup> _powerupList;

    public float DownwardSpeed
    {
        get { return _downwardSpeed; }
    }

    public List<Powerup> PowerUpList
    {
        get { return _powerupList; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //initializes the spawn interval value
        //_spawnInterval = _spawnIntervalCoefficient * _interval.Evaluate(Time.deltaTime);
        _gameManager = FindObjectOfType<GameManager>();

        //StartSpawn();
        //SpawnObstacle();
    }

    public void DisableSpawn()
    {
        StopAllCoroutines();
    }

    public void SetDownwardSpeed(float speed)
    {
        _downwardSpeed = speed; 
        //Debug.Log("GAME OBS SPEED 2: " + _downwardSpeed);
    }

    public void SetPauseInterval(bool pause)
    {
        _pauseInterval = pause;
    }

    //Sets a temporary spawn interval
    public void SetSpawnInterval(float interval)
    {
        //the temporary spawn interval should always be relative to the current spawn interval
        _spawnIntervalCoefficient = interval;
        _spawnInterval = 
            _spawnIntervalCoefficient * _interval.Evaluate(_gameManager.ScoreDistance);

        //StartCoroutine(SpawnObstacleCountDown());
    }

    // Update is called once per frame
    void Update()
    {
        //once the score of the player is greater than 100, spawn interval gets smaller until
        //notice that the curves for OBSTACLE SPEED and SPAWN INTERVAL are reversed, the faster the OBS SPD, the lesser the SPWN INTERVAL until they both reach a constant number
      /*  if (_gameManager._scoreDistance >= 100)
            _spawnInterval =
                _spawnIntervalCoefficient * _interval.Evaluate(_gameManager._scoreDistance);*/
        //SetSpawnInterval(_spawnIntervalCoefficient);

        if (_gameManager.GameStateGetter != GameState.PLAY)
            return;
    }

    public void SpawnObstacle()
    {

        Debug.Log("SPAWN OBS");
        int index = _rand.Next(_obstacles.Count);

        Obstacle obstacle = Instantiate(_obstacles[index], transform.position, Quaternion.identity);
        //obstacle.InitObstacle(false);
        //obstacle.DownwardSpeed = _downwardSpeed;
    }

    public Obstacle SpawnTutorialObstacle()
    {
        Debug.Log("SPAWN TUTORIAL OBS");
        Obstacle obstacle = Instantiate(_obstacles[6], transform.position, Quaternion.identity);

        Debug.Log("SPAWNED OBSTACLE TUT" + obstacle);

        Obstacle.StopObstacle = true; 
        //obstacle.InitObstacle(true);

        return obstacle;
    }
}
