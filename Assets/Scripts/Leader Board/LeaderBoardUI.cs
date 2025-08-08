using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderBoardUI : MonoBehaviour
{
    public GameObject textPrefab;
    public Transform container;

    public TextMeshProUGUI bestScore;
    public TextMeshProUGUI bestScoreShadow;
    public TextMeshProUGUI bestName;
    public TextMeshProUGUI bestNameShadow;
    public static LeaderBoardUI Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        RefreshLeaderBoard();
    }

    private void OnEnable()
    {
        RefreshLeaderBoard();
    }

    public void RefreshLeaderBoard()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        var leaderboard = FindObjectOfType<LeaderboardManager>().LoadLeaderboard();
        int ctr = 0;

        foreach (var entry in leaderboard)
        {
            ctr++;

            if (ctr == 1)
            {
                bestScore.text = $"{entry.score}";
                bestScoreShadow.text = $"{entry.score}";
                bestName.text = $"{entry.playerName}";
                bestNameShadow.text = $"{entry.playerName}";
            }
            else if(ctr > 1)
            {
                var textObj = Instantiate(textPrefab, container).GetComponent<LeaderBoardEntry>();
                textObj.playerScore.text = $"{entry.score}";
                textObj.playerName.text = $"{entry.playerName}";

                if (ctr == 2)
                    textObj.rank2.SetActive(true);
                else if (ctr == 3)
                    textObj.rank3.SetActive(true);
                else
                    textObj.playerRank.text = ctr.ToString();
            }
        }
    }

    public void AddEntryAndRefresh(string name, int score)
    {
        LeaderboardManager.Instance.AddEntry(name, score);
        RefreshLeaderBoard();
    }
}
