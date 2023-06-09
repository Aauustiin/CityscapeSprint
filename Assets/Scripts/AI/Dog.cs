using UnityEngine;

public class Dog : MonoBehaviour
{
    [SerializeField] private float minX, maxX, speed, runSpeed, bumpForce;
    [SerializeField] private bool runOnDeath;
    [SerializeField] private Sprite defeatPose;
    
    [Header("Immutable State")]
    private float _duration, _y;
    private Transform _transform;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    
    [Header("Mutable State")]
    private LTDescr _movementTween;

    private void OnEnable()
    {
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _transform = transform;
        _y = _transform.position.y;
        _transform.position = new Vector3(minX,_y, 0f);
        var distance = Mathf.Abs(maxX - minX);
        _duration = distance / speed;
        MoveRight();
    }
    
    private void MoveRight()
    {
        _movementTween = _transform.LeanMoveLocal(new Vector3(maxX, _y, 0f), _duration);
        _spriteRenderer.flipX = false;
        _movementTween.setOnComplete(MoveLeft);
    }

    private void MoveLeft()
    {
        _movementTween = _transform.LeanMoveLocal(new Vector3(minX, _y, 0f), _duration);
        _spriteRenderer.flipX = true;
        _movementTween.setOnComplete(MoveRight);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = collision.collider.gameObject;
        if (obj.layer != LayerMask.NameToLayer("Player")) return;

        var vec =  (obj.transform.position - _transform.position).normalized;
        var playerRb = obj.GetComponent<Rigidbody2D>();
        _animator.Play("dog_idle");
        
        if (Vector2.Dot(_transform.up, vec) < 0)
        {
            playerRb.AddForce(((vec + _transform.up)/2) * bumpForce, ForceMode2D.Impulse);
            _movementTween.pause();
            StartCoroutine(Utils.ExecuteAfterSeconds(() =>
            {
                _movementTween.resume();
                _animator.Play("dog_run");
            }, 2));
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = false;
            LeanTween.cancel(_movementTween.id);
            var playerSprite = obj.GetComponent<SpriteRenderer>();
            
            var force =  !playerSprite.flipX ? _transform.right : -_transform.right;
            force = (force + _transform.up) / 2;
            playerRb.AddForce(force * bumpForce * 0.8f, ForceMode2D.Impulse);
            
            var destination = playerSprite.flipX ? maxX + 5 : minX - 5;
            var distance = Mathf.Abs(_transform.position.x - destination);
            var duration = distance / runSpeed;

            if (runOnDeath)
            {
                _movementTween = _transform.LeanMoveLocal(new Vector3(destination, _y, 0f), duration);
                _movementTween.setDelay(1);

                var newFlip = !playerSprite.flipX;

                StartCoroutine(Utils.ExecuteAfterSeconds(() =>
                {
                    _animator.Play("dog_run");
                    _spriteRenderer.flipX = newFlip;
                }, 1));
                _movementTween.setOnComplete(() => Destroy(gameObject));
            }
            else
            {
                _transform.LeanScale(Vector3.zero, 0.75f).setEaseInQuart()
                    .setOnComplete(() => Destroy(gameObject));
            }
        }
    }
}
