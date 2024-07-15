using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Для работы с UI элементами

public class MoveImageOnBeat : MonoBehaviour
{
    public Image image;  // Ссылка на Image
    private Vector3 centerPosition;  // Центровая позиция
    private Vector3 initialPosition;  // Начальная позиция
    public float moveDuration = 0.2f;  // Длительность движения до центра
    private bool isMoving = false;  // Флаг, чтобы избежать одновременного запуска нескольких корутин

    private void Start()
    {
        // Устанавливаем начальную позицию image
        initialPosition = image.rectTransform.localPosition;
        centerPosition = new Vector3(0, 0, initialPosition.z);  // Предположим, что центр экрана в (0,0)
        transform.localPosition = centerPosition;
    }

    // Этот метод должен вызываться в такт музыке
    public void OnBeat()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveToCenterAndBack());
        }
    }

    private IEnumerator MoveToCenterAndBack()
    {
        isMoving = true;

        // Перемещение к центру
        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            image.rectTransform.localPosition = Vector3.Lerp(initialPosition, centerPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.rectTransform.localPosition = centerPosition;

        // Задержка, если необходимо
        yield return new WaitForSeconds(0.1f);  // Пауза в центре

        // Перемещение обратно к начальной позиции
        elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            image.rectTransform.localPosition = Vector3.Lerp(centerPosition, initialPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.rectTransform.localPosition = initialPosition;

        isMoving = false;
    }
}