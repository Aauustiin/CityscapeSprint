using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameObject coreLevelComponents;
    [SerializeField] private GameObject level;
    
    public void StartGame()
    {
        coreLevelComponents.SetActive(true);
        level.SetActive(true);
        EventManager.TriggerRestart();
    }

    public void EndGame()
    {
        coreLevelComponents.SetActive(false);
        level.SetActive(false);
    }
}
