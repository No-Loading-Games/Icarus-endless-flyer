using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WreathHandler : MonoBehaviour
{
    [SerializeField]
    private int[] _scoreMilestones;
    [SerializeField]
    private Sprite[] _wreathSprites;
    [SerializeField]
    private string[] _wreathNames;
    [SerializeField]
    private string[] _wreathDataNames;

    [SerializeField]
    private TextMeshProUGUI _wreathName;
    [SerializeField]
    private Image _wreathImageUI;
    [SerializeField]
    private GameObject _wreathGO;

    [SerializeField]
    private NotificationBarUI _notificationBarUI;

    private List<int> _queuedWreaths;

    [SerializeField]
    private int _displayedWreaths = 0;
    private int _wreathsObtained;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GetComponent<GameManager>();

        //_gameManager.FinalScoreEvent += CheckFinalScore;
        _gameManager.ScoreUpdateEvent += CheckScore;

        _queuedWreaths = new List<int>();

        if (PlayerPrefs.HasKey("wreaths-obtained"))
        {
            _wreathsObtained = PlayerPrefs.GetInt("wreaths-obtained");
            return;
        }
        
        //--------Continue below if player prefs does not have the key "wreaths-obtained"
        _wreathsObtained = 0;
        PlayerPrefs.SetInt("wreaths-obtained", 0);

        foreach (string dataName in _wreathDataNames)
        {
            PlayerPrefs.SetInt(dataName, 0);
            HeadWearManager.Instance.LockHeadWear(dataName);
        }
    }

    /*private void CheckFinalScore(float score)
    {
        for(int i = _wreathsObtained; i < _scoreMilestones.Length; i++)
        {
            if (score < _scoreMilestones[i])
                break;

            Debug.Log("MILESTONE");
            QueueWreathe(_wreathsObtained);

            //PlayerPrefs.SetInt(_wreathDataNames[_wreathsObtained], 1);

            _wreathsObtained++;
            PlayerPrefs.SetInt("wreaths-obtained", _wreathsObtained);

            Debug.Log("MILESTONE ACHIEVED!");

        }

    }*/

    private void CheckScore(float score)
    {
        if (_wreathsObtained >= _scoreMilestones.Length)
            return;
        if(score == _scoreMilestones[_wreathsObtained])
        {
            QueueWreathe(_wreathsObtained);
            
            //---------Call Notification to add sprite and name
            _notificationBarUI.gameObject.SetActive(true);
            _notificationBarUI.ShowNotification(_wreathSprites[_wreathsObtained], _wreathNames[_wreathsObtained] + " Unlocked!");

            //---------Unlock Wreath Head wear to be accessible to be equipped;
            HeadWearManager.Instance.UnlockHeadWear(_wreathNames[_wreathsObtained]);

            _wreathsObtained++;
            PlayerPrefs.SetInt("wreaths-obtained", _wreathsObtained);
            
            Debug.Log("MILESTONE ACHIEVED!");
        }

    }    

    private void QueueWreathe(int wreathIndex)
    {
        //int i = _queuedWreaths.Count; // updates the new length of the queued wreathes to +1
        Debug.Log("MILESTONE! To Queue Wreathe #" + wreathIndex);
        _queuedWreaths.Add(wreathIndex);

        Debug.Log("MILESTONE! Wreath Queued");
    }

    public void CheckWreathsToShow()
    {
        //Disable all images shown
        _wreathGO.SetActive(false);

        if (_wreathsObtained > _scoreMilestones.Length)
            return;

        int index;

        if (_displayedWreaths < _queuedWreaths.Count)
        {
            index = _queuedWreaths[_displayedWreaths];
            ShowWreath(_wreathSprites[index], _wreathNames[index]);
        }
        else
        {
            _wreathGO.SetActive(false);
            _queuedWreaths = null;
            _queuedWreaths = new List<int>();
            _displayedWreaths = 0;
            _gameManager.CheckScoreForLeaderBoard();
        }
        //if all wreathes are shown, show Game Over UI
    }

    private void ShowWreath(Sprite sprite, string name)
    {
        Debug.Log("MILESTONE! Wreathe with name " + name + " shown");
        _wreathGO.SetActive(true);
        _wreathImageUI.sprite = sprite;
        _wreathName.text = name;
        //Show Wreath UI
        //When Screen is touched, trigger CheckWreathesToShow

        _displayedWreaths++;
    }



}
