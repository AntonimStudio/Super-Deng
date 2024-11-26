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


    public bool isRedFaceTurnOn;
    public bool isRedFaceRandom;
    public int[] arrayOfRedFaces;
    public int quantityOfRedFaces;

    public bool isRedWaveTurnOn;
    public bool isRedWaveRandom;
    public int[] arrayOfRedWaves;
    public int quantityOfRedWaves;
    public int proximityLimitOfRedWaves;

    public bool isFallFaceTurnOn;
    public bool isFallFaceRandom;
    public int[] arrayOfFallFaces;
    public int quantityOfFallFaces;
    public int proximityLimitOfFallFaces;
    public bool isResetDelay;
    public int resetDelayTime;

    public bool isResetFallFaceTurnOn;

    public bool isBonusTurnOn;
    public bool isBonusRandom;
    public int[] arrayOfBonuses;
    public int quantityOfBonuses;
    public int proximityLimitOfBonuses;

    public bool isPortalTurnOn;
    public bool isPortalRandom;
    public int[] arrayOfPortals;
    public int quantityOfPortals;
    public int proximityLimitOfPortals;

    public bool isFaceDanceTurnOn;
    public bool isSetFaceDanceIncrease;
    public bool isSetFaceDanceDecrease;

    public bool isSphereDanceTurnOn;

    public bool isCameraRotationTurnOn;

    public bool isRGBTurnOn;
    public int speedRGB;
    public bool isSetRGBIncrease;
    public bool isSetRGBDecrease;
}

