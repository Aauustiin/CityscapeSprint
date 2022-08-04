using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameObject coreLevelComponents;
    
    public void StartGame()
    {
        coreLevelComponents.SetActive(true);
    }
    
}
