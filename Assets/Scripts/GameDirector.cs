using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerInput;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip startSfx;
    [SerializeField] private AudioClip mainMenuSfx;

    private void Start()
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

    private static void GameOver()
    {
        Time.timeScale = 0f;
    }

    private void Restart()
    {
        Time.timeScale = 1f;
        EventManager.TriggerSoundEffect(startSfx);
    }

    private void SetPlayerName()
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
        SceneManager.LoadScene("Level", LoadSceneMode.Additive);
        mainMenuUI.SetActive(false);
        EventManager.TriggerSoundEffect(startSfx);
    }

    public void LoadMainMenu()
    {
        SceneManager.UnloadSceneAsync("Level");
        mainMenuUI.SetActive(true);
        Time.timeScale = 1f;
        EventManager.TriggerSoundEffect(mainMenuSfx);
    }
}