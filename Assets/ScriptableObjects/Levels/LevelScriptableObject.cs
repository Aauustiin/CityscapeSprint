using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/LevelScriptableObject", order = 1)]
public class LevelScriptableObject : ScriptableObject
{
    public int targetScore;
    public string scene;
    public int testLeaderboardId;
    public int leaderboardId;
}
