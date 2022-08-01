using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    [SerializeField] private List<Vector2> spawnLocations;
    [SerializeField] private float comboBuffer = 5f;
    
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private RectTransform scoreImage;
    [SerializeField] private AudioClip collectSfx;
    [SerializeField] private ParticleSystem p;

    [SerializeField] private TextMeshProUGUI[] comboText;
    [SerializeField] private GameObject[] comboObject;
    [SerializeField] private GameObject comboMask;
    [SerializeField] private GameObject outlinedComboText;

    [SerializeField] private float moveStrength;

    private int _score;
    private int _combo;
    private bool _timerUnderway = false;
    private float _startTime;

    private void Start()
    {
        _score = 0;
        scoreText.text = "0";
        _combo = 0;
        foreach (var o in comboObject)
        {
            o.SetActive(false);
            //o.transform.position = new Vector3(0f, 600f);
        }
    }

    private void OnEnable()
    {
        EventManager.Restart += Restart;
    }

    private void OnDisable()
    {
        EventManager.Restart -= Restart;
    }

    private void Update()
    {
        // If combo runs out
        if (_timerUnderway && (comboBuffer <= Time.time - _startTime))
        {
            _combo = 0;
            foreach (var cT in comboText)
            {
                cT.text = "0";
            }
            foreach (var o in comboObject)
            {
                o.SetActive(false);
                //o.transform.position = new Vector3(0f, 600f);
            }
            _timerUnderway = false;
        }
    }

    private void Restart()
    {
        _score = 0;
        scoreText.text = "0";
        _combo = 0;
        _timerUnderway = false;
        foreach (var cT in comboText)
        {
            cT.text = "0";
        }
        foreach (var o in comboObject)
        {
            o.SetActive(false);
            //o.transform.position = new Vector3(0f, 600f);
        }
    }

    public void OnCollectableGrabbed(Vector2 location, Collectable c)
    {
        StartTimer();
        _score += _combo;
        foreach (var cT in comboText)
        {
            cT.text = _combo.ToString();
        }

        bool flag = _combo == 1;
        foreach (var o in comboObject)
        {
            o.SetActive(true);
            //if (flag)
                //o.GetComponent<Rigidbody2D>().position = Camera.main.WorldToScreenPoint(c.transform.position);
        }

        LeanTween.cancel(comboMask);
        LeanTween.cancel(outlinedComboText);
        comboMask.LeanMoveLocal(Vector3.zero, 0f);
        outlinedComboText.LeanMoveLocal(Vector3.zero, 0f);
        comboMask.LeanMoveLocal(Vector3.down * moveStrength, comboBuffer);
        outlinedComboText.LeanMoveLocal(Vector3.up * moveStrength, comboBuffer);
        
        scoreText.text = _score.ToString();

        scoreImage.sizeDelta = new Vector2(scoreText.GetRenderedValues().x + 50f, scoreImage.sizeDelta.y);
        Vector2 spawnLocation = PickRandomSpawnLocation();
        FindObjectOfType<Timer>().AddExtraTime(3);

        p.transform.position = location + new Vector2(0f, 0.25f);
        p.Clear();
        //p.Play();
        EventManager.TriggerSoundEffect(collectSfx);

        SpawnCollectable(spawnLocation, c);
        spawnLocations.Remove(spawnLocation);
        spawnLocations.Add(location);
        
    }

    private void StartTimer()
    {
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