using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    
    private void Start()
    {
        SetMusicVolume();
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
        _audioSource.PlayOneShot(soundEffect, PlayerPrefs.GetFloat("SfxVolume"));
    }

    public void SetMusicVolume()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void PlaySoundtrack(AudioClip soundtrack)
    {
        _audioSource.clip = soundtrack;
    }
}
