using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectableManager : MonoBehaviour
{
    [SerializeField] private List<Vector2> spawnLocations;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject scoreTextParent;
    private int collectablesGrabbed;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip collectSFX;

    private void Start()
    {
        collectablesGrabbed = 0;
    }

    private void OnEnable()
    {
        EventManager.Restart += Restart;
    }

    private void OnDisable()
    {
        EventManager.Restart -= Restart;
    }

    public void Restart()
    {
        collectablesGrabbed = 0;
    }

    public void OnCollectableGrabbed(Vector2 location, Collectable c)
    {
        collectablesGrabbed++;
        StartCoroutine(activateScoreText(c));
        Vector2 spawnLocation = pickRandomSpawnLocation();
        spawnCollectable(spawnLocation, c);
        spawnLocations.Remove(spawnLocation);
        audioSource.PlayOneShot(collectSFX, 0.5f);
        spawnLocations.Add(location);
    }

    private IEnumerator activateScoreText(Collectable c)
    {
        scoreTextParent.SetActive(true);
        scoreText.text = collectablesGrabbed.ToString();
        scoreTextParent.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint((Vector2)c.transform.position + new Vector2(0f, 0.5f));
        scoreTextParent.GetComponent<Animator>().Play("Base Layer.score", 0, 0);
        yield return new WaitForSeconds(0.5f);
        scoreTextParent.SetActive(false);
    }

    private Vector2 pickRandomSpawnLocation()
    {
        System.Random random = new System.Random();
        int index = random.Next(spawnLocations.Count);
        return spawnLocations[index];
    }

    private void spawnCollectable(Vector2 spawnLocation, Collectable c)
    {
        c.transform.position = spawnLocation;
    }

    public int GetCollectablesGrabbed()
    {
        return collectablesGrabbed;
    }
}
