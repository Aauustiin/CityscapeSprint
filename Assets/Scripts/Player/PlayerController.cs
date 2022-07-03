using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    
    private IPlayerState _currentState;

    private Vector2 _startPosition;

    public float timeLastGrounded;

    public float jumpVelocity;
    public float leapVelocity;
    public float jumpFalloff;
    public float runForce;
    public float rollImpulse;
    public bool inputCancelledBuff;
    public float drag;
    [SerializeField] private float maxSpeed;
    public float coyoteThreshold;
    
    public AudioSource audioSource;
    private Vector2 _velocityLastFrame;
    public Vector2 runDirection;
    public ParticleSystem dust;
    public AudioClip jumpSfx;
    public AudioClip slideSfx;
    public AudioClip landSfx;
    public AudioClip grabSfx;

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
    }

    private void OnDisable()
    {
        EventManager.Restart -= Restart;
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

    public IEnumerator ExecuteAfterSeconds(System.Action executable, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        executable();
    }
    
    public void OnAction(InputAction.CallbackContext value)
    {
        if (value.canceled)
        {
            inputCancelledBuff = true;
            StartCoroutine(ExecuteAfterSeconds(() => inputCancelledBuff = false, 0.2f));
        }
        IPlayerState newState = _currentState.HandleAction(value);
        SwapState(newState);
    }

    public void SwapState(IPlayerState newState)
    {
        _currentState.OnExit();
        _currentState = newState;
        _currentState.OnEntry();
    }

    public event System.Action Grounded;
    public event System.Action Grab;
    public event System.Action Fell;
    public event System.Action LetGo;

    private Collision2D _lastSurfaceTouched;
    private ContactPoint2D _lastContact;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string layerName = LayerMask.LayerToName(collision.collider.gameObject.layer);

        switch (layerName)
        {
            case "Ground":
            {
                Vector2 collisionNormal = collision.GetContact(0).normal;
            
                if (collisionNormal == Vector2.up)
                {
                    Grounded?.Invoke();
                    audioSource.PlayOneShot(landSfx, 0.5f);
                    dust.Play();
                
                }
                else if (collisionNormal == -runDirection)
                {
                    Grab?.Invoke();
                    audioSource.PlayOneShot(grabSfx, 0.5f);
                }
                _lastContact = collision.GetContact(0);
                break;
            }
            case "Wall":
                Flip();
                break;
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
                Fell?.Invoke();
            }
            else if ((collisionNormal == Vector2.left) || (collisionNormal == Vector2.right) || (collisionNormal.y < 0f))
            {
                LetGo?.Invoke();
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
