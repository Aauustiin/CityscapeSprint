using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        // Immutable state
        public Rigidbody2D rb;
        public SpriteRenderer sprite;
        private Vector2 _startPosition;

        // Movement parameters
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
        public float slideWindow;
        [SerializeField] private float maxSpeed;

        // Mutable state.
        private IPlayerState _currentState;
        private Vector2 _velocityLastFrame;
        private Collision2D _lastSurfaceTouched;
        public Vector2 runDirection;
        public float timeLastGrounded;
        public bool inputCancelledBuff;

        // Assets
        public ParticleSystem dust;
        public AudioClip jumpSfx;
        public AudioClip slideSfx;
        public AudioClip landSfx;
        public AudioClip grabSfx;

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
        }

        private void OnDisable()
        {
            EventManager.Restart -= Restart;
            EventManager.ActionInput -= OnAction;
        }

        public void Restart()
        {
            rb.velocity = Vector2.zero;
            rb.position = _startPosition;
            rb.drag = drag;
            sprite.flipX = false;
            _velocityLastFrame = Vector2.zero;
            runDirection = Vector2.right;
            SwapState(new RunningState(this));
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

        public void SetInputCancelledBuff()
        {
            inputCancelledBuff = true;
            StartCoroutine(Utils.ExecuteAfterSeconds(() => inputCancelledBuff = false, slideWindow));
        }

        public void OnAction(InputAction.CallbackContext value)
        {
            IPlayerState newState = _currentState.HandleAction(value);
            SwapState(newState);
        }

        public void SwapState(IPlayerState newState)
        {
            _currentState.OnExit();
            _currentState = newState;
            _currentState.OnEntry();
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
                        EventManager.TriggerSoundEffect(grabSfx);
                    }
                    break;
                }
                case "Wall":
                {
                    if (collisionNormal != Vector2.down)
                        Flip();
                    break;
                }
            }

            _lastSurfaceTouched = collision;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {

            string layerName = LayerMask.LayerToName(_lastSurfaceTouched.collider.gameObject.layer);

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

        public void Flip()
        {
            runDirection = -runDirection;
            sprite.flipX = !sprite.flipX;
            rb.velocity = new Vector2(-_velocityLastFrame.x, _velocityLastFrame.y);
        }
    }
}
