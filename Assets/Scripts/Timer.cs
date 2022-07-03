using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private int duration;
    [SerializeField] private TextMeshProUGUI timer;
    private float _startTime;

    private void OnEnable()
    {
        EventManager.Restart += Restart;
        StartTimer();
    }

    private void OnDisable()
    {
        EventManager.Restart -= Restart;
    }

    private void StartTimer()
    {
        StartCoroutine(GameOverAfterSeconds(duration));
        _startTime = Time.time;
    }

    private void Update()
    {
        float displayTime = duration - (Time.time - _startTime);
        int roundedTime = Mathf.RoundToInt(displayTime);
        timer.text = roundedTime.ToString();
    }

    private IEnumerator GameOverAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        EventManager.TriggerGameOver();
    }

    public void Restart()
    {
        StartTimer();
    }
}
