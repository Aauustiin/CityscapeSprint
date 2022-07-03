using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpingState : IPlayerState
{
    private PlayerController _player;
    private bool _actionBuffer;
    private bool _jumped;
    private bool _actionCommitted;
    private float _jumpVelocity;

    public JumpingState(PlayerController player, float jumpVelocity = 7.5f)
    {
        this._player = player;
        this._jumpVelocity = jumpVelocity;
        _jumped = false;
        _actionCommitted = false;
    }

    public void StateFixedUpdate() {}

    public IPlayerState HandleAction(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if (Time.time - _player.timeLastGrounded < _player.coyoteThreshold)
            {
                return new JumpingState(_player, 7.5f);
            }
            else if (!_actionCommitted)
            {
                _actionBuffer = true;
                _actionCommitted = true;
                _player.StartCoroutine(_player.ExecuteAfterSeconds(() => _actionBuffer = false, 0.2f));
            }
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
        _player.rb.drag = 0;
        _player.GetComponent<Animator>().Play("Base Layer.jump", 0, 0);

        if (_jumped) return;
        
        if (_jumpVelocity != 0)
        {
            _player.rb.velocity = new Vector2(_player.rb.velocity.x, _jumpVelocity);
            _player.audioSource.PlayOneShot(_player.jumpSfx, 0.5f);
        }
        _jumped = true;
    }

    public void OnExit()
    {
        _player.Grounded -= OnLand;
        _player.Grab -= OnGrab;
        _player.rb.drag = _player.drag; ;
    }
}
