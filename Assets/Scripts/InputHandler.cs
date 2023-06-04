using UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private UiManager _uiManager;

    private void Start()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        InputActionAsset inputActionAsset = playerInput.actions;

        StartCoroutine(Utils.ExecuteWhenTrue(() => {
            inputActionAsset.LoadBindingOverridesFromJson(SaveSystem.Instance.data.bindings);
        },
        SaveSystem.Instance.finishedInitialising));

        _uiManager = FindObjectOfType<UiManager>();
    }

    public void OnAction(InputAction.CallbackContext value)
    {
        EventManager.TriggerActionInput(value);
    }

    public void OnPause(InputAction.CallbackContext value)
    {
        if (value.performed) _uiManager.OnPause();
    }
}
