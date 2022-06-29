using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public static class Leaderboard
{
    private const int leaderboardID = 3537;

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

    public static IEnumerator SubmitPlayerScore(int score)
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, score, leaderboardID, (response) =>
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

    public static IEnumerator FetchPlayerRank(GameOverUI UI)
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.GetMemberRank(leaderboardID.ToString(), playerID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully fetched player rank");
                UI.DisplayPlayerRank(response.rank, response.score);
                done = true;
            }
            else
            {
                Debug.Log("Error fetching player rank: " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public static IEnumerator FetchLeaderboardHighScores(int numEntries, GameOverUI UI)
    {
        bool done = false;
        LootLockerSDKManager.GetScoreListMain(leaderboardID, numEntries, 0, (response) => 
        {
            if (response.success)
            {
                Debug.Log("Successfully fetched high scores from the leaderboard.");

                LootLockerLeaderboardMember[] members = response.items;
                Dictionary<string, int> result = new Dictionary<string, int>() { };
                
                foreach(LootLockerLeaderboardMember m in members)
                {
                    string tempName = m.rank + ". ";
                    if (m.player.name != "")
                    {
                        tempName += m.player.name;
                    }
                    else
                    {
                        tempName += m.player.id;
                    }
                    result.Add(tempName, m.score);
                }

                UI.DisplayLeaderboard(result);
                done = true;
            }
            else
            {
                Debug.Log("Error fetching high scores from the leaderboard: " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}
