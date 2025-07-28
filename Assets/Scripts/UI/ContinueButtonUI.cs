using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButtonUI : MonoBehaviour
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

    public void OnMouseUpAsButton()
    {
        AudioManager.Instance.PlaySFX("UI Click", 0f);
        AudioManager.Instance.UnPauseAllsSFX();

        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController.LockInput(false);

        Time.timeScale = 1;

        _pauseMenuUI.SetActive(false);
    }
}
