using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectableManager : MonoBehaviour
{
    [SerializeField] private List<Vector2> spawnLocations;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject scoreTextParent;
    private int _collectablesGrabbed;
    [SerializeField] private AudioClip collectSfx;
    [SerializeField] private ParticleSystem p;

    private void Start()
    {
        _collectablesGrabbed = 0;
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

    private void GameOver()
    {
        scoreTextParent.SetActive(false);
    }
    
    private void Restart()
    {
        _collectablesGrabbed = 0;
    }

    //private IEnumerator MiniFreeze(float seconds)
    //{
    //    Time.timeScale = 0f;
    //    yield return new WaitForSecondsRealtime(seconds);
    //    Time.timeScale = 1f;
    //}

    public void OnCollectableGrabbed(Vector2 location, Collectable c)
    {
        _collectablesGrabbed++;
        Vector2 spawnLocation = PickRandomSpawnLocation();

        //StartCoroutine(MiniFreeze(0.05f));

        StartCoroutine(ActivateScoreText(c));
        p.transform.position = location;
        p.Play();
        EventManager.TriggerSoundEffect(collectSfx);

        SpawnCollectable(spawnLocation, c);
        spawnLocations.Remove(spawnLocation);
        spawnLocations.Add(location); 
    }

    private IEnumerator ActivateScoreText(Collectable c)
    {
        scoreTextParent.SetActive(true);
        scoreText.text = _collectablesGrabbed.ToString();
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
        return _collectablesGrabbed;
    }
}
