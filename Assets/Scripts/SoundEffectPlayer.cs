using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
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
        FindObjectOfType<AudioSource>().PlayOneShot(soundEffect, 0.5f);
    }
}
