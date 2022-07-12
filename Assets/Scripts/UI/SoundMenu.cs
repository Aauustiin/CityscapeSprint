using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SoundMenu : MonoBehaviour
    {
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private AudioPlayer audioPlayer;

        private void OnEnable()
        {
            StartCoroutine(Utils.ExecuteWhenTrue(() => {
                musicSlider.value = SaveSystem.Instance.Data.MusicVolume;
                sfxSlider.value = SaveSystem.Instance.Data.EffectsVolume;
            },
            SaveSystem.Instance.FinishedInitialising));
        }

        public void ResetVolume()
        {
            musicSlider.value = audioPlayer.defaultMusicVolume;
            sfxSlider.value = audioPlayer.defaultEffectsVolume;
        }

        public void Save()
        {
            SaveSystem.Instance.SaveSoundSettings(musicSlider.value, sfxSlider.value);
            audioPlayer.SetMusicVolume(musicSlider.value);
            audioPlayer.SetEffectsVolume(sfxSlider.value);
        }
    }
}