using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabbingState : IPlayerState
{
    private PlayerController _player;
    private float _startTime;

    public GrabbingState(PlayerController player)
    {
        this._player = player;
    }

    public void StateFixedUpdate()
    {
        _player.rb.gravityScale = CalculateGravity(2, Time.time - _startTime);
    }

    public IPlayerState HandleAction(InputAction.CallbackContext value)
    {
        IPlayerState returnValue;
        if (value.started)
        {
            returnValue = new LeapState(_player);
        }
        else
        {
            returnValue = this;
        }
        return returnValue;
    }

    private void OnLetGo()
    {
        _player.SwapState(new JumpingState(_player, 0));
    }

    private float CalculateGravity(double aggresiveness, double x)
    {
        var dGravity = 0.5 + (0.5*Math.Tanh((aggresiveness*x)-2));
        return (float)dGravity;
    }

    private void OnGrounded()
    {
        _player.Flip();
        _player.SwapState(new RunningState(_player));
    }
    
    public void OnEntry()
    {
        _player.LetGo += OnLetGo;
        _player.Grounded += OnGrounded;
        _startTime = Time.time;
        _player.GetComponent<Animator>().Play("Base Layer.grab", 0, 0);
        _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void OnExit()
    {
        _player.LetGo -= OnLetGo;
        _player.Grounded -= OnGrounded;
        _player.GetComponent<Rigidbody2D>().gravityScale = 1;
    }
}