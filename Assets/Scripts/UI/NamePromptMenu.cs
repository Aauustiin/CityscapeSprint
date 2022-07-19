using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class NamePromptMenu : MonoBehaviour
    {
        [SerializeField] private Leaderboard leaderboard;
        [SerializeField] private UiManager uiManager;
        [SerializeField] private TMP_InputField nameInput;

        private void OnEnable()
        {
            nameInput.Select();
        }

        public void OnSubmit()
        {
            string name = nameInput.text;
            if (nameInput.text != "")
            {
                StartCoroutine(Leaderboard.SetPlayerName(nameInput.text));
                uiManager.Back();
            }
        }
    }
}