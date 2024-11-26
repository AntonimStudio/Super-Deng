using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDanceScript : MonoBehaviour
{
    public float scaleFactor = 1.15f;  // Factor by which the size increases
    public float duration = 0.2f;  // Duration for scaling up and down
    public bool isOn = false;  // Boolean to control the scaling behavior
    private bool inProcess = false;
    public Coroutine constantCoroutine;
    private Vector3 originalScale;
    private FaceScript FS;
    [SerializeField] private EnemySpawnSettings enemySpawnSettings;
    [SerializeField] private TimerController TC;
    private bool[] spawnExecuted;
    
    private void Start()
    {
        FS = gameObject.GetComponent<FaceScript>();
        duration += Random.Range(-0.2f, 0.05f);
        originalScale = gameObject.GetComponent<FaceScript>().glowingPart.transform.localScale;
        if (enemySpawnSettings != null)
        {
            spawnExecuted = new bool[enemySpawnSettings.spawnTimes.Length];
        }
    }

    private void Update()
    {/*
        if (TC != null && enemySpawnSettings != null)
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
            if (isOn && !inProcess && !FS.havePlayer && !FS.isBlinking && !FS.isKilling && !FS.isBlocked)
            {
                constantCoroutine = StartCoroutine(ScaleObject(FS.glowingPart, scaleFactor, duration));
            }
        }*/
        if (Input.GetKeyDown(KeyCode.V))
        {
            isOn = !isOn;
        }
    }
    
    private IEnumerator ScaleObject(GameObject obj, float factor, float time)
    {
        if (isOn && !inProcess && !FS.havePlayer && !FS.isKilling && !FS.isBlinking && !FS.isBlocked)
        {
            inProcess = true;
            Vector3 targetScale = new Vector3(originalScale.x, originalScale.y, originalScale.z * factor);

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
        else
        {
            yield return null;
        }
        yield return null;
    }
}