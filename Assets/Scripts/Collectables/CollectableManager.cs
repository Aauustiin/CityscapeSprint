using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectableManager : MonoBehaviour
{
    [SerializeField] private List<Vector2> spawnLocations;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject scoreTextParent;
    private int score;
    [SerializeField] private AudioClip collectSfx;
    [SerializeField] private ParticleSystem p;
    private int _combo = 0;
    private float _comboBuffer = 5f;
    private bool _timerUnderway = false;
    private float _startTime;

    private void Start()
    {
        score = 0;
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
        if (_timerUnderway && (_comboBuffer > Time.time - _startTime))
        {
            float timePassedRatio = (_comboBuffer + _startTime - Time.time) / _comboBuffer;
            scoreTextParent.transform.localScale = Vector3.one * timePassedRatio;
        }
        else if (_timerUnderway && (_comboBuffer <= Time.time - _startTime))
        {
            _combo = 0;
            _timerUnderway = false;
            scoreTextParent.SetActive(false);
        }
    }

    private void GameOver()
    {
        scoreTextParent.SetActive(false);
    }
    
    private void Restart()
    {
        score = 0;
        _combo = 0;
        _timerUnderway = false;
        scoreTextParent.SetActive(false);
    }

    public void OnCollectableGrabbed(Vector2 location, Collectable c)
    {
        StartTimer();
        score += _combo;
        Vector2 spawnLocation = PickRandomSpawnLocation();
        FindObjectOfType<Timer>().extraTime += _combo;

        ActivateComboText(c);
        p.transform.position = location;
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

    private void ActivateComboText(Collectable c)
    {
        scoreTextParent.SetActive(true);
        scoreText.text = _combo.ToString();
        RectTransform sTPRT = scoreTextParent.GetComponent<RectTransform>();
        sTPRT.position = Camera.main.WorldToScreenPoint((Vector2)c.transform.position + new Vector2(0f, 0.5f));
        sTPRT.LeanMoveY(sTPRT.localPosition.y + 50f, 0.25f);
        //LTDescr thing = sTPRT.LeanScale(new Vector3(1, 1, 1), 5f);
    }

    private IEnumerator ActivateScoreText(Collectable c)
    {
        scoreTextParent.SetActive(true);
        scoreText.text = score.ToString();
        scoreTextParent.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint((Vector2)c.transform.position + new Vector2(0f, 0.5f));
        scoreTextParent.GetComponent<Animator>().Play("Base Layer.score", 0, 0);
        yield return new WaitForSeconds(0.5f);
        scoreTextParent.SetActive(false);
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
        return score;
    }
}
