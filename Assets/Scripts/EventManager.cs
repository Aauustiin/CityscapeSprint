using UnityEngine;
using UnityEngine.InputSystem;

public static class EventManager
{
    public static event System.Action Pause;

    public static void TriggerPause()
    {
        Pause?.Invoke();
    }
    
    public static event System.Action UnPause;

    public static void TriggerUnPause()
    {
        UnPause?.Invoke();
    }
    
    public static event System.Action GameOver;

    public static void TriggerGameOver()
    {
        GameOver?.Invoke();
    }

    public static event System.Action Restart;

    public static void TriggerRestart()
    {
        Restart?.Invoke();
    }

    public static event System.Action<AudioClip> SoundEffectEvent;

    public static void TriggerSoundEffect(AudioClip soundEffect)
    {
        SoundEffectEvent?.Invoke(soundEffect);
    }

    public static event System.Action<InputAction.CallbackContext> ActionInput;

    public static void TriggerActionInput(InputAction.CallbackContext value)
    {
        ActionInput?.Invoke(value);
    }

    public static event System.Action MenuOpen;
    
    public static void TriggerMenuOpen()
    {
        MenuOpen?.Invoke();
    }
    
    public static event System.Action MenuClose;
    
    public static void TriggerMenuClose()
    {
        MenuClose?.Invoke();
    }
}
