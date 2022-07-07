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
        private void OnEnable()
        {
            targetScoreText.text = FindObjectOfType<LevelLoader>().GetTargetScore().ToString();
            scoreText.text = FindObjectOfType<CollectableManager>().GetCollectablesGrabbed().ToString();
            // Get highscore from somewhere
            // Set opacity based on high score and target score
        }

        public void Restart()
        {
            EventManager.TriggerRestart();
        }
    }
}