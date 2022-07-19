using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class LevelSelectMenu : MonoBehaviour
    {
        [SerializeField] private Button firstSelected;
        [SerializeField] private Image[] levelGraphics;
        [SerializeField] private UI.UiManager uiManager;
        [SerializeField] private LevelLoader levelLoader;
        [SerializeField] private TextMeshProUGUI[] highScoreText;

        private void OnEnable()
        {
            firstSelected.Select();

            for(int i = 0; i < levelGraphics.Length; i++)
            {
                bool levelUnlocked = levelLoader.CanLoadLevel(i);
                if (levelUnlocked)
                {
                    levelGraphics[i].color = new Color(levelGraphics[i].color.r, levelGraphics[i].color.g, levelGraphics[i].color.b, 1f);
                    highScoreText[i].color = new Color(highScoreText[i].color.r, highScoreText[i].color.g, highScoreText[i].color.b, 1f);
                }
                else
                {
                    levelGraphics[i].color = new Color(levelGraphics[i].color.r, levelGraphics[i].color.g, levelGraphics[i].color.b, 0.5f);
                    highScoreText[i].color = new Color(highScoreText[i].color.r, highScoreText[i].color.g, highScoreText[i].color.b, 0.5f);
                }
                int score = SaveSystem.Instance.Data.HighScores[i];
                highScoreText[i].text = score.ToString();
            }
        }

        public void LoadLevelOne()
        {
            bool loadedLevel = FindObjectOfType<LevelLoader>().LoadLevel(0);
            if (loadedLevel)
                uiManager.CloseMenu();
        }

        public void LoadLevelTwo()
        {
            bool loadedLevel = FindObjectOfType<LevelLoader>().LoadLevel(1);
            if (loadedLevel)
                uiManager.CloseMenu();
        }

        public void LoadLevelThree()
        {
            bool loadedLevel = FindObjectOfType<LevelLoader>().LoadLevel(2);
            if (loadedLevel)
                uiManager.CloseMenu();
        }

        public void LoadLevelFour()
        {
            bool loadedLevel = FindObjectOfType<LevelLoader>().LoadLevel(3);
            if (loadedLevel)
                uiManager.CloseMenu();
        }

        public void LoadLevelFive()
        {
            bool loadedLevel = FindObjectOfType<LevelLoader>().LoadLevel(4);
            if (loadedLevel)
                uiManager.CloseMenu();
        }

        public void LoadLevelSix()
        {
            bool loadedLevel = FindObjectOfType<LevelLoader>().LoadLevel(5);
            if (loadedLevel)
                uiManager.CloseMenu();
        }

        public void LoadLevelSeven()
        {
            bool loadedLevel = FindObjectOfType<LevelLoader>().LoadLevel(6);
            if (loadedLevel)
                uiManager.CloseMenu();
        }

        public void LoadLevelEight()
        {
            bool loadedLevel = FindObjectOfType<LevelLoader>().LoadLevel(7);
            if (loadedLevel)
                uiManager.CloseMenu();
        }
    }
}