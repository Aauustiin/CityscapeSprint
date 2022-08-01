using System.Collections.Generic;
using Collectables;
using TMPro;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    [SerializeField] private List<Vector2> spawnLocations;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private RectTransform scoreImage;

    private int _score;

    private void Start()
    {
        ResetScore();
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
        ResetScore();
    }

    public void OnCollectableGrabbed(Collectable c)
    {
        _score++;
        scoreText.text = _score.ToString();
        scoreImage.sizeDelta = new Vector2(scoreText.GetRenderedValues().x + 50f, scoreImage.sizeDelta.y);
        
        FindObjectOfType<Timer>().AddExtraTime(3);

        GameObject collectableParent = c.transform.parent.gameObject;
        Vector2 oldLocation = collectableParent.transform.position;
        Vector2 newLocation = PickRandomSpawnLocation();
        collectableParent.transform.position = newLocation;
        spawnLocations.Remove(newLocation);
        spawnLocations.Add(oldLocation);
    }

    private Vector2 PickRandomSpawnLocation()
    {
        System.Random random = new System.Random();
        int index = random.Next(spawnLocations.Count);
        return spawnLocations[index];
    }

    private void ResetScore()
    {
        _score = 0;
        scoreText.text = "0";
        scoreImage.sizeDelta = new Vector2(100f, scoreImage.sizeDelta.y);
    }

    public int GetCollectablesGrabbed()
    {
        return _score;
    }
}