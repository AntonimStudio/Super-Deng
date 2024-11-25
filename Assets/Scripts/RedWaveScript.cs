using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RedWaveScript : MonoBehaviour
{
    private GameObject[] faces;
    [SerializeField] private int proximityLimit = 5;
    [SerializeField] private float colorChangeDuration = 2f;
    [SerializeField] private float scaleChangeDurationUp = 1f;
    [SerializeField] private float scaleChangeDurationDown = 1f;
    [SerializeField] private float waitDuration = 1f;
    [SerializeField] private float scaleChange = 4f;
    [SerializeField] private float positionChange = -4f;
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

    private void Start()
    {
        faces = FAS.GetAllFaces();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isRandomSpawnTime = true;
            StartSettingRedWave();
        }
    }

    public void StartSettingRedWave()
    {
        if (isTurnOn)
        {
            if (isRandomSpawnTime)
            {
                List<int> availableFaces = new List<int>();

                // Сбор подходящих граней
                for (int i = 0; i < faces.Length; i++)
                {
                    FaceScript FS = faces[i].GetComponent<FaceScript>();
                    if (!FS.havePlayer &&
                        !FS.isBlinking &&
                        !FS.isKilling &&
                        !FS.isBlocked &&
                        !FS.isColored &&
                        !FS.isPortal &&
                        !FS.isBonus &&
                        FS.pathObjectCount >= proximityLimit)
                    {
                        availableFaces.Add(i);
                    }
                }

                if (availableFaces.Count == 0) return;

                int randomIndex = Random.Range(0, availableFaces.Count);
                int selectedFaceIndex = availableFaces[randomIndex];

                StartCoroutine(SetRedWave(faces[selectedFaceIndex]));
            }
        }
    }

    private IEnumerator SetRedWave(GameObject face)
    {
        FaceScript FS = face.GetComponent<FaceScript>();
        FaceDanceScript FDC = face.GetComponent<FaceDanceScript>();
        FS.isKilling = true;
        FDC.isOn = false;
        float timer = 0f;
        while (timer < colorChangeDuration)
        {
            SetMaterial(FS, materialRed);
            timer += Time.deltaTime;
            yield return null;
        }
        SetNextStep(FS);

        yield return StartCoroutine(ChangeScale(face, new Vector3(1f, 1f, scaleChange), new Vector3(0f, positionChange, 0f), scaleChangeDurationUp, true));
        
        yield return new WaitForSeconds(waitDuration);
       
        yield return StartCoroutine(ChangeScale(face, new Vector3(1f, 1f, 1f), new Vector3(0f, 0f, 0f), scaleChangeDurationDown, false));

        SetMaterialBack(FS);

        FS.isKilling = false;

    }

    private IEnumerator ChangeScale(GameObject face, Vector3 targetScale, Vector3 targetPosition, float duration, bool flag)
    {
        FaceScript FS = face.GetComponent<FaceScript>();
        Vector3 startScale = FS.glowingPart.transform.localScale;
        Vector3 startPosition = FS.glowingPart.transform.localPosition;
        float timer = 0f;

        while (timer < duration)
        {
            SetMaterial(FS, materialRed);
            if (flag)
            {
                FS.glowingPart.transform.localScale = Vector3.Lerp(new Vector3(0f, 0f, 0f), targetScale, timer / duration);
            }
            else
            {
                FS.glowingPart.transform.localScale = Vector3.Lerp(startScale, targetScale, timer / duration);
            }
            
            FS.glowingPart.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, timer / duration);

            timer += Time.deltaTime;
            yield return null;
        }
        FS.glowingPart.transform.localScale = targetScale;
    }

    private void SetMaterial(FaceScript FS, Material material)
    {
        if (!FS.havePlayer) FS.rend.material = material;
        else PS.SetPartsMaterial(material);
    }

    private void SetMaterialBack(FaceScript FS)
    {
        if (FS.havePlayer) PS.SetPartsMaterial(materialPlayer); 
        else if (FS.isRight) FS.rend.material = FS.materialRightFace;
        else if (FS.isLeft) FS.rend.material = FS.materialLeftFace;
        else if (FS.isTop) FS.rend.material = FS.materialTopFace;
        else FS.rend.material = materialWhite;
    }

    private void SetNextStep(FaceScript facescript)
    {
        FaceScript objectWithMinPathCounter = null;
        int minPathCounter = int.MaxValue;

        FaceScript[] faces = { facescript.FS1, facescript.FS2, facescript.FS3 };

        foreach (FaceScript face in faces)
        {
            if (face == null) continue;

            if (face != null && (face.pathObjectCount < minPathCounter))
            {
                if (!face.isBlinking &&
                    !face.isKilling &&
                    !face.isBlocked &&
                    !face.isColored &&
                    !face.isPortal &&
                    !face.isBonus)
                {
                    minPathCounter = face.pathObjectCount;
                    objectWithMinPathCounter = face;
                }
            }
        }
        if (facescript.pathObjectCount != 0 && objectWithMinPathCounter != null) 
            StartCoroutine(SetRedWave(objectWithMinPathCounter.gameObject));
    }
}
