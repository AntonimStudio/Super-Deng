using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RedFaceScript : MonoBehaviour
{
    private GameObject[] faces;
    //[SerializeField] private Image imageLose;
    [SerializeField] private float colorChangeDuration = 2f;
    [SerializeField] private float scaleChangeDuration = 1f;
    [SerializeField] private float scaleChange = 25f;
    [SerializeField] private Material materialWhite;
    [SerializeField] private Material materialRed;
    [SerializeField] private Material materialPlayer;
    [SerializeField] private StartCountDown SCD;
    [SerializeField] private FaceArrayScript FAS;
    [SerializeField] private TimerController TC;
    [SerializeField] private PlayerScript PS;
    [SerializeField] private ComboManager CM;
    [SerializeField] private EnemySpawnSettings enemySpawnSettings;
    public bool isTurnOn = false;


    private List<int> lastCubeIndices = new List<int>();
    private List<int> newCubeIndices = new List<int>();
    private bool[] spawnExecuted;
    private int colvo = 1;
    private bool isRandomSpawnTime = true;
    public bool isTutorial = false;
    public TutorialController TuC;
    public Image panel;
    /*
    private void Start()
    {
        faces = FAS.GetAllFaces();
        spawnExecuted = new bool[enemySpawnSettings.spawnTimes.Length];
        //PS.rend.material = targetMaterial;
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

    public void ChangeFaceColor()
    {
        if (isTurnOn)
        {
            foreach (int index in lastCubeIndices)
            {
                //StartCoroutine(FadeColorAndScale(faces[index], materialWhite, new Vector3(1f, 1f, 1f), colorChangeDuration, scaleChangeDuration));
                StartCoroutine(ChangeScale(faces[index], new Vector3(1f, 1f, 1f), scaleChangeDuration, materialWhite, false));
                faces[index].GetComponent<FaceScript>().isKilling = false;
            }
            lastCubeIndices.Clear();

            if (isRandomSpawnTime) 
            { 
                for (int i = 0; i < colvo; i++)
                {
                    int randomIndex;
                    FaceScript FS;
                    do
                    {
                        randomIndex = Random.Range(0, faces.Length);
                        FS = faces[randomIndex].GetComponent<FaceScript>();
                    }
                    while (lastCubeIndices.Contains(randomIndex) || FS.havePlayer || FS.isBlinking || FS.isKilling || FS.isBlocked || FS.isBonus);
                    lastCubeIndices.Add(randomIndex);
                    StartCoroutine(ChangeColorThenScale(faces[randomIndex], materialRed, new Vector3(1f, 1f, scaleChange), colorChangeDuration, scaleChangeDuration));
                    FS.isKilling = true;
                    //faces[randomIndex].GetComponent<FaceDanceScript>().StopScaling();
                }
            }
            else
            {
                for (int i = 0; i < newCubeIndices.Count; i++)
                {
                    FaceScript FS = faces[i].GetComponent<FaceScript>();
                    /*if (!(FS.havePlayer || FS.isBlinking || FS.isKilling || FS.isBlocked || FS.isBonus))
                    {
                        StartCoroutine(ChangeColorThenScale(faces[newCubeIndices[i]], materialRed, new Vector3(1f, 1f, scaleChange), colorChangeDuration, scaleChangeDuration));
                    }
                    StartCoroutine(ChangeColorThenScale(faces[newCubeIndices[i]], materialRed, new Vector3(1f, 1f, scaleChange), colorChangeDuration, scaleChangeDuration));
                }
            }
        }
    }


    private IEnumerator ChangeColorThenScale(GameObject face, Material targetMaterial, Vector3 targetScale, float colorDuration, float scaleDuration)
    {

        yield return StartCoroutine(FadeColor(face, targetMaterial, colorDuration));
        yield return StartCoroutine(ChangeScale(face, targetScale, scaleDuration, targetMaterial, true));
        /*
       if (face.GetComponent<FaceScript>().havePlayer)
       {
           Lose(face);
       }
    }
    
    private IEnumerator FadeColorAndScale(GameObject cube, Material targetMaterial, Vector3 targetScale, float colorDuration, float scaleDuration)
    {
        //yield return StartCoroutine(FadeColor(cube, targetMaterial, colorDuration));
        yield return StartCoroutine(ChangeScale(cube, targetScale, scaleDuration, materialRed, false));
        //cube.GetComponent<FaceScript>().isKilling = false;
        /*FaceDanceScript FDC = cube.GetComponent<FaceDanceScript>();
        if (FDC.isOn && FDC != null)
        {
            cube.GetComponent<FaceDanceScript>().StartScaling();
        }
    }

    IEnumerator FadeColor(GameObject face, Material targetMaterial, float duration)
    {
        if (!face.GetComponent<FaceScript>().havePlayer)
        {
            face.GetComponent<FaceScript>().rend.material = targetMaterial;
        }
        else
        {
            PS.rendPartTop.material = targetMaterial;
            PS.rendPartMiddle.material = targetMaterial;
            PS.rendPartLeft.material = targetMaterial;
            PS.rendPartRight.material = targetMaterial;
        }
            
        float timer = 0f;
        while (timer < duration)
        {
            if (!face.GetComponent<FaceScript>().havePlayer)
            {
                face.GetComponent<FaceScript>().rend.material = targetMaterial;
            }
            else
            {
                PS.rendPartTop.material = targetMaterial;
                PS.rendPartMiddle.material = targetMaterial;
                PS.rendPartLeft.material = targetMaterial;
                PS.rendPartRight.material = targetMaterial;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        PS.rendPartTop.material = materialPlayer;
        PS.rendPartMiddle.material = materialPlayer;
        PS.rendPartLeft.material = materialPlayer;
        PS.rendPartRight.material = materialPlayer;
    }

    IEnumerator ChangeScale(GameObject face, Vector3 targetScale, float duration, Material targetMaterial, bool isGettingBigger)
    {
        FaceScript FS = face.GetComponent<FaceScript>();
        Vector3 startScale = FS.glowingPart.transform.localScale;
        float timer = 0f;
        
        while (timer < duration)
        {
            FS.rend.material = targetMaterial;
            /*if (FS.havePlayer)
            {
                Lose(face);
            }
            FS.glowingPart.transform.localScale = Vector3.Lerp(startScale, targetScale, timer / duration);
            timer += Time.deltaTime;
            Debug.Log(targetScale);
            yield return null;
        }
        if (!isGettingBigger)
        {
            FS.rend.material = materialWhite;
        }
        else FS.rend.material = materialRed;
        FS.glowingPart.transform.localScale = targetScale;
    }

    private void Lose(GameObject face)
    {
        if (!isTutorial)
        {
            face.GetComponent<FaceScript>().havePlayer = false;
            PS.Lose();
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
    }*/
}
