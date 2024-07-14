using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIButtonScript : MonoBehaviour
{
    public Button reloadButton; // Переменная для кнопки UI

    public void ReloadCurrentScene()
    {
        // Получаем текущую сцену и перезагружаем ее
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
