using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class FinishMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private Button firstSelected;

        private void OnEnable()
        {
            firstSelected.Select();
            
            CollectableManager collectableManager = FindObjectOfType<CollectableManager>();
            int score = collectableManager.GetCollectablesGrabbed();
            scoreText.text = score.ToString();
            int oldHighScore = SaveSystem.Instance.data.highScore;
            int newHighScore;
            if (score > oldHighScore) newHighScore = score;
            else newHighScore = oldHighScore;
            highScoreText.text = "Best: " + newHighScore;
            bool beatHighScore = newHighScore > oldHighScore;
            if (beatHighScore)
            {
                SaveSystem.Instance.SaveHighScore(newHighScore);
            }
        }

        public void Restart()
        {
            EventManager.TriggerRestart();
        }
    }
}