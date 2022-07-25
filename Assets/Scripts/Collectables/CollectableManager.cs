using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Collectable spawner
// Score tracker
// Combo system

public class CollectableManager : MonoBehaviour
{
    [SerializeField] private List<Vector2> spawnLocations;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private AudioClip collectSfx;
    [SerializeField] private ParticleSystem p;
    private int score;
    private int _combo = 0;
    private float _comboBuffer = 10f;
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
            comboText.gameObject.transform.localScale = Vector3.one * timePassedRatio;
        }
        else if (_timerUnderway && (_comboBuffer <= Time.time - _startTime))
        {
            _combo = 0;
            _timerUnderway = false;
            comboText.gameObject.SetActive(false);
        }
    }

    private void GameOver()
    {
        comboText.gameObject.SetActive(false);
    }
    
    private void Restart()
    {
        score = 0;
        _combo = 0;
        _timerUnderway = false;
        comboText.gameObject.SetActive(false);
    }

    public void OnCollectableGrabbed(Vector2 location, Collectable c)
    {
        StartTimer();
        score += _combo;
        Vector2 spawnLocation = PickRandomSpawnLocation();
        FindObjectOfType<Timer>().AddExtraTime(_combo);

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
        comboText.gameObject.SetActive(true);
        comboText.text = _combo.ToString();
        RectTransform sTPRT = comboText.gameObject.GetComponent<RectTransform>();
        sTPRT.position = Camera.main.WorldToScreenPoint((Vector2)c.transform.position + new Vector2(0f, 0.5f));
        sTPRT.LeanMoveY(sTPRT.localPosition.y + 50f, 1f).setEase(LeanTweenType.easeOutElastic);
        sTPRT.LeanScale(new Vector3(0f, 0f, 0f), 0f);
        sTPRT.LeanScale(new Vector3(1f, 1f, 1f), 0.5f).setEase(LeanTweenType.easeOutElastic);
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

//public class Spawner
//{
//    private List<Vector2> _spawnLocations;
//    private IObjectPool _pool;
//
//    public Spawner(Pool pool, GameObject subject, List<Vector2> spawnLocations)
//    {
//        _spawnLocations = spawnLocations;
//        _pool = pool;
//    }
//
//    public void Spawn()
//    {
//        GameObject spawned = _pool.Instantiate();
//        spawned.transform.position = PickRandomSpawnLocation();
//    }
//
//    private Vector2 PickRandomSpawnLocation()
//    {
//        System.Random random = new System.Random();
//        int index = random.Next(_spawnLocations.Count);
//        return _spawnLocations[index];
//    }
//}
//
//public class Pool : MonoBehaviour
//{
//    private GameObject[] _objects;
//    private int _activeCount;
//    private bool _destroyIfFull = false;
//
//    public Pool(GameObject subject, int size, bool destroyIfFull)
//    {
//        _objects = new GameObject[size];
//        for (int i = 0; i < size; i++)
//        {
//            _objects[i] = GameObject.Instantiate(subject, transform);
//            _objects[i].SetActive(false);
//        }
//        _activeCount = 0;
//        _destroyIfFull = destroyIfFull;
//    }
//
//    public GameObject Instantiate()
//    {
//        if (_activeCount < _objects.Length)
//        {
//            _objects[_activeCount].SetActive(true);
//            _activeCount++;
//            return _objects[_activeCount - 1];
//        }
//        else if (_destroyIfFull)
//        {
//            Destroy(_objects[0]);
//            return Instantiate();
//        }
//        else return null;
//    }
//
//    public void Destroy(GameObject victim)
//    {
//        int victimIndex = System.Array.IndexOf(_objects, victim);
//        (_objects[victimIndex], _objects[_activeCount - 1]) = (_objects[_activeCount - 1], _objects[victimIndex]);
//        _activeCount--;
//        victim.SetActive(false);
//    }
//}