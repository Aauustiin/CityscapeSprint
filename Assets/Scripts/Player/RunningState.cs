using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class RunningState : IPlayerState
    {
        private readonly PlayerController _player;

        public RunningState(PlayerController player)
        {
            _player = player;
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

        public void OnEntry()
        {
            _player.LeftGround += OnFall;
            Animator anim = _player.GetComponent<Animator>();
            if (anim.isActiveAndEnabled)
                _player.GetComponent<Animator>().Play("Base Layer.run", 0, 0);
            _player.StartCoroutine(Utils.ExecuteAfterSeconds(() => _player.dust.Stop(), 0.5f));
        }

        public void OnExit()
        {
            _player.LeftGround -= OnFall;
        }
    }
}
