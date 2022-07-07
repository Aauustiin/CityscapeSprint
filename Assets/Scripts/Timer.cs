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
        EventManager.GameOver += GameOver;
        StartTimer();
    }

    private void OnDisable()
    {
        EventManager.Restart -= Restart;
        EventManager.GameOver -= GameOver;
    }

    private void StartTimer()
    {
        StartCoroutine(Utils.ExecuteAfterSeconds(EventManager.TriggerGameOver, duration));
        timer.gameObject.SetActive(true);
        _startTime = Time.time;
    }

    private void Update()
    {
        float displayTime = duration - (Time.time - _startTime);
        int roundedTime = Mathf.RoundToInt(displayTime);
        timer.text = roundedTime.ToString();
    }

    private void GameOver()
    {
        timer.gameObject.SetActive(false);
    }
    
    private void Restart()
    {
        StartTimer();
    }
}