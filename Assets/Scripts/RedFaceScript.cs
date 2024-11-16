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
    public bool isTutorial = false;
    public TutorialController TuC;
    public Image panel;
    
    private void Start()
    {
        faces = FAS.GetAllFaces();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ChangeFaceColor();
        }
    }

    public void ChangeFaceColor()
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
                    
                StartCoroutine(ChangeColorThenScale(faces[randomIndex], materialRed, new Vector3(1f, 1f, scaleChange), colorChangeDuration, scaleChangeDuration));
            }
        }
    }


    private IEnumerator ChangeColorThenScale(GameObject face, Material targetMaterial, Vector3 targetScale, float colorDuration, float scaleDuration)
    {
        yield return StartCoroutine(ChangeScale(face, new Vector3(1f, 1f, scaleChange), scaleDuration));
        yield return new WaitForSeconds(0.3333f);
        yield return StartCoroutine(ChangeScale(face, new Vector3(1f, 1f, 1f), scaleDuration)); ///materialRed

    }

    IEnumerator ChangeScale(GameObject face, Vector3 targetScale, float duration)
    {
        FaceScript FS = face.GetComponent<FaceScript>();
        Vector3 startScale = FS.glowingPart.transform.localScale;
        float timer = 0f;

        while (timer < duration)
        {
            FS.glowingPart.transform.localScale = Vector3.Lerp(startScale, targetScale, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        FS.glowingPart.transform.localScale = targetScale;
    }
}
