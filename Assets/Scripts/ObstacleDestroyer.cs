using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    public bool collided = false;

    void Start() { }

    // Update is called once per frame
    void Update() {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collided = true;
        Debug.Log("COLLISION");
        Obstacle obstacle = collision.gameObject.GetComponentInParent<Obstacle>();
        //Changed getting the component of the parent to get the obstacle even when destroyed by zeus

        //If artemis power up is active, the obstacle that holds the powerup will not be destroyed until the powerup duration runs out
        if (obstacle != null && !obstacle.Indestructable)
            Destroy(obstacle.gameObject);
        else if (obstacle != null && obstacle.Indestructable)
            obstacle.toBeDestroyed = true;

  

        //Or Check if it is coin and destroy
        Coin coin = collision.GetComponent<Coin>();

        if (coin != null)
            Destroy(coin.gameObject);

        //Or Check if it is a Powerup and destroy
        Powerup powerup = collision.GetComponent<Powerup>();

        if(powerup != null) Destroy(powerup.gameObject);

        //Check if it is heart and destroy
        Heart heart = collision.GetComponent<Heart>();  
        if(heart != null) Destroy(heart.gameObject);

    }
}
