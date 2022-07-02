using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlidingState : IPlayerState
{
    private PlayerController player;
    private bool slid;

    public SlidingState(PlayerController player)
    {
        this.player = player;
        slid = false;
    }

    public void StateFixedUpdate() {}

    public IPlayerState HandleAction(InputAction.CallbackContext value)
    {
        IPlayerState returnValue;
        if (value.canceled)
        {
            returnValue = new RunningState(player);
        }
        else
        {
            returnValue = this;
        }
        return returnValue;
    }

    private void OnFall()
    {
        player.SwapState(new JumpingState(player, player.JUMP_VELOCITY));
    }

    public void OnEntry() 
    {
        player.Fell += OnFall;
        
        if (!slid)
        {
            player.rb.AddForce(player.runDirection * player.RollImpulse, ForceMode2D.Impulse);
            player.GetComponent<Animator>().Play("Base Layer.slide", 0, 0);
            player.audioSource.PlayOneShot(player.SlideSFX, 0.5f);
            slid = true;
        }

        if (player.inputCancelledBuff)
        {
            player.SwapState(new RunningState(player));
        }
    }

    public void OnExit() 
    {
        player.Fell -= OnFall;
    }
}