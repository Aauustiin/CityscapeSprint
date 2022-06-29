using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI tipText;
    [SerializeField] private TextMeshProUGUI playerRankText;
    [SerializeField] private TextMeshProUGUI leaderboardText;
    [SerializeField] private GameObject UI;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gameOverSFX;
    [SerializeField] private List<string> tips;
    private int lastTip;
 

    private void OnEnable()
    {
        EventManager.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        EventManager.GameOver -= OnGameOver;
    }

    public void OnGameOver()
    {
        StartCoroutine(Leaderboard.SubmitPlayerScore(FindObjectOfType<CollectableManager>().GetCollectablesGrabbed()));
        UI.SetActive(true);
        displayPlayerScore();
        System.Random random = new System.Random();
        int index = random.Next(tips.Count);
        if (index == lastTip)
        {
            index++;
        }
        if (index > tips.Count - 1)
        {
            index -= 2;
        }
        tipText.text = tips[index];
        lastTip = index;
        StartCoroutine(Leaderboard.FetchLeaderboardHighScores(3, this));
        StartCoroutine(Leaderboard.FetchPlayerRank(this));
        audioSource.PlayOneShot(gameOverSFX, 0.5f);
    }

    private void displayPlayerScore()
    {
        int score = FindObjectOfType<CollectableManager>().GetCollectablesGrabbed();
        playerScoreText.text = "Score:  " + score;
    }

    public void DisplayPlayerRank(int rank, int highScore)
    {
        playerRankText.text = "You are rank " + rank + " on the leaderboard, with a high score of " + highScore + " points!";
    }

    public void DisplayLeaderboard(Dictionary<string, int> entries)
    {
        string result = "";
        foreach(KeyValuePair<string, int> e in entries)
        {
            result += e.Key + ": " + e.Value + "\n";
        }
        leaderboardText.text = result;
    }

    public void CloseUI()
    {
        UI.SetActive(false);
    }

    public void Restart()
    {
        EventManager.TriggerRestart();
        CloseUI();
    }
}
