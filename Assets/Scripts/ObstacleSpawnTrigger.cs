using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawnTrigger : MonoBehaviour
{

    [SerializeField]
    private ObstacleSpawner _obstacleSpawner;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("SPAWN TRIGGER collide something");
        /*Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
        if (obstacle == null)
            return;*/

        if(collision.tag != "ObstacleSpawnTrigger")
        {
            return;
        }
        Debug.Log("SPAWN TRIGGER collide obstacle");
        _obstacleSpawner.SpawnObstacle();
    }
}
