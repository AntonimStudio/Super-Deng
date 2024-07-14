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
    private int lastCubeIndex = -1;

    public void StartKilling()
    {
        InvokeRepeating("ChangeFaceColor", 0f, 60/90f);
    }

    private void ChangeFaceColor()
    {
        int randomIndex = Random.Range(0, faces.Length);
        while (randomIndex == lastCubeIndex)
        {
            randomIndex = Random.Range(0, faces.Length);
        }

        if (lastCubeIndex != -1)
        {
            StartCoroutine(FadeColorAndScale(faces[lastCubeIndex], materialWhite, new Vector3(1f, 1f, 1f), colorChangeDuration, scaleChangeDuration));
        }

        StartCoroutine(ChangeColorThenScale(faces[randomIndex], materialRed, new Vector3(1f, 1f, scaleChange), colorChangeDuration, scaleChangeDuration));
        lastCubeIndex = randomIndex;
    }

    IEnumerator ChangeColorThenScale(GameObject face, Material targetMaterial, Vector3 targetScale, float colorDuration, float scaleDuration)
    {
        // Сначала меняем цвет
        yield return StartCoroutine(FadeColor(face, targetMaterial, colorDuration));
        
        if (face.GetComponent<FaceScript>().havePlayer)
        {
            //face.GetComponent<FaceScript>().player.gameObject.SetActive(false);
            //face.GetComponent<FaceScript>().havePlayer = false;
            image.gameObject.SetActive(true);
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
        // Последовательное изменение цвета и масштаба
        yield return StartCoroutine(FadeColor(cube, targetMaterial, colorDuration));
        yield return StartCoroutine(ChangeScale(cube, targetScale, scaleDuration, materialRed));
    }
}
