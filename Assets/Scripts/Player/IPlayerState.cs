using UnityEngine.InputSystem;

namespace Player
{
    public interface IPlayerState
    {
        public void StateFixedUpdate();
        public IPlayerState HandleAction(InputAction.CallbackContext value);
        public void OnEntry();
        public void OnExit();
    }
}
