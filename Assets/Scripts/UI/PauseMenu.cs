using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Button firstSelected;

        private void OnEnable()
        {
            firstSelected.Select();
        }

        public void Retry()
        {
            EventManager.TriggerRestart();
        }
    }
}