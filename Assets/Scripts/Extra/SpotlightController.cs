using UnityEngine;
using System.Collections.Generic;

public class SpotLightController : MonoBehaviour
{
    public List<Light> spotLights; // Список Spot Light
    [SerializeField] public float rotationSpeed = 10f; // Скорость вращения
    private Quaternion[] targetRotations; // Целевые вращения для каждого Spot Light
    [SerializeField] private float minAngleDifference = 7f; // Минимальный угол между лучами
    [SerializeField] private float maxRotationAngle = 50f; // Максимальное отклонение от нулевой позиции

    void Start()
    {
        // Инициализация целевых вращений
        targetRotations = new Quaternion[spotLights.Count];
        for (int i = 0; i < spotLights.Count; i++)
        {
            targetRotations[i] = spotLights[i].transform.rotation;
        }
    }

    void Update()
    {
        for (int i = 0; i < spotLights.Count; i++)
        {
            // Плавное вращение к целевому вращению
            spotLights[i].transform.rotation = Quaternion.Slerp(spotLights[i].transform.rotation, targetRotations[i], Time.deltaTime * rotationSpeed);

            // Проверка, если достигли целевого вращения
            if (Quaternion.Angle(spotLights[i].transform.rotation, targetRotations[i]) < 0.1f)
            {
                // Генерация нового целевого вращения
                GenerateNewTargetRotation(i);
            }
        }
    }

    void GenerateNewTargetRotation(int index)
    {
        bool validRotation = false;
        Quaternion newTargetRotation = Quaternion.identity;

        while (!validRotation)
        {
            // Генерация случайного вращения в пределах ограничений
            Vector3 randomRotation = new Vector3(
                Random.Range(-maxRotationAngle, maxRotationAngle),
                Random.Range(-maxRotationAngle, maxRotationAngle),
                Random.Range(-maxRotationAngle, maxRotationAngle)
            );
            newTargetRotation = Quaternion.Euler(randomRotation);

            validRotation = true;
            for (int i = 0; i < spotLights.Count; i++)
            {
                if (i != index)
                {
                    float angle = Quaternion.Angle(newTargetRotation, spotLights[i].transform.rotation);
                    if (angle < minAngleDifference)
                    {
                        validRotation = false;
                        break;
                    }
                }
            }
        }

        targetRotations[index] = newTargetRotation;
    }
}
