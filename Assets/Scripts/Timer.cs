using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private int duration;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private UnityEngine.UI.Slider timeBar;
    private float _startTime;
    private bool _timerOn = false;
    public float extraTime;

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
        extraTime = 0;
        _startTime = Time.time;
        _timerOn = true;
    }

    private void Update()
    {
        float displayTime = duration + extraTime - (Time.time - _startTime);
        float fractionTimeLeft = displayTime / duration;
        if (_timerOn && displayTime > 0)
        {
            timeBar.value = fractionTimeLeft;
            int roundedTime = Mathf.RoundToInt(displayTime);
            timer.text = roundedTime.ToString();
        }
        else if (_timerOn && displayTime < 0)
        {
            _timerOn = false;
            EventManager.TriggerGameOver();
        }
    }

    public void AddExtraTime(int time)
    {
        if ((duration + extraTime + time - (Time.time - _startTime)) <= duration)
        {
            extraTime += time;
        }
        else
        {
            _startTime = Time.time;
            extraTime = 0;
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