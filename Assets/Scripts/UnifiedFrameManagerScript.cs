using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class UnifiedFrameManagerScript : MonoBehaviour
{
    [SerializeField] private TimerController TC;
    [SerializeField] private RedFaceScript RFS;
    [SerializeField] private RedWaveScript RWS;
    [SerializeField] private FallManager FM;
    [SerializeField] private BonusSpawnerScript BSS;
    [SerializeField] private PortalSpawnerScript PSS;

    [SerializeField] private EnemySpawnSettings enemySpawnSettings;
    private bool[] spawnExecuted;
    private int currentSpawnIndex = 0;

    private void Start()
    {
        spawnExecuted = new bool[enemySpawnSettings.spawnTimes.Length];
    }

    private void Update()
    {
        if (TC != null)
        {
            float elapsedTime = TC.timeElapsed;

            if (currentSpawnIndex < enemySpawnSettings.spawnTimes.Length)
            {
                var spawnTimeData = enemySpawnSettings.spawnTimes[currentSpawnIndex];
                var nextSpawnTimeData = currentSpawnIndex < enemySpawnSettings.spawnTimes.Length - 1
                    ? enemySpawnSettings.spawnTimes[currentSpawnIndex + 1]
                    : new SpawnTimeData { time = float.MaxValue };

                if (elapsedTime >= spawnTimeData.time && elapsedTime <= nextSpawnTimeData.time && !spawnExecuted[currentSpawnIndex])
                {
                    if (spawnTimeData.isRedFaceTurnOn)
                    {
                        RFS.isTurnOn = true;
                        if (spawnTimeData.isRedFaceRandom)
                        {
                            RFS.colvo = spawnTimeData.quantityOfRedFaces;
                            RFS.isRandomSpawnTime = true;
                        }
                        else
                        {
                            RFS.faceIndices.Clear();
                            RFS.faceIndices = new List<int>(spawnTimeData.arrayOfRedFaces);
                            RFS.isRandomSpawnTime = false;
                        }
                        
                    }
                    else
                    {
                        RFS.faceIndices.Clear();
                        RFS.isRandomSpawnTime = false;
                        RFS.isTurnOn = false;
                    }

                    if (spawnTimeData.isRedWaveTurnOn)
                    {
                        RWS.isTurnOn = true;
                        RWS.proximityLimit = spawnTimeData.proximityLimitOfRedWaves;

                        if (spawnTimeData.isRedWaveRandom)
                        {
                            RWS.colvo = spawnTimeData.quantityOfRedWaves;
                            RWS.isRandomSpawnTime = true;
                        }
                        else
                        {
                            RWS.faceIndices.Clear();
                            RWS.faceIndices = new List<int>(spawnTimeData.arrayOfRedWaves);
                            RWS.isRandomSpawnTime = false;
                        }
                        
                    }
                    else
                    {
                        RWS.faceIndices.Clear();
                        RWS.isRandomSpawnTime = false;
                        RWS.isTurnOn = false ;
                    }

                    if (spawnTimeData.isFallFaceTurnOn)
                    {

                        FM.isTurnOn = true;
                        FM.proximityLimit = spawnTimeData.proximityLimitOfFallFaces;
                        

                        if (spawnTimeData.isFallFaceRandom)
                        {
                            FM.colvo = spawnTimeData.quantityOfFallFaces;
                            FM.isRandomSpawnTime = true;
                        }
                        else
                        {
                            FM.faceIndices.Clear();
                            FM.faceIndices = new List<int>(spawnTimeData.arrayOfFallFaces);
                            FM.isRandomSpawnTime = false;
                        }

                        if (spawnTimeData.isResetDelay)
                        {
                            FM.isResetDelay = true;
                            FM.resetDelayTime = spawnTimeData.resetDelayTime;
                        }
                        else
                        {
                            FM.isResetDelay = false;
                            FM.resetDelayTime = 0f;
                        }

                    }
                    else
                    {
                        FM.faceIndices.Clear();
                        FM.isRandomSpawnTime = false;
                        FM.isTurnOn = false;
                    }

                    if (spawnTimeData.isResetFallFaceTurnOn)
                    {
                        FM.ResetFall();
                        FM.isResetDelay = false;
                        FM.resetDelayTime = 0f;
                    }





                    
                    if (spawnTimeData.isBonusTurnOn)
                    {

                        BSS.isTurnOn = true;
                        if (spawnTimeData.isBonusRandom)
                        {
                            BSS.colvo = spawnTimeData.quantityOfBonuses;
                            BSS.isRandomSpawnTime = true;
                        }
                        else
                        {
                            BSS.faceIndices.Clear();
                            BSS.faceIndices = new List<int>(spawnTimeData.arrayOfBonuses);
                            BSS.isRandomSpawnTime = false;
                        }
                        BSS.isBonusCombo = spawnTimeData.isBonusCombo;
                        BSS.isBonusHealth = spawnTimeData.isBonusHealth;
                    }
                    else
                    {
                        BSS.faceIndices.Clear();
                        BSS.isRandomSpawnTime = false;
                        BSS.isTurnOn = false;
                        BSS.isBonusCombo = false;
                        BSS.isBonusHealth = false;
                    }

                    if (spawnTimeData.isPortalTurnOn)
                    {

                        PSS.isTurnOn = true;
                        if (spawnTimeData.isPortalRandom)
                        {
                            PSS.colvo = spawnTimeData.quantityOfPortals;
                            PSS.isRandomSpawnTime = true;
                        }
                        else
                        {
                            PSS.faceIndices.Clear();
                            PSS.faceIndices = new List<int>(spawnTimeData.arrayOfBonuses);
                            PSS.isRandomSpawnTime = false;
                        }
                    }
                    else
                    {
                        PSS.faceIndices.Clear();
                        PSS.isRandomSpawnTime = false;
                        PSS.isTurnOn = false;
                    }








                    spawnExecuted[currentSpawnIndex] = true;
                }
                if (elapsedTime > nextSpawnTimeData.time)
                {
                    currentSpawnIndex++;
                }
            }
        }
    }
}
