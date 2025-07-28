using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HeartsButton : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _heartsRemainingText;
    //[SerializeField]
    //TextMeshProUGUI _heartsCostText;
    [SerializeField]
    GameObject _gameOverGroup;


    HeartManager _heartManager;
    GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    private void OnEnable()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _heartManager = FindObjectOfType<HeartManager>();
        _heartsRemainingText.text = _heartManager.GetHearts().ToString();
        //_heartsCostText.text = _heartManager.GetHeartsCost().ToString();

        Button button = GetComponent<Button>();

        if (_gameManager.HeartUse >= _gameManager.MaxHeartUse)
        {
            button.interactable = false;
            //this.gameObject.SetActive(false);
            //GetComponent<Button>().interactable = false;
        }
        else
        {
            button.interactable = true;
            //GetComponent<Button>().interactable = true;
            this.gameObject.SetActive(true);
        }
    }

    public void ConsumeHeart()
    {
        AudioManager.Instance.PlaySFX("UI Click", 0f);

        if (_heartManager.GetHearts() == 0)
        {
            return;
        }

        _heartManager.ConsumeHeart(1);


        //Continue Game here...
        _gameManager.HeartUse += 1;
        _gameManager.ReContinueGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
