using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartCountDown : MonoBehaviour
{
    [SerializeField] private TMP_Text countDownText; // Ссылка на UI элемент для отображения текста
    [SerializeField] private GameObject icosahedron; // Ссылка на объект куба
    [SerializeField] private GameObject rhythmManager;
    [SerializeField] private GameObject moveImageOnBeat;
    [SerializeField] private GameObject moveImageOnBeat1;
    [SerializeField] private GameObject killerManager;
    public bool isOn = false;

    private void Start()
    {
        StartCoroutine(CountDownRoutine());
    }

    IEnumerator CountDownRoutine()
    {
        for (int i = 3; i > 0; i--)
        {
            countDownText.text = i.ToString();
            yield return new WaitForSeconds(0.7f);
        }

        countDownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);
        countDownText.gameObject.SetActive(false);
        isOn = true;
        icosahedron.GetComponent<CubeRotation>().enabled = true; // Включаем скрипт вращения куба
        rhythmManager.GetComponent<RhythmManager>().enabled = true;
        moveImageOnBeat.GetComponent<MoveImageOnBeat>().enabled = true;
        moveImageOnBeat1.GetComponent<MoveImageOnBeat>().enabled = true;
        killerManager.GetComponent<RedFaceScript>().StartKilling();
    }
}

