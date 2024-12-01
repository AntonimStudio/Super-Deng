using System.Collections;
using UnityEngine;

public class FaceDanceScript : MonoBehaviour
{
    private FaceScript FS;
    private float scaleFactor;
    private float duration = 0.2f;
    public bool isTurnOn = false;
    private bool inProcess = false;
    private bool isChanging = false;
    private bool isFaceDanceIncrease = false;
    public Coroutine constantCoroutine;
    private Coroutine intensityCoroutine;
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
            constantCoroutine = StartCoroutine(ScaleObject(FS.glowingPart));
            inProcess = true;
        }
        //Debug.Log(scaleFactor);
    }

    public void SetParameters(bool newIsTurnOn, bool newIsChanging, bool newIsFaceDanceIncrease, float newScaleFactor, float newDuration)
    {
        isTurnOn = newIsTurnOn;
        isChanging = newIsChanging;
        isFaceDanceIncrease = newIsFaceDanceIncrease;
        if (isFaceDanceIncrease && isChanging)
        {
            scaleFactor = 1; 
        }
        else 
        {
            scaleFactor = newScaleFactor;

        }
        duration = newDuration + Random.Range(-0.1f, 0.05f);
    }

    public void StartScaling()
    {
        FS.glowingPart.transform.localScale = originalScale;
        constantCoroutine = StartCoroutine(ScaleObject(FS.glowingPart));
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
        if (intensityCoroutine != null)
        {
            StopCoroutine(intensityCoroutine);
            intensityCoroutine = null;
        }
    }

    public void AdjustEffectIntensity(float targetScaleFactor, float adjustmentDuration)
    {
        if (isChanging)
        {
            if (intensityCoroutine != null)
            {
                StopCoroutine(intensityCoroutine);
            }
            intensityCoroutine = StartCoroutine(AdjustIntensityCoroutine(targetScaleFactor, adjustmentDuration));
        }
    }

    private IEnumerator AdjustIntensityCoroutine(float targetScaleFactor, float adjustmentDuration)
    {
        float initialScaleFactor = scaleFactor;
        float elapsedTime = 0f;
        if (isChanging)
        {
            while (elapsedTime < adjustmentDuration)
            {
                scaleFactor = Mathf.Lerp(initialScaleFactor, targetScaleFactor, elapsedTime / adjustmentDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            scaleFactor = targetScaleFactor;
        }
        else
        {
            scaleFactor = targetScaleFactor;
            yield return null;
        }
        isChanging = false;
    }

    private IEnumerator ScaleObject(GameObject obj)
    {
        if (isTurnOn)
        {
            inProcess = true;

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                obj.transform.localScale = Vector3.Lerp(originalScale, new Vector3(originalScale.x, originalScale.y, originalScale.z * scaleFactor), elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            obj.transform.localScale = new(originalScale.x, originalScale.y, originalScale.z * scaleFactor);

            elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                obj.transform.localScale = Vector3.Lerp(new Vector3(originalScale.x, originalScale.y, originalScale.z * scaleFactor), originalScale, elapsedTime / duration);
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
