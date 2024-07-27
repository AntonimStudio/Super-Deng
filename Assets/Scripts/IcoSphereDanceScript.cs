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

    private void Update()
    {
        if (isOn && !inProcess)
        {
            foreach (GameObject obj in objects)
            {
                StartCoroutine(RotateObject(obj, rotationAngle, duration));
            }
            //isOn = false;  // Сбрасываем isOn, чтобы предотвратить повторное срабатывание
        }
    }

    private IEnumerator RotateObject(GameObject obj, float angle, float time)
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
}