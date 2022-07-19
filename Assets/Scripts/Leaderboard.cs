using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class Leaderboard : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Login());
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
}
