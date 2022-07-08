using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        public void Retry()
        {
            EventManager.TriggerRestart();
        }
    }
}