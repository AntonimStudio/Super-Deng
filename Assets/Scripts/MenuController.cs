using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Метод для загрузки сцены по её индексу
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void EnableImage(GameObject image)
    {
        image.SetActive(true);
    }

    public void DisableImage(GameObject image)
    {
        image.SetActive(false);
    }
}
