using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class UiManager : MonoBehaviour
    {
        private Stack<GameObject> _menuHistory;

        [Header("UI Elements")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject finishMenu;
        [SerializeField] private GameObject commonBackground;
        [SerializeField] private GameObject hud;

        [Header("Sounds")]
        [SerializeField] private AudioClip selectSound;
        [SerializeField] private AudioClip interactSound;

        [Header("Input Actions")]
        [SerializeField] private InputActionReference uiNavigateAction;
        [SerializeField] private InputActionReference uiSubmitAction;
        [SerializeField] private InputActionReference uiCancelAction;
        [SerializeField] private InputActionReference uiPointAction;

        private void OnEnable()
        {
            EventManager.GameOver += OpenFinishMenu;
            _menuHistory = new Stack<GameObject>();
            _menuHistory.Push(mainMenu);
        }

        private void OnDisable()
        {
            EventManager.GameOver -= OpenFinishMenu;
        }

        private void Update()
        {
            if (_menuHistory.Count <= 0) return;
            
            if (uiNavigateAction.action.WasPerformedThisFrame() |
                uiSubmitAction.action.WasPerformedThisFrame() |
                uiCancelAction.action.WasPerformedThisFrame())
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if (uiPointAction.action.WasPerformedThisFrame())
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        public void PlaySelectSound()
        {
            EventManager.TriggerSoundEffect(selectSound);
        }

        public void PlayInteractSound()
        {
            EventManager.TriggerSoundEffect(interactSound);
        }

        public void OpenMenu(GameObject menu)
        {
            if (_menuHistory.Count > 0) 
                _menuHistory.Peek().SetActive(false);
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                hud.SetActive(false);
                EventManager.TriggerMenuOpen();
            }
            menu.SetActive(true);
            _menuHistory.Push(menu);
            Time.timeScale = 0f;
            commonBackground.SetActive(true);
        }

        public void CloseMenu()
        {
            _menuHistory.Pop().SetActive(false);
            _menuHistory = new Stack<GameObject>();
            Time.timeScale = 1f;
            commonBackground.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            hud.SetActive(true);
            EventManager.TriggerMenuClose();
        }

        public void Back()
        {
            _menuHistory.Pop().SetActive(false);
            if (_menuHistory.Count == 0)
                CloseMenu();
            else
                _menuHistory.Peek().SetActive(true);
        }
        
        public void OnPause()
        {
            EventManager.TriggerSoundEffect(interactSound);

            if (_menuHistory.Count == 0)
                OpenMenu(pauseMenu);
            else if (_menuHistory.Peek() == pauseMenu)
                CloseMenu();
            else if (_menuHistory.Peek() == mainMenu)
                Application.Quit();
            else if (_menuHistory.Count > 1)
                Back();
        }
        
        private void OpenFinishMenu()
        {
            OpenMenu(finishMenu);
        }
    }
}