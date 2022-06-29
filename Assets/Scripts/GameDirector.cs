using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerInput;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip startSFX;
    [SerializeField] private AudioClip mainMenuSFX;

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
        audioSource.PlayOneShot(startSFX, 0.5f);
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
    [SerializeField] private PlayerController player;

    public void StartGame()
    {
        SetPlayerName();
        timer.SetActive(true);
        collectable.SetActive(true);
        mainMenuUI.SetActive(false);
        player.Restart();
        audioSource.PlayOneShot(startSFX, 0.5f);
    }

    public void LoadMainMenu()
    {
        timer.SetActive(false);
        collectable.SetActive(false);
        mainMenuUI.SetActive(true);
        player.Restart();
        Time.timeScale = 1f;
        audioSource.PlayOneShot(mainMenuSFX, 0.5f);
    }
}