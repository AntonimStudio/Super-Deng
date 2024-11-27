using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class RedFaceScript : MonoBehaviour
{
    private GameObject[] faces;
    //[SerializeField] private Image imageLose;
    [SerializeField] private float colorChangeDuration = 2f;
    [SerializeField] private float scaleChangeDurationUp = 1f;
    [SerializeField] private float scaleChangeDurationDown = 1f;
    [SerializeField] private float scaleChange = 25f;
    [SerializeField] private float waitDuration = 1f;
    [SerializeField] private float positionChange = -4.5f;
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

    private List<int> faceIndices = new List<int>();
    private int colvo = 0;
    private bool[] spawnExecuted;
    private bool isRandomSpawnTime = true;
    private int currentSpawnIndex = 0;

    private void Start()
    {
        faces = FAS.GetAllFaces();
        spawnExecuted = new bool[enemySpawnSettings.spawnTimes.Length];
    }

    private void Update()
    {
        if (TC != null)
        {
            float elapsedTime = TC.timeElapsed;

            if (currentSpawnIndex < enemySpawnSettings.spawnTimes.Length)
            {
                var spawnTimeData = enemySpawnSettings.spawnTimes[currentSpawnIndex];
                var nextSpawnTimeData = currentSpawnIndex < enemySpawnSettings.spawnTimes.Length - 1
                    ? enemySpawnSettings.spawnTimes[currentSpawnIndex + 1]
                    : new SpawnTimeData { time = float.MaxValue }; 

                if (elapsedTime >= spawnTimeData.time && elapsedTime <= nextSpawnTimeData.time && !spawnExecuted[currentSpawnIndex])
                {
                    if (spawnTimeData.isRedFaceTurnOn)
                    {
                        if (spawnTimeData.isRedFaceRandom)
                        {
                            colvo = spawnTimeData.quantityOfRedFaces;
                            isRandomSpawnTime = true;
                        }
                        else
                        {
                            faceIndices.Clear();
                            faceIndices = new List<int>(spawnTimeData.arrayOfRedFaces);
                            isRandomSpawnTime = false;
                        }
                        spawnExecuted[currentSpawnIndex] = true;
                    }
                    else
                    {
                        faceIndices.Clear();
                        isRandomSpawnTime = false;
                    }
                }
                if (elapsedTime > nextSpawnTimeData.time)
                {
                    currentSpawnIndex++;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            isRandomSpawnTime = true;
            StartSettingRedFace();
        }
    }

    public void StartSettingRedFace()
    {
        if (isTurnOn)
        {
            List<int> availableFaces = new List<int>();

            for (int i = 0; i < faces.Length; i++)
            {
                FaceScript FS = faces[i].GetComponent<FaceScript>();
                if (//!FS.havePlayer &&
                    !FS.isBlinking &&
                    !FS.isKilling &&
                    !FS.isBlocked &&
                    !FS.isColored &&
                    !FS.isPortal &&
                    !FS.isBonus)
                {
                    availableFaces.Add(i);
                }
            }

            if (isRandomSpawnTime)
            {
                //Debug.Log(colvo);
                for (int i = 0; i < colvo; i++)
                {
                    if (availableFaces.Count == 0) return;

                    int randomIndex = Random.Range(0, availableFaces.Count);
                    int selectedFaceIndex = availableFaces[randomIndex];
                    StartCoroutine(SetRedFace(faces[selectedFaceIndex], materialRed));
                    availableFaces.RemoveAt(randomIndex);
                }
            }
            else
            {
                var intersectedIndices = faceIndices.Intersect(availableFaces);
                foreach (int index in intersectedIndices)
                {
                    StartCoroutine(SetRedFace(faces[index], materialRed));
                    //Debug.Log(faces[index].name);
                }
            }
        }
    }

    private IEnumerator SetRedFace(GameObject face, Material targetMaterial)
    {
        FaceScript FS = face.GetComponent<FaceScript>();
        FaceDanceScript FDC = face.GetComponent<FaceDanceScript>();
        
        /*
        if (FDC.isOn && FDC != null)
        {
            FDC.StopScaling();
        }*/
        FDC.isOn = false;
        FS.isColored = true;
        float timer = 0f;
        while (timer < colorChangeDuration)
        {
            if (!FS.havePlayer) FS.rend.material = targetMaterial;
            else PS.SetPartsMaterial(targetMaterial);
            timer += Time.deltaTime;
            yield return null;
        }
        FS.isKilling = true;
        FS.isColored = false ;
        yield return StartCoroutine(ChangeScale(face, new Vector3(1f, 1f, scaleChange), new Vector3(0f, positionChange, 0f), scaleChangeDurationUp));

        yield return new WaitForSeconds(waitDuration);

        yield return StartCoroutine(ChangeScale(face, new Vector3(1f, 1f, 1f), new Vector3(0f, 0f, 0f), scaleChangeDurationDown)); 

        if (FS.havePlayer) { PS.SetPartsMaterial(materialPlayer); }
        else if (FS.isRight) FS.rend.material = FS.materialRightFace;
        else if (FS.isLeft) FS.rend.material = FS.materialLeftFace;
        else if (FS.isTop) FS.rend.material = FS.materialTopFace;
        else FS.rend.material = materialWhite;


        FS.isKilling = false;

    }

    IEnumerator ChangeScale(GameObject face, Vector3 targetScale, Vector3 targetPosition, float duration)
    {
        FaceScript FS = face.GetComponent<FaceScript>();
        Vector3 startScale = FS.glowingPart.transform.localScale;
        Vector3 startPosition = FS.glowingPart.transform.localPosition;
        float timer = 0f;

        while (timer < duration)
        {
            if (!FS.havePlayer) FS.rend.material = materialRed;
            else PS.SetPartsMaterial(materialPlayer);

            FS.glowingPart.transform.localScale = Vector3.Lerp(startScale, targetScale, timer / duration);
            FS.glowingPart.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, timer / duration);

            timer += Time.deltaTime;
            yield return null;
        }
        FS.glowingPart.transform.localScale = targetScale;
    }
}
