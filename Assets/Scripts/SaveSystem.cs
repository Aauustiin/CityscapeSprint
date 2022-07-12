using UnityEngine;
using System;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;
    public bool FinishedInitialising;
    [SerializeField] private AudioPlayer audioPlayer;

    private string _path;
    private string _persistentPath;

    private void OnEnable()
    {
        if (Instance != null)
            Destroy(gameObject);
        else Instance = this;
    }

    public SaveData Data;

    private void Start()
    {
        FinishedInitialising = false;
        SetPaths();
        if (File.Exists(_path))
        {
            using StreamReader reader = new StreamReader(_path);
            string json = reader.ReadToEnd();
            Data = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            Data.ResolutionX = Screen.currentResolution.width;
            Data.ResolutionY = Screen.currentResolution.height;
            Data.Windowed = false;
            Data.Borderless = false;
            Data.MusicVolume = audioPlayer.defaultMusicVolume;
            Data.EffectsVolume = audioPlayer.defaultEffectsVolume;
            SaveData();
        }
        FinishedInitialising = true;
    }

    private void SetPaths()
    {
        _path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        _persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
    }

    public void SaveSoundSettings(float MusicVolume, float EffectsVolume)
    {
        Data.MusicVolume = MusicVolume;
        Data.EffectsVolume = EffectsVolume;
        SaveData();
    }

    private void SaveData()
    {
        string savePath = _path;
        string json = JsonUtility.ToJson(Data);
        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    public void SaveVideoSettings(int ResolutionX, int ResolutionY, bool Windowed, bool Borderless)
    {
        Data.ResolutionX = ResolutionX;
        Data.ResolutionY = ResolutionY;
        Data.Windowed = Windowed;
        Data.Borderless = Borderless;
        SaveData();
    }
}

[Serializable] public class SaveData 
{
    public int ResolutionX;
    public int ResolutionY;
    public bool Windowed;
    public bool Borderless;

    public float MusicVolume;
    public float EffectsVolume;
}