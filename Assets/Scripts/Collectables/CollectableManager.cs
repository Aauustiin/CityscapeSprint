using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Collectable spawner
// Score tracker
// Combo system

public class CollectableManager : MonoBehaviour
{
    [SerializeField] private List<Vector2> spawnLocations;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private RectTransform scoreImage;
    [SerializeField] private List<GameObject> comboIcons;
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
            _combo--;
            _timerUnderway = false;
            comboIcons[_combo].SetActive(false);
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
        _timerUnderway = false;
    }

    public void OnCollectableGrabbed(Vector2 location, Collectable c)
    {
        StartTimer();
        comboIcons[_combo  - 1].SetActive(true);
        comboIcons[_combo - 1].GetComponent<Rigidbody2D>().position = Camera.main.WorldToScreenPoint(c.transform.position);
        _score += _combo;
        scoreText.text = _score.ToString();

        scoreImage.sizeDelta = new Vector2(scoreText.GetRenderedValues().x + 50f, scoreImage.sizeDelta.y);
        Vector2 spawnLocation = PickRandomSpawnLocation();
        FindObjectOfType<Timer>().AddExtraTime(_combo);

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
        if (_combo != 3) _combo++;
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