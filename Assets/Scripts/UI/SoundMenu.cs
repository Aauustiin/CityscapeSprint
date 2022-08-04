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
                musicSlider.value = SaveSystem.Instance.Data.MusicVolume;
                sfxSlider.value = SaveSystem.Instance.Data.EffectsVolume;
                _finishedInitialising = true;
            },
            SaveSystem.Instance.finishedInitialising));
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
                SaveSystem.Instance.SaveSoundSettings(musicSlider.value, sfxSlider.value);
                audioPlayer.SetMusicVolume(musicSlider.value);
                audioPlayer.SetEffectsVolume(sfxSlider.value);
            }
        }
    }
}