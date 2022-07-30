using System;
using Player;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class UiOffsetFollow : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;
        [SerializeField] private Transform target;
        [SerializeField] private float speed;
        private bool _iAmUi;
        private bool _targetIsUi;
        private RectTransform _targetRectTransform, _myRectTransform;
        private Rigidbody2D _rb;

        private void Start()
        {
            _myRectTransform = GetComponent<RectTransform>();
            _iAmUi = _myRectTransform != null;
            _targetRectTransform = target.gameObject.GetComponent<RectTransform>();
            _targetIsUi = _targetRectTransform != null;
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Camera camera = Camera.main;
            Vector3 pos;
            if (_iAmUi)
                pos = camera.ScreenToWorldPoint(_myRectTransform.position);
            else
                pos = transform.position;

            Vector3 targetPos;
            if (_targetIsUi)
                targetPos = camera.ScreenToWorldPoint(_targetRectTransform.position);
            else
                targetPos = target.position;
            
            Vector2 direction = (Vector2)(targetPos + offset - pos).normalized;
            _rb.velocity = direction * speed;
        }
    }
}
