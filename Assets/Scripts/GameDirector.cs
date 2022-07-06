using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerInput;
    [SerializeField] private AudioClip startSfx;
    [SerializeField] private AudioClip mainMenuSfx;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject settingsUI;

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

    public void StartGame()
    {
        SetPlayerName();
        SceneManager.LoadScene("Level", LoadSceneMode.Additive);
        EventManager.TriggerSoundEffect(startSfx);
    }

    public void OpenSettingsMenu()
    {
        mainUI.SetActive(false);
        settingsUI.SetActive(true);
    }
    
    public void LoadMainMenu()
    {
        SceneManager.UnloadSceneAsync("Level");
        Time.timeScale = 1f;
        EventManager.TriggerSoundEffect(mainMenuSfx);
    }
}