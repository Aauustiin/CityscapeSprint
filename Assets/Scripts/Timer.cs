using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private int duration;
    [SerializeField] private TextMeshProUGUI timer;
    private float startTime;

    private void OnEnable()
    {
        EventManager.Restart += Restart;
        startTimer();
    }

    private void OnDisable()
    {
        EventManager.Restart -= Restart;
    }

    private void startTimer()
    {
        StartCoroutine(gameOverAfterSeconds(duration));
        startTime = Time.time;
    }

    private void Update()
    {
        float displayTime = duration - (Time.time - startTime);
        int roundedTime = Mathf.RoundToInt(displayTime);
        timer.text = roundedTime.ToString();
    }

    private IEnumerator gameOverAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        EventManager.TriggerGameOver();
    }

    public void Restart()
    {
        startTimer();
    }
}
