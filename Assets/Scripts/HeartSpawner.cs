using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSpawner : MonoBehaviour
{

    [SerializeField]
    private Heart _heartPrefab;


    public void SpawnHeart()
    {
        Instantiate(_heartPrefab, transform.position, Quaternion.identity);
        Debug.Log("HEART SPAWNED");
    }
}
