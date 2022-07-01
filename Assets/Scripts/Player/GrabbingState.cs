using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabbingState : IPlayerState
{
    private PlayerController player;
    private float startTime;

    public GrabbingState(PlayerController player)
    {
        this.player = player;
    }

    public void StateFixedUpdate()
    {
        player.GetComponent<Rigidbody2D>().gravityScale = calculateGravity(2, Time.time - startTime);
    }

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

    private float calculateGravity(double aggresiveness, double x)
    {
        var dGravity = 0.5 + (0.5*Math.Tanh((aggresiveness*x)-2));
        return (float)dGravity;
    }
    
    public void OnEntry()
    {
        player.LetGo += OnLetGo;
        startTime = Time.time;
        player.GetComponent<Animator>().Play("Base Layer.grab", 0, 0);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void OnExit()
    {
        player.LetGo -= OnLetGo;
        player.GetComponent<Rigidbody2D>().gravityScale = 1;
    }
}