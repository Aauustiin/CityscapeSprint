using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SoundMenu : MonoBehaviour
    {
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private AudioPlayer audioPlayer;

        private bool _finishedInitialising = false;

        private void OnEnable()
        {
            musicSlider.Select();
        }

        private void Start()
        {
            StartCoroutine(Utils.ExecuteWhenTrue(() => {
                musicSlider.value = SaveSystem.instance.data.musicVolume;
                sfxSlider.value = SaveSystem.instance.data.effectsVolume;
                _finishedInitialising = true;
            },
            SaveSystem.instance.finishedInitialising));
        }

        public void ResetVolume()
        {
            if (_finishedInitialising)
            {
                musicSlider.value = audioPlayer.defaultMusicVolume;
                sfxSlider.value = audioPlayer.defaultEffectsVolume;
            }
        }

        public void Save()
        {
            if (_finishedInitialising)
            {
                SaveSystem.instance.SaveSoundSettings(musicSlider.value, sfxSlider.value);
                audioPlayer.SetMusicVolume(musicSlider.value);
                audioPlayer.SetEffectsVolume(sfxSlider.value);
            }
        }
    }
}