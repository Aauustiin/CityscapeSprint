using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace UI
{
    public class UiManager : MonoBehaviour
    {
        private Stack<GameObject> _menuHistory;

        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private GameObject soundMenu;
        [SerializeField] private GameObject videoMenu;
        [SerializeField] private GameObject controlsMenu;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject finishMenu;
        [SerializeField] private GameObject commonBackground;

        private void OnEnable()
        {
            EventManager.GameOver += OpenFinishMenu;
        }

        private void OnDisable()
        {
            EventManager.GameOver -= OpenFinishMenu;
        }

        private void Start()
        {
            _menuHistory = new Stack<GameObject>();
            _menuHistory.Push(mainMenu);
        }
        
        private void OpenMenu(GameObject menu)
        {
            if (_menuHistory.Count > 0) 
                _menuHistory.Peek().SetActive(false);
            else
                EventManager.TriggerMenuOpen();
            menu.SetActive(true);
            _menuHistory.Push(menu);
            Time.timeScale = 0f;
            commonBackground.SetActive(true);
        }

        public void CloseMenu()
        {
            _menuHistory.Pop().SetActive(false);
            Time.timeScale = 1f;
            commonBackground.SetActive(false);
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
        
        public void OpenSettingsMenu()
        {
            OpenMenu(settingsMenu);
        }

        public void OpenMainMenu()
        {
            OpenMenu(mainMenu);
        }

        public void OpenSoundMenu()
        {
            OpenMenu(soundMenu);
        }

        public void OpenVideoMenu()
        {
            OpenMenu(videoMenu);
        }

        public void OpenControlsMenu()
        {
            OpenMenu(controlsMenu);
        }

        public void OnPause()
        {
            if (_menuHistory.Count == 0)
                OpenMenu(pauseMenu);
            else if (_menuHistory.Peek() == pauseMenu)
                CloseMenu();
            else if (_menuHistory.Peek() != mainMenu)
                Back();
        }
        
        public void OpenFinishMenu()
        {
            OpenMenu(finishMenu);
        }
    }
}