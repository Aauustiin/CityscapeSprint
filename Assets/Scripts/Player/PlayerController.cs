using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private IPlayerState currentState;

    public float JUMP_VELOCITY;
    public float LEAP_VELOCITY;
    public float JUMP_FALLOFF;
    public float RUN_FORCE;
    public float ROLL_FORCE;
    [SerializeField] private float DRAG;
    [SerializeField] private float maxSpeed;
    
    public AudioSource audioSource;
    private Vector2 velocityLastFrame;
    public Vector2 runDirection;
    public ParticleSystem Dust;
    public AudioClip JumpSFX;
    public AudioClip SlideSFX;
    public AudioClip LandSFX;
    public AudioClip GrabSFX;

    void Start()
    {
        GetComponent<Rigidbody2D>().drag = DRAG;
        runDirection = Vector2.right;
        currentState = new RunningState(this);
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
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().position = new Vector2(-5.5f, -4.5f);
        GetComponent<Rigidbody2D>().drag = DRAG;
        GetComponent<SpriteRenderer>().flipX = false;
        
        velocityLastFrame = Vector2.zero;
        runDirection = Vector2.right;
        SwapState(new RunningState(this));
    }

    void FixedUpdate()
    {
        currentState.StateFixedUpdate();
        if (GetComponent<Rigidbody2D>().velocity.magnitude > maxSpeed)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * maxSpeed;
        }
        velocityLastFrame = GetComponent<Rigidbody2D>().velocity;
    }

    public void OnAction(InputAction.CallbackContext value)
    {
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

    public IEnumerator StopAfterSeconds(ParticleSystem p, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        p.Stop();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string layerName = LayerMask.LayerToName(collision.collider.gameObject.layer);

        if (layerName == "Ground")
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
        }
        else if (layerName == "Wall")
        {
            flip();
        }

        lastSurfaceTouched = collision;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        string layerName = LayerMask.LayerToName(lastSurfaceTouched.collider.gameObject.layer);
        Vector2 collisionNormal = lastContact.normal;

        if ((layerName == "Ground") && (collisionNormal == Vector2.up))
        {
            Fell?.Invoke();
        } 
        else if ((layerName == "Ground") && ( (collisionNormal == Vector2.right) || (collisionNormal == Vector2.left)))
        {
            LetGo?.Invoke();
        }
    }

    private void flip()
    {
        runDirection = -runDirection;
        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        GetComponent<Rigidbody2D>().velocity = new Vector2(-velocityLastFrame.x, velocityLastFrame.y);
    }

    public float GetDrag()
    {
        return DRAG;
    }
}
