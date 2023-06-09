using System.Collections;
using UnityEngine;
using LootLocker.Requests;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private int leaderboardId;
    [SerializeField] private string leaderboardKey;

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
        string playerID = SaveSystem.instance.data.playerId;
        LootLockerSDKManager.SubmitScore(playerID, score, leaderboardKey, (response) =>
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
                SaveSystem.instance.SavePlayerId(response.player_id.ToString());
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

    public static IEnumerator IsPlayerNameSet(System.Action<bool> callback)
    {
        var done = false;
        LootLockerSDKManager.GetPlayerName((response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully fetched player name.");
                if (response.name != "") callback.Invoke(true);
                done = true;
            }
            else
            {
                Debug.Log("Error fetching player name: " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator FetchLeaderboardHighScores(int numEntries, LeaderboardResponse lResponse)
    {
        bool done = false;
        LootLockerSDKManager.GetScoreListMain(leaderboardId, numEntries, 0, (response) => 
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

    public IEnumerator FetchLeaderboardMeScores(int numEntries, LeaderboardResponse lResponse)
    {
        var playerRank = 0;
        bool done = false;
        
        LootLockerSDKManager.GetMemberRank(leaderboardKey, SaveSystem.instance.data.playerId, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully fetched player rank.");
                playerRank = response.rank;
                done = true;
                lResponse.done = true;
            }
            else
            {
                Debug.Log("Error fetching player rank: " + response.Error);
                lResponse.error = true;
                lResponse.done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
        
        if (!lResponse.error)
        {
            done = false;
            var playerPosition = (numEntries + 1) / 2; // Let's say we get 10 entries back, we want the player to be number 6
            var rank = playerRank - playerPosition;
            rank = rank < 0 ? 0 : rank;
            LootLockerSDKManager.GetScoreListMain(leaderboardId, numEntries, rank, (response) =>
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
}

public class LeaderboardResponse
{
    public bool done = false;
    public bool error = false;
    public LootLockerLeaderboardMember[] data;
}