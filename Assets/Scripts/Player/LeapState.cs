using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeapState : IPlayerState
{
    private PlayerController _player;
    private bool _actionBuffer;
    private bool _jumped;

    public LeapState(PlayerController player)
    {
        this._player = player;
        _jumped = false;
    }

    public void StateFixedUpdate()
    {
        _player.rb.AddForce(_player.runDirection * _player.runForce);
    }

    public IPlayerState HandleAction(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            _actionBuffer = true;
            _player.StartCoroutine(_player.ExecuteAfterSeconds(() => _actionBuffer = false, 0.2f));
        }
        else if (value.canceled)
        {
            Vector2 velocity = _player.rb.velocity;
            
            if (velocity.y > 0f)
            {
                velocity = new Vector2(velocity.x, velocity.y * _player.jumpFalloff);
            }
            
            _player.rb.velocity = velocity;
        }

        return this;
    }

    private void OnLand()
    {
        IPlayerState newState;
        if (_actionBuffer)
        {
            newState = new SlidingState(_player);
        }
        else
        {
            newState = new RunningState(_player);
        }
        _player.SwapState(newState);
    }

    private void OnGrab()
    {
        _player.SwapState(new GrabbingState(_player));
    }

    public void OnEntry()
    {
        _player.Grounded += OnLand;
        _player.Grab += OnGrab;
        _player.GetComponent<Rigidbody2D>().drag = 0;
        _player.GetComponent<Animator>().Play("Base Layer.jump", 0, 0);
        if (!_jumped)
        {
            _player.GetComponent<Rigidbody2D>().AddForce(_player.runDirection * -75f);
            _player.GetComponent<Rigidbody2D>().velocity = new Vector2(_player.GetComponent<Rigidbody2D>().velocity.x, _player.leapVelocity);
            _jumped = true;
            _player.audioSource.PlayOneShot(_player.jumpSfx, 0.5f);
        }
    }

    public void OnExit()
    {
        _player.Grounded -= OnLand;
        _player.Grab -= OnGrab;
        _player.GetComponent<Rigidbody2D>().drag = _player.drag; ;
    }
}
