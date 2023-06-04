using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class Dog : MonoBehaviour
{
    [SerializeField] private float minX, maxX, speed;
    private float _distance, _duration;
    [CanBeNull] private Coroutine _movementCoroutine;

    private void OnEnable()
    {
        transform.position = new Vector3(minX, transform.position.y, 0f);
        _distance = Mathf.Abs(maxX - minX);
        _duration = _distance / speed;
        _movementCoroutine = StartCoroutine(LoopMovement());
    }

    private IEnumerator LoopMovement()
    {
        while (true)
        {
            transform.LeanMoveLocal(new Vector3(maxX, transform.position.y, 0f), _duration);
            yield return new WaitForSeconds(_duration);
            GetComponent<SpriteRenderer>().flipX = true;
            transform.LeanMoveLocal(new Vector3(minX, transform.position.y, 0f), _duration);
            yield return new WaitForSeconds(_duration);
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private IEnumerator Pounce()
    {
        if (_movementCoroutine != null) StopCoroutine(_movementCoroutine);
        // Go to stationary pose
        // Wait a moment
        yield return new WaitForSeconds(0.2f);
        // Go to jump pose
        // Do Jump
        // When I land, Pause a moment and then go back to moving normally.
    }
}
