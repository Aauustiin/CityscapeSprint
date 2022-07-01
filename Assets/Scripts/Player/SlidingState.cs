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

    public void OnFall()
    {
        player.SwapState(new JumpingState(player, player.JUMP_VELOCITY));
    }

    public void OnEntry() 
    {
        player.Fell += OnFall;
        player.GetComponent<Animator>().Play("Base Layer.slide", 0, 0);
        if (!slid)
        {
            player.GetComponent<Rigidbody2D>().AddForce(player.runDirection * player.ROLL_FORCE, ForceMode2D.Impulse);
            slid = true;
            player.audioSource.PlayOneShot(player.SlideSFX, 0.5f);
        }

        if (player.cancelledBuff)
        {
            player.SwapState(new RunningState(player));
        }
    }

    public void OnExit() 
    {
        player.Fell -= OnFall;
    }
}