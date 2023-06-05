using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NamePromptMenu : MonoBehaviour
{
    [SerializeField] private Selectable firstSelected;
    [SerializeField] private TMP_InputField nameInput;

    private void OnEnable()
    {
        firstSelected.Select();
    }

    public void OnSubmit()
    {
       StartCoroutine(Leaderboard.SetPlayerName(nameInput.text));
    }
}
