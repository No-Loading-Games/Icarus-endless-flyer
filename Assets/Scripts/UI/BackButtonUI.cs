using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BackButtonUI : MonoBehaviour
{
    public GameObject shownUI, hiddenUI, gameStartObjects, flyUI;

    public bool isFromMainMenu = true;


    public void ShowUI()
    {
        AudioManager.Instance.PlaySFX("UI Click", 0f);

        if (isFromMainMenu)
        {
            //flyUI.SetActive(false);
            flyUI.SetActive(true);
            FindObjectOfType<BirdBehaviour>().GetComponent<SpriteRenderer>().enabled = false;
            gameStartObjects.transform.DOMoveY(0, 0.6f).OnComplete(() =>
            {
                FindObjectOfType<BirdBehaviour>().GetComponent<SpriteRenderer>().enabled = true;
                FindObjectOfType<BirdBehaviour>().ResetPosition();
                //flyUI.SetActive(true);
            });
        }

        if (PlayerPrefs.HasKey("done-tutorial"))
        {
            PlayerPrefs.SetInt("done-tutorial", 1);
            //firstGame = false;
        }
       
        shownUI.SetActive(true);
        hiddenUI.SetActive(false);


    }



}
