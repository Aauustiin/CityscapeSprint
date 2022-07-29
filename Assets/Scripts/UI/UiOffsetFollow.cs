using System;
using Player;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UiOffsetFollow : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;
        [SerializeField] private Transform target;
        [SerializeField] private float speed;
        
        private RectTransform _rt;

        private void Start()
        {
            _rt = GetComponent<RectTransform>();
        }

        private void Update()
        {
            Vector2 direction = (Vector2)(target.position + offset - Camera.main.ScreenToWorldPoint(_rt.position)).normalized;
            GetComponent<Rigidbody2D>().velocity = direction * speed;
        }
    }
}
