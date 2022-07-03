using UnityEngine;
using TMPro;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerInput;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip startSfx;
    [SerializeField] private AudioClip mainMenuSfx;

    void Start()
    {
        StartCoroutine(Leaderboard.Login());
    }

    private void OnEnable()
    {
        EventManager.GameOver += GameOver;
        EventManager.Restart += Restart;
    }

    private void OnDisable()
    {
        EventManager.GameOver -= GameOver;
        EventManager.Restart -= Restart;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        audioSource.PlayOneShot(startSfx, 0.5f);
    }

    public void SetPlayerName()
    {
        string playerName = playerInput.text;
        if (playerName != "")
        {
            StartCoroutine(Leaderboard.SetPlayerName(playerName));
        }
    }

    [SerializeField] private GameObject timer, collectable, mainMenuUI;
    [SerializeField] private Player.PlayerController player;

    public void StartGame()
    {
        SetPlayerName();
        timer.SetActive(true);
        collectable.SetActive(true);
        mainMenuUI.SetActive(false);
        player.Restart();
        audioSource.PlayOneShot(startSfx, 0.5f);
    }

    public void LoadMainMenu()
    {
        timer.SetActive(false);
        collectable.SetActive(false);
        mainMenuUI.SetActive(true);
        player.Restart();
        Time.timeScale = 1f;
        audioSource.PlayOneShot(mainMenuSfx, 0.5f);
    }
}