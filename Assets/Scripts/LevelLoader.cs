using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private LevelScriptableObject[] levels;
    [SerializeField] private int currentLevel;
    [SerializeField] private UI.UiManager uiManager;
    [SerializeField] private bool development;

    public void LoadLevel(LevelScriptableObject level)
    {
        SceneManager.LoadSceneAsync(level.scene, LoadSceneMode.Additive);
        currentLevel = System.Array.IndexOf(levels, level);
    }
    
    public bool LoadLevel(int levelIndex)
    {
        if (CanLoadLevel(levelIndex))
        {
            SceneManager.LoadSceneAsync(levels[levelIndex].scene, LoadSceneMode.Additive);
            currentLevel = levelIndex;
        }
        return CanLoadLevel(levelIndex);
    }

    public bool CanLoadLevel(int levelIndex)
    {
        bool canLoadLevel = false;
        if (levelIndex == 0)
        {
            canLoadLevel = true;
        }
        else if (levels[levelIndex].targetScore <= SaveSystem.Instance.Data.HighScores[levelIndex - 1])
        {
            canLoadLevel = true;
        }
        return canLoadLevel;
    }

    public void UnloadLevel()
    {
        SceneManager.UnloadSceneAsync(levels[currentLevel].scene);
    }
    
    public void Continue()
    {
        if (currentLevel + 1 == levels.Length)
        {
            SceneManager.UnloadSceneAsync(levels[currentLevel].scene);
            uiManager.OpenDemoEndMenu();
        }
        else if (CanLoadLevel(currentLevel + 1))
        {
            SceneManager.UnloadSceneAsync(levels[currentLevel].scene);
            currentLevel++;
            SceneManager.LoadSceneAsync(levels[currentLevel].scene, LoadSceneMode.Additive);
            uiManager.CloseMenu();
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public int GetLevelCount()
    {
        return levels.Length;
    }

    public int GetTargetScore()
    {
        return levels[currentLevel].targetScore;
    }

    public int GetLeaderboardId()
    {
        return development ? levels[currentLevel].testLeaderboardId : levels[currentLevel].leaderboardId;
    }
}
