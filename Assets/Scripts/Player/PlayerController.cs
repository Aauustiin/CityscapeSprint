using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Parameters")]
        public float jumpVelocity;
        public float leapVelocity;
        public float jumpFalloff;
        public float runForce;
        public float leapForce;
        public float grabFallSpeed;
        public float slideForce;
        public float rollImpulse;
        public float drag;
        public float coyoteThreshold;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float bumpVelocity;
        [SerializeField] private float slideWindow;

        [Header("Assets")]
        public ParticleSystem dust;
        public AudioClip jumpSfx;
        public AudioClip slideSfx;
        public AudioClip landSfx;
        public AudioClip grabSfx;
        
        [Header("Immutable State")]
        public Rigidbody2D rb;
        public SpriteRenderer sprite;
        private Vector2 _startPosition;
        
        [Header("Mutable State")]
        public Vector2 runDirection;
        public float timeLastGrounded;
        private bool _attemptingSlide;
        [CanBeNull] private Coroutine _slideCancelCoroutine;
        private IPlayerState _currentState;
        private Vector2 _velocityLastFrame;
        private bool _isPaused;

        // Events
        public event System.Action HitGround;
        public event System.Action HitSide;
        public event System.Action LeftGround;
        public event System.Action LeftSide;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            _startPosition = rb.position;
            rb.drag = drag;
            runDirection = Vector2.right;
            _currentState = new RunningState(this);
            _currentState.OnEntry();
        }

        private void OnEnable()
        {
            EventManager.Restart += Restart;
            EventManager.ActionInput += OnAction;
            EventManager.MenuOpen += OnMenuOpen;
            EventManager.MenuClose += OnMenuClose;
        }

        private void OnDisable()
        {
            EventManager.Restart -= Restart;
            EventManager.ActionInput -= OnAction;
            EventManager.MenuOpen -= OnMenuOpen;
            EventManager.MenuClose -= OnMenuClose;
        }

        private void OnMenuOpen()
        {
            _isPaused = true;
        }

        private void OnMenuClose()
        {
            _isPaused = false;
        }

        private void Restart()
        {
            StartCoroutine(Utils.ExecuteWhenTrue(() =>
            {
                rb.velocity = Vector2.zero;
                rb.position = _startPosition;
                rb.drag = drag;
                sprite.flipX = false;
                _velocityLastFrame = Vector2.zero;
                runDirection = Vector2.right;
                SwapState(new RunningState(this));
            }, sprite != null));
        }

        private void FixedUpdate()
        {
            _currentState.StateFixedUpdate();
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
            _velocityLastFrame = rb.velocity;
        }

        private void OnAction(InputAction.CallbackContext value)
        {
            if (!_isPaused)
            {
                IPlayerState newState = _currentState.HandleAction(value);
                SwapState(newState);
            }
        }

        public void SwapState(IPlayerState newState)
        {
            _currentState.OnExit();
            _currentState = newState;
            _currentState.OnEntry();
        }
        
        private void Flip()
        {
            runDirection = -runDirection;
            sprite.flipX = !sprite.flipX;
            rb.velocity = new Vector2(-_velocityLastFrame.x, _velocityLastFrame.y);
        }

        public void AttemptSlide()
        {
            _attemptingSlide = true;
            _slideCancelCoroutine = StartCoroutine(Utils.ExecuteAfterSeconds(() => _attemptingSlide = false, slideWindow));
        }

        public void CancelSlide()
        {
            _attemptingSlide = false;
            if (_slideCancelCoroutine != null) StopCoroutine(_slideCancelCoroutine);
        }

        public bool IsAttemptingSlide()
        {
            return _attemptingSlide;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            string layerName = LayerMask.LayerToName(collision.collider.gameObject.layer);
            Vector2 collisionNormal = collision.GetContact(0).normal;

            switch (layerName)
            {
                case "Ground":
                {
                    if (collisionNormal == Vector2.up)
                    {
                        HitGround?.Invoke();
                        EventManager.TriggerSoundEffect(landSfx);
                        dust.Play();
                    }
                    else if (collisionNormal == -runDirection)
                    {
                        HitSide?.Invoke();
                    }
                    break;
                }
                case "Wall":
                {
                    if (collisionNormal == -runDirection) Flip();
                    break;
                }
                case "Enemy":
                {
                    Vector2 direction = (transform.position - collision.collider.gameObject.transform.position);
                    direction = direction.normalized;
                    rb.velocity = direction * bumpVelocity;
                    break;
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            string layerName = LayerMask.LayerToName(collision.collider.gameObject.layer);

            Vector2 pos = rb.position;
            Vector2 collisionNormal = (pos - collision.collider.ClosestPoint(pos)).normalized;

            if (layerName == "Ground")
            {
                if ((collisionNormal == Vector2.up) || (collisionNormal.y > 0f))
                {
                    timeLastGrounded = Time.time;
                    LeftGround?.Invoke();
                }
                else if ((collisionNormal == Vector2.left) || (collisionNormal == Vector2.right) ||
                         (collisionNormal.y < 0f))
                {
                    LeftSide?.Invoke();
                }
            }
        }
    }
}