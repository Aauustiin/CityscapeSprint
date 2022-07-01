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
        player.GetComponent<Rigidbody2D>().AddForce(player.runDirection * player.RUN_FORCE);
    }

    public IPlayerState HandleAction(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            actionBuffer = true;
            player.StartCoroutine(clearAfterSeconds(0.1f));
        }
        else if (value.canceled)
        {
            if (player.GetComponent<Rigidbody2D>().velocity.y > 0f)
            {
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, player.GetComponent<Rigidbody2D>().velocity.y*player.JUMP_FALLOFF);
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
        player.GetComponent<Rigidbody2D>().drag = player.GetDrag(); ;
    }

    private IEnumerator clearAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        actionBuffer = false;
    }
}
