using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RedFaceScript : MonoBehaviour
{
    [SerializeField] private GameObject[] faces;
    [SerializeField] private Image imageLose;
    [SerializeField] private float colorChangeDuration = 2f;
    [SerializeField] private float scaleChangeDuration = 1f;
    [SerializeField] private float scaleChange = 25f;
    [SerializeField] private Material materialWhite;
    [SerializeField] private Material materialRed;
    [SerializeField] private Material materialPlayer;
    [SerializeField] private StartCountDown SCD;
    [SerializeField] private TimerController TC;
    [SerializeField] private EnemySpawnSettings enemySpawnSettings;
    [SerializeField] private PlayerScript PS;
    [SerializeField] private ComboManager CM;
    public bool isTurnOn = false;
    private List<int> lastCubeIndices = new List<int>();
    private List<int> newCubeIndices = new List<int>();
    private bool[] spawnExecuted;
    private int colvo = 1;
    private bool isRandomSpawnTime = true;
    public bool isTutorial = false;
    public TutorialController TuC;
    public Image panel;

    private void Start()
    {
        spawnExecuted = new bool[enemySpawnSettings.spawnTimes.Length];
        //PS.rend.material = targetMaterial;
    }

    public void ChangeFaceColor()
    {
        if (isTurnOn)
        {
            foreach (int index in lastCubeIndices)
            {
                StartCoroutine(FadeColorAndScale(faces[index], materialWhite, new Vector3(1f, 1f, 1f), colorChangeDuration, scaleChangeDuration));
            }
            lastCubeIndices.Clear();

            if (isRandomSpawnTime) 
            { 
                for (int i = 0; i < colvo; i++)
                {
                    int randomIndex;
                
                    do
                    {
                        randomIndex = Random.Range(0, faces.Length);
                    }
                    while (lastCubeIndices.Contains(randomIndex));
                    lastCubeIndices.Add(randomIndex);
                    StartCoroutine(ChangeColorThenScale(faces[randomIndex], materialRed, new Vector3(1f, 1f, scaleChange), colorChangeDuration, scaleChangeDuration));
                    faces[randomIndex].GetComponent<FaceScript>().isKilling = true;
                    faces[randomIndex].GetComponent<FaceDanceScript>().StopScaling();
                }
            }
            else
            {
                for (int i = 0; i < newCubeIndices.Count; i++)
                {
                    StartCoroutine(ChangeColorThenScale(faces[newCubeIndices[i]], materialRed, new Vector3(1f, 1f, scaleChange), colorChangeDuration, scaleChangeDuration));
                }
            }
        }
    }
    private void Update()
    {
        if (TC != null)
        {
            float elapsedTime = TC.timeElapsed;

            for (int i = 0; i < enemySpawnSettings.spawnTimes.Length - 1; i++)
            {
                var spawnTimeData = enemySpawnSettings.spawnTimes[i];
                var nextSpawnTimeData = enemySpawnSettings.spawnTimes[i + 1];

                if (elapsedTime >= spawnTimeData.time && elapsedTime <= nextSpawnTimeData.time && !spawnExecuted[i])
                {
                    if (spawnTimeData.isRandom)
                    {
                        colvo = spawnTimeData.colvo;
                        isRandomSpawnTime = true;
                    }
                    else
                    {
                        newCubeIndices.Clear();
                        newCubeIndices = new List<int>(spawnTimeData.gameObjects);
                        isRandomSpawnTime = false;
                    }
                    spawnExecuted[i] = true;
                }
            }
        }
    }


    IEnumerator ChangeColorThenScale(GameObject face, Material targetMaterial, Vector3 targetScale, float colorDuration, float scaleDuration)
    {

        yield return StartCoroutine(FadeColor(face, targetMaterial, colorDuration));
        
        if (face.GetComponent<FaceScript>().havePlayer)
        {
            Lose(face);
        }
        yield return StartCoroutine(ChangeScale(face, targetScale, scaleDuration, targetMaterial, true));
    }

    IEnumerator FadeColor(GameObject face, Material targetMaterial, float duration)
    {
        if (!face.GetComponent<FaceScript>().havePlayer)
        {
            face.GetComponent<FaceScript>().rend.material = targetMaterial;
        }
        else
            PS.rend.material = targetMaterial;
        float timer = 0f;
        while (timer < duration)
        {
            if (!face.GetComponent<FaceScript>().havePlayer)
            {
                face.GetComponent<FaceScript>().rend.material = targetMaterial;
            }
            else
                PS.rend.material = targetMaterial;
            timer += Time.deltaTime;
            yield return null;
        }
        PS.rend.material = materialPlayer;
    }

    IEnumerator ChangeScale(GameObject face, Vector3 targetScale, float duration, Material targetMaterial, bool isGettingBigger)
    {
        Vector3 startScale = face.GetComponent<FaceScript>().glowingPart.transform.localScale;
        float timer = 0f;

        while (timer < duration)
        {
            face.GetComponent<FaceScript>().rend.material = targetMaterial;
            if (face.GetComponent<FaceScript>().havePlayer)
            {
                Lose(face);
            }
            face.GetComponent<FaceScript>().glowingPart.transform.localScale = Vector3.Lerp(startScale, targetScale, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        if (!isGettingBigger)
        {
            face.GetComponent<FaceScript>().rend.material = materialWhite;
        }
        face.GetComponent<FaceScript>().glowingPart.transform.localScale = targetScale;
    }

    IEnumerator FadeColorAndScale(GameObject cube, Material targetMaterial, Vector3 targetScale, float colorDuration, float scaleDuration)
    {
        yield return StartCoroutine(FadeColor(cube, targetMaterial, colorDuration));
        yield return StartCoroutine(ChangeScale(cube, targetScale, scaleDuration, materialRed, false));
        cube.GetComponent<FaceScript>().isKilling = false;
        FaceDanceScript FDC = cube.GetComponent<FaceDanceScript>();
        if (FDC.isOn && FDC != null)
        {
            cube.GetComponent<FaceDanceScript>().StartScaling();
        }
    }

    private void Lose(GameObject face)
    {
        if (!isTutorial)
        {
            face.GetComponent<FaceScript>().havePlayer = false;
            imageLose.gameObject.SetActive(true);
            if (TC != null)
            {
                TC.timerIsRunning = false;
            }
            isTurnOn = false;
            SCD.isOn = false;
        }
        else
        {
            StartCoroutine(ToggleImageCoroutine());
            TuC.LoseTutorial();
        }
    }

    IEnumerator ToggleImageCoroutine()
    {
        panel.enabled = true;
        yield return new WaitForSeconds(0.1f);
        panel.enabled = false;
    }
}
