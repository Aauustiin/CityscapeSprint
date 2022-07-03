using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlidingState : IPlayerState
{
    private PlayerController _player;
    private bool _slid;

    public SlidingState(PlayerController player)
    {
        this._player = player;
        _slid = false;
    }

    public void StateFixedUpdate()
    {
        _player.rb.AddForce(_player.runDirection * 5f);
    }

    public IPlayerState HandleAction(InputAction.CallbackContext value)
    {
        IPlayerState returnValue;
        if (value.canceled)
        {
            returnValue = new RunningState(_player);
        }
        else
        {
            returnValue = this;
        }
        return returnValue;
    }

    private void OnFall()
    {
        _player.SwapState(new JumpingState(_player, _player.jumpVelocity));
    }

    private void OnGrab()
    {
        _player.Flip();
    }
    
    public void OnEntry() 
    {
        _player.Fell += OnFall;
        _player.Grab += OnGrab;
        
        if (!_slid)
        {
            _player.GetComponent<BoxCollider2D>().size = new Vector2(0.08f, 0.06f);
            _player.GetComponent<BoxCollider2D>().offset = new Vector2(0f,-0.01f);
            _player.rb.AddForce(_player.runDirection * _player.rollImpulse, ForceMode2D.Impulse);
            _player.GetComponent<Animator>().Play("Base Layer.slide", 0, 0);
            _player.audioSource.PlayOneShot(_player.slideSfx, 0.5f);
            _slid = true;
        }

        if (_player.inputCancelledBuff)
        {
            _player.SwapState(new RunningState(_player));
        }
    }

    public void OnExit() 
    {
        _player.Fell -= OnFall;
        _player.Grab -= OnGrab;
        _player.GetComponent<BoxCollider2D>().size = new Vector2(0.08f, 0.08f);
        _player.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 0f);
    }
}