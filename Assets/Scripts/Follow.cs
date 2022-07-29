using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Follow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float strength = 1f;
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
            targetPos = Camera.main.ScreenToWorldPoint(target.gameObject.GetComponent<RectTransform>().position);
        else
            targetPos = target.position;
        
        Vector2 displacement = targetPos - offset - pos;
        Vector2 direction = displacement.normalized;
        float distance = displacement.magnitude;
        Vector2 force = direction * (strength * distance);
        _rb.AddForce(force, ForceMode2D.Impulse);
    }
}