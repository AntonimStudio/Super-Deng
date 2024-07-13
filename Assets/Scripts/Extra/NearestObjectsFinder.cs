using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class NearestObjectsFinder : MonoBehaviour
{
    // Update is called once per frame
    void Start()
    {
        FindThreeNearestObjects();
    }

    void FindThreeNearestObjects()
    {
        // Найти все объекты с компонентом FaceScript
        FaceScript[] allFaceScripts = FindObjectsOfType<FaceScript>();

        // Проверка на наличие объектов с FaceScript
        if (allFaceScripts.Length == 0)
        {
            Debug.Log("No objects with FaceScript found");
            return;
        }

        // Текущая позиция объекта, с которого происходит поиск
        Vector3 currentPosition = transform.position;

        // Сортировка объектов по расстоянию до текущего объекта
        var sortedObjects = allFaceScripts
            .OrderBy(faceScript => Vector3.Distance(currentPosition, faceScript.transform.position))
            .ToList();

        // Получение трех ближайших объектов
        var nearestObjects = sortedObjects.Take(3);

        // Вывод информации о ближайших объектах в консоль
        foreach (var obj in nearestObjects)
        {
            Debug.Log($"Found object {obj.gameObject.name} at distance {Vector3.Distance(currentPosition, obj.transform.position)}");
        }
    }
}
