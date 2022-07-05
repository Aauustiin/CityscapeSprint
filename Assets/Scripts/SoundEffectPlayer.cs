using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
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
        Debug.Log(_audioSource);
        _audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
    }
}
