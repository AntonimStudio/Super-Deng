using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Reflection;

public class MovementButtonsChanger : MonoBehaviour
{
    [SerializeField] private Button[] buttons; // Массив для всех трёх кнопок
    [SerializeField] private TextMeshProUGUI[] buttonTexts; // Массив для текстов на кнопках (TMPro)
    [SerializeField] private Image[] buttonImages;

    private int currentButtonIndex = -1; // Индекс текущей кнопки, для которой ждём нажатия клавиши

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttonTexts[i].text = "D"; // Изначальный текст на всех кнопках
            buttons[i].onClick.AddListener(() => OnButtonClick(index)); // Привязываем обработчик для каждой кнопки
        }
    }

    private void Update()
    {
        // Если мы ждём нажатия клавиши для одной из кнопок
        if (currentButtonIndex != -1)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    // Если нажата клавиша, которая является буквой, цифрой или стрелкой
                    if (Input.GetKeyDown(keyCode) && IsValidKey(keyCode))
                    {
                        buttonTexts[currentButtonIndex].text = keyCode.ToString(); // Изменяем текст на выбранной кнопке
                        buttonImages[currentButtonIndex].enabled = true;
                        currentButtonIndex = -1; // Сбрасываем индекс
                        break;
                    }
                }
            }
        }
    }

    void OnButtonClick(int index)
    {
        // При нажатии на кнопку изменяем текст и начинаем ждать нажатие клавиши для конкретной кнопки
        buttonTexts[index].text = "Press";
        currentButtonIndex = index;
        buttonImages[index].enabled = false;
    }

    // Проверяем, является ли клавиша буквой, цифрой или стрелкой
    bool IsValidKey(KeyCode keyCode)
    {
        return (keyCode >= KeyCode.A && keyCode <= KeyCode.Z) || // Проверка на буквы
               (keyCode >= KeyCode.Alpha0 && keyCode <= KeyCode.Alpha9) || // Проверка на цифры
               (keyCode >= KeyCode.Keypad0 && keyCode <= KeyCode.Keypad9) || // Проверка на цифры на цифровой клавиатуре
               (keyCode == KeyCode.LeftArrow || keyCode == KeyCode.RightArrow || keyCode == KeyCode.UpArrow || keyCode == KeyCode.DownArrow); // Проверка на стрелки
    }
}