using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Для работы с UI элементами

public class MoveImageOnBeat : MonoBehaviour
{
    public RhythmManager RM;
    public Image uiImage; // Ссылка на UI Image

    private RectTransform imageRectTransform;
    private Canvas parentCanvas;

    private float beatInterval; // Интервал времени между ударами
    private float nextBeatTime; // Время следующего удара
    private Vector2 startPosition; // Начальная позиция Image
    private Vector2 centerPosition; // Центральная позиция в Canvas
    private bool isMoving = false; // Флаг движения
    private float moveStartTime; // Время начала движения

    void Start()
    {
        beatInterval = RM.beatInterval;
        nextBeatTime = Time.time + beatInterval;
        imageRectTransform = uiImage.GetComponent<RectTransform>();
        parentCanvas = imageRectTransform.GetComponentInParent<Canvas>();
        startPosition = imageRectTransform.anchoredPosition;
        centerPosition = Vector2.zero; 
        if (imageRectTransform.anchorMin != Vector2.one * 0.5f || imageRectTransform.anchorMax != Vector2.one * 0.5f)
        {
            RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();
            centerPosition = new Vector2(canvasRect.rect.width / 2, canvasRect.rect.height / 2);
        }
        this.enabled = false;
    }

    void Update()
    {
        // Проверить, достигло ли текущее время следующего времени удара
        if (Time.time >= nextBeatTime)
        {
            nextBeatTime += beatInterval;
            StartMoveToCenter();
        }
        if (isMoving)
        {
            MoveToCenter();
        }
    }

    void StartMoveToCenter()
    {
        isMoving = true;
        moveStartTime = Time.time;
    }

    void MoveToCenter()
    {
        float elapsed = Time.time - moveStartTime;
        float t = elapsed / beatInterval;

        if (t <= 1f)
        {
            imageRectTransform.anchoredPosition = Vector2.Lerp(startPosition, centerPosition, t);
        }
        else
        {
            imageRectTransform.anchoredPosition = startPosition;
            isMoving = false;
        }
    }
}
