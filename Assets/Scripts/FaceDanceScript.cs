using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDanceScript : MonoBehaviour
{
    public GameObject[] objects; // Массив объектов, которые нужно масштабировать
    public float scaleFactor = 1.2f;  // Factor by which the size increases
    public float duration = 0.2f;  // Duration for scaling up and down
    public bool isOn = false;  // Boolean to control the scaling behavior
    private bool inProcess = false;

    private void Update()
    {
        if (isOn && !inProcess)
        {
            foreach (GameObject obj in objects)
            {
                StartCoroutine(ScaleObject(obj, scaleFactor, duration));
            }
            
        }
    }

    private IEnumerator ScaleObject(GameObject obj, float factor, float time)
    {
        inProcess = true;
        Vector3 originalScale = obj.transform.localScale;
        Vector3 targetScale = new Vector3(originalScale.x, originalScale.y * factor, originalScale.z);
        
        // Scale up
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            obj.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.transform.localScale = targetScale;

        // Scale down
        elapsedTime = 0f;
        while (elapsedTime < time)
        {
            obj.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.transform.localScale = originalScale;
        inProcess = false;
    }
}