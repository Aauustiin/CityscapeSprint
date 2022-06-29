using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayerState
{
    public void StateFixedUpdate();
    public IPlayerState HandleAction(InputAction.CallbackContext value);
    public void OnEntry();
    public void OnExit();
}
