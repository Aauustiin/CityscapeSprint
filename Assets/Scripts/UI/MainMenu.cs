using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button firstSelected;
        
        private void OnEnable()
        {
            firstSelected.Select();
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void Steam()
        {
            Application.OpenURL("https://store.steampowered.com/app/2071290/Cityscape_Sprint/?beta=0");
        }

        public void Twitter()
        {
            Application.OpenURL("https://twitter.com/AustinLongDev");
        }

        public void Feedback()
        {
            Application.OpenURL("https://forms.gle/v71NgYeZVPvGbg7r8");
        }
    }
}