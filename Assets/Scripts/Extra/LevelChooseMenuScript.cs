using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class LevelChooseMenuScript : MonoBehaviour
{
    [SerializeField] private Button buttonStart;
    [SerializeField] private LevelButtonTransition LBT;
    [SerializeField] private int[] scenes;

    private void Start()
    {
        buttonStart.onClick.AddListener(LoadCorrespondingScene);
    }

    private void LoadCorrespondingScene()
    {
        SceneManager.LoadScene(scenes[LBT._numberMenu]);
    }
}
