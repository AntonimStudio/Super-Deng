using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnSettings", menuName = "ScriptableObjects/EnemySpawnSettings", order = 1)]
public class EnemySpawnSettings : ScriptableObject
{
    public SpawnTimeData[] spawnTimes;
}

[System.Serializable]
public class SpawnTimeData
{
    public float time;
    public bool isRandom;
    public int[] gameObjects;
    public int colvo; 
}