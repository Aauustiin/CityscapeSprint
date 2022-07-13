using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class FinishMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI targetScoreText;
        [SerializeField] private RawImage targetScoreBackground;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;

        [SerializeField] private LevelLoader levelLoader;

        private void OnEnable()
        {
            int targetScore = levelLoader.GetTargetScore();
            targetScoreText.text = targetScore.ToString();
            
            CollectableManager collectableManager = FindObjectOfType<CollectableManager>();
            int score = collectableManager.GetCollectablesGrabbed();
            scoreText.text = score.ToString();
            int currentLevel = levelLoader.GetCurrentLevel();
            int oldHighScore = SaveSystem.Instance.Data.HighScores[currentLevel];
            int newHighScore;
            if (score > oldHighScore) newHighScore = score;
            else newHighScore = oldHighScore;
            highScoreText.text = "Best: " + newHighScore.ToString();
            bool beatHighScore = newHighScore > oldHighScore;
            if (beatHighScore)
            {
                SaveSystem.Instance.SaveHighScore(currentLevel, newHighScore);
            }
            float opacity;
            if (newHighScore >= targetScore) opacity = 1f;
            else opacity = 0.5f;
            targetScoreBackground.color = new Color(targetScoreBackground.color.r, targetScoreBackground.color.g, targetScoreBackground.color.b, opacity);
        }

        public void Restart()
        {
            EventManager.TriggerRestart();
        }
    }
}