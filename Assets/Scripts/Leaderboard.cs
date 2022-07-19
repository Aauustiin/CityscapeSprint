using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;

    private void OnEnable()
    {
        StartCoroutine(Login());
        EventManager.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        EventManager.GameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        CollectableManager cm = FindObjectOfType<CollectableManager>();
        int score = cm.GetCollectablesGrabbed();
        StartCoroutine(SubmitPlayerScore(score));
    }

    public IEnumerator SubmitPlayerScore(int score)
    {
        bool done = false;
        int LeaderboardID = levelLoader.GetLeaderboardId();
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, score, LeaderboardID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully submitted score");
            }
            else
            {
                Debug.Log("Error submitting score: " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public static IEnumerator Login()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully started lootlocker session.");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.Log("Error starting lootlocker session: " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public static IEnumerator SetPlayerName(string playerName)
    {
        bool done = false;
        LootLockerSDKManager.SetPlayerName(playerName, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully set player name.");
                done = true;
            }
            else
            {
                Debug.Log("Error setting player name: " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator FetchLeaderboardHighScores(int numEntries, LeaderboardResponse lResponse)
    {
        bool done = false;
        int LeaderboardID = levelLoader.GetLeaderboardId();
        LootLockerSDKManager.GetScoreListMain(LeaderboardID, numEntries, 0, (response) => 
        {
            if (response.success)
            {
                Debug.Log("Successfully fetched high scores from the leaderboard.");
    
                lResponse.data = response.items;
                done = true;
                lResponse.done = true;
            }
            else
            {
                Debug.Log("Error fetching high scores from the leaderboard: " + response.Error);
                done = true;
                lResponse.error = true;
                lResponse.done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}

public class LeaderboardResponse
{
    public bool done = false;
    public bool error = false;
    public LootLockerLeaderboardMember[] data;
}