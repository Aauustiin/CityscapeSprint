using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpingState : IPlayerState
{
    private PlayerController player;
    private bool actionBuffer;
    private bool jumped;
    private bool actionCommitted;
    private float JUMP_FORCE;

    public JumpingState(PlayerController player, float jumpForce = 7.5f)
    {
        this.player = player;
        this.JUMP_FORCE = jumpForce;
        jumped = false;
        actionCommitted = false;
    }

    public void StateFixedUpdate() {}

    public IPlayerState HandleAction(InputAction.CallbackContext value)
    {
        if (value.started && !actionCommitted)
        {
            actionBuffer = true;
            actionCommitted = true;
            player.StartCoroutine(clearAfterSeconds(0.2f));
        }
        else if (value.canceled)
        {
            if (player.GetComponent<Rigidbody2D>().velocity.y > 0f)
            {
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, player.GetComponent<Rigidbody2D>().velocity.y * player.JUMP_FALLOFF);
            }
        }
        
        return this;
    }

    public void OnLand()
    {
        IPlayerState newState;
        if (actionBuffer)
        {
            newState = new SlidingState(player);
        }
        else
        {
            newState = new RunningState(player);
        }
        player.SwapState(newState);
    }

    public void OnGrab()
    {
        player.SwapState(new GrabbingState(player));
    }

    public void OnEntry()
    {
        player.Grounded += OnLand;
        player.Grab += OnGrab;
        player.GetComponent<Rigidbody2D>().drag = 0;
        player.GetComponent<Animator>().Play("Base Layer.jump", 0, 0);
        if (!jumped)
        {
            jumped = true;
            if (JUMP_FORCE != 0)
            {
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, JUMP_FORCE);
                player.audioSource.PlayOneShot(player.JumpSFX, 0.5f);
            }
        }
    }

    public void OnExit()
    {
        player.Grounded -= OnLand;
        player.Grab -= OnGrab;
        player.GetComponent<Rigidbody2D>().drag = player.GetDrag(); ;
    }

    private IEnumerator clearAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        actionBuffer = false;
    }
}
