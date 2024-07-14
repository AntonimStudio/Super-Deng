using UnityEngine;
using UnityEngine.UI;
using TMPro; // Если вы используете TextMeshPro для текста

public class TimerController : MonoBehaviour
{
    public Slider timerSlider; // Ссылка на ползунок
    public TextMeshProUGUI timerText; // Ссылка на текстовое поле (используйте Text вместо TextMeshProUGUI, если используете стандартный текст)

    private float timeElapsed = 0f; // Прошедшее время
    private float totalTime = 120f; // Общее время в секундах (2 минуты)
    private bool timerIsRunning = false;

    void Start()
    {
        // Инициализация ползунка
        timerSlider.maxValue = totalTime;
        timerSlider.value = 0;

        // Запуск таймера
        timerIsRunning = true;
    }

    void Update()
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
                // Таймер завершен
                Debug.Log("Таймер завершен!");
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