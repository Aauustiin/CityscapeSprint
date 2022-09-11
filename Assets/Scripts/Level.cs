using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Level Data", order = 1)]
public class Level : ScriptableObject
{
    public Vector3[] spawnLocations;
    public int numberLive;
    public int totalSpawns;
    public int spawnType;
}
