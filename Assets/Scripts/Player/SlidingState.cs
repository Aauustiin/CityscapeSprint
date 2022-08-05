using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class SlidingState : IPlayerState
    {
        private readonly PlayerController _player;
        private bool _slid;

        public SlidingState(PlayerController player)
        {
            _player = player;
            _slid = false;
        }

        public void StateFixedUpdate()
        {
            _player.rb.AddForce(_player.runDirection * _player.slideForce);
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

        public void OnEntry()
        {
            _player.LeftGround += OnFall;

            if (!_slid)
            {
                _player.GetComponent<BoxCollider2D>().size = new Vector2(0.08f, 0.04f);
                _player.GetComponent<BoxCollider2D>().offset = new Vector2(0f, -0.02f);
                _player.rb.AddForce(_player.runDirection * _player.rollImpulse, ForceMode2D.Impulse);
                _player.GetComponent<Animator>().Play("Base Layer.slide", 0, 0);
                EventManager.TriggerSoundEffect(_player.slideSfx);
                _slid = true;
            }
        }

        public void OnExit()
        {
            _player.LeftGround -= OnFall;
            _player.GetComponent<BoxCollider2D>().size = new Vector2(0.08f, 0.08f);
            _player.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 0f);
        }
    }
}