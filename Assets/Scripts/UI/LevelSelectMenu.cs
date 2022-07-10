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

        public void LoadLevelThree()
        {
            FindObjectOfType<LevelLoader>().LoadLevel(2);
        }

        public void LoadLevelFour()
        {
            FindObjectOfType<LevelLoader>().LoadLevel(3);
        }

        public void LoadLevelFive()
        {
            FindObjectOfType<LevelLoader>().LoadLevel(4);
        }

        public void LoadLevelSix()
        {
            FindObjectOfType<LevelLoader>().LoadLevel(5);
        }

        public void LoadLevelSeven()
        {
            FindObjectOfType<LevelLoader>().LoadLevel(6);
        }

        public void LoadLevelEight()
        {
            FindObjectOfType<LevelLoader>().LoadLevel(7);
        }
    }
}