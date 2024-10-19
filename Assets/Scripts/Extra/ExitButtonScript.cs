using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButtonScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textQuiting;
    public float holdTime = 3.5f;
    private float holdTimer = 0f;  // Таймер для отслеживания времени удержания

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            holdTimer += Time.deltaTime;
            if (holdTimer <= 0.5f) textQuiting.text = "Quiting.";
            else if (holdTimer >= 0.5f && holdTimer <= 1f) textQuiting.text = "Quiting..";
            else if (holdTimer >= 1f && holdTimer <= 1.5f) textQuiting.text = "Quiting...";
            if (holdTimer >= holdTime)
            {
                Application.Quit();
                holdTimer = 0f;
            }
        }
        else
        {
            holdTimer = 0f;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
