using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerController : MonoBehaviour
{
    public Slider timerSlider; // Ссылка на ползунок
    public TextMeshProUGUI timerText; // Ссылка на текстовое поле (используйте Text вместо TextMeshProUGUI, если используете стандартный текст)
    public float timeElapsed = 0f; // Прошедшее время
    public float totalTime = 130f; // Общее время в секундах (2 минуты)
    public bool timerIsRunning = false;
    [SerializeField] private Image imageCompleted;
    [SerializeField] private RedFaceScript RFS;
    [SerializeField] private StartCountDown SCD;
    [SerializeField] private ComboManager CM;

    private void Start()
    {
        timerSlider.maxValue = totalTime;
        timerSlider.value = 0;
        timerIsRunning = true;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeElapsed < totalTime)
            {
                timeElapsed += Time.deltaTime;
                UpdateTimerDisplay(timeElapsed);
            }
            else
            {
                timeElapsed = totalTime;
                timerIsRunning = false;
                UpdateTimerDisplay(timeElapsed);
                imageCompleted.gameObject.SetActive(true);
                RFS.isTurnOn = false;
                SCD.isOn = false;
            }
        }
    }

    void UpdateTimerDisplay(float time)
    {
        // Обновление ползунка
        timerSlider.value = time;

        // Обновление текстового отображения
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}