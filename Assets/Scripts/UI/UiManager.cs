using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class UiManager : MonoBehaviour
    {
        private Stack<GameObject> _menuHistory;
        private bool _transitioning;

        [Header("Animation Settings")]
        [SerializeField] private float curtainDrawDuration;
        [SerializeField] private float curtainClosedDuration;

        [Header("UI Elements")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject finishMenu;
        [SerializeField] private RectTransform[] commonBackground;
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
            if (_transitioning) return;

            if (_menuHistory.Count > 0)
            {
                StartCoroutine(MenuSwitchTransition(menu));
            }
            else
            {
                StartCoroutine(OpenMenuTransition(menu));
            }
        }

        private IEnumerator OpenMenuTransition(GameObject menu)
        {
            _transitioning = true;
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            hud.SetActive(false);
            EventManager.TriggerMenuOpen();
            if (menu == pauseMenu) EventManager.TriggerPause();
            
            foreach (var bg in commonBackground)
            {
                bg.gameObject.SetActive(true);
            }
            menu.SetActive(true);
            _menuHistory.Push(menu);
            
            LeanTween.moveY(menu, -270, 0f);
            LeanTween.moveY(commonBackground[0], -1080, 0f);
            LeanTween.scaleX(commonBackground[1].gameObject, 0f, 0f);
            LeanTween.scaleX(commonBackground[2].gameObject, 0f, 0f);
            
            LeanTween.moveY(menu, 270, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            LeanTween.moveY(commonBackground[0], 0, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            LeanTween.scaleX(commonBackground[1].gameObject, 1f, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            LeanTween.scaleX(commonBackground[2].gameObject, 1f, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            
            yield return new WaitForSecondsRealtime(curtainDrawDuration);
            _transitioning = false;
        }

        private IEnumerator MenuSwitchTransition(GameObject newMenu)
        {
            _transitioning = true;
            LeanTween.scaleX(commonBackground[1].gameObject, 2.1f, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            LeanTween.scaleX(commonBackground[2].gameObject, 2.1f, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            yield return new WaitForSecondsRealtime(curtainDrawDuration + curtainClosedDuration);
            _menuHistory.Peek().SetActive(false);
            newMenu.SetActive(true);
            _menuHistory.Push(newMenu);
            LeanTween.scaleX(commonBackground[1].gameObject, 1f, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            LeanTween.scaleX(commonBackground[2].gameObject, 1f, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            _transitioning = false;
        }

        // This is for when you want to close all menus (Gameplay is starting).
        public void CloseMenu()
        {
            if (_transitioning) return;

            StartCoroutine(CloseMenuTransition());
        }

        private IEnumerator CloseMenuTransition()
        {
            _transitioning = true;

            var menuToClose = _menuHistory.Pop();
            hud.SetActive(true);
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            EventManager.TriggerMenuClose();
            if (menuToClose == pauseMenu) EventManager.TriggerUnPause();
            
            LeanTween.moveY(commonBackground[0], 1080, curtainDrawDuration)
                .setIgnoreTimeScale(true).setEaseOutExpo();;
            LeanTween.moveY(menuToClose, 675, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            LeanTween.scaleX(commonBackground[1].gameObject, 0f, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            LeanTween.scaleX(commonBackground[2].gameObject, 0f, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            
            yield return new WaitForSecondsRealtime(curtainDrawDuration);
            
            LeanTween.moveY(commonBackground[0], 0, 0)
                .setIgnoreTimeScale(true).setEaseOutExpo();;
            LeanTween.moveY(menuToClose, 270, 0).
                setIgnoreTimeScale(true).setEaseOutExpo();
            LeanTween.scaleX(commonBackground[1].gameObject, 1f, 0).
                setIgnoreTimeScale(true).setEaseOutExpo();
            LeanTween.scaleX(commonBackground[2].gameObject, 1f, 0).
                setIgnoreTimeScale(true).setEaseOutExpo();
            
            foreach (var bg in commonBackground)
            {
                bg.gameObject.SetActive(false);
            }
            menuToClose.SetActive(false);
            _menuHistory = new Stack<GameObject>();

            _transitioning = false;
        }

        public void Back()
        {
            if (_transitioning) return;
            
            if (_menuHistory.Count == 1)
                CloseMenu();
            else
            {
                StartCoroutine(BackTransition());
            }
        }

        private IEnumerator BackTransition()
        {
            _transitioning = true;
            LeanTween.scaleX(commonBackground[1].gameObject, 2.1f, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            LeanTween.scaleX(commonBackground[2].gameObject, 2.1f, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            yield return new WaitForSecondsRealtime(curtainDrawDuration + curtainClosedDuration);
            _menuHistory.Pop().SetActive(false);
            _menuHistory.Peek().SetActive(true);
            LeanTween.scaleX(commonBackground[1].gameObject, 1f, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            LeanTween.scaleX(commonBackground[2].gameObject, 1f, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            _transitioning = false;
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