using System.Collections;
using System.Collections.Generic;
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

    public void OnFriends()
    {

    }

    public void OnMe()
    {

    }

    private IEnumerator DisplayData(LeaderboardResponse response)
    {
        yield return new WaitUntil(() => response.done == true);

        if (!response.error)
        {
            for(int i = 0; i < response.data.Length; i++)
            {
                ranks[i].text = response.data[i].rank.ToString() + ".";
                string playerName = response.data[i].player.name;
                names[i].text = playerName == "" ? response.data[i].member_id : playerName;
                scores[i].text = response.data[i].score.ToString();
            }
        }
    }
}
