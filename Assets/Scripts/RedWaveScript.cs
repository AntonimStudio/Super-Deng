using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RedWaveScript : MonoBehaviour
{
    private GameObject[] faces;
    //[SerializeField] private Image imageLose;
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
                for (int i = 0; i < colvo; i++)
                {
                    int randomIndex;
                    FaceScript FS;
                    do
                    {
                        randomIndex = Random.Range(0, faces.Length);
                        FS = faces[randomIndex].GetComponent<FaceScript>();
                    }
                    while (FS.havePlayer || FS.isBlinking || FS.isKilling || FS.isBlocked || FS.isBonus || FS.pathObjectCount <5); //lastCubeIndices.Contains(randomIndex) 
                    Debug.Log(FS.pathObjectCount);
                    StartCoroutine(SetRedWave(faces[randomIndex], materialRed, new Vector3(1f, 1f, scaleChange), colorChangeDuration, scaleChangeDurationUp, scaleChangeDurationDown));
                }
            }
        }
    }

    private IEnumerator SetRedWave(GameObject face, Material targetMaterial, Vector3 targetScale, float colorDuration, float scaleDurationUp, float scaleDurationDown)
    {
        FaceScript FS = face.GetComponent<FaceScript>();
        FaceDanceScript FDC = face.GetComponent<FaceDanceScript>();
        FS.isKilling = true;
        /*
        if (FDC.isOn && FDC != null)
        {
            FDC.StopScaling();
        }*/
        FDC.isOn = false;
        float timer = 0f;
        while (timer < colorDuration)
        {
            if (!FS.havePlayer) FS.rend.material = targetMaterial;
            else SetPartsMaterial(targetMaterial);
            timer += Time.deltaTime;
            yield return null;
        }
        SetNextStep(face.GetComponent<FaceScript>());
        yield return StartCoroutine(ChangeScale(face, new Vector3(1f, 1f, scaleChange), new Vector3(0f, positionChange, 0f), scaleDurationUp, true));
        
        yield return new WaitForSeconds(waitDuration);
       
        yield return StartCoroutine(ChangeScale(face, new Vector3(1f, 1f, 1f), new Vector3(0f, 0f, 0f), scaleDurationDown, false));

        if (!FS.havePlayer) FS.rend.material = materialWhite;
        else SetPartsMaterial(materialPlayer);

        if (FS.havePlayer) { SetPartsMaterial(materialPlayer); }
        else if (FS.isRight)
        {
            FS.rend.material = FS.materialRightFace;
        }
        else if (FS.isLeft)
        {
            FS.rend.material = FS.materialLeftFace;
        }
        else if (FS.isTop)
        {
            FS.rend.material = FS.materialTopFace;
        }
        else FS.rend.material = materialWhite;


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
            if (!FS.havePlayer) FS.rend.material = materialRed;
            else SetPartsMaterial(materialPlayer);
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


    private void SetPartsMaterial(Material material)
    {
        PS.rendPartTop.material = material;
        PS.rendPartMiddle.material = material;
        PS.rendPartLeft.material = material;
        PS.rendPartRight.material = material;
    }

    private void SetNextStep(FaceScript facescript)
    {

        
        FaceScript objectWithMinPathCounter = null;
        int minPathCounter = int.MaxValue;

        FaceScript[] objects = { facescript.FS1, facescript.FS2, facescript.FS3 };

        foreach (FaceScript obj in objects)
        {
            if (obj == null) continue;

            FaceScript faceScript = obj.GetComponent<FaceScript>();
            if (faceScript != null)
            {
                if (faceScript.pathObjectCount < minPathCounter)
                {
                    minPathCounter = faceScript.pathObjectCount;
                    objectWithMinPathCounter = obj;
                }
            }
        }
        if (facescript.pathObjectCount != 0) StartCoroutine(SetRedWave(objectWithMinPathCounter.gameObject, materialRed, new Vector3(1f, 1f, scaleChange), colorChangeDuration, scaleChangeDurationUp, scaleChangeDurationDown));
    }
}
