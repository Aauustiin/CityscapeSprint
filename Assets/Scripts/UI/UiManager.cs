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
        [SerializeField] private GameObject levelSelectMenu;
        [SerializeField] private GameObject namePrompt;
        [SerializeField] private GameObject leaderboardMenu;
        [SerializeField] private GameObject demoEndMenu;
        [SerializeField] private GameObject commonBackground;

        [SerializeField] private AudioClip selectSound;
        [SerializeField] private AudioClip interactSound;

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

        public void PlaySelectSound()
        {
            EventManager.TriggerSoundEffect(selectSound);
        }

        public void PlayInteractSound()
        {
            EventManager.TriggerSoundEffect(interactSound);
        }

        private void OpenMenu(GameObject menu)
        {
            if (_menuHistory.Count > 0) 
                _menuHistory.Peek().SetActive(false);
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
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

        public void OpenNamePrompt()
        {
            OpenMenu(namePrompt);
        }

        public void OpenLeaderboardMenu()
        {
            OpenMenu(leaderboardMenu);
        }

        public void OpenDemoEndMenu()
        {
            OpenMenu(demoEndMenu);
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

        public void OpenLevelSelect()
        {
            OpenMenu(levelSelectMenu);
        }
        
        public void OpenFinishMenu()
        {
            OpenMenu(finishMenu);
        }
    }
}