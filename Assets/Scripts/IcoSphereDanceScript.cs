using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcoSphereDanceScript : MonoBehaviour
{
    public GameObject[] objects;  // Массив объектов для вращения
    public float rotationAngle = 15f;  // Угол вращения в градусах
    public float duration = 0.2f;  // Время для выполнения вращения в одну сторону
    public bool isOn = false;  // Переменная для управления вращением
    private bool inProcess = false;
    private int side = -1;
    [SerializeField] private EnemySpawnSettings enemySpawnSettings;
    [SerializeField] private TimerController TC;
    private bool[] spawnExecuted;

    private void Start()
    {
        spawnExecuted = new bool[enemySpawnSettings.spawnTimes.Length];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            isOn = !isOn;
        }
        float elapsedTime = TC.timeElapsed;

        for (int i = 0; i < enemySpawnSettings.spawnTimes.Length - 1; i++)
        {
            var spawnTimeData = enemySpawnSettings.spawnTimes[i];
            var nextSpawnTimeData = enemySpawnSettings.spawnTimes[i + 1];

            // Проверяем, прошло ли указанное время и не был ли спавн уже выполнен
            if (elapsedTime >= spawnTimeData.time && elapsedTime <= nextSpawnTimeData.time && !spawnExecuted[i])
            {
                //isOn = spawnTimeData.isSphereDance;
                spawnExecuted[i] = true;
            }
        }
        if (isOn && !inProcess)
        {
            foreach (GameObject obj in objects)
            {
                StartCoroutine(RotateObject(obj, rotationAngle, duration));
            }
        }
    }

    private IEnumerator RotateObject(GameObject obj, float angle, float time)
    {
        if (isOn)
        {
            inProcess = true;
            Quaternion originalRotation = obj.transform.rotation;
            Quaternion targetRotation = originalRotation * Quaternion.Euler(0, side * angle, 0);

            // Вращение вперёд
            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                obj.transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            obj.transform.rotation = targetRotation;

            // Вращение обратно
            elapsedTime = 0f;
            while (elapsedTime < time)
            {
                obj.transform.rotation = Quaternion.Slerp(targetRotation, originalRotation, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            obj.transform.rotation = originalRotation;
            inProcess = false;
            side = -side;
        }
        else
        {
            yield return null;
        }
    }
}