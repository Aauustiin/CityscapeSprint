using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

        [Header("UI Elements")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject finishMenu;
        [SerializeField] private GameObject nameMenu;
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
            StartCoroutine(Leaderboard.IsPlayerNameSet((result) =>
            {
                if (!result) OpenMenu(nameMenu);
            }));
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
            var menuTfm = menu.GetComponent<RectTransform>();
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            hud.SetActive(false);
            if (menu == pauseMenu) EventManager.TriggerPause();
            
            foreach (var bg in commonBackground)
            {
                bg.gameObject.SetActive(true);
            }
            menu.SetActive(true);
            _menuHistory.Push(menu);
            
            menuTfm.anchoredPosition = new Vector2(0, -Screen.currentResolution.height);
            commonBackground[0].anchoredPosition = new Vector2(0, -Screen.currentResolution.height);
            commonBackground[1].localScale = new Vector3(0, 1, 1);
            commonBackground[2].localScale = new Vector3(0, 1, 1);
            
            LeanTween.value(menu,  new Vector2(0, -Screen.currentResolution.height), Vector2.zero, curtainDrawDuration ).setOnUpdate( 
                (Vector2 val)=>{
                    menuTfm.anchoredPosition = val;
                }
            ).setIgnoreTimeScale(true).setEaseOutExpo();
            
            LeanTween.value(commonBackground[0].gameObject,  new Vector2(0, -Screen.currentResolution.height), Vector2.zero, curtainDrawDuration ).setOnUpdate( 
                (Vector2 val)=>{
                    commonBackground[0].anchoredPosition = val;
                }
            ).setIgnoreTimeScale(true).setEaseOutExpo();
            
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
            yield return new WaitForSecondsRealtime(curtainDrawDuration);
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
            var menuToCloseTfm = menuToClose.GetComponent<RectTransform>();
            hud.SetActive(true);
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (menuToClose == pauseMenu) EventManager.TriggerUnPause();
            
            LeanTween.value(commonBackground[0].gameObject,  Vector2.zero, new Vector2(0,Screen.currentResolution.height), curtainDrawDuration ).setOnUpdate( 
                (Vector2 val)=>{
                    commonBackground[0].anchoredPosition = val;
                }
            ).setIgnoreTimeScale(true).setEaseOutExpo();
            
            LeanTween.value(menuToClose,  Vector2.zero, new Vector2(0,Screen.currentResolution.height), curtainDrawDuration ).setOnUpdate( 
                (Vector2 val)=>{
                    menuToCloseTfm.anchoredPosition = val;
                }
            ).setIgnoreTimeScale(true).setEaseOutExpo();
            
            LeanTween.scaleX(commonBackground[1].gameObject, 0f, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            LeanTween.scaleX(commonBackground[2].gameObject, 0f, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            
            yield return new WaitForSecondsRealtime(curtainDrawDuration);
            
            commonBackground[0].anchoredPosition = Vector2.zero;
            menuToCloseTfm.anchoredPosition = Vector2.zero;
            commonBackground[1].localScale = new Vector3(1, 1, 1);
            commonBackground[2].localScale = new Vector3(1, 1, 1);
            
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
            yield return new WaitForSecondsRealtime(curtainDrawDuration);
            _menuHistory.Pop().SetActive(false);
            _menuHistory.Peek().SetActive(true);
            LeanTween.scaleX(commonBackground[1].gameObject, 1f, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            LeanTween.scaleX(commonBackground[2].gameObject, 1f, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            _transitioning = false;
        }

        public void ReturnToMainMenu()
        {
            if (!_transitioning) StartCoroutine(ReturnToMainMenuTransition());
        }

        private IEnumerator ReturnToMainMenuTransition()
        {
            _transitioning = true;
            var menuToClose = _menuHistory.Pop();
            if (menuToClose == pauseMenu) EventManager.TriggerUnPause();
            LeanTween.scaleX(commonBackground[1].gameObject, 2.1f, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            LeanTween.scaleX(commonBackground[2].gameObject, 2.1f, curtainDrawDuration).
                setIgnoreTimeScale(true).setEaseOutExpo();
            yield return new WaitForSecondsRealtime(curtainDrawDuration);
            menuToClose.SetActive(false);
            _menuHistory = new Stack<GameObject>();
            mainMenu.SetActive(true);
            _menuHistory.Push(mainMenu);
            mainMenu.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
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