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

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip collectSfx;

    private void Start()
    {
        _collectablesGrabbed = 0;
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        EventManager.Restart += Restart;
    }

    private void OnDisable()
    {
        EventManager.Restart -= Restart;
    }

    private void Restart()
    {
        _collectablesGrabbed = 0;
    }

    public void OnCollectableGrabbed(Vector2 location, Collectable c)
    {
        _collectablesGrabbed++;
        StartCoroutine(ActivateScoreText(c));
        Vector2 spawnLocation = PickRandomSpawnLocation();
        SpawnCollectable(spawnLocation, c);
        spawnLocations.Remove(spawnLocation);
        EventManager.TriggerSoundEffect(collectSfx);
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
