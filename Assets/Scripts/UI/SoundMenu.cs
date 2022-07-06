using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SoundMenu : MonoBehaviour
    {
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private float defaultSfxVolume, defaultMusicVolume;

        private void OnEnable()
        {
            if (PlayerPrefs.GetInt("SfxSet") == 0)
            {
                PlayerPrefs.SetFloat("SfxVolume", defaultSfxVolume);
                PlayerPrefs.SetInt("SfxSet", 1);
            }

            if (PlayerPrefs.GetInt("MusicSet") == 0)
            {
                PlayerPrefs.SetFloat("MusicVolume", defaultSfxVolume);
                PlayerPrefs.SetInt("MusicSet", 1);
            }

            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume");
        }

        public void ResetVolume()
        {
            PlayerPrefs.SetFloat("SfxVolume", defaultSfxVolume);
            PlayerPrefs.SetInt("SfxSet", 1);
            PlayerPrefs.SetFloat("MusicVolume", defaultMusicVolume);
            PlayerPrefs.SetInt("MusicSet", 1);
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume");
        }

        public void SetSfxVolume()
        {
            PlayerPrefs.SetFloat("SfxVolume", sfxSlider.value);
            PlayerPrefs.SetInt("SfxSet", 1);
        }

        public void SetMusicVolume()
        {
            PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
            PlayerPrefs.SetInt("MusicSet", 1);
            FindObjectOfType<SoundEffectPlayer>().SetMusicVolume();
        }
    }
}