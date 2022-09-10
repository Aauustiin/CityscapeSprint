using System;
using System.Collections;
using UnityEngine;

public class Dog : MonoBehaviour
{
    [SerializeField] private float minX, maxX, speed;
    private float _distance, _duration;

    private void OnEnable()
    {
        transform.position = new Vector3(minX, transform.position.y, 0f);
        _distance = Mathf.Abs(maxX - minX);
        _duration = _distance / speed;
        StartCoroutine(LoopMovement());
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
}
