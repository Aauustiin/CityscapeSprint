using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class LeapState : IPlayerState
    {
        private readonly PlayerController _player;
        private bool _actionBuffer;
        private bool _jumped;

        public LeapState(PlayerController player)
        {
            _player = player;
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
                _player.StartCoroutine(Utils.ExecuteAfterSeconds(() => _actionBuffer = false, _player.slideWindow));
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

        private void OnHitSide()
        {
            _player.SwapState(new GrabbingState(_player));
        }

        public void OnEntry()
        {
            _player.HitGround += OnLand;
            _player.HitSide += OnHitSide;
            _player.rb.drag = 0;
            _player.GetComponent<Animator>().Play("Base Layer.jump", 0, 0);
            if (!_jumped)
            {
                _player.rb.AddForce(_player.runDirection * -_player.leapForce);
                _player.rb.velocity = new Vector2(_player.rb.velocity.x, _player.leapVelocity);
                _jumped = true;
                EventManager.TriggerSoundEffect(_player.jumpSfx);
            }
        }

        public void OnExit()
        {
            _player.HitGround -= OnLand;
            _player.HitSide -= OnHitSide;
            _player.rb.drag = _player.drag;
            ;
        }
    }
}
