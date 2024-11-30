using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class IcoSphereDanceScript : MonoBehaviour
{
    public GameObject[] objects;  // Массив объектов для вращения
    public float rotationAngle = 15f;  // Угол вращения в градусах
    public float duration = 0.2f;  // Время для выполнения вращения в одну сторону
    public bool isTurnOn = false;  // Переменная для управления вращением
    private bool inProcess = false;
    private int side = -1;
    [SerializeField] private EnemySpawnSettings enemySpawnSettings;
    [SerializeField] private TimerController TC;

    private void Update()
    {
        if (isTurnOn && !inProcess)
        {
            foreach (GameObject obj in objects)
            {
                StartCoroutine(RotateObject(obj, rotationAngle, duration));
            }
        }
    }

    private IEnumerator RotateObject(GameObject obj, float angle, float time)
    {
        if (isTurnOn)
        {
            inProcess = true;
            Quaternion originalRotation = obj.transform.rotation;
            Quaternion targetRotation = originalRotation * Quaternion.Euler(0, side * angle, 0);

            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                obj.transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            obj.transform.rotation = targetRotation;

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
            inProcess = false;
            yield return null;
        }
    }
}