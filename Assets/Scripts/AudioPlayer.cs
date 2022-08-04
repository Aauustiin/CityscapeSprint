using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource effectsAudioSource;
    private float _sfxVolume;
    [SerializeField] public float defaultMusicVolume, defaultEffectsVolume;
    
    private void Start()
    {
        StartCoroutine(Utils.ExecuteWhenTrue(() => {
            SetEffectsVolume(SaveSystem.Instance.data.effectsVolume);
            SetMusicVolume(SaveSystem.Instance.data.musicVolume);
        },
        SaveSystem.Instance.finishedInitialising));
    }
    
    private void OnEnable()
    {
        EventManager.SoundEffectEvent += HandleSoundEffect;
    }

    private void OnDisable()
    {
        EventManager.SoundEffectEvent -= HandleSoundEffect;
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
