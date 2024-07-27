using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public bool isOn = false;
    [SerializeField] private EnemySpawnSettings enemySpawnSettings;
    [SerializeField] private TimerController TC;
    private bool[] spawnExecuted;
    private int colvo = 1;

    private void Start()
    {
        spawnExecuted = new bool[enemySpawnSettings.spawnTimes.Length];
    }

    private void Update()
    {
        float elapsedTime = TC.timeElapsed;

        for (int i = 0; i < enemySpawnSettings.spawnTimes.Length - 1; i++)
        {
            var spawnTimeData = enemySpawnSettings.spawnTimes[i];
            var nextSpawnTimeData = enemySpawnSettings.spawnTimes[i + 1];

            // Проверяем, прошло ли указанное время и не был ли спавн уже выполнен
            if (elapsedTime >= spawnTimeData.time && elapsedTime <= nextSpawnTimeData.time && !spawnExecuted[i])
            {
                isOn = spawnTimeData.isRotate;
                spawnExecuted[i] = true;
            }
        }
        if (isOn)
        {
            float rotation = rotationSpeed * Time.deltaTime;
            transform.Rotate(0, 0, rotation);
        }
    }
}