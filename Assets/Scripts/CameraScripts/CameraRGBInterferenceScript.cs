using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRGBInterferenceScript : MonoBehaviour
{
    [SerializeField] private RGBShiftEffect RGBShiftEffect;

    public float variableValue = 0f; // Значение переменной
    public float targetValue = 0.1f; // Целевое значение, до которого увеличивается переменная
    public float speed = 0.01f; // Скорость изменения переменной

    private bool isIncreasing = false; // Направление изменения значения
    private bool isChanging = false; // Указывает, меняется ли значение сейчас
    private float initialValue; // Начальное значение переменной

    void Start()
    {
        initialValue = variableValue;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y)) // Нажатие клавиши (например, пробел)
        {
            isIncreasing = !isIncreasing; // Переключаем направление
            isChanging = true; // Запускаем изменение значения
            RGBShiftEffect.on = true;
        }

        if (isChanging)
        {
            if (isIncreasing)
            {
                // Увеличиваем значение
                variableValue = Mathf.MoveTowards(variableValue, targetValue, speed * Time.deltaTime);
                RGBShiftEffect.amount = variableValue;
                if (Mathf.Approximately(variableValue, targetValue))
                {
                    isChanging = false; // Останавливаем изменение, если достигли цели
                }
            }
            else
            {
                // Уменьшаем значение
                RGBShiftEffect.amount = variableValue;
                variableValue = Mathf.MoveTowards(variableValue, initialValue, speed * Time.deltaTime);
                if (Mathf.Approximately(variableValue, initialValue))
                {
                    isChanging = false; // Останавливаем изменение, если достигли начального значения
                    RGBShiftEffect.on = false;
                }
            }
        }
    }
}