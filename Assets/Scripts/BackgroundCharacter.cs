using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCharacter : MonoBehaviour
{
    [SerializeField] private float minX, maxX;
    [SerializeField] private float minSpeed, maxSpeed;
    [SerializeField] private float minWait, maxWait;

    private Vector3 _startPosition, _destination;
    private float _speed, _startTime, _distance;
    private bool _walking;
    private bool _waiting;

    private void OnEnable()
    {
        _walking = false;
        _waiting = false;
    }

    private void Update()
    {
        if (_walking)
        {
            transform.position = Vector3.Lerp(_startPosition, _destination, (Time.time - _startTime)/(_distance / _speed));
            _walking = !(transform.position == _destination);
        }
        else if (!_waiting)
        {
            _speed = Random.Range(minSpeed, maxSpeed);
            _destination = new Vector3(Random.Range(minX, maxX), transform.position.y, transform.position.z);
            _distance = Mathf.Abs(transform.position.x - _destination.x);
            GetComponent<Animator>().Play("Base Layer.idle", 0, 0);
            _waiting = true;
            StartCoroutine(Utils.ExecuteAfterSeconds(() => { StartWalk(); }, Random.Range(minWait, maxWait)));
        }
    }

    private void StartWalk()
    {
        GetComponent<SpriteRenderer>().flipX = transform.position.x > _destination.x;
        GetComponent<Animator>().Play("Base Layer.walk", 0, 0);
        _startTime = Time.time;
        _startPosition = transform.position;
        _waiting = false;
        _walking = true;
    }
}
