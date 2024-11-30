
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDanceScript : MonoBehaviour
{
    private FaceScript FS;
    private float scaleFactor;
    private float duration = 0.2f;
    public bool isTurnOn = false;  
    private bool inProcess = false;
    private bool isFaceDanceIncrease = false;
    private bool isFaceDanceDecrease = false;
    public Coroutine constantCoroutine;
    private Vector3 originalScale;

    private void Start()
    {
        FS = gameObject.GetComponent<FaceScript>();
        
        originalScale = FS.glowingPart.transform.localScale;
    }

    private void Update()
    {
        if (isTurnOn && !inProcess && !FS.havePlayer && !FS.isBlinking && !FS.isKilling && !FS.isBlocked)
        {
            constantCoroutine = StartCoroutine(ScaleObject(FS.glowingPart, scaleFactor, duration));
            inProcess = true;
        }
    }

    public void SetParameters(bool newIsTurnOn, bool newIsFaceDanceIncrease, bool newIsFaceDanceDecrease, float newScaleFactor, float newDuration)
    {
        isTurnOn = newIsTurnOn;
        isFaceDanceIncrease = newIsFaceDanceIncrease;
        isFaceDanceDecrease = newIsFaceDanceDecrease;
        scaleFactor = newScaleFactor;
        duration = newDuration + Random.Range(-0.2f, 0.05f);
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
        if (isTurnOn)
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