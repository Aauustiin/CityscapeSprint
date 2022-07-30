using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    [SerializeField] private List<Vector2> spawnLocations;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private RectTransform scoreImage;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private AudioClip collectSfx;
    [SerializeField] private ParticleSystem p;
    private int _score;
    private int _combo = 0;
    private float _comboBuffer = 5f;
    private bool _timerUnderway = false;
    private float _startTime;

    private void Start()
    {
        _score = 0;
        scoreText.text = "0";
    }

    private void OnEnable()
    {
        EventManager.Restart += Restart;
        EventManager.GameOver += GameOver;
    }

    private void OnDisable()
    {
        EventManager.Restart -= Restart;
        EventManager.GameOver -= GameOver;
    }

    private void Update()
    {
        if (_timerUnderway && (_comboBuffer <= Time.time - _startTime))
        {
            _combo = 0;
            comboText.transform.parent.gameObject.SetActive(false);
            _timerUnderway = false;
        }
    }

    private void GameOver()
    {
    }
    
    private void Restart()
    {
        _score = 0;
        scoreText.text = "0";
        _combo = 0;
        comboText.text = "0";
        _timerUnderway = false;
    }

    public void OnCollectableGrabbed(Vector2 location, Collectable c)
    {
        StartTimer();
        if (_combo == 1)
        {
            Rigidbody2D[] rbs = comboText.transform.parent.GetComponentsInChildren<Rigidbody2D>();
            Vector2 cPos = Camera.main.WorldToScreenPoint(c.transform.position);
            foreach (Rigidbody2D rb in rbs)
            {
                rb.position = cPos;
            }
        }
        
        _score += _combo;
        comboText.text = _combo.ToString();
        scoreText.text = _score.ToString();

        scoreImage.sizeDelta = new Vector2(scoreText.GetRenderedValues().x + 50f, scoreImage.sizeDelta.y);
        Vector2 spawnLocation = PickRandomSpawnLocation();
        FindObjectOfType<Timer>().AddExtraTime(3);

        p.transform.position = location + new Vector2(0f, 0.25f);
        p.Clear();
        p.Play();
        EventManager.TriggerSoundEffect(collectSfx);

        SpawnCollectable(spawnLocation, c);
        spawnLocations.Remove(spawnLocation);
        spawnLocations.Add(location);
        
    }

    private void StartTimer()
    {
        comboText.transform.parent.gameObject.SetActive(true);
        
        _combo++;
        _startTime = Time.time;
        _timerUnderway = true;
    }

    private Vector2 PickRandomSpawnLocation()
    {
        System.Random random = new System.Random();
        int index = random.Next(spawnLocations.Count);
        return spawnLocations[index];
    }

    private void SpawnCollectable(Vector2 spawnLocation, Collectable c)
    {
        c.transform.position = spawnLocation;
    }

    public int GetCollectablesGrabbed()
    {
        return _score;
    }
}