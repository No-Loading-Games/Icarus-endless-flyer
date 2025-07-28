using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherUI : MonoBehaviour
{

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateFeatherPosition(float tick, float stamina)
    {
        if(stamina > 0.01f)
        {
            transform.position = new Vector2(1.16f - (Mathf.Round((2.06f * tick) * 100) / 100), transform.position.y);
        }
        else
        {
            transform.position = new Vector2(-0.9f, transform.position.y);
        }

    }
}
