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
    
    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadSceneAsync(levels[levelIndex].scene, LoadSceneMode.Additive);
        currentLevel = levelIndex;
    }

    public void UnloadLevel()
    {
        SceneManager.UnloadSceneAsync(levels[currentLevel].scene);
    }
    
    public void Continue()
    {
        SceneManager.UnloadSceneAsync(levels[currentLevel].scene);
        currentLevel++;
        if (currentLevel < levels.Length)
        {
            uiManager.CloseMenu();
            SceneManager.LoadSceneAsync(levels[currentLevel].scene, LoadSceneMode.Additive);
        }
        else
        {
            uiManager.OpenDemoEndMenu();
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
