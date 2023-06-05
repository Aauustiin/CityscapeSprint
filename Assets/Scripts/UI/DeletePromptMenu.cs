using UnityEngine;
using UnityEngine.UI;

public class DeletePromptMenu : MonoBehaviour
{
    [SerializeField] private Button firstSelected;

    private void OnEnable()
    {
        firstSelected.Select();
    }
    
    public void DeleteSaveData()
    {
        SaveSystem.instance.SaveDefaultData();
    }
}
