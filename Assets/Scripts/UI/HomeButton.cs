using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeButton : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameOverUI;
    private GameManager _gameManager;

    [SerializeField]
    private bool _willPlayIdleMusic;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Home()
    {
        AudioManager.Instance.PlaySFX("UI Click", 0f);

        if(_willPlayIdleMusic)
            AudioManager.Instance.PlayMusic("Idle");

        _gameManager.Home();
    }
}
