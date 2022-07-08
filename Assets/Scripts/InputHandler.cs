using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public void OnAction(InputAction.CallbackContext value)
    {
        EventManager.TriggerActionInput(value);
    }

    public void OnPause(InputAction.CallbackContext value)
    {
        if (value.performed)
            FindObjectOfType<UiManager>().OnPause();
    }
}
