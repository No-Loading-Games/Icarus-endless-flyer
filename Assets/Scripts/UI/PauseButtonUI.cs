using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenuUI;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        AudioManager.Instance.PlaySFX("UI Click", 0f);
        AudioManager.Instance.PauseAllsSFX();

        PlayerController playerController = FindObjectOfType <PlayerController>();
        playerController.LockInput(true);

        Time.timeScale = 0;

        //if(_playerController == null)
        
        _pauseMenuUI.SetActive(true);
    }

}
