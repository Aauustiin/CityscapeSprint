using System;
using System.Collections.Generic;
using Collectables;
using TMPro;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    [SerializeField] private List<Vector2> spawnLocations;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private RectTransform scoreMask;

    private int _score;
    private Vector2 _startingPosition;
    private Transform _collectableParent;

    private void Start()
    {
        _collectableParent = GetComponentInChildren<Collectable>().transform.parent;
        _startingPosition = _collectableParent.position;
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
        StartCoroutine(Utils.ExecuteWhenTrue(() =>
        {
            spawnLocations.Add(_collectableParent.position);
            spawnLocations.Remove(_startingPosition);
            _collectableParent.position = _startingPosition;
            ResetScore();
        }, _collectableParent != null));
    }

    public void OnCollectableGrabbed(Collectable c)
    {
        _score++;
        scoreText.text = _score.ToString();
        scoreText.gameObject.LeanScale(Vector3.zero, 0f);
        scoreText.gameObject.LeanScale(Vector3.one, 0.5f).setEase(LeanTweenType.easeOutBounce);
        scoreMask.LeanSize(new Vector2(scoreText.GetRenderedValues().x + 50f, scoreMask.sizeDelta.y), 0.1f);
        
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
        scoreMask.sizeDelta = new Vector2(100f, scoreMask.sizeDelta.y);
    }

    public int GetCollectablesGrabbed()
    {
        return _score;
    }

    public void SetSpawnLocations(List<Vector2> locations)
    {
        spawnLocations = locations;
    }
}