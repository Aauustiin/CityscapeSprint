using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class VideoMenu : MonoBehaviour
    {
        [SerializeField] private Dropdown resolutionDropdown;
        [SerializeField] private Toggle windowedToggle;
        [SerializeField] private Toggle borderlessToggle;
        [SerializeField] private TextMeshProUGUI borderlessLabel;
        [SerializeField] private Image borderlessBackground;
        [SerializeField] private Image borderlessCheckmark;
        [SerializeField] private Button applyButton;

        private void OnEnable()
        {
            resolutionDropdown.Select();
        }

        private void Start()
        {
            InitialiseDropdown();

            StartCoroutine(Utils.ExecuteWhenTrue(() => {
                windowedToggle.isOn = SaveSystem.instance.data.windowed;
                if (windowedToggle.isOn) EnableBorderlessOption();
                else DisableBorderlessOption();
                borderlessToggle.isOn = SaveSystem.instance.data.borderless;
            },
            SaveSystem.instance.finishedInitialising));
        }

        private void DisableBorderlessOption()
        {
            borderlessToggle.enabled = false;
            Navigation wN = windowedToggle.navigation;
            wN.selectOnDown = applyButton;
            windowedToggle.navigation = wN;
            Navigation aN = applyButton.navigation;
            aN.selectOnUp = windowedToggle;
            applyButton.navigation = aN;
            borderlessToggle.interactable = false;
            borderlessLabel.alpha = 0.5f;
            borderlessBackground.color = new Color(borderlessBackground.color.r, borderlessBackground.color.g, borderlessBackground.color.b, 0.5f);
            borderlessCheckmark.color = new Color(borderlessCheckmark.color.r, borderlessCheckmark.color.g, borderlessCheckmark.color.b, 0.5f);
        }

        private void EnableBorderlessOption()
        {
            borderlessToggle.enabled = true;
            Navigation wN = windowedToggle.navigation;
            wN.selectOnDown = borderlessToggle;
            windowedToggle.navigation = wN;
            Navigation aN = applyButton.navigation;
            aN.selectOnUp = borderlessToggle;
            applyButton.navigation = aN;
            borderlessToggle.interactable = true;
            borderlessLabel.alpha = 1f;
            borderlessBackground.color = new Color(borderlessBackground.color.r, borderlessBackground.color.g, borderlessBackground.color.b, 1f);
            borderlessCheckmark.color = new Color(borderlessCheckmark.color.r, borderlessCheckmark.color.g, borderlessCheckmark.color.b, 1f);
        }

        private void InitialiseDropdown()
        {
            AddResolutionOptions();
            SelectCurrentResolution();
        }

        private void AddResolutionOptions()
        {
            Resolution[] resolutions = Screen.resolutions.Where(r => (16 * r.height) / 9 == r.width).ToArray();
            List<string> resolutionOptions = new List<string>() { };
            
            foreach (var r in resolutions)
            {
                var optionText = r.width + "x" + r.height;
                if (!resolutionOptions.Contains(optionText))
                    resolutionOptions.Add(optionText);
            }
            
            resolutionDropdown.AddOptions(resolutionOptions);
        }

        private void SelectCurrentResolution()
        {
            string targetOption = Screen.currentResolution.width + "x" + Screen.currentResolution.height;
            foreach (var option in resolutionDropdown.options)
            {
                if (option.text == targetOption)
                    resolutionDropdown.value = resolutionDropdown.options.IndexOf(option);
            }
        }

        private Vector2 GetSelectedResolution()
        {
            string resolutionString = resolutionDropdown.options[resolutionDropdown.value].text;
            string[] dimensionStrings = resolutionString.Split('x');
            Vector2 resolution = new Vector2(float.Parse(dimensionStrings[0]),
                float.Parse(dimensionStrings[dimensionStrings.Length - 1]));
            return resolution;
        }
        
        public void ToggleBorderlessOption()
        {
            if (windowedToggle.isOn) EnableBorderlessOption();
            else DisableBorderlessOption();
        }

        public void ApplySettings()
        {
            Vector2 res = GetSelectedResolution();

            SaveSystem.instance.SaveVideoSettings((int)res.x, (int)res.y, windowedToggle.isOn, borderlessToggle.isOn);

            FullScreenMode fullScreenMode;
            
            if (windowedToggle.isOn && borderlessToggle.isOn)
                fullScreenMode = FullScreenMode.FullScreenWindow;
            else if (windowedToggle.isOn)
                fullScreenMode = FullScreenMode.Windowed;
            else
                fullScreenMode = FullScreenMode.ExclusiveFullScreen;

            Screen.SetResolution((int)res.x, (int)res.y, fullScreenMode);
        }
    }
}