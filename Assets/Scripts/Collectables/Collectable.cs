using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private CollectableManager _collectableManager;
    [SerializeField] private ParticleSystem p;

    private void OnEnable()
    {
        _collectableManager = FindObjectOfType<CollectableManager>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        p.transform.position = transform.position;
        p.Play();
        _collectableManager.OnCollectableGrabbed(transform.position, this);
    }
}
