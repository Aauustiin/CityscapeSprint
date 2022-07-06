using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UiManager : MonoBehaviour
    {
        private GameObject _currentMenu;

        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private GameObject soundMenu;
        [SerializeField] private GameObject videoMenu;
        [SerializeField] private GameObject controlsMenu;
        [SerializeField] private GameObject pauseMenu;

        private void Start()
        {
            _currentMenu = mainMenu;
        }
        
        private void OpenMenu(GameObject menu)
        {
            _currentMenu.SetActive(false);
            menu.SetActive(true);
            _currentMenu = menu;
        }

        public void CloseMenu()
        {
            _currentMenu.SetActive(false);
            _currentMenu = null;
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

        public void OpenPauseMenu()
        {
            OpenMenu(pauseMenu);
        }
        
    }
}