using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabbingState : IPlayerState
{
    private PlayerController player;

    public GrabbingState(PlayerController player)
    {
        this.player = player;
    }

    public void StateFixedUpdate() {}

    public IPlayerState HandleAction(InputAction.CallbackContext value)
    {
        IPlayerState returnValue;
        if (value.started)
        {
            returnValue = new LeapState(player);
        }
        else
        {
            returnValue = this;
        }
        return returnValue;
    }

    public void OnEntry()
    {
        player.GetComponent<Animator>().Play("Base Layer.grab", 0, 0);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    public void OnExit()
    {
        player.GetComponent<Rigidbody2D>().gravityScale = 1;
    }
}