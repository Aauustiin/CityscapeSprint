using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private void Start()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        InputActionAsset inputActionAsset = playerInput.actions;

        StartCoroutine(Utils.ExecuteWhenTrue(() => {
            inputActionAsset.LoadBindingOverridesFromJson(SaveSystem.Instance.Data.Bindings);
        },
        SaveSystem.Instance.FinishedInitialising));
    }

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
