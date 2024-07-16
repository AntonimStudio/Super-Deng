using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatController : MonoBehaviour
{
    public float beatInterval = 0.6666f;
    private float elapsedTime = 0f;
    public bool canPress = true;

    [SerializeField] private Image image;
    [SerializeField] private Vector3 startPos = new Vector3(-300f, 0f, 0f);
    [SerializeField] private Vector3 midPos = new Vector3(-100f, 0f, 0f);
    [SerializeField] private Vector3 almostEndPos = new Vector3(0f, 0f, 0f);
    [SerializeField] private Vector3 endPos = new Vector3(50f, 0f, 0f);

    private float lastBeatTime = 0f;

    void Start()
    {
        image.enabled = false;
        image.rectTransform.localPosition = startPos; // Установка начальной позиции
    }

    private void Update()
    {
        if (elapsedTime > 0f)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime <= beatInterval / 3f)
            {
                canPress = true;
                float t = elapsedTime / (beatInterval / 3f);
                image.rectTransform.localPosition = Vector3.Lerp(almostEndPos, endPos, t);
            }
            else if (Mathf.Abs(elapsedTime - (2f * beatInterval) / 3f) < 0.01f)
            {
                //image.rectTransform.localPosition = startPos;
            }
            else if (elapsedTime < (2f * beatInterval) / 3f)
            {
                canPress = false;
                float t = (elapsedTime - (beatInterval / 3f)) / (beatInterval / 3f);
                image.rectTransform.localPosition = Vector3.Lerp(startPos, midPos, t);
                /*
                float t = (elapsedTime - (beatInterval / 3f)) / (beatInterval / 3f);
                image.rectTransform.localPosition = Vector3.Lerp(midPos, midPos, t);
                */
            }
            else if (elapsedTime < beatInterval)
            {
                // Третья треть beatInterval
                canPress = true;
                // Анимация перемещения к endPos
                float t = (elapsedTime - ((2f * beatInterval) / 3f)) / (beatInterval / 3f);
                image.rectTransform.localPosition = Vector3.Lerp(midPos, almostEndPos, t);
            }
            else
            {
                elapsedTime = 0f;
            }
        }
    }

    public void OnBeat()
    {
        float currentTime = Time.time;
        if (lastBeatTime != 0f)
        {
            beatInterval = currentTime - lastBeatTime;
            Debug.Log(beatInterval);
        }
        lastBeatTime = currentTime;

        elapsedTime = 0.0001f; // Запуск процесса
        //image.rectTransform.localPosition = startPos; // Телепортирование на начальную позицию
    }
}