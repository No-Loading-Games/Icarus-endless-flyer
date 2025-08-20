using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

#region Dan Online Leader Board Manager Demo
/*
// NOTE: Make sure to include the following namespace wherever you want to access Leaderboard Creator methods
using Dan.Main;

namespace LeaderboardCreatorDemo
{
    public class LeaderboardManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] _entryTextObjects;
        [SerializeField] private TMP_InputField _usernameInputField;

        // Make changes to this section according to how you're storing the player's score:
        // ------------------------------------------------------------
        //[SerializeField] private ExampleGame _exampleGame;

        //private int _userName;

        //private int Score => _exampleGame.Score;
        // ------------------------------------------------------------

        private void Start()
        {
            LoadEntries();
        }

        private void LoadEntries()
        {
            // Q: How do I reference my own leaderboard?
            // A: Leaderboards.<NameOfTheLeaderboard>

            Leaderboards.DemoSceneLeaderboard.GetEntries(entries =>
            {
                foreach (var t in _entryTextObjects)
                    t.text = "";

                var length = Mathf.Min(_entryTextObjects.Length, entries.Length);
                for (int i = 0; i < length; i++)
                    _entryTextObjects[i].text = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score}";
            });
        }

        public void UploadEntry()
        {
            int score = (int)FindObjectOfType<GameManager>().ScoreDistance;

            Leaderboards.DemoSceneLeaderboard.UploadNewEntry(_usernameInputField.text, score, isSuccessful =>
            {
                if (isSuccessful)
                    LoadEntries();
            });
        }
    }
}
*/
#endregion

public class LeaderboardManager : MonoBehaviour
{
    private const int maxEntries = 10;
    private const string leaderboardKey = "leaderboard";

    private GameManager _gameManager;

    public static LeaderboardManager Instance;

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

        _gameManager = FindObjectOfType<GameManager>();

        //_gameManager.FinalScoreEvent += QualifiesForLeaderboard;
    }

    public List<ScoreEntry> LoadLeaderboard()
    {
        string json = PlayerPrefs.GetString(leaderboardKey, "");
        if (string.IsNullOrEmpty(json)) 
                return new List<ScoreEntry>();
        return JsonUtility.FromJson<LeaderboardListWrapper>(json).entries;
    }

    public void AddEntry(string playerName, int score)
    {
        var leaderboard = LoadLeaderboard();
        leaderboard.Add(new ScoreEntry { playerName = playerName, score = score });

        leaderboard = leaderboard
            .OrderByDescending(entry => entry.score)
            .Take(maxEntries)
            .ToList();

        SaveLeaderboard(leaderboard);
    }

    private void SaveLeaderboard(List<ScoreEntry> leaderboard)
    {
        var wrapper = new LeaderboardListWrapper { entries = leaderboard };
        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(leaderboardKey, json);
        PlayerPrefs.Save();

        //LeaderBoardUI.Instance.RefreshLeaderBoard(this);
    }

    public bool QualifiesForLeaderboard(int score)
    {
        var leaderboard = LoadLeaderboard();

        if (leaderboard.Count < maxEntries)
            return true;

        int lowestScore = leaderboard.Min(entry => entry.score);
        return score > lowestScore;
    }

    public void AddEntryAndRefresh(string name, int score)
    {
        AddEntry(name, score);
        //LeaderBoardUI.Instance.RefreshLeaderBoard();
    }


    [System.Serializable]
    private class LeaderboardListWrapper
    {
        public List<ScoreEntry> entries;
    }



}
