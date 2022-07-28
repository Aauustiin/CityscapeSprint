using System;
using Player;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UiOffsetFollow : MonoBehaviour
    {
        [SerializeField] private Vector2 offset;
        [SerializeField] private Transform target;
        private RectTransform _rt;

        private void Start()
        {
            _rt = GetComponent<RectTransform>();
        }

        private void Update()
        {
            _rt.position = Camera.main.WorldToScreenPoint((Vector2)target.position + offset);
        }
    }
}
