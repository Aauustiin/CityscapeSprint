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

    private void OnLetGo()
    {
        player.SwapState(new JumpingState(player, 0));
    }
    
    public void OnEntry()
    {
        player.LetGo += OnLetGo;
        player.GetComponent<Animator>().Play("Base Layer.grab", 0, 0);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
    }

    public void OnExit()
    {
        player.LetGo -= OnLetGo;
        player.GetComponent<Rigidbody2D>().gravityScale = 1;
    }
}