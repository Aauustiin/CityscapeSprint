using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private LevelScriptableObject[] levels;
    [SerializeField] private int currentLevel;

    public void LoadLevel(LevelScriptableObject level)
    {
        SceneManager.LoadSceneAsync(level.scene, LoadSceneMode.Additive);
        currentLevel = System.Array.IndexOf(levels, level);
        Debug.Log(currentLevel);
    }

    public void UnloadLevel()
    {
        Debug.Log(currentLevel);
        SceneManager.UnloadSceneAsync(levels[currentLevel].scene);
    }
    
    public void Continue()
    {
        SceneManager.UnloadSceneAsync(levels[currentLevel].scene);
        currentLevel++;
        if (currentLevel >= levels.Length) return;
        SceneManager.LoadSceneAsync(levels[currentLevel].scene, LoadSceneMode.Additive);
    }

    public int GetTargetScore()
    {
        return levels[currentLevel].targetScore;
    }
}
