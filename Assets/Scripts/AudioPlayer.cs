using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _sfxVolume;
    [SerializeField] public float defaultMusicVolume, defaultEffectsVolume;
    
    private void Start()
    {
        Utils.ExecuteWhenTrue(() => {
            SetEffectsVolume(SaveSystem.Instance.Data.EffectsVolume);
            SetMusicVolume(SaveSystem.Instance.Data.MusicVolume);
        },
        SaveSystem.Instance.FinishedInitialising);
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
        _audioSource.PlayOneShot(soundEffect, _sfxVolume);
    }

    public void SetEffectsVolume(float volume)
    {
        _sfxVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = volume;
    }

    public void PlaySoundtrack(AudioClip soundtrack)
    {
        _audioSource.clip = soundtrack;
    }
}
