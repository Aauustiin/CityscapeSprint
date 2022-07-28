using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Avoid : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool targetIsUi;
    [SerializeField] private bool iAmUi;
    private Rigidbody2D _rb;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 pos;
        if (iAmUi)
            pos = Camera.main.ScreenToWorldPoint(GetComponent<RectTransform>().position);
        else
            pos = transform.position;

        Vector3 targetPos;
        if (targetIsUi)
            targetPos =Camera.main.ScreenToWorldPoint(target.gameObject.GetComponent<RectTransform>().position);
        else
            targetPos = target.position;
        
        Vector2 displacement = pos - offset - targetPos;
        Vector2 direction = displacement.normalized;
        float distance = displacement.magnitude;
        Vector2 force = direction * (1 / distance);
        _rb.AddForce(force * 100);
    }
}
