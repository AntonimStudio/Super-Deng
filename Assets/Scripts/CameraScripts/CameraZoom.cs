using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera mainCamera;  // Ссылка на основную камеру
    public float zoomFactor = 1.5f;  // Фактор увеличения зума
    public float zoomTime = 0.5f;  // Время для зумирования в тактах (в секундах)
    public float returnTime = 0.2f;  // Время для возвращения к исходному зуму (в секундах)
    public float beatsPerMinute = 90f;  // Указанный BPM (ударов в минуту)

    private float originalFOV;  // Исходное поле зрения камеры
    private float targetFOV;  // Целевое поле зрения камеры
    [SerializeField] private BeatController BC;
    public bool isOn;

    private void Start()
    {
        originalFOV = mainCamera.orthographicSize;
        targetFOV = originalFOV / zoomFactor;
    }

    public void StartZooming()
    {
        if (isOn)
            StartCoroutine(ZoomCamera());
    }

    IEnumerator ZoomCamera()
    {
        float zoomSpeed = (targetFOV - mainCamera.orthographicSize) / (zoomTime * (beatsPerMinute / 60f));
        float elapsedTime = 0f;

        // Зумируем камеру
        while (elapsedTime < zoomTime)
        {
            mainCamera.orthographicSize += zoomSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Возвращаем камеру к исходному зуму
        float returnSpeed = (originalFOV - mainCamera.orthographicSize) / returnTime;
        elapsedTime = 0f;

        while (elapsedTime < returnTime)
        {
            mainCamera.orthographicSize += returnSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Заканчиваем выполнение корутины
        mainCamera.orthographicSize = originalFOV;
        BC.isAlreadyZoomed = false;

    }
}