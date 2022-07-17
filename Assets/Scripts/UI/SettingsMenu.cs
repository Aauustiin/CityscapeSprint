using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private Button firstSelected;

        private void OnEnable()
        {
            firstSelected.Select();
        }
    }
}

