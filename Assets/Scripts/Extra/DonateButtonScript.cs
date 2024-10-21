using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DonateButtonScript : MonoBehaviour
{
    [SerializeField] private string url = "https://example.com";
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(OpenURL);
    }

    private void OpenURL()
    {
        Application.OpenURL(url);
    }
}