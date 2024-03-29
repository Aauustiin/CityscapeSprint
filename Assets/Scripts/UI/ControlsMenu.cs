using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class ControlsMenu : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private TextMeshProUGUI actionText;
        [SerializeField] private TextMeshProUGUI pauseText;
        [SerializeField] private Button firstSelected;
        private InputAction _action;
        private InputAction _pause;

        private void OnEnable()
        {
            firstSelected.Select();
            _action = inputActionAsset.FindAction("Action");
            _pause = inputActionAsset.FindAction("Pause");
            actionText.text = _action.controls[0].name;
            pauseText.text = _pause.controls[0].name;
        }

        public void RemapPause()
        {
            StartCoroutine(RemapButtonClicked(_pause));
        }
        
        public void RemapAction()
        {
            StartCoroutine(RemapButtonClicked(_action));
        }
        
        public void Reset()
        {
            SaveSystem.instance.SaveControlSettings("");
            inputActionAsset.LoadBindingOverridesFromJson("");

            _action = inputActionAsset.FindAction("Action");
            _pause = inputActionAsset.FindAction("Pause");
            actionText.text = _action.controls[0].name;
            pauseText.text = _pause.controls[0].name;
        }

        private IEnumerator RemapButtonClicked(InputAction actionToRebind)
        {
            if (actionToRebind == _action)
                actionText.text = "...";
            else pauseText.text = "...";
            actionToRebind.Disable();
            var rebindOperation = actionToRebind.PerformInteractiveRebinding()
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.1f)
                .Start();
            yield return new WaitWhile(() => !rebindOperation.completed);
            actionToRebind.Enable();
            actionText.text = _action.controls[0].name;
            pauseText.text = _pause.controls[0].name;
            rebindOperation.Dispose();
            SaveSystem.instance.SaveControlSettings(inputActionAsset.SaveBindingOverridesAsJson());
        }
    }
}