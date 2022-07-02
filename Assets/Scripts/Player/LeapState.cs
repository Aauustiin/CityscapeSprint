using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeapState : IPlayerState
{
    private PlayerController player;
    private bool actionBuffer;
    private bool jumped;

    public LeapState(PlayerController player)
    {
        this.player = player;
        jumped = false;
    }

    public void StateFixedUpdate()
    {
        player.rb.AddForce(player.runDirection * player.RUN_FORCE);
    }

    public IPlayerState HandleAction(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            actionBuffer = true;
            player.StartCoroutine(player.ExecuteAfterSeconds(() => actionBuffer = false, 0.2f));
        }
        else if (value.canceled)
        {
            Vector2 velocity = player.rb.velocity;
            
            if (velocity.y > 0f)
            {
                velocity = new Vector2(velocity.x, velocity.y * player.JUMP_FALLOFF);
            }
            
            player.rb.velocity = velocity;
        }

        return this;
    }

    private void OnLand()
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

    private void OnGrab()
    {
        player.SwapState(new GrabbingState(player));
    }

    public void OnEntry()
    {
        player.Grounded += OnLand;
        player.Grab += OnGrab;
        player.GetComponent<Rigidbody2D>().drag = 0;
        player.GetComponent<Animator>().Play("Base Layer.jump", 0, 0);
        player.GetComponent<BoxCollider2D>().size = new Vector2(0.0602237172f,0.0696409717f);
        player.GetComponent<BoxCollider2D>().offset = new Vector2(-5.75240701e-05f, -0.00483418629f);
        if (!jumped)
        {
            player.GetComponent<Rigidbody2D>().AddForce(player.runDirection * -75f);
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, player.LEAP_VELOCITY);
            jumped = true;
            player.audioSource.PlayOneShot(player.JumpSFX, 0.5f);
        }
    }

    public void OnExit()
    {
        player.Grounded -= OnLand;
        player.Grab -= OnGrab;
        player.GetComponent<Rigidbody2D>().drag = player.drag; ;
    }
}
