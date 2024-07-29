using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDanceScript : MonoBehaviour
{
    public float scaleFactor = 1.2f;  // Factor by which the size increases
    public float duration = 0.2f;  // Duration for scaling up and down
    public bool isOn = false;  // Boolean to control the scaling behavior
    private bool inProcess = false;
    public Coroutine constantCoroutine;
    private Vector3 originalScale;
    [SerializeField] private EnemySpawnSettings enemySpawnSettings;
    [SerializeField] private TimerController TC;
    private bool[] spawnExecuted;

    private void Start()
    {
        duration += Random.Range(-0.2f, 0.05f);
        originalScale = gameObject.GetComponent<FaceScript>().glowingPart.transform.localScale;
        spawnExecuted = new bool[enemySpawnSettings.spawnTimes.Length];
    }

    private void Update()
    {
        float elapsedTime = TC.timeElapsed;

        for (int i = 0; i < enemySpawnSettings.spawnTimes.Length - 1; i++)
        {
            var spawnTimeData = enemySpawnSettings.spawnTimes[i];
            var nextSpawnTimeData = enemySpawnSettings.spawnTimes[i + 1];

            // Проверяем, прошло ли указанное время и не был ли спавн уже выполнен
            if (elapsedTime >= spawnTimeData.time && elapsedTime <= nextSpawnTimeData.time && !spawnExecuted[i])
            {
                isOn = spawnTimeData.isFaceDance;
                spawnExecuted[i] = true;
            }
        }
        if (isOn && !inProcess)
        {
            constantCoroutine = StartCoroutine(ScaleObject(gameObject.GetComponent<FaceScript>().glowingPart, scaleFactor, duration));
        }

    }

    public void StartScaling()
    {
        gameObject.GetComponent<FaceScript>().glowingPart.transform.localScale = originalScale;
        constantCoroutine = StartCoroutine(ScaleObject(gameObject.GetComponent<FaceScript>().glowingPart, scaleFactor, duration));

    }

    public void StopScaling()
    {
        gameObject.GetComponent<FaceScript>().glowingPart.transform.localScale = originalScale;
        if (constantCoroutine != null)
        {
            StopCoroutine(constantCoroutine);
            gameObject.GetComponent<FaceScript>().glowingPart.transform.localScale = originalScale;
            constantCoroutine = null;
        }
    }

    private IEnumerator ScaleObject(GameObject obj, float factor, float time)
    {
        inProcess = true;
        Vector3 targetScale = new Vector3(originalScale.x, originalScale.y , originalScale.z * factor);
        
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