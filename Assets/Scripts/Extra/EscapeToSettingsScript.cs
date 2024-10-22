using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeToSettingsScript : MonoBehaviour
{
    public Image pauseMenuImage; // Image для отображения меню паузы
    private bool isPaused = false; // Индикатор того, на паузе ли игра

    private void Start()
    {
        // Скрываем Image в начале игры
        if (pauseMenuImage != null)
        {
            pauseMenuImage.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Отслеживаем нажатие клавиши Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    // Метод для паузы игры и включения Image
    public void Pause()
    {
        if (pauseMenuImage != null)
        {
            pauseMenuImage.gameObject.SetActive(true); // Включаем изображение
        }
        Time.timeScale = 0f; // Останавливаем игровое время
        isPaused = true;
    }

    // Метод для снятия паузы и отключения Image
    public void Resume()
    {
        if (pauseMenuImage != null)
        {
            pauseMenuImage.gameObject.SetActive(false); // Выключаем изображение
        }
        Time.timeScale = 1f; // Возвращаем нормальную скорость игры
        isPaused = false;
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        Time.timeScale = 1f; isPaused = false;
        SceneManager.LoadScene(sceneIndex);
    }
}
