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
    
    private IPlayerState currentState;

    private Vector2 startPosition;

    public float timeLastGrounded;
    
    //struct MovementParameters
    //{
    //    private float jumpVelocity;
    //    private float leapVelocity;
    //    private float runForce;
    //    private float rollImpulse;
    //    private float drag;
    //    private float maxSpeed;
    //}
    
    public float JUMP_VELOCITY;
    public float LEAP_VELOCITY;
    public float JUMP_FALLOFF;
    public float RUN_FORCE;
    public float RollImpulse;
    public bool inputCancelledBuff;
    public float drag;
    [SerializeField] private float maxSpeed;
    public float coyoteThreshold;
    
    public AudioSource audioSource;
    private Vector2 velocityLastFrame;
    public Vector2 runDirection;
    public ParticleSystem Dust;
    public AudioClip JumpSFX;
    public AudioClip SlideSFX;
    public AudioClip LandSFX;
    public AudioClip GrabSFX;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        startPosition = rb.position;
        rb.drag = drag;
        runDirection = Vector2.right;
        currentState = new RunningState(this);
        currentState.OnEntry();
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
        rb.position = startPosition;
        rb.drag = drag;
        sprite.flipX = false;
        velocityLastFrame = Vector2.zero;
        runDirection = Vector2.right;
        SwapState(new RunningState(this));
    }

    private void FixedUpdate()
    {
        currentState.StateFixedUpdate();
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        velocityLastFrame = rb.velocity;
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
        IPlayerState newState = currentState.HandleAction(value);
        SwapState(newState);
    }

    public void SwapState(IPlayerState newState)
    {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEntry();
    }

    public event System.Action Grounded;
    public event System.Action Grab;
    public event System.Action Fell;
    public event System.Action LetGo;

    private Collision2D lastSurfaceTouched;
    private ContactPoint2D lastContact;

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
                    audioSource.PlayOneShot(LandSFX, 0.5f);
                    Dust.Play();
                
                }
                else if (collisionNormal == -runDirection)
                {
                    Grab?.Invoke();
                    audioSource.PlayOneShot(GrabSFX, 0.5f);
                }
                lastContact = collision.GetContact(0);
                break;
            }
            case "Wall":
                flip();
                break;
        }

        lastSurfaceTouched = collision;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
        string layerName = LayerMask.LayerToName(lastSurfaceTouched.collider.gameObject.layer);
        
        Vector2 collisionNormal = (rb.position - collision.collider.ClosestPoint(rb.position)).normalized;

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

    public void flip()
    {
        runDirection = -runDirection;
        sprite.flipX = !sprite.flipX;
        rb.velocity = new Vector2(-velocityLastFrame.x, velocityLastFrame.y);
    }
}
