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
        faces = FAS.GetAllFaces();
    }

    private void Update()
    {/*
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
        }*/

        if (Input.GetKeyDown(KeyCode.X))
        {
            isRandomSpawnTime = true;
            StartSettingRedFace();
        }
    }
    /*
    public void StartSettingRedFace()
    {
        if (isTurnOn)
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
                while (FS.havePlayer);
                    
                StartCoroutine(SetRedFace(faces[randomIndex], materialRed, new Vector3(1f, 1f, scaleChange), colorChangeDuration, scaleChangeDuration));
            }
        }
    }*/

    public void StartSettingRedFace()
    {
        if (isTurnOn)
        {/*
            foreach (int index in lastCubeIndices)
            {
                //GameObject face, Material targetMaterial, Vector3 targetScale, float colorDuration, float scaleDuration
                //StartCoroutine(FadeColorAndScale(faces[index], materialWhite, new Vector3(1f, 1f, 1f), colorChangeDuration, scaleChangeDuration));
                StartCoroutine(ChangeScaleThenColor(faces[index], materialWhite, new Vector3(1f, 1f, 1f), colorChangeDuration, scaleChangeDuration));
                faces[index].GetComponent<FaceScript>().isKilling = false;
            }
            lastCubeIndices.Clear();
            */
            if (isRandomSpawnTime)
            {
                List<int> availableFaces = new List<int>();

                // ���� ���������� ������
                for (int i = 0; i < faces.Length; i++)
                {
                    FaceScript FS = faces[i].GetComponent<FaceScript>();
                    if (!FS.havePlayer &&
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

                // ���� ��������� ������ ���, �������
                if (availableFaces.Count == 0) return;

                // �������� ��������� ������ �� ��������� ������
                int randomIndex = Random.Range(0, availableFaces.Count);
                int selectedFaceIndex = availableFaces[randomIndex];

                // ��������� �������� ��� ��������� �����
                StartCoroutine(SetRedFace(faces[selectedFaceIndex],materialRed));
            }
            else
            {
                for (int i = 0; i < newCubeIndices.Count; i++)
                {
                    FaceScript FS = faces[i].GetComponent<FaceScript>();
                    /*if (!(FS.havePlayer || FS.isBlinking || FS.isKilling || FS.isBlocked || FS.isBonus))
                    {
                        StartCoroutine(ChangeColorThenScale(faces[newCubeIndices[i]], materialRed, new Vector3(1f, 1f, scaleChange), colorChangeDuration, scaleChangeDuration));
                    }*/
                    StartCoroutine(SetRedFace(faces[newCubeIndices[i]], materialRed));
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
