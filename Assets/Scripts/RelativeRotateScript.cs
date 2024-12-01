using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeRotateScript : MonoBehaviour
{
    public Transform targetObject;  // Объект, относительно которого происходит вращение
    public float rotationSpeed = 10f;  // Скорость вращения (градусы в секунду)

    void Update()
    {
        // Вычисляем направление от текущего объекта к targetObject
        float angle = targetObject.eulerAngles.x;

        // Вращаем сферу вокруг оси X в зависимости от угла поворота камеры
        transform.rotation = Quaternion.Euler(angle * rotationSpeed * Time.deltaTime, 0f, 0f);
    }
}