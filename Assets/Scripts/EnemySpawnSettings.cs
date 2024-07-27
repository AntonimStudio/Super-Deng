using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    public bool isFaceDance;
    public bool isSphereDance;
    public bool isRotate;
}