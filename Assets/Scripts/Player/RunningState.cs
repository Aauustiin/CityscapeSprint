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
        player.rb.AddForce(player.runDirection * player.RUN_FORCE);
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
        player.GetComponent<BoxCollider2D>().size = new Vector2(0.0602237172f,0.0696409717f);
        player.GetComponent<BoxCollider2D>().offset = new Vector2(-5.75240701e-05f, -0.00483418629f);
    }

    public void OnExit()
    {
        player.Fell -= OnFall;
    }
}
