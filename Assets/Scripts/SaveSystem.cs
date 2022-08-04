using UnityEngine;
using System;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    public bool finishedInitialising;
    [SerializeField] private AudioPlayer audioPlayer;
    private string _path;
    private string _persistentPath;

    private void OnEnable()
    {
        if (Instance != null)
            Destroy(gameObject);
        else Instance = this;

        finishedInitialising = false;
        SetPaths();
        if (File.Exists(_path))
        {
            using StreamReader reader = new StreamReader(_path);
            string json = reader.ReadToEnd();
            data = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            SaveDefaultData();
        }
        finishedInitialising = true;
    }

    public SaveData data;

    private void SetPaths()
    {
        _path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        _persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
    }

    private void SaveDefaultData()
    {
        data.resolutionX = Screen.currentResolution.width;
        data.resolutionY = Screen.currentResolution.height;
        data.windowed = false;
        data.borderless = false;

        data.musicVolume = audioPlayer.defaultMusicVolume;
        data.effectsVolume = audioPlayer.defaultEffectsVolume;

        data.bindings = "";
        data.playerId = "";
        data.highScore = 0;

        SaveData();
    }

    public void SaveSoundSettings(float musicVolume, float effectsVolume)
    {
        data.musicVolume = musicVolume;
        data.effectsVolume = effectsVolume;
        SaveData();
    }

    public void SaveVideoSettings(int resolutionX, int resolutionY, bool windowed, bool borderless)
    {
        data.resolutionX = resolutionX;
        data.resolutionY = resolutionY;
        data.windowed = windowed;
        data.borderless = borderless;
        SaveData();
    }

    public void SaveControlSettings(string bindings)
    {
        data.bindings = bindings;
        SaveData();
    }

    public void SaveHighScore(int score)
    {
        data.highScore = score;
        SaveData();
    }

    public void SavePlayerId(string playerId)
    {
        data.playerId = playerId;
        SaveData();
    }

    private void SaveData()
    {
        string savePath = _path;
        string json = JsonUtility.ToJson(data);
        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }
}

[Serializable] public class SaveData 
{
    public int resolutionX;
    public int resolutionY;
    public bool windowed;
    public bool borderless;

    public float musicVolume;
    public float effectsVolume;

    public string bindings;
    public int highScore;
    public string playerId;
}