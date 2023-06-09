using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardMenu : MonoBehaviour
{
    [SerializeField] private Leaderboard leaderboard;
    [SerializeField] private TextMeshProUGUI[] ranks;
    [SerializeField] private TextMeshProUGUI[] names;
    [SerializeField] private TextMeshProUGUI[] scores;
    [SerializeField] private Button firstSelected;

    private void OnEnable()
    {
        firstSelected.Select();
        OnTop();
    }

    public void OnTop()
    {
        LeaderboardResponse response = new LeaderboardResponse();
        StartCoroutine(leaderboard.FetchLeaderboardHighScores(8, response));
        StartCoroutine(DisplayData(response));
    }

    public void OnMe()
    {
        LeaderboardResponse response = new LeaderboardResponse();
        StartCoroutine(leaderboard.FetchLeaderboardMeScores(8, response));
        StartCoroutine(DisplayData(response));
    }

    private IEnumerator DisplayData(LeaderboardResponse response)
    {
        yield return new WaitUntil(() => response.done == true);

        if (response.error) yield break;
        
        for(var i = 0; i < ranks.Length; i++)
        {
            if (i < response.data.Length)
            {
                ranks[i].text = response.data[i].rank.ToString() + ".";
                string playerName = response.data[i].player.name;
                names[i].text = playerName == "" ? response.data[i].member_id : playerName;
                scores[i].text = response.data[i].score.ToString();
            }
            else
            {
                ranks[i].text = "";
                names[i].text = "";
                scores[i].text = "";
            }
        }
    }
}
