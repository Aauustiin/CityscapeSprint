using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI tipText;
    [SerializeField] private TextMeshProUGUI playerRankText;
    [SerializeField] private TextMeshProUGUI leaderboardText;
    [SerializeField] private GameObject ui;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gameOverSfx;
    [SerializeField] private List<string> tips;
    private int _lastTip;

    private void Start()
    {
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
    }

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
        ui.SetActive(true);
        DisplayPlayerScore();
        System.Random random = new System.Random();
        int index = random.Next(tips.Count);
        if (index == _lastTip)
        {
            index++;
        }
        if (index > tips.Count - 1)
        {
            index -= 2;
        }
        tipText.text = tips[index];
        _lastTip = index;
        StartCoroutine(Leaderboard.FetchLeaderboardHighScores(3, this));
        StartCoroutine(Leaderboard.FetchPlayerRank(this));
        EventManager.TriggerSoundEffect(gameOverSfx);
    }

    private void DisplayPlayerScore()
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
        ui.SetActive(false);
    }

    public void ExitLevel()
    {
        CloseUI();
        FindObjectOfType<GameDirector>().LoadMainMenu();
        FindObjectOfType<UiManager>().OpenMainMenu();
    }
    
    public void Restart()
    {
        EventManager.TriggerRestart();
        CloseUI();
    }
}
