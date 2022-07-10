using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class LevelSelectMenu : MonoBehaviour
    {
        public void LoadLevelOne()
        {
            FindObjectOfType<LevelLoader>().LoadLevel(0);
        }

        public void LoadLevelTwo()
        {
            FindObjectOfType<LevelLoader>().LoadLevel(1);
        }
    }
}