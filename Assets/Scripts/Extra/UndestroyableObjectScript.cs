using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UndestroyableObjectScript : MonoBehaviour
{
    private void Awake()
    {
        // Проверяем, есть ли уже объекты этого типа
        UndestroyableObjectScript[] objects = FindObjectsOfType<UndestroyableObjectScript>();

        if (objects.Length > 1)
        {
            // Если объект уже существует, уничтожаем новый (дубликат)
            Destroy(gameObject);
            return;
        }

        // Проверяем, не является ли текущая сцена 2-й по индексу
        if (SceneManager.GetActiveScene().buildIndex != 2)
        {
            // Делаем объект недеструктивным при загрузке новых сцен
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Уничтожаем объект, если это сцена с индексом 2
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Удаляем объект, если сцена имеет индекс, отличный от 0 или 1
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            Destroy(gameObject);
        }
    }
}
