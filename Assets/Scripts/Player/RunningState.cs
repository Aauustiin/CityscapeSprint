using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RunningState : IPlayerState
{
    private PlayerController _player;

    public RunningState(PlayerController player)
    {
        this._player = player;
    }

    public void StateFixedUpdate()
    {
        _player.rb.AddForce(_player.runDirection * _player.runForce);
    }

    public IPlayerState HandleAction(InputAction.CallbackContext value)
    {
        IPlayerState returnValue;
        if (value.started)
        {
            returnValue = new JumpingState(_player, _player.jumpVelocity);
        }
        else
        {
            returnValue = this;
        }
        return returnValue;
    }

    private void OnFall()
    {
        _player.SwapState(new JumpingState(_player, 0f));
    }

    private void OnGrab()
    {
        _player.Flip();
    }
    
    public void OnEntry()
    {
        _player.Fell += OnFall;
        _player.Grab += OnGrab;
        _player.GetComponent<Animator>().Play("Base Layer.run", 0, 0);
        _player.StartCoroutine(_player.ExecuteAfterSeconds(() => _player.dust.Stop(), 0.5f));
    }

    public void OnExit()
    {
        _player.Fell -= OnFall;
        _player.Grab -= OnGrab;
    }
}
