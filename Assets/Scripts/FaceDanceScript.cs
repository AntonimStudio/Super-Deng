
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDanceScript : MonoBehaviour
{
    [SerializeField] private FaceDanceCenterScript FDCS;
    private FaceScript FS;
    public float scaleFactor;
    public float duration = 0.2f;  
    public bool isTurnOn = false;  
    private bool inProcess = false;
    public Coroutine constantCoroutine;
    private Vector3 originalScale;

    private void Start()
    {
        FS = gameObject.GetComponent<FaceScript>();
        scaleFactor = FDCS.scaleFactor;
        duration = FDCS.duration;
        duration += Random.Range(-0.2f, 0.05f);
        originalScale = FS.glowingPart.transform.localScale;
    }

    private void Update()
    {
        if (FDCS.isTurnOn && !inProcess)
        {
            constantCoroutine = StartCoroutine(ScaleObject(FS.glowingPart, scaleFactor, duration));
            inProcess = true;
        }
    }

    public void StartScaling()
    {
        FS.glowingPart.transform.localScale = originalScale;
        constantCoroutine = StartCoroutine(ScaleObject(FS.glowingPart, scaleFactor, duration));
    }

    public void StopScaling()
    {
        FS.glowingPart.transform.localScale = originalScale;
        if (constantCoroutine != null)
        {
            StopCoroutine(constantCoroutine);
            FS.glowingPart.transform.localScale = originalScale;
            constantCoroutine = null;
        }
    }

    private IEnumerator ScaleObject(GameObject obj, float factor, float time)
    {
        if (FDCS.isTurnOn)
        {
            inProcess = true;
            Vector3 targetScale = new(originalScale.x, originalScale.y, originalScale.z * factor);

            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                obj.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            obj.transform.localScale = targetScale;
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
        else
        {
            yield return null;
        }
    }
}