using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private int duration;
    [SerializeField] private TextMeshProUGUI timer;
    private float _startTime;
    private bool _timerOn = false;

    private void OnEnable()
    {
        EventManager.Restart += Restart;
        EventManager.MenuOpen += OnMenuOpen;
        EventManager.MenuClose += OnMenuClose;
        StartTimer();
    }

    private void OnDisable()
    {
        EventManager.Restart -= Restart;
        EventManager.MenuOpen -= OnMenuOpen;
        EventManager.MenuClose -= OnMenuClose;
    }

    private void StartTimer()
    {
        _startTime = Time.time;
        _timerOn = true;
    }

    private void Update()
    {
        float displayTime = duration - (Time.time - _startTime);
        if (_timerOn && displayTime > 0)
        {
            int roundedTime = Mathf.RoundToInt(displayTime);
            timer.text = roundedTime.ToString();
        }
        else if (_timerOn && displayTime < 0)
        {
            _timerOn = false;
            EventManager.TriggerGameOver();
        }
    }

    private void OnMenuOpen()
    {
        timer.gameObject.SetActive(false);
    }

    private void OnMenuClose()
    {
        timer.gameObject.SetActive(true);
    }
    
    private void Restart()
    {
        StartTimer();
    }
}