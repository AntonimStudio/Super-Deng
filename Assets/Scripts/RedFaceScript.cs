using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedFaceScript : MonoBehaviour
{
    [SerializeField] private GameObject[] faces;
    [SerializeField] private Image image;
    [SerializeField] private float colorChangeDuration = 2f;
    [SerializeField] private float scaleChangeDuration = 1f;
    [SerializeField] private float scaleChange = 25f;
    [SerializeField] private Material materialWhite;
    [SerializeField] private Material materialRed;
    [SerializeField] private StartCountDown SCD;
    [SerializeField] private TimerController TC;
    [SerializeField] private EnemySpawnSettings spawnSettings;
    private float timeElapsed = 0f;
    private int lastCubeIndex = -1;
    public bool isTurnOn = false;


    private List<int> lastCubeIndices = new List<int>();

    public void ChangeFaceColor(int n)
    {
        if (isTurnOn)
        {
            // Call FadeColorAndScale on previously changed faces
            foreach (int index in lastCubeIndices)
            {
                StartCoroutine(FadeColorAndScale(faces[index], materialWhite, new Vector3(1f, 1f, 1f), colorChangeDuration, scaleChangeDuration));
            }

            // Clear the lastCubeIndices list for new indices
            lastCubeIndices.Clear();

            for (int i = 0; i < n; i++)
            {
                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, faces.Length);
                }
                while (lastCubeIndices.Contains(randomIndex));

                lastCubeIndices.Add(randomIndex);
                StartCoroutine(ChangeColorThenScale(faces[randomIndex], materialRed, new Vector3(1f, 1f, scaleChange), colorChangeDuration, scaleChangeDuration));
            }
        }
    }



    private void Update()
    {
        timeElapsed += Time.deltaTime;
    }


    IEnumerator ChangeColorThenScale(GameObject face, Material targetMaterial, Vector3 targetScale, float colorDuration, float scaleDuration)
    {
        // Сначала меняем цвет
        yield return StartCoroutine(FadeColor(face, targetMaterial, colorDuration));
        
        if (face.GetComponent<FaceScript>().havePlayer)
        {
            face.GetComponent<FaceScript>().havePlayer = false;
            image.gameObject.SetActive(true);
            TC.timerIsRunning = false;
            isTurnOn = false;
            SCD.isOn = false;
        }
        // Затем меняем масштаб
        yield return StartCoroutine(ChangeScale(face, targetScale, scaleDuration, targetMaterial));
    }

    IEnumerator FadeColor(GameObject face, Material targetMaterial, float duration)
    {
        face.GetComponent<FaceScript>().rend.material = targetMaterial;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ChangeScale(GameObject face, Vector3 targetScale, float duration, Material targetMaterial)
    {
        Vector3 startScale = face.GetComponent<FaceScript>().glowingPart.transform.localScale;
        float timer = 0f;

        while (timer < duration)
        {
            face.GetComponent<FaceScript>().rend.material = targetMaterial;
            if (face.GetComponent<FaceScript>().havePlayer)
            {
                //face.GetComponent<FaceScript>().player.gameObject.SetActive(false);
                image.gameObject.SetActive(true);
            }
            face.GetComponent<FaceScript>().glowingPart.transform.localScale = Vector3.Lerp(startScale, targetScale, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        face.GetComponent<FaceScript>().rend.material = materialWhite;
        face.GetComponent<FaceScript>().glowingPart.transform.localScale = targetScale;
    }

    IEnumerator FadeColorAndScale(GameObject cube, Material targetMaterial, Vector3 targetScale, float colorDuration, float scaleDuration)
    {
        yield return StartCoroutine(FadeColor(cube, targetMaterial, colorDuration));
        yield return StartCoroutine(ChangeScale(cube, targetScale, scaleDuration, materialRed));
    }
}
