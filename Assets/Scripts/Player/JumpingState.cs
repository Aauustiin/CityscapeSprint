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
    private float jumpVelocity;

    public JumpingState(PlayerController player, float jumpVelocity = 7.5f)
    {
        this.player = player;
        this.jumpVelocity = jumpVelocity;
        jumped = false;
        actionCommitted = false;
    }

    public void StateFixedUpdate() {}

    public IPlayerState HandleAction(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if (Time.time - player.timeLastGrounded < player.coyoteThreshold)
            {
                return new JumpingState(player, 7.5f);
            }
            else if (!actionCommitted)
            {
                actionBuffer = true;
                actionCommitted = true;
                player.StartCoroutine(player.ExecuteAfterSeconds(() => actionBuffer = false, 0.2f));
            }
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
        player.rb.drag = 0;
        player.GetComponent<Animator>().Play("Base Layer.jump", 0, 0);
        player.GetComponent<BoxCollider2D>().size = new Vector2(0.0602237172f,0.0696409717f);
        player.GetComponent<BoxCollider2D>().offset = new Vector2(-5.75240701e-05f, -0.00483418629f);
        
        if (jumped) return;
        
        if (jumpVelocity != 0)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, jumpVelocity);
            player.audioSource.PlayOneShot(player.JumpSFX, 0.5f);
        }
        jumped = true;
    }

    public void OnExit()
    {
        player.Grounded -= OnLand;
        player.Grab -= OnGrab;
        player.rb.drag = player.drag; ;
    }
}
