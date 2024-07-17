using UnityEngine;
using UnityEngine.UI;

public class BeatController : MonoBehaviour
{
    public float beatInterval = 0.6666f;
    private float elapsedTime = 0f;
    public bool canPress = false;
    public bool canCombo = false;

    [SerializeField] private Image image1;
    private Vector3 startPos1 = new Vector3(-350f, 0f, 0f);
    private Vector3 midPos1 = new Vector3(-200f, 0f, 0f);
    private Vector3 almostEndPos1 = new Vector3(-50f, 0f, 0f);
    private Vector3 endPos1 = new Vector3(100f, 0f, 0f);
    [SerializeField] private Image image2;
    private Vector3 startPos2 = new Vector3(350f, 0f, 0f);
    private Vector3 midPos2 = new Vector3(200f, 0f, 0f);
    private Vector3 almostEndPos2 = new Vector3(50f, 0f, 0f);
    private Vector3 endPos2 = new Vector3(-100f, 0f, 0f);
    [SerializeField] private CameraZoom CZ;
    public int beatcount = 0;
    private float lastBeatTime = 0f;
    public bool isAlreadyPressed = false;
    public bool isAlreadyPressedIsAlreadyPressed = false;
    public bool isAlreadyZoomed = false;

    void Start()
    {

        image1.enabled = false;
        image1.rectTransform.localPosition = startPos1;
        image2.enabled = false;
        image2.rectTransform.localPosition = startPos2;
    }

    private void Update()
    {
        if (0f < elapsedTime && ((elapsedTime < 0.25f * beatInterval) || (elapsedTime > 0.75f * beatInterval)))
        {
            canCombo = true;
        }
        else canCombo = false;

        if (elapsedTime > 0f)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < beatInterval / 3f)
            {

                if (!isAlreadyPressed)
                    canPress = true;
                else
                    canPress = false;
                float t = elapsedTime / (beatInterval / 3f);
                image1.rectTransform.localPosition = Vector3.Lerp(almostEndPos1, endPos1, t);
                image2.rectTransform.localPosition = Vector3.Lerp(almostEndPos2, endPos2, t);

            }
            else if (elapsedTime < (2f * beatInterval) / 3f)
            {
                canPress = false;
                if (!isAlreadyPressedIsAlreadyPressed)
                    PressIsAlreadyPress();
                float t = (elapsedTime - (beatInterval / 3f)) / (beatInterval / 3f);
                image1.enabled = true;
                image1.rectTransform.localPosition = Vector3.Lerp(startPos1, midPos1, t);
                image2.enabled = true;
                image2.rectTransform.localPosition = Vector3.Lerp(startPos2, midPos2, t);
            }
            else if (elapsedTime < beatInterval)
            {
                if (!isAlreadyPressed)
                    canPress = true;
                else
                    canPress = false;
                float t = (elapsedTime - ((2f * beatInterval) / 3f)) / (beatInterval / 3f);
                image1.rectTransform.localPosition = Vector3.Lerp(midPos1, almostEndPos1, t);
                image2.rectTransform.localPosition = Vector3.Lerp(midPos2, almostEndPos2, t);
                if (! isAlreadyZoomed)
                {
                    PressIsAlreadyZoomed();
                }
            }
            else
            {
                elapsedTime = 0f;
            }
        }
    }

    private void PressIsAlreadyPress()
    {
        isAlreadyPressed = false;
        isAlreadyPressedIsAlreadyPressed = true;
    }

    private void PressIsAlreadyZoomed()
    {
        isAlreadyZoomed = true;
        CZ.StartZooming();
        
    }

    public void OnBeat()
    {
        float currentTime = Time.time;
        if (lastBeatTime != 0f)
        {
            beatInterval = currentTime - lastBeatTime;
            //Debug.Log(beatInterval);
        }
        lastBeatTime = currentTime;
        beatcount++;
        elapsedTime = 0.0001f; // Запуск процесса
        //image.rectTransform.localPosition = startPos; // Телепортирование на начальную позицию
    }
}