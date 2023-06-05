using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource effectsAudioSource;
    [SerializeField] private AudioLowPassFilter musicMuffler;
    [SerializeField] public float defaultMusicVolume, defaultEffectsVolume;
    private float _sfxVolume;
    
    private void Start()
    {
        StartCoroutine(Utils.ExecuteWhenTrue(() => {
            SetEffectsVolume(SaveSystem.instance.data.effectsVolume);
            SetMusicVolume(SaveSystem.instance.data.musicVolume);
        },
        SaveSystem.instance.finishedInitialising));
    }
    
    private void OnEnable()
    {
        EventManager.SoundEffectEvent += HandleSoundEffect;
        EventManager.Pause += ToggleMusicMuffle;
        EventManager.UnPause += ToggleMusicMuffle;
    }

    private void OnDisable()
    {
        EventManager.SoundEffectEvent -= HandleSoundEffect;
        EventManager.Pause -= ToggleMusicMuffle;
        EventManager.UnPause -= ToggleMusicMuffle;
    }

    private void ToggleMusicMuffle()
    {
        musicMuffler.enabled = !musicMuffler.enabled;
    }

    private void HandleSoundEffect(AudioClip soundEffect)
    {
        effectsAudioSource.PlayOneShot(soundEffect, _sfxVolume);
    }

    public void SetEffectsVolume(float volume)
    {
        _sfxVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        musicAudioSource.volume = volume;
    }

    public void PlaySoundtrack(AudioClip soundtrack)
    {
        musicAudioSource.clip = soundtrack;
    }
}
