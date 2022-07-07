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
    }
}