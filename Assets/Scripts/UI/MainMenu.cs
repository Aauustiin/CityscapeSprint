using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private LevelLoader levelLoader;
        [SerializeField] private LevelScriptableObject level;
        
        public void QuitGame()
        {
            Application.Quit();
        }

        public void Play()
        {
            levelLoader.LoadLevel(level);
        }

        public void Steam()
        {
            Application.OpenURL("https://store.steampowered.com/app/2071290/Cityscape_Sprint/?beta=0");
        }

        public void Twitter()
        {
            Application.OpenURL("https://twitter.com/AustinLongDev");
        }

        public void Feedback()
        {
            Application.OpenURL("https://forms.gle/v71NgYeZVPvGbg7r8");
        }
    }
}