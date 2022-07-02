using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RunningState : IPlayerState
{
    private PlayerController player;

    public RunningState(PlayerController player)
    {
        this.player = player;
    }

    public void StateFixedUpdate()
    {
        player.GetComponent<Rigidbody2D>().AddForce(player.runDirection * player.RUN_FORCE);
    }

    public IPlayerState HandleAction(InputAction.CallbackContext value)
    {
        IPlayerState returnValue;
        if (value.started)
        {
            returnValue = new JumpingState(player, player.JUMP_VELOCITY);
        }
        else
        {
            returnValue = this;
        }
        return returnValue;
    }

    public void OnFall()
    {
        player.SwapState(new JumpingState(player, 0f));
    }

    public void OnEntry()
    {
        player.Fell += OnFall;
        player.GetComponent<Animator>().Play("Base Layer.run", 0, 0);
        player.StartCoroutine(player.ExecuteAfterSeconds(() => player.Dust.Stop(), 0.5f));
    }

    public void OnExit()
    {
        player.Fell -= OnFall;
    }
}
