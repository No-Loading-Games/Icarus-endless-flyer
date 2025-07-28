using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject _gameOverUI;
    [SerializeField]
    private bool isPaused = false;
    [SerializeField]
    private GameObject _pauseUI;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Replay()
    {
        AudioManager.Instance.PlaySFX("UI Click", 0f);

        StartCoroutine(FindObjectOfType<GameManager>().Replay());
        if (!isPaused)
            _gameOverUI.GetComponent<Animator>().CrossFade("GameOverUIDashUp", 0.1f);

        Time.timeScale = 1;
    }

/*    public void Retry()
    {
        AudioManager.Instance.PlaySFX("UI Click", 0f);

        FindObjectOfType<GameManager>().Retry();
        Time.timeScale = 1;
    }*/
}
