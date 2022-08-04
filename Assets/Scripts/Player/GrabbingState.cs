using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class GrabbingState : IPlayerState
    {
        private readonly PlayerController _player;
        private float _startTime;

        public GrabbingState(PlayerController player)
        {
            _player = player;
        }

        public void StateFixedUpdate()
        {
            _player.rb.gravityScale = CalculateGravity(_player.grabFallSpeed, Time.time - _startTime);
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

        private void OnLeftSide()
        {
            _player.SwapState(new JumpingState(_player, 0));
        }

        private static float CalculateGravity(double coefficient, double x)
        {
            var dGravity = 0.5 + (0.5 * Math.Tanh((coefficient * x) - 2));
            return (float)dGravity;
        }

        public void OnEntry()
        {
            _player.LeftSide += OnLeftSide;
            _startTime = Time.time;
            _player.GetComponent<Animator>().Play("Base Layer.grab", 0, 0);
            EventManager.TriggerSoundEffect(_player.grabSfx);
            _player.rb.velocity = Vector2.zero;
        }

        public void OnExit()
        {
            _player.LeftSide -= OnLeftSide;
            _player.rb.gravityScale = 1;
        }
    }
}